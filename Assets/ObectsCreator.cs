using UnityEngine;

public class ObectsCreator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject toCreate;
    // Update is called once per frame
    public void createNewGameObject()
    {
        Instantiate(toCreate);
    }
}
