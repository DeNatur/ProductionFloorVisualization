using UnityEngine;

public interface GameObjectEditor
{
    void setName(GameObject gameObject, string name);
    string getName(GameObject gameObject);
    void setPose(GameObject gameObject, Pose pose);
}


public class GameObjectEditorImpl : GameObjectEditor
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