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
    private readonly IGameObjectEditor _gameObjectEditor;


    public RemoveAnchorUseCase(
        IAnchorsRepository anchorsRepository,
        IAnchorRemover anchorRemover,
        IGameObjectEditor gameObjectEditor
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

        await _anchorRemover.deleteCloudAnchor(id);
        _anchorRemover.deleteNativeAnchor(theObject);
        _anchorsRepository.removeAnchor(id);
    }
}
