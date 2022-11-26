using System;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

public class MachinePresenter
{

    public class State
    {
        public bool isAddAnchorVisible = true;
        public bool isTapToPlaceVisible = true;
        public bool isDeleteMachineVisible = true;
        public bool isRemoveAnchorVisible = false;
        public bool areBoundControlsVisible = false;
        public MachineInfo? machineInfo = null;

        public State() { }
        public State(State state)
        {
            isAddAnchorVisible = state.isAddAnchorVisible;
            isTapToPlaceVisible = state.isTapToPlaceVisible;
            isDeleteMachineVisible = state.isDeleteMachineVisible;
            isRemoveAnchorVisible = state.isRemoveAnchorVisible;
            areBoundControlsVisible = state.areBoundControlsVisible;
            machineInfo = state.machineInfo;
        }

        public struct MachineInfo
        {
            public int hallId;
            public string name;
            public int efficiency;
            public string status;
            public string symbol;
            public DateTime technicalExaminationDate;
        }
    }
    public event Action disableTapToPlace;
    public event Action deleteCurrentMachine;


    public IReadOnlyReactiveProperty<State> state => _state;

    private readonly ReactiveProperty<State> _state = new ReactiveProperty<State>(new State());

    private bool isAnchorCreated = false;

    private CompositeDisposable disposables = new CompositeDisposable();

    readonly int _machineIndex;

    readonly IAddAnchorUseCase _addAnchorUseCase;

    readonly IRemoveAnchorUseCase _removeAnchorUseCase;

    readonly IBoundsControlVisibilityProvider _boundsControlProvider;

    readonly IMachineInfoRepository _machineInfoRepository;

    public MachinePresenter(
        int index,
        IAddAnchorUseCase addAnchorUseCase,
        IRemoveAnchorUseCase removeAnchorUseCase,
        IBoundsControlVisibilityProvider boundsControlProvider,
        IMachineInfoRepository machineInfoRepository
        )
    {
        _machineIndex = index;
        _addAnchorUseCase = addAnchorUseCase;
        _removeAnchorUseCase = removeAnchorUseCase;
        _boundsControlProvider = boundsControlProvider;
        _machineInfoRepository = machineInfoRepository;

        initializeReactiveProperties(index);
    }

    public void setAnchorCreatedState()
    {
        State newState = new State(_state.Value);
        newState.isAddAnchorVisible = false;
        newState.isTapToPlaceVisible = false;
        newState.isDeleteMachineVisible = false;
        newState.isRemoveAnchorVisible = true;
        newState.areBoundControlsVisible = false;
        _state.Value = newState;

        isAnchorCreated = true;
    }

    public void setAnchorNotCreatedState()
    {
        State newState = new State(_state.Value);
        newState.isAddAnchorVisible = true;
        newState.isTapToPlaceVisible = true;
        newState.isDeleteMachineVisible = true;
        newState.isRemoveAnchorVisible = false;
        newState.areBoundControlsVisible = _boundsControlProvider.isBoundsVisibilityEnabled.Value;

        _state.Value = newState;

        isAnchorCreated = false;
    }

    public async Task setAnchor(GameObject gameObject)
    {
        bool result = await _addAnchorUseCase.createAzureAnchor(gameObject, _machineIndex);

        if (result)
        {
            setAnchorCreatedState();
        }
    }

    public async Task removeAnchor(GameObject gameObject)
    {
        await _removeAnchorUseCase.removeAzureAnchor(gameObject);
        setAnchorNotCreatedState();
    }

    public void delete()
    {
        if (!isAnchorCreated)
        {
            deleteCurrentMachine.Invoke();
        }
    }

    public void setTapToPlaceNotStarted()
    {
        disableTapToPlace.Invoke();
    }

    public void onDestroy()
    {
        disposables.Clear();
    }

    public class Factory : PlaceholderFactory<int, MachinePresenter>
    {
    }

    private void initializeReactiveProperties(int index)
    {

        _boundsControlProvider.isBoundsVisibilityEnabled.Subscribe((areBoundsVisible) =>
        {
            State newState = new State(_state.Value);
            newState.areBoundControlsVisible = areBoundsVisible && !isAnchorCreated;
            _state.Value = newState;
        }).AddTo(disposables);

        _machineInfoRepository.getMachineInfo(index).Subscribe((machineInfo) =>
        {
            if (machineInfo != null)
            {
                State newState = new State(_state.Value);
                newState.machineInfo = new State.MachineInfo
                {
                    hallId = machineInfo.Value.hallId,
                    name = machineInfo.Value.name,
                    efficiency = machineInfo.Value.efficiency,
                    status = machineInfo.Value.status,
                    symbol = machineInfo.Value.symbol,
                    technicalExaminationDate = machineInfo.Value.technicalExaminationDate
                };
                _state.Value = newState;
            };
        }).AddTo(disposables);
    }

}