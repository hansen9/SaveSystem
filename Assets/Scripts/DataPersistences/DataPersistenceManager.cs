using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    public GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects = new List<IDataPersistence>();

    public static DataPersistenceManager instance { get; private set; }
    private FileDataHandler dataHandler;

    private string selectedProfileId = "";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("more than 1 data persistence manager instance found");
        }
        instance = this;
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void Start()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load(selectedProfileId);

        if(this.gameData == null)
        {
            Debug.Log("data not found. initialize new game");
            NewGame();
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        dataHandler.Save(gameData, selectedProfileId);
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    public void ChangeSelectedProfileId(string newProfileId, bool isLoading)
    {
        this.selectedProfileId = newProfileId;
        if (isLoading)
        {
            LoadGame();
        }
        else
        {
            SaveGame();
        }
        
    }
}
