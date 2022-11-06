using System;
using UniRx;
using UnityEngine;
using Zenject;

public class AnchorPresenter
{
    public IReadOnlyReactiveProperty<State> state => _state;

    private readonly ReactiveProperty<State> _state = new ReactiveProperty<State>();

    public AnchorPresenter(int index, AddAnchorUseCase addAnchorUseCase, RemoveAnchorUseCase removeAnchorUseCase, IBoundsControlProvider boundsControlProvider)
    {
        _machineIndex = index;
        _addAnchorUseCase = addAnchorUseCase;
        _removeAnchorUseCase = removeAnchorUseCase;
        _boundsControlProvider = boundsControlProvider;

        _boundsControlProvider.areBoundsEnabled.Subscribe((areBoundsEnabled) =>
        {
            State newState = new State(_state.Value);
            newState.areBoundControlsVisible = areBoundsEnabled;
            _state.Value = newState;
        });
    }

    private int _machineIndex;

    private AddAnchorUseCase _addAnchorUseCase;

    private RemoveAnchorUseCase _removeAnchorUseCase;

    private IBoundsControlProvider _boundsControlProvider;

    private bool isAnchorCreated = false;

    public event Action disableTapToPlace;
    public event Action deleteCurrentMachine;

    public void setAnchorCreatedState()
    {
        State newState = new State(_state.Value);
        newState.isAddAnchorVisible = false;
        newState.isTapToPlaceVisible = false;
        newState.isDeleteMachineVisible = false;
        newState.isRemoveAnchorVisible = true;
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
        _state.Value = newState;

        isAnchorCreated = false;
    }

    public async void setAnchor(GameObject gameObject)
    {
        if (_addAnchorUseCase == null)
        {
            Debug.Log("NULLL _addAnchorUseCase");
        }
        bool result = await _addAnchorUseCase.createAzureAnchor(gameObject, _machineIndex);
        if (result)
        {
            setAnchorCreatedState();
        }
    }

    public async void removeAnchor(GameObject gameObject)
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

    public class Factory : PlaceholderFactory<int, AnchorPresenter>
    {
    }

    public class State
    {
        public bool isAddAnchorVisible = true;
        public bool isTapToPlaceVisible = true;
        public bool isDeleteMachineVisible = true;
        public bool isRemoveAnchorVisible = false;
        public bool areBoundControlsVisible = false;

        public State(State state)
        {
            isAddAnchorVisible = state.isAddAnchorVisible;
            isTapToPlaceVisible = state.isTapToPlaceVisible;
            isDeleteMachineVisible = state.isDeleteMachineVisible;
            isRemoveAnchorVisible = state.isRemoveAnchorVisible;
            areBoundControlsVisible = state.areBoundControlsVisible;
        }
    }
}
