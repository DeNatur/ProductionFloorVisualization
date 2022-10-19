using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;


#if WINDOWS_UWP
using Windows.Storage;
#endif


public class AzureSessionCoordinator : MonoBehaviour
{
    private ObjectsCreator objectsCreator;

    private SpatialAnchorManager cloudManager;
    private AnchorsRepository anchorsRepository;

    private readonly Queue<Action> dispatchQueue = new Queue<Action>();
    private AnchorLocateCriteria anchorLocateCriteria = new AnchorLocateCriteria();

    [Inject]
    public void Construct(
        AnchorsRepository anchorsRepository,
        SpatialAnchorManager cloudManager,
        ObjectsCreator objectsCreator
    )
    {
        this.anchorsRepository = anchorsRepository;
        this.cloudManager = cloudManager;
        this.objectsCreator = objectsCreator;
    }

    #region Unity Lifecycle
    void Start()
    {
        cloudManager.AnchorLocated += CloudManager_AnchorLocated;

        StartCoroutine(
                runAfterFrame(async () =>
                {
                    await startAzureSession();
                    findAzureAnchor();
                }
            )
        );
    }

    void Update()
    {
        lock (dispatchQueue)
        {
            if (dispatchQueue.Count > 0)
            {
                dispatchQueue.Dequeue()();
            }
        }
    }
    #endregion

    #region public methods

    public void findAzureAnchor()
    {
        Debug.Log("\nAnchorModuleScript.FindAzureAnchor()");

        List<string> anchorsToFind = anchorsRepository.getAnchorsIds();

        if (anchorsToFind.Count == 0)
        {
            return;
        }

        anchorLocateCriteria.Identifiers = anchorsToFind.ToArray();
        Debug.Log($"Anchor locate criteria configured to look for Azure anchor with ID '{anchorsToFind.ToArray()}'");

        if ((cloudManager != null) && (cloudManager.Session != null))
        {
            cloudManager.Session.CreateWatcher(anchorLocateCriteria);
            Debug.Log("Looking for Azure anchor... please wait...");
        }
        else
        {
            Debug.Log("Attempt to create watcher failed, no session exists");
        }
    }

    #endregion

    private void CloudManager_AnchorLocated(object sender, AnchorLocatedEventArgs args)
    {
        QueueOnUpdate(new Action(() => Debug.Log($"Anchor recognized as a possible Azure anchor")));

        if (args.Status == LocateAnchorStatus.Located)
        {

            QueueOnUpdate(() =>
            {
                Debug.Log($"Azure anchor located successfully");

                if (args.Anchor != null)
                {

                    Debug.Log("Local anchor position successfully set to Azure anchor position");
                    int index = int.Parse(args.Anchor.AppProperties["ANCHOR_TYPE"]);

                    GameObject newAnchor = objectsCreator.createNewMachineWithGO(objectsCreator.allMachines[index]);

                    TapToPlace tapToPlaceScript = newAnchor.GetComponent<TapToPlace>();
                    tapToPlaceScript.enabled = false;
                    tapToPlaceScript.AutoStart = false;

                    newAnchor.CreateNativeAnchor();
                    newAnchor.name = args.Identifier;

                    Pose anchorPose = Pose.identity;
                    anchorPose = args.Anchor.GetPose();
                    newAnchor.SetActive(true);
                    newAnchor.GetComponent<AnchorScript>().setAnchorCreatedState();

                    Debug.Log($"Setting object to anchor pose with position '{anchorPose.position}' and rotation '{anchorPose.rotation}' and name '{newAnchor.name}'");
                    newAnchor.transform.position = anchorPose.position;
                    newAnchor.transform.rotation = anchorPose.rotation;

                    newAnchor.CreateNativeAnchor();

                    anchorsRepository.addAnchor(
                        new AnchorsRepository.AnchorGameObject
                        {
                            anchor = args.Anchor,
                            gameObject = newAnchor,
                        }
                    );
                }
            });
        }
        else
        {
            QueueOnUpdate(new Action(() => Debug.Log($"Attempt to locate Anchor with ID '{args.Identifier}' failed, locate anchor status was not 'Located' but '{args.Status}'")));
        }
    }

    private IEnumerator runAfterFrame(Action actionToInvoke)
    {
        yield return new WaitForEndOfFrame();
        actionToInvoke();
    }

    private void QueueOnUpdate(Action updateAction)
    {
        lock (dispatchQueue)
        {
            dispatchQueue.Enqueue(updateAction);
        }
    }

    private async Task startAzureSession()
    {
        Debug.Log("Starting Azure session... please wait...");

        if (cloudManager.Session == null)
        {
            await cloudManager.CreateSessionAsync();
        }

        await cloudManager.StartSessionAsync();

        Debug.Log("Azure session started successfully");
    }
}
