using System.Threading.Tasks;
using UnityEngine;

public interface IRemoveAnchorUseCase
{
    public Task removeAzureAnchor(GameObject theObject);
}


public class RemoveAnchorUseCase : IRemoveAnchorUseCase
{
    private readonly IAnchorsRepository _anchorsRepository;
    private readonly IAnchorRemover _anchorRemover;
    private readonly GameObjectEditor _gameObjectEditor;


    public RemoveAnchorUseCase(
        IAnchorsRepository anchorsRepository,
        IAnchorRemover anchorRemover,
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
        IAnchorsRepository.AnchorGameObject? data = _anchorsRepository.getAnchor(id);
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
