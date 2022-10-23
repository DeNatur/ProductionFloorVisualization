using System.Threading.Tasks;
using UnityEngine;

public class AddAnchorUseCase
{

    readonly AnchorsRepository _anchorsRepository;
    readonly SaveAnchor _saveAnchor;
    readonly AwarnessValidator _sceneAwarnessValidator;
    readonly GameObjectEditor _gameObjectEditor;

    public AddAnchorUseCase(
        AnchorsRepository anchorsRepository,
        SaveAnchor saveAnchor,
        AwarnessValidator sceneAwarnessValidator,
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

        SaveAnchor.Result result = await _saveAnchor.createCloudAnchor(theObject, index);
        if (result is SaveAnchor.Result.Success)
        {
            saveNewAnchor(theObject, result as SaveAnchor.Result.Success);
            return true;
        }
        else
        {
            if ((result as SaveAnchor.Result.Failure).exception != null)
            {
                Debug.Log((result as SaveAnchor.Result.Failure).exception);
            }
            return false;
        }
    }

    private void saveNewAnchor(GameObject theObject, SaveAnchor.Result.Success result)
    {
        _gameObjectEditor.setName(theObject, result.anchorIdentifier);
        AnchorsRepository.AnchorGameObject anchorToSave = new AnchorsRepository.AnchorGameObject
        {
            identifier = result.anchorIdentifier,
            gameObject = theObject,
        };
        _anchorsRepository.addAnchor(anchorToSave);
    }

    private bool checkAnchorAlreadyCreated(string anchorName)
    {
        AnchorsRepository.AnchorGameObject? data = _anchorsRepository.getAnchor(anchorName);

        if (data != null)
        {
            Debug.Log("\nAnchor already created");
            return true;
        }
        return false;
    }
}
