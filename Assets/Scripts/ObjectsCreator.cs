using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using UnityEngine;
using Zenject;

public class ObjectsCreator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] allMachines;

    private AnchorScript.Factory _anchorObjectFactory;

    [Inject]
    public void Construct(AnchorScript.Factory anchorObjectFactory)
    {
        _anchorObjectFactory = anchorObjectFactory;
    }


    public void enableBoundsControl()
    {
        AnchorScript[] allObjectsWithAnchorScript = FindObjectsOfType<AnchorScript>();
        foreach (AnchorScript anchorScript in allObjectsWithAnchorScript)
        {
            anchorScript.enableBoundsControl();
        }
    }

    public void disableBoundsControl()
    {
        AnchorScript[] allObjectsWithAnchorScript = FindObjectsOfType<AnchorScript>();
        foreach (AnchorScript anchorScript in allObjectsWithAnchorScript)
        {
            anchorScript.disableBoundsControl();
        }
    }

    public void createNewMachine(GameObject obj)
    {
        _anchorObjectFactory.Create(obj);
    }

    public GameObject createNewMachineWithGO(GameObject obj)
    {
        AnchorScript newMachine = _anchorObjectFactory.Create(obj);

        newMachine.setAnchorCreatedState();
        TapToPlace tapToPlaceScript = newMachine.GetComponent<TapToPlace>();
        tapToPlaceScript.enabled = false;
        tapToPlaceScript.AutoStart = false;

        return newMachine.gameObject;
    }
}
