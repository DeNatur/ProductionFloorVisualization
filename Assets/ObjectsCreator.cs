using UnityEngine;

public class ObjectsCreator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] allMachines;
    public void createNewObject(GameObject theObject)
    {
        Instantiate(theObject);
    }

    public void enableBoundsControl()
    {
        AnchorScript[] allObjectsWithAnchorScript = FindObjectsOfType<AnchorScript>();
        foreach (AnchorScript anchorScript in allObjectsWithAnchorScript)
        {
            anchorScript.enableBoundsControl();
        }
    }

    public void disableBoundsControl()
    {
        AnchorScript[] allObjectsWithAnchorScript = FindObjectsOfType<AnchorScript>();
        foreach (AnchorScript anchorScript in allObjectsWithAnchorScript)
        {
            anchorScript.disableBoundsControl();
        }
    }
}
