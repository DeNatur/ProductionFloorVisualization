using System.Collections.Generic;
using UnityEngine;
public interface IAnchorsRepository
{

    public void addAnchor(AnchorGameObject anchor);

    public void removeAnchor(string id);

    public AnchorGameObject? getAnchor(string id);

    public List<string> getAnchorsIds();

    public struct AnchorGameObject
    {
        public GameObject gameObject;

        public string identifier;
    }
}
