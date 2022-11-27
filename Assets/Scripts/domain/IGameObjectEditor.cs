using UnityEngine;

public interface IGameObjectEditor
{
    void setName(GameObject gameObject, string name);
    string getName(GameObject gameObject);
    void setPose(GameObject gameObject, Pose pose, Vector3 scale);
}


public class GameObjectEditorImpl : IGameObjectEditor
{
    public string getName(GameObject gameObject)
    {
        return gameObject.name;
    }

    public void setName(GameObject gameObject, string name)
    {
        gameObject.name = name;
    }

    public void setPose(GameObject gameObject, Pose pose, Vector3 scale)
    {
        gameObject.transform.rotation = pose.rotation;
        gameObject.transform.position = pose.position;
        gameObject.transform.localScale = scale;
    }
}