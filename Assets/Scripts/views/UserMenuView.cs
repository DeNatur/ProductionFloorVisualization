using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using Zenject;

public class UserMenuView : MonoBehaviour
{
    public Interactable[] machinesButtons;
    public Interactable enableBoundsButton;
    public Interactable disableBoundsButton;

    private UserMenuPresenter _presenter;

    [Inject]
    public void Construct(UserMenuPresenter presenter)
    {
        _presenter = presenter;
    }

    private void Start()
    {
        enableBoundsButton.OnClick.AddListener(() => _presenter.enableBoundsControl());
        disableBoundsButton.OnClick.AddListener(() => _presenter.disableBoundControls());
        for (int i = 0; i < machinesButtons.Length; i++)
        {
            int currentIndex = i;
            machinesButtons[i].OnClick.AddListener(() => _presenter.createNewMachine(currentIndex));
        }
    }
}
