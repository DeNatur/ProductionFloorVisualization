using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using System;
using System.Threading.Tasks;
using UnityEngine;


public interface IStartAzureSession
{
    Task invoke();
}

public interface IAnchorRemover
{

    public void deleteNativeAnchor(GameObject anchorGameObject);

    public Task deleteCloudAnchor(string indetifier);
}

public class AzureCloudManager : IAnchorCreator, IAnchorRemover, IStartAzureSession
{
    static string ANCHOR_TYPE_PROP = "ANCHOR_TYPE";
    static string ANCHOR_SCALE_X = "ANCHOR_SCALE_X";
    static string ANCHOR_SCALE_Y = "ANCHOR_SCALE_Y";
    static string ANCHOR_SCALE_Z = "ANCHOR_SCALE_Z";

    readonly SpatialAnchorManager _cloudManager;

    public AzureCloudManager(SpatialAnchorManager cloudManager)
    {
        _cloudManager = cloudManager;
    }

    public async Task<IAnchorCreator.Result> createCloudAnchor(GameObject gameObject, int propIndex)
    {
        CloudSpatialAnchor localCloudAnchor = await getLocalAnchorWithObjectProperties(gameObject, propIndex);

        try
        {
            await _cloudManager.CreateAnchorAsync(localCloudAnchor);
            if (localCloudAnchor != null)
            {
                return new IAnchorCreator.Result.Success(localCloudAnchor.Identifier);
            }
            else
            {
                return new IAnchorCreator.Result.Failure();
            }
        }
        catch (Exception ex)
        {
            return new IAnchorCreator.Result.Failure(ex);
        }
    }

    public void createNativeAnchor(GameObject gameObject)
    {
        gameObject.CreateNativeAnchor();
    }

    private static async Task<CloudSpatialAnchor> getLocalAnchorWithObjectProperties(GameObject theObject, int index)
    {
        CloudSpatialAnchor localCloudAnchor = await getLocalAnchorFromGameObject(theObject);
        localCloudAnchor.AppProperties[ANCHOR_TYPE_PROP] = index.ToString();
        localCloudAnchor.AppProperties[ANCHOR_SCALE_X] = theObject.transform.localScale.x.ToString();
        localCloudAnchor.AppProperties[ANCHOR_SCALE_Y] = theObject.transform.localScale.y.ToString();
        localCloudAnchor.AppProperties[ANCHOR_SCALE_Z] = theObject.transform.localScale.z.ToString();
        return localCloudAnchor;
    }

    private static async Task<CloudSpatialAnchor> getLocalAnchorFromGameObject(GameObject gameObject)
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

    public async Task deleteCloudAnchor(string indetifier)
    {
        CloudSpatialAnchor localCloudAnchor = await _cloudManager.Session.GetAnchorPropertiesAsync(indetifier);
        await _cloudManager.DeleteAnchorAsync(localCloudAnchor);

    }
}
