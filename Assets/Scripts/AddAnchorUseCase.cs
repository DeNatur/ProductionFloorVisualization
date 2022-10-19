using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class AddAnchorUseCase
{

    readonly AnchorsRepository _anchorsRepository;
    readonly SpatialAnchorManager _cloudManager;
    readonly SceneAwarnessValidator _sceneAwarnessValidator;

    public AddAnchorUseCase(
        AnchorsRepository anchorsRepository,
        SpatialAnchorManager cloudManager,
        SceneAwarnessValidator sceneAwarnessValidator
    )
    {
        _anchorsRepository = anchorsRepository;
        _cloudManager = cloudManager;
        _sceneAwarnessValidator = sceneAwarnessValidator;
    }

    public async Task<bool> createAzureAnchor(GameObject theObject, int index)
    {
        AnchorsRepository.AnchorGameObject? data = _anchorsRepository.getAnchor(theObject.name);

        if (data != null)
        {
            Debug.Log("\nAnchor already created");
            return true;
        }


        Debug.Log("\nAnchorModuleScript.CreateAzureAnchor()");


        // First we create a native XR anchor at the location of the object in question
        theObject.CreateNativeAnchor();


        await Task.Delay(1000);

        // Then we create a new local cloud anchor
        CloudSpatialAnchor localCloudAnchor = new CloudSpatialAnchor();

        // Now we set the local cloud anchor's position to the native XR anchor's position
        localCloudAnchor.LocalAnchor = await theObject.FindNativeAnchor().GetPointer();
        localCloudAnchor.AppProperties["ANCHOR_TYPE"] = index.ToString();

        // Check to see if we got the local XR anchor pointer
        if (localCloudAnchor.LocalAnchor == IntPtr.Zero)
        {
            Debug.Log("Didn't get the local anchor...");
            return false;
        }
        else
        {
            Debug.Log("Local anchor created");
        }

        // In this sample app we delete the cloud anchor explicitly, but here we show how to set an anchor to expire automatically
        localCloudAnchor.Expiration = DateTimeOffset.Now.AddDays(7);

        // Save anchor to cloud
        await _sceneAwarnessValidator.validateSceneReadiness();

        bool success = false;

        try
        {
            Debug.Log("Creating Azure anchor... please wait...");

            // Actually save
            await _cloudManager.CreateAnchorAsync(localCloudAnchor);

            // Success?
            success = localCloudAnchor != null;

            if (success)
            {
                Debug.Log($"Azure anchor with ID '{localCloudAnchor.Identifier}' created successfully");
                theObject.name = localCloudAnchor.Identifier;

                // Update the current Azure anchor ID
                Debug.Log($"Current Azure anchor ID updated to '{localCloudAnchor.Identifier}'");
                _anchorsRepository.addAnchor(
                    new AnchorsRepository.AnchorGameObject
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
        return success;
    }
}
