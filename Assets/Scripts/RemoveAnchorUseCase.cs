using Microsoft.Azure.SpatialAnchors.Unity;
using UnityEngine;

public class RemoveAnchorUseCase : MonoBehaviour
{
    private AzureAnchorsReporitory anchorsRepository;
    private SpatialAnchorManager cloudManager;

    private void Awake()
    {
        anchorsRepository = GetComponent<AzureAnchorsReporitory>();
        cloudManager = GetComponent<SpatialAnchorManager>();
    }

    public async void removeAzureAnchor(GameObject theObject)
    {
        string id = theObject.name;
        AzureAnchorsReporitory.AnchorGameObject? data = anchorsRepository.getAnchorDataById(id);
        if (data == null)
        {
            Debug.Log("\nNo Anchor");
            return;
        }


        Debug.Log("\nAnchorModuleScript.RemoveAzureAnchor()");

        theObject.DeleteNativeAnchor();

        await cloudManager.DeleteAnchorAsync(data?.anchor);

        anchorsRepository.removeAnchor(id);

        Destroy(theObject);

        Debug.Log("\nSuccessfully deleted anchor");

    }
}
