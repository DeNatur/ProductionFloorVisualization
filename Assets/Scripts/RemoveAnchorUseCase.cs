using Microsoft.Azure.SpatialAnchors.Unity;
using UnityEngine;

public class RemoveAnchorUseCase
{
    private AnchorsRepository anchorsRepository;
    private SpatialAnchorManager cloudManager;

    public RemoveAnchorUseCase(AnchorsRepository anchorsRepository, SpatialAnchorManager spatialAnchorManager)
    {
        this.anchorsRepository = anchorsRepository;
        this.cloudManager = spatialAnchorManager;
    }

    public async void removeAzureAnchor(GameObject theObject)
    {
        string id = theObject.name;
        AnchorsRepository.AnchorGameObject? data = anchorsRepository.getAnchor(id);
        if (data == null)
        {
            Debug.Log("\nNo Anchor");
            return;
        }


        Debug.Log("\nAnchorModuleScript.RemoveAzureAnchor()");

        theObject.DeleteNativeAnchor();

        await cloudManager.DeleteAnchorAsync(data?.anchor);

        anchorsRepository.removeAnchor(id);

        Debug.Log("\nSuccessfully removed anchor");

    }
}
