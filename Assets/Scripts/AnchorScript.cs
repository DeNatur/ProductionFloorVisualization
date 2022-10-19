using Microsoft.MixedReality.Toolkit.UI.BoundsControl;
using UnityEngine;
using Zenject;

public class AnchorScript : MonoBehaviour
{
    public GameObject addAnchorButton;
    public GameObject removeAnchorButton;
    public GameObject tapToPlaceButton;
    public GameObject deleteButton;
    public int index;

    private AddAnchorUseCase addAnchorUseCase;

    private RemoveAnchorUseCase removeAnchorUseCase;

    [Inject]
    public void Construct(AddAnchorUseCase addAnchorUseCase, RemoveAnchorUseCase removeAnchorUseCase)
    {
        this.addAnchorUseCase = addAnchorUseCase;
        this.removeAnchorUseCase = removeAnchorUseCase;
    }

    private bool isAnchorCreated = false;
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
        if (addAnchorUseCase == null)
        {
            Debug.Log("NULLL");
        }
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

    private void Awake()
    {
        setAnchorNotCreatedState();
    }

    public class Factory : PlaceholderFactory<UnityEngine.Object, AnchorScript>
    {
    }
}
