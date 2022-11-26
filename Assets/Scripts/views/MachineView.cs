using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MachineView : MonoBehaviour
{
    public Interactable addAnchorButton;
    public Interactable removeAnchorButton;
    public Interactable tapToPlaceButton;
    private Text nameText;
    private Text symbolText;
    private Text statusText;
    private Text hallIdText;
    private Text efficiencyText;
    private Text technicalExaminationDateText;
    public Interactable deleteButton;
    private TapToPlace tapToPlaceScript;
    private MachinePresenter _anchorPresenter;
    private BoundsControl _boundsControl;

    public void initViews()
    {
        tapToPlaceScript = GetComponent<TapToPlace>();
        _boundsControl = GetComponent<BoundsControl>();
        nameText = transform.Find("MachineInfo/Name/NameText").GetComponent<Text>();
        symbolText = transform.Find("MachineInfo/Symbol/SymbolText").GetComponent<Text>();
        statusText = transform.Find("MachineInfo/Status/StatusText").GetComponent<Text>();
        hallIdText = transform.Find("MachineInfo/HallId/HallIdText").GetComponent<Text>();
        efficiencyText = transform.Find("MachineInfo/Efficiency/EfficiencyText").GetComponent<Text>();
        technicalExaminationDateText = transform.Find("MachineInfo/TechnicalExaminationDate/TechnicalExaminationDateText").GetComponent<Text>();
        addAnchorButton = transform.Find("Buttons/AddAnchor").GetComponent<Interactable>();
        removeAnchorButton = transform.Find("Buttons/RemoveAnchor").GetComponent<Interactable>();
        tapToPlaceButton = transform.Find("Buttons/TapToPlace").GetComponent<Interactable>();
        deleteButton = transform.Find("Buttons/Delete").GetComponent<Interactable>();
    }
    public void Initialize(MachinePresenter anchorPresenter)
    {
        initViews();
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
        _boundsControl.enabled = state.areBoundControlsVisible;
        if (state.machineInfo != null)
        {
            nameText.text = string.Format(
                "Machine Name: {0}",
                state.machineInfo.Value.name
                );
            symbolText.text = string.Format(
                "Symbol: {0}",
                state.machineInfo.Value.symbol
                );
            statusText.text = string.Format(
                "Status: {0}",
                state.machineInfo.Value.status
                );
            hallIdText.text = string.Format(
                "Hall ID: {0}",
                state.machineInfo.Value.hallId
                );
            efficiencyText.text = string.Format(
                "Efficiency: {0}",
                state.machineInfo.Value.efficiency
                );
            technicalExaminationDateText.text = string.Format(
                "Technical Examination Date: {0}",
                state.machineInfo.Value.technicalExaminationDate.ToString("yyyy//MM/dd HH:mm:ss")
                );
        }

    }

    public void OnDestroy()
    {
        _anchorPresenter.onDestroy();
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
