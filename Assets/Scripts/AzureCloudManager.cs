using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class AzureCloudManager : SaveAnchor
{

    readonly SpatialAnchorManager _cloudManager;

    public AzureCloudManager(SpatialAnchorManager cloudManager)
    {
        _cloudManager = cloudManager;
    }

    public async Task<SaveAnchor.Result> createCloudAnchor(GameObject gameObject, int propIndex)
    {
        CloudSpatialAnchor localCloudAnchor = await getLocalAnchorWithObjectProperties(gameObject, propIndex);

        await _cloudManager.CreateAnchorAsync(localCloudAnchor);
        try
        {
            await _cloudManager.CreateAnchorAsync(localCloudAnchor);
            if (localCloudAnchor != null)
            {
                return new SaveAnchor.Result.Success(localCloudAnchor.Identifier);
            }
            else
            {
                return new SaveAnchor.Result.Failure();
            }
        }
        catch (Exception ex)
        {
            return new SaveAnchor.Result.Failure(ex);
        }
    }

    public void createNativeAnchor(GameObject gameObject)
    {
        gameObject.CreateNativeAnchor();
    }

    private static async Task<CloudSpatialAnchor> getLocalAnchorWithObjectProperties(GameObject theObject, int index)
    {
        CloudSpatialAnchor localCloudAnchor = new CloudSpatialAnchor();
        localCloudAnchor.LocalAnchor = await theObject.FindNativeAnchor().GetPointer();
        localCloudAnchor.AppProperties["ANCHOR_TYPE"] = index.ToString();
        return localCloudAnchor;
    }

}
