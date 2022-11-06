using UnityEngine;
using Zenject;

public interface ObjectCreator
{
    public GameObject[] allMachines { get; set; }

    public abstract void createNewMachine(int machineIndex);

    public abstract GameObject createNewMachineWithGO(int machineIndex);
}

public class ObjectsCreatorImpl : MonoBehaviour, ObjectCreator
{
    // Start is called before the first frame update
    public GameObject[] allMachines;

    private AnchorPresenter.Factory _anchorPresenterFactory;
    private MachineView.Factory _machineViewFactory;

    GameObject[] ObjectCreator.allMachines { get => allMachines; set => allMachines = value; }

    [Inject]
    public void Construct(MachineView.Factory machineViewFactory, AnchorPresenter.Factory anchorPresenterFactory)
    {
        _anchorPresenterFactory = anchorPresenterFactory;
        _machineViewFactory = machineViewFactory;
    }

    public void createNewMachine(int machineIndex)
    {
        AnchorPresenter anchorPresenter = _anchorPresenterFactory.Create(machineIndex);
        _machineViewFactory.Create(allMachines[machineIndex], anchorPresenter);
    }

    public GameObject createNewMachineWithGO(int machineIndex)
    {
        AnchorPresenter anchorPresenter = _anchorPresenterFactory.Create(machineIndex);
        MachineView machineView = _machineViewFactory.Create(allMachines[machineIndex], anchorPresenter);

        anchorPresenter.setTapToPlaceNotStarted();

        return machineView.gameObject;
    }
}
