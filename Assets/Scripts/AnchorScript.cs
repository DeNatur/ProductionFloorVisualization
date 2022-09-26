using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using UnityEngine;

public class AnchorScript : MonoBehaviour
{
    public GameObject addAnchorButton;
    public GameObject removeAnchorButton;
    public GameObject tapToPlaceButton;
    public GameObject deleteButton;
    public int index;

    private AddAnchorUseCase addAnchorUseCase;
    private RemoveAnchorUseCase removeAnchorUseCase;
    // Start is called before the first frame update

    private bool isAnchorCreated = false;
    private void Awake()
    {
        addAnchorUseCase = FindObjectOfType<AddAnchorUseCase>();
        removeAnchorUseCase = FindObjectOfType<RemoveAnchorUseCase>();
        setAnchorNotCreatedState();
    }
    public void setAnchorCreatedState()
    {
        addAnchorButton.SetActive(false);
        tapToPlaceButton.SetActive(false);
        deleteButton.SetActive(false);
        removeAnchorButton.SetActive(true);
        isAnchorCreated = true;
    }

    public void setAnchorNotCreatedState()
    {
        addAnchorButton.SetActive(true);
        tapToPlaceButton.SetActive(true);
        deleteButton.SetActive(true);
        removeAnchorButton.SetActive(false);
        isAnchorCreated = false;
    }

    public async void addAnchor()
    {
        bool result = await addAnchorUseCase.createAzureAnchor(gameObject, index);
        if (result)
        {
            setAnchorCreatedState();
        }
    }

    public void removeAnchor()
    {
        removeAnchorUseCase.removeAzureAnchor(gameObject);
        setAnchorNotCreatedState();
    }

    public void deleteObject()
    {
        if (!isAnchorCreated)
        {
            Destroy(gameObject);
        }
    }

    public void enableBoundsControl()
    {
        if (!isAnchorCreated)
        {
            gameObject.GetComponent<BoundsControl>().enabled = true;
        }
    }

    public void disableBoundsControl()
    {
        if (!isAnchorCreated)
        {
            gameObject.GetComponent<BoundsControl>().enabled = false;
        }
    }
}
