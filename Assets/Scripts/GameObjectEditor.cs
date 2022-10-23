using UnityEngine;

public interface GameObjectEditor
{
    void setName(GameObject gameObject, string name);
    string getName(GameObject gameObject);
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
}