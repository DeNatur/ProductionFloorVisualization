using Microsoft.Azure.SpatialAnchors;
using Microsoft.Azure.SpatialAnchors.Unity;
using System;
using UnityEngine;


public delegate void CloudAnchorLocated(object sender, AnchorLocator.CloudAnchorLocatedArgs args);
public interface AnchorLocator
{
    public event CloudAnchorLocated CloudAnchorLocated;

    public class CloudAnchorLocatedArgs : EventArgs
    {
        public Pose pose { get; }
        public int type { get; }
        public string identifier { get; }

        public CloudAnchorLocatedArgs(Pose pose, int type, string identifier)
        {
            this.pose = pose;
            this.type = type;
            this.identifier = identifier;
        }
    }

    public void startLocatingAzureAnchors(string[] idsToFind);
}
public class AzureAnchorLocator : AnchorLocator
{
    static string ANCHOR_TYPE_PROP = "ANCHOR_TYPE";

    readonly SpatialAnchorManager _cloudManager;

    public AzureAnchorLocator(SpatialAnchorManager cloudManager)
    {
        _cloudManager = cloudManager;
        _cloudManager.AnchorLocated += CloudManager_AnchorLocated;

    }

    public event CloudAnchorLocated CloudAnchorLocated;

    public void startLocatingAzureAnchors(string[] idsToFind)
    {
        AnchorLocateCriteria anchorLocateCriteria = new AnchorLocateCriteria();
        anchorLocateCriteria.Identifiers = idsToFind;
        _cloudManager.Session.CreateWatcher(anchorLocateCriteria);
    }

    private void CloudManager_AnchorLocated(object sender, AnchorLocatedEventArgs args)
    {
        if (args.Status == LocateAnchorStatus.Located && args.Anchor != null)
        {
            Debug.Log($"Azure anchor located successfully");
            AnchorLocator.CloudAnchorLocatedArgs anchorLocatedArgs =
                new AnchorLocator.CloudAnchorLocatedArgs(
                    pose: args.Anchor.GetPose(),
                    type: int.Parse(args.Anchor.AppProperties[ANCHOR_TYPE_PROP]),
                    identifier: args.Identifier
                );
            this.CloudAnchorLocated?.Invoke(this, anchorLocatedArgs);
        }
    }
}