using UnityEngine;

public interface IGameObjectEditor
{
    void setName(GameObject gameObject, string name);
    string getName(GameObject gameObject);
    void setPose(GameObject gameObject, Pose pose);
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

    public void setPose(GameObject gameObject, Pose pose)
    {
        gameObject.transform.rotation = pose.rotation;
        gameObject.transform.position = pose.position;
    }
}