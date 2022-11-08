using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static IAnchorsRepository;


#if WINDOWS_UWP
using Windows.Storage;
#endif
public class AzureAnchorsReporitory : IAnchorsRepository
{

    private Dictionary<string, GameObject> createdAnchors = new Dictionary<string, GameObject>();
    private char[] charSeparators = new char[] { ';' };

    public void addAnchor(AnchorGameObject anchorGameObject)
    {
        createdAnchors.Add(anchorGameObject.identifier, anchorGameObject.gameObject);
        refreshDataOnDisk();
        Debug.Log("\nAdded anchor to repository");
    }

    public void removeAnchor(String id)
    {
        AnchorGameObject? data = getAnchor(id);
        if (data != null)
        {
            createdAnchors.Remove(data?.identifier);
            refreshDataOnDisk();
            Debug.Log("\nDeleted anchor from repository");
        }
    }


    public AnchorGameObject? getAnchor(String id)
    {
        foreach (KeyValuePair<string, GameObject> entry in createdAnchors)
        {
            if (entry.Key.Equals(id))
            {
                return new AnchorGameObject
                {
                    identifier = entry.Key,
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
        foreach (KeyValuePair<string, GameObject> entry in createdAnchors)
        {
            idsToSave = entry.Key + ";" + idsToSave;
        }
        return idsToSave;
    }
}
