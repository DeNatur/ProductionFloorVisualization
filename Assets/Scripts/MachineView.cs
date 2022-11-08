using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using UniRx;
using UnityEngine;
using Zenject;

public class MachineView : MonoBehaviour
{
    public Interactable addAnchorButton;
    public Interactable removeAnchorButton;
    public Interactable tapToPlaceButton;
    public Interactable deleteButton;
    private TapToPlace tapToPlaceScript;
    private MachinePresenter _anchorPresenter;

    public void Start()
    {
        tapToPlaceScript = GetComponent<TapToPlace>();
    }
    public void Initialize(MachinePresenter anchorPresenter)
    {
        _anchorPresenter = anchorPresenter;
        _anchorPresenter.state.Subscribe((state) => { bindState(state); }).AddTo(this);
        _anchorPresenter.deleteCurrentMachine += AnchorPresenter_DeleteObject;
        _anchorPresenter.disableTapToPlace += AnchorPresenter_DisableTapToPlace;

        addAnchorButton.OnClick.AddListener(() => _anchorPresenter.setAnchor(gameObject: gameObject));
        removeAnchorButton.OnClick.AddListener(() => _anchorPresenter.removeAnchor(gameObject: gameObject));
        deleteButton.OnClick.AddListener(() => _anchorPresenter.delete());
    }

    private void bindState(MachinePresenter.State state)
    {
        addAnchorButton.gameObject.SetActive(state.isAddAnchorVisible);
        removeAnchorButton.gameObject.SetActive(state.isRemoveAnchorVisible);
        tapToPlaceButton.gameObject.SetActive(state.isTapToPlaceVisible);
        deleteButton.gameObject.SetActive(state.isDeleteMachineVisible);
    }

    private void AnchorPresenter_DeleteObject()
    {
        Destroy(gameObject);
    }

    private void AnchorPresenter_DisableTapToPlace()
    {
        tapToPlaceScript.enabled = false;
        tapToPlaceScript.AutoStart = false;
    }

    public class Factory : PlaceholderFactory<UnityEngine.Object, MachinePresenter, MachineView>
    {
    }
}

public class MachineViewFactory : IFactory<UnityEngine.Object, MachinePresenter, MachineView>
{
    readonly DiContainer _container;

    public MachineViewFactory(DiContainer container)
    {
        _container = container;
    }

    public MachinePresenter Create(Object prefab)
    {
        return _container.InstantiatePrefabForComponent<MachinePresenter>(prefab);
    }

    public MachineView Create(Object prefab, MachinePresenter presenter)
    {
        MachineView view = _container.InstantiatePrefabForComponent<MachineView>(prefab);
        view.Initialize(presenter);
        return view;
    }
}
