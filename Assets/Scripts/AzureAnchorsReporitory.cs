using Microsoft.Azure.SpatialAnchors;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static AnchorsRepository;


#if WINDOWS_UWP
using Windows.Storage;
#endif
public class AzureAnchorsReporitory : AnchorsRepository
{

    private Dictionary<CloudSpatialAnchor, GameObject> createdAnchors = new Dictionary<CloudSpatialAnchor, GameObject>();
    private char[] charSeparators = new char[] { ';' };

    public void addAnchor(AnchorGameObject anchorGameObject)
    {
        createdAnchors.Add(anchorGameObject.anchor, anchorGameObject.gameObject);
        refreshDataOnDisk();
        Debug.Log("\nAdded anchor to repository");
    }

    public void removeAnchor(String id)
    {
        AnchorGameObject? data = getAnchor(id);
        if (data != null)
        {
            createdAnchors.Remove(data?.anchor);
            refreshDataOnDisk();
            Debug.Log("\nDeleted anchor from repository");
        }
    }


    public AnchorGameObject? getAnchor(String id)
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

    public List<string> getAnchorsIds()
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

    private string getIdsString()
    {
        string idsToSave = "";
        foreach (KeyValuePair<CloudSpatialAnchor, GameObject> entry in createdAnchors)
        {
            idsToSave = entry.Key.Identifier + ";" + idsToSave;
        }
        return idsToSave;
    }
}
