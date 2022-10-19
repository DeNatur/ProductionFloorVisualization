using Microsoft.Azure.SpatialAnchors;
using System.Collections.Generic;
using UnityEngine;
public interface AnchorsRepository
{

    public void addAnchor(AnchorGameObject anchor);

    public void removeAnchor(string id);

    public AnchorGameObject? getAnchor(string id);

    public List<string> getAnchorsIds();

    public struct AnchorGameObject
    {
        public GameObject gameObject;

        public CloudSpatialAnchor anchor;
    }
}
