using UnityEngine;

public class AnchorScript : MonoBehaviour
{
    public GameObject buttons;

    private AzureSessionCoordinator sessionCoordinator;
    private AddAnchorUseCase addAnchorUseCase;
    private RemoveAnchorUseCase removeAnchorUseCase;
    // Start is called before the first frame update


    private void Awake()
    {
        sessionCoordinator = FindObjectOfType<AzureSessionCoordinator>();
        addAnchorUseCase = FindObjectOfType<AddAnchorUseCase>();
        removeAnchorUseCase = FindObjectOfType<RemoveAnchorUseCase>();
    }
    public void toggleVisibilityOfbuttons()
    {
        buttons.SetActive(!buttons.activeSelf);
    }

    public void addAnchor()
    {
        addAnchorUseCase.createAzureAnchor(gameObject);
    }

    public void removeAnchor()
    {
        removeAnchorUseCase.removeAzureAnchor(gameObject);
    }
}
