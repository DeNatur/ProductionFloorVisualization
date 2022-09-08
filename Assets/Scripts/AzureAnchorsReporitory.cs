using Microsoft.Azure.SpatialAnchors;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


#if WINDOWS_UWP
using Windows.Storage;
#endif
public class AzureAnchorsReporitory : MonoBehaviour
{

    private Dictionary<CloudSpatialAnchor, GameObject> createdAnchors = new Dictionary<CloudSpatialAnchor, GameObject>();
    char[] charSeparators = new char[] { ';' };

    public void addAnchor(AnchorGameObject anchorGameObject)
    {
        createdAnchors.Add(anchorGameObject.anchor, anchorGameObject.gameObject);
        refreshDataOnDisk();
        Debug.Log("\nAdded anchor to repository");
    }

    public void removeAnchor(String id)
    {
        AnchorGameObject? data = getAnchorDataById(id);
        if (data != null)
        {
            createdAnchors.Remove(data?.anchor);
            refreshDataOnDisk();
            Debug.Log("\nDeleted anchor from repository");
        }
    }


    public AnchorGameObject? getAnchorDataById(String id)
    {
        foreach (KeyValuePair<CloudSpatialAnchor, GameObject> entry in createdAnchors)
        {
            if (entry.Key.Identifier.Equals(id))
            {
                return new AnchorGameObject
                {
                    anchor = entry.Key,
                    gameObject = entry.Value,
                };
            }
        }
        return null;
    }

    private void refreshDataOnDisk()
    {
        Debug.Log("\nRefreshDataToDisk()");

        string filename = "SavedAzureAnchorID.txt";
        string path = Application.persistentDataPath;

#if WINDOWS_UWP
        StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        path = storageFolder.Path.Replace('\\', '/') + "/";
#endif

        string filePath = Path.Combine(path, filename);
        string idsToSave = getIdsString();
        File.WriteAllText(filePath, idsToSave);

        Debug.Log($"Current Azure anchor IDs '{idsToSave}' successfully saved to path '{filePath}'");
    }

    public string getIdsString()
    {
        string idsToSave = "";
        foreach (KeyValuePair<CloudSpatialAnchor, GameObject> entry in createdAnchors)
        {
            idsToSave = entry.Key.Identifier + ";" + idsToSave;
        }
        return idsToSave;
    }

    public List<string> getAnchorsIdsToFind()
    {
        Debug.Log("\nAnchorModuleScript.LoadAzureAnchorIDFromDisk()");

        string filename = "SavedAzureAnchorID.txt";
        string path = Application.persistentDataPath;

#if WINDOWS_UWP
        StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        path = storageFolder.Path.Replace('\\', '/') + "/";
#endif

        string filePath = Path.Combine(path, filename);
        string idsFromFile = File.ReadAllText(filePath);
        List<string> anchorsToFind = new List<string>();
        string[] ids = idsFromFile.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
        foreach (var id in ids)
        {
            anchorsToFind.Add(id);
        }
        return anchorsToFind;
    }

    public struct AnchorGameObject
    {
        public GameObject gameObject;

        public CloudSpatialAnchor anchor;
    }
}
