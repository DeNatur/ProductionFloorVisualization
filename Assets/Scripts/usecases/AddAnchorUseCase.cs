using System.Threading.Tasks;
using UnityEngine;

public interface IAddAnchorUseCase
{
    public Task<bool> createAzureAnchor(GameObject theObject, int index);
}

public class AddAnchorUseCase : IAddAnchorUseCase
{
    readonly IAnchorsRepository _anchorsRepository;
    readonly IAnchorCreator _saveAnchor;
    readonly IAwarnessValidator _sceneAwarnessValidator;
    readonly GameObjectEditor _gameObjectEditor;

    public AddAnchorUseCase(
        IAnchorsRepository anchorsRepository,
        IAnchorCreator saveAnchor,
        IAwarnessValidator sceneAwarnessValidator,
        GameObjectEditor gameObjectEditor
    )
    {
        _anchorsRepository = anchorsRepository;
        _saveAnchor = saveAnchor;
        _sceneAwarnessValidator = sceneAwarnessValidator;
        _gameObjectEditor = gameObjectEditor;
    }

    public async Task<bool> createAzureAnchor(GameObject theObject, int index)
    {

        if (checkAnchorAlreadyCreated(_gameObjectEditor.getName(theObject)))
        {
            return false;
        }

        bool success = await createAndSaveNewAnchor(theObject, index);
        return success;
    }

    private async Task<bool> createAndSaveNewAnchor(GameObject theObject, int index)
    {
        // First we create a native XR anchor at the location of the object in question
        _saveAnchor.createNativeAnchor(theObject);

        await _sceneAwarnessValidator.validateSceneReadiness();

        IAnchorCreator.Result result = await _saveAnchor.createCloudAnchor(theObject, index);
        if (result is IAnchorCreator.Result.Success)
        {
            saveNewAnchor(theObject, result as IAnchorCreator.Result.Success);
            return true;
        }
        else
        {
            if ((result as IAnchorCreator.Result.Failure).exception != null)
            {
                Debug.Log((result as IAnchorCreator.Result.Failure).exception);
            }
            return false;
        }
    }

    private void saveNewAnchor(GameObject theObject, IAnchorCreator.Result.Success result)
    {
        _gameObjectEditor.setName(theObject, result.anchorIdentifier);
        IAnchorsRepository.AnchorGameObject anchorToSave = new IAnchorsRepository.AnchorGameObject
        {
            identifier = result.anchorIdentifier,
            gameObject = theObject,
        };
        _anchorsRepository.addAnchor(anchorToSave);
    }

    private bool checkAnchorAlreadyCreated(string anchorName)
    {
        IAnchorsRepository.AnchorGameObject? data = _anchorsRepository.getAnchor(anchorName);

        if (data != null)
        {
            return true;
        }
        return false;
    }
}
