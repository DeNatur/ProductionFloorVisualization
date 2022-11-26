using UnityEngine;
using Zenject;

public interface IObjectsCreator
{
    public GameObject[] allMachines { get; set; }

    public abstract void createNewMachine(int machineIndex);

    public abstract GameObject createNewMachineWithGO(int machineIndex);
}

public class ObjectsCreatorImpl : MonoBehaviour, IObjectsCreator
{
    // Start is called before the first frame update
    public GameObject[] allMachines;

    private MachinePresenter.Factory _anchorPresenterFactory;
    private MachineView.Factory _machineViewFactory;

    GameObject[] IObjectsCreator.allMachines { get => allMachines; set => allMachines = value; }

    [Inject]
    public void Construct(MachineView.Factory machineViewFactory, MachinePresenter.Factory anchorPresenterFactory)
    {
        _anchorPresenterFactory = anchorPresenterFactory;
        _machineViewFactory = machineViewFactory;
    }

    public void createNewMachine(int machineIndex)
    {
        MachinePresenter anchorPresenter = _anchorPresenterFactory.Create(machineIndex);
        _machineViewFactory.Create(allMachines[machineIndex], anchorPresenter);
    }

    public GameObject createNewMachineWithGO(int machineIndex)
    {
        MachinePresenter anchorPresenter = _anchorPresenterFactory.Create(machineIndex);
        MachineView machineView = _machineViewFactory.Create(allMachines[machineIndex], anchorPresenter);

        anchorPresenter.setTapToPlaceNotStarted();
        anchorPresenter.setAnchorCreatedState();

        return machineView.gameObject;
    }
}
