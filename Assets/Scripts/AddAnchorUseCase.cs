using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AddAnchorUseCase : MonoBehaviour
{

    private AzureSessionCoordinator sessionCoordinator;
    private AzureAnchorsReporitory anchorsRepository;
    private SpatialAnchorManager cloudManager;

    private readonly Queue<Action> dispatchQueue = new Queue<Action>();

    private void Awake()
    {
        sessionCoordinator = GetComponent<AzureSessionCoordinator>();
        anchorsRepository = GetComponent<AzureAnchorsReporitory>();
        cloudManager = GetComponent<SpatialAnchorManager>();
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

    public async void createAzureAnchor(GameObject theObject)
    {
        AzureAnchorsReporitory.AnchorGameObject? data = anchorsRepository.getAnchorDataById(theObject.name);

        if (data != null)
        {
            Debug.Log("\nAnchor already created");
            return;
        }


        Debug.Log("\nAnchorModuleScript.CreateAzureAnchor()");


        // First we create a native XR anchor at the location of the object in question
        theObject.CreateNativeAnchor();


        await Task.Delay(1000);

        // Then we create a new local cloud anchor
        CloudSpatialAnchor localCloudAnchor = new CloudSpatialAnchor();

        // Now we set the local cloud anchor's position to the native XR anchor's position
        localCloudAnchor.LocalAnchor = await theObject.FindNativeAnchor().GetPointer();

        // Check to see if we got the local XR anchor pointer
        if (localCloudAnchor.LocalAnchor == IntPtr.Zero)
        {
            Debug.Log("Didn't get the local anchor...");
            return;
        }
        else
        {
            Debug.Log("Local anchor created");
        }

        // In this sample app we delete the cloud anchor explicitly, but here we show how to set an anchor to expire automatically
        localCloudAnchor.Expiration = DateTimeOffset.Now.AddDays(7);

        // Save anchor to cloud
        while (!cloudManager.IsReadyForCreate)
        {
            await Task.Delay(330);
            float createProgress = cloudManager.SessionStatus.RecommendedForCreateProgress;
            QueueOnUpdate(new Action(() => Debug.Log($"Move your device to capture more environment data: {createProgress:0%}")));
        }

        bool success;

        try
        {
            Debug.Log("Creating Azure anchor... please wait...");

            // Actually save
            await cloudManager.CreateAnchorAsync(localCloudAnchor);

            // Success?
            success = localCloudAnchor != null;

            if (success)
            {
                Debug.Log($"Azure anchor with ID '{localCloudAnchor.Identifier}' created successfully");
                theObject.name = localCloudAnchor.Identifier;

                // Update the current Azure anchor ID
                Debug.Log($"Current Azure anchor ID updated to '{localCloudAnchor.Identifier}'");
                anchorsRepository.addAnchor(
                    new AzureAnchorsReporitory.AnchorGameObject
                    {
                        anchor = localCloudAnchor,
                        gameObject = theObject,
                    }
                );
            }
            else
            {
                Debug.Log($"Failed to save cloud anchor with ID '{localCloudAnchor.Identifier}' to Azure");
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    private void QueueOnUpdate(Action updateAction)
    {
        lock (dispatchQueue)
        {
            dispatchQueue.Enqueue(updateAction);
        }
    }

}
