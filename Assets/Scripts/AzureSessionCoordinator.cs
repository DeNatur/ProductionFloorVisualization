using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


#if WINDOWS_UWP
using Windows.Storage;
#endif


public class AzureSessionCoordinator : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The unique identifier used to identify the shared file (containing the Azure anchor ID) on the web server.")]
    private string publicSharingPin = "1982734901747";

    public GameObject mainGameObject;


    private SpatialAnchorManager cloudManager;
    private AnchorLocateCriteria anchorLocateCriteria;
    private CloudSpatialAnchorWatcher currentWatcher;
    private AzureAnchorsReporitory anchorsRepository;
    private AddAnchorUseCase addAnchorUseCase;

    private readonly Queue<Action> dispatchQueue = new Queue<Action>();


    #region Unity Lifecycle
    void Start()
    {
        anchorsRepository = GetComponent<AzureAnchorsReporitory>();

        cloudManager = GetComponent<SpatialAnchorManager>();

        // Register for Azure Spatial Anchor events
        cloudManager.AnchorLocated += CloudManager_AnchorLocated;

        anchorLocateCriteria = new AnchorLocateCriteria();

        StartCoroutine(
                runAfterFrame(async () =>
                {

                    await startAzureSession();
                    findAzureAnchor();
                }
            )
        );
    }

    // Update is called once per frame
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

    #region Private Methods

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
        Debug.Log("\nAnchorModuleScript.StartAzureSession()");

        // Notify AnchorFeedbackScript
        //OnStartASASession?.Invoke();

        Debug.Log("Starting Azure session... please wait...");

        if (cloudManager.Session == null)
        {
            // Creates a new session if one does not exist
            await cloudManager.CreateSessionAsync();
        }

        // Starts the session if not already started
        Task sessionStarter = cloudManager.StartSessionAsync();
        await sessionStarter;

        Debug.Log("Azure session started successfully");
    }



    #endregion

    #region public methods

    public void findAzureAnchor()
    {
        Debug.Log("\nAnchorModuleScript.FindAzureAnchor()");

        // Notify AnchorFeedbackScript
        // OnFindASAAnchor?.Invoke();

        // Set up list of anchor IDs to locate
        List<string> anchorsToFind = anchorsRepository.getAnchorsIdsToFind();

        foreach (var id in anchorsToFind)
        {
            Debug.Log($"\nAnchorToFind: {id}");
        }

        if (anchorsToFind.Count == 0)
        {
            Debug.Log("Current Azure anchor ID is empty");
            return;
        }

        anchorLocateCriteria.Identifiers = anchorsToFind.ToArray();
        Debug.Log($"Anchor locate criteria configured to look for Azure anchor with ID '{anchorsToFind.ToArray()}'");

        // Start watching for Anchors
        if ((cloudManager != null) && (cloudManager.Session != null))
        {
            currentWatcher = cloudManager.Session.CreateWatcher(anchorLocateCriteria);
            Debug.Log("Watcher created");
            Debug.Log("Looking for Azure anchor... please wait...");
        }
        else
        {
            Debug.Log("Attempt to create watcher failed, no session exists");
            currentWatcher = null;
        }
    }

    #endregion

    #region Event Handlers
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

                    GameObject newAnchor = Instantiate(mainGameObject);

                    TapToPlace tapToPlaceScript = newAnchor.GetComponent<TapToPlace>();
                    tapToPlaceScript.enabled = false;
                    tapToPlaceScript.AutoStart = false;

                    newAnchor.CreateNativeAnchor();
                    newAnchor.name = args.Identifier;

                    Pose anchorPose = Pose.identity;
                    anchorPose = args.Anchor.GetPose();
                    newAnchor.SetActive(true);

                    Debug.Log($"Setting object to anchor pose with position '{anchorPose.position}' and rotation '{anchorPose.rotation}' and name '{newAnchor.name}'");
                    newAnchor.transform.position = anchorPose.position;
                    newAnchor.transform.rotation = anchorPose.rotation;

                    newAnchor.CreateNativeAnchor();

                    anchorsRepository.addAnchor(
                        new AzureAnchorsReporitory.AnchorGameObject
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


    #endregion
}
