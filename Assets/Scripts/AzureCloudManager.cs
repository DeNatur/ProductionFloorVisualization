using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using System;
using System.Threading.Tasks;
using UnityEngine;


public interface StartAzureSession
{
    Task invoke();
}

public interface AnchorRemover
{

    public void deleteNativeAnchor(GameObject anchorGameObject);

    public Task deleteCloudAnchor(GameObject anchorGameObject);
}

public class AzureCloudManager : AnchorCreator, AnchorRemover, StartAzureSession
{
    static string ANCHOR_TYPE_PROP = "ANCHOR_TYPE";

    readonly SpatialAnchorManager _cloudManager;

    public AzureCloudManager(SpatialAnchorManager cloudManager)
    {
        _cloudManager = cloudManager;
    }

    public async Task<AnchorCreator.Result> createCloudAnchor(GameObject gameObject, int propIndex)
    {
        CloudSpatialAnchor localCloudAnchor = await getLocalAnchorWithObjectProperties(gameObject, propIndex);

        try
        {
            await _cloudManager.CreateAnchorAsync(localCloudAnchor);
            if (localCloudAnchor != null)
            {
                return new AnchorCreator.Result.Success(localCloudAnchor.Identifier);
            }
            else
            {
                return new AnchorCreator.Result.Failure();
            }
        }
        catch (Exception ex)
        {
            return new AnchorCreator.Result.Failure(ex);
        }
    }

    public void createNativeAnchor(GameObject gameObject)
    {
        gameObject.CreateNativeAnchor();
    }

    private static async Task<CloudSpatialAnchor> getLocalAnchorWithObjectProperties(GameObject theObject, int index)
    {
        CloudSpatialAnchor localCloudAnchor = await getLocalAnchorFromGamObject(theObject);
        localCloudAnchor.AppProperties[ANCHOR_TYPE_PROP] = index.ToString();
        return localCloudAnchor;
    }

    private static async Task<CloudSpatialAnchor> getLocalAnchorFromGamObject(GameObject gameObject)
    {
        CloudSpatialAnchor localCloudAnchor = new CloudSpatialAnchor();
        localCloudAnchor.LocalAnchor = await gameObject.FindNativeAnchor().GetPointer();
        return localCloudAnchor;
    }

    public async Task invoke()
    {
        Debug.Log("Starting Azure session... please wait...");
        if (_cloudManager.Session == null)
        {
            await _cloudManager.CreateSessionAsync();
        }
        await _cloudManager.StartSessionAsync();
        Debug.Log("Azure session started successfully");
    }

    public void deleteNativeAnchor(GameObject anchorGameObject)
    {
        anchorGameObject.DeleteNativeAnchor();
    }

    public async Task deleteCloudAnchor(GameObject anchorGameObject)
    {
        CloudSpatialAnchor localCloudAnchor = await getLocalAnchorFromGamObject(anchorGameObject);
        await _cloudManager.DeleteAnchorAsync(localCloudAnchor);
    }
}
