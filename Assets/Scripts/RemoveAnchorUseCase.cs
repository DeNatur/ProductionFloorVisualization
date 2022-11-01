using System.Threading.Tasks;
using UnityEngine;

public class RemoveAnchorUseCase
{
    private readonly AnchorsRepository _anchorsRepository;
    private readonly AnchorRemover _anchorRemover;
    private readonly GameObjectEditor _gameObjectEditor;


    public RemoveAnchorUseCase(
        AnchorsRepository anchorsRepository,
        AnchorRemover anchorRemover,
        GameObjectEditor gameObjectEditor
        )
    {
        _anchorsRepository = anchorsRepository;
        _anchorRemover = anchorRemover;
        _gameObjectEditor = gameObjectEditor;
    }

    public async Task removeAzureAnchor(GameObject theObject)
    {
        string id = _gameObjectEditor.getName(theObject);
        AnchorsRepository.AnchorGameObject? data = _anchorsRepository.getAnchor(id);
        if (data == null)
        {
            Debug.Log("\nNo Anchor");
            return;
        }

        Debug.Log("\nRemoveAnchorUseCase.RemoveAzureAnchor()");
        _anchorRemover.deleteNativeAnchor(theObject);
        await _anchorRemover.deleteCloudAnchor(theObject);
        _anchorsRepository.removeAnchor(id);
        Debug.Log("\nSuccessfully removed anchor");
    }
}
