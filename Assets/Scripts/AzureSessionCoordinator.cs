using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class AzureSessionCoordinator : MonoBehaviour
{

    private ObjectCreator _objectsCreator;
    private AnchorsRepository _anchorsRepository;
    private AnchorLocator _anchorLocator;
    private StartAzureSession _startAzureSession;
    private AnchorCreator _saveAnchor;
    private GameObjectEditor _gameObjectEditor;

    public bool isStarted = false;

    [Inject]
    public void Construct(
        AnchorsRepository anchorsRepository,
        AnchorLocator anchorLocator,
        ObjectCreator objectsCreator,
        StartAzureSession startAzureSession,
        AnchorCreator saveAnchor,
        GameObjectEditor gameObjectEditor
    )
    {
        _anchorsRepository = anchorsRepository;
        _objectsCreator = objectsCreator;
        _anchorLocator = anchorLocator;
        _startAzureSession = startAzureSession;
        _saveAnchor = saveAnchor;
        _gameObjectEditor = gameObjectEditor;
    }

    public void Start()
    {
        _anchorLocator.CloudAnchorLocated += AnchorLocator_CloudAnchorLocated;
        StartCoroutine(
                runAfterFrame(async () =>
                {
                    await _startAzureSession.invoke();
                    findAzureAnchor();
                    isStarted = true;
                }
            )
        );
    }


    private void findAzureAnchor()
    {
        List<string> anchorsToFind = _anchorsRepository.getAnchorsIds();
        if (anchorsToFind.Count == 0)
        {
            return;
        }
        _anchorLocator.startLocatingAzureAnchors(anchorsToFind.ToArray());
    }

    private void AnchorLocator_CloudAnchorLocated(object sender, AnchorLocator.CloudAnchorLocatedArgs args)
    {
        GameObject newAnchor = _objectsCreator.createNewMachineWithGO(
            _objectsCreator.allMachines[args.type]
        );

        _gameObjectEditor.setName(newAnchor, args.identifier);

        Debug.Log($"Setting object to anchor pose with position '{args.pose.position}' and rotation '{args.pose.rotation}' and name '{newAnchor.name}'");
        _gameObjectEditor.setPose(newAnchor, args.pose);

        _saveAnchor.createNativeAnchor(newAnchor);

        _anchorsRepository.addAnchor(
            new AnchorsRepository.AnchorGameObject
            {
                identifier = args.identifier,
                gameObject = newAnchor,
            }
        );
    }

    private IEnumerator runAfterFrame(Action actionToInvoke)
    {
        yield return null;
        actionToInvoke();
    }
}
