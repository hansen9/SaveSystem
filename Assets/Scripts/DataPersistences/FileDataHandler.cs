using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using SimpleDiskUtils;

public class FileDataHandler
{
    private string dataDirPath = "";

    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(string profileId)
    {
        string fullpath = Path.Combine(dataDirPath, profileId, dataFileName);
        
        GameData loadedData = null;

        if (File.Exists(fullpath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream fs = new FileStream(fullpath, FileMode.Open))
                {
                    using(StreamReader sr = new StreamReader(fs))
                    {
                        dataToLoad = sr.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullpath + @"\n" + ex);
            }            
        }

        return loadedData;
    }
    public void Save(GameData data, string profileId)
    {
        string fullpath = Path.Combine(dataDirPath, profileId, dataFileName);

        if (HasFreeSpace(data))
        {
            
        try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullpath));

                string dataToStore = JsonUtility.ToJson(data);

                using(FileStream fs = new FileStream(fullpath, FileMode.Create))
                {
                    using(StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(dataToStore);
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error occured when trying to save data to file: " + fullpath + "\n" + ex);
            }
        }
    }

    private bool HasFreeSpace(GameData data)
    {
        long estimatedFileSize = 0;
        try
        {
            byte[] levelBytes = BitConverter.GetBytes(data.level);

            using(MemoryStream ms = new MemoryStream())
            {
                ms.Write(levelBytes, 0, levelBytes.Length);
                estimatedFileSize = ms.Length;
            }
            
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error occured when trying to save data to file: " + ex);
        }
        bool isSpaceEnough = estimatedFileSize > DiskUtils.CheckAvailableSpace() ? false : true;

        return isSpaceEnough;
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            string fullpath = Path.Combine(dataDirPath, profileId, dataFileName);
            fullpath = fullpath.Replace(@"\", "/");

            if (!File.Exists(fullpath))
            {
                Debug.LogWarning("skipping directories, no data: " + profileId + " found");
                continue;                
            }

            GameData profileData = Load(profileId);

            if(profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("something went wrong, profileId: " + profileId);
            }
        }
        return profileDictionary;
    }
}