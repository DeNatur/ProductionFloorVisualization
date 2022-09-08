using UnityEngine;

public class AnchorScript : MonoBehaviour
{
    public GameObject buttons;

    private AzureSessionCoordinator sessionCoordinator;
    private AddAnchorUseCase addAnchorUseCase;
    // Start is called before the first frame update


    private void Awake()
    {
        sessionCoordinator = FindObjectOfType<AzureSessionCoordinator>();
        if (sessionCoordinator != null)
        {
            Debug.Log($"\nFound session coordinato {sessionCoordinator}");

        }
        addAnchorUseCase = FindObjectOfType<AddAnchorUseCase>();
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
        //sessionCoordinator.removeAzureAnchor(gameObject.name);
    }
}
