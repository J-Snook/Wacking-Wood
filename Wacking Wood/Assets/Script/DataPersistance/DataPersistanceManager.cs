using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class DataPersistanceManager : MonoBehaviour
{

    [Header("File Storage Config")] 
    
    [SerializeField] private string fileName;

    [SerializeField] private bool useEncryption;
    
    public GameData gameData;

    private List<IDataPersistance> dataPersistanceObjects;

    private FileDataHandler dataHandler;
    public static DataPersistanceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data persistance manager in scene. Destroying newest one");
            Destroy(this.gameObject);
            return;
        }

        
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
        
    }

    

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {

        this.gameData = dataHandler.Load();
        
        if (this.gameData == null)
        {
            Debug.Log("No Data was found. Initializing data to defaults.");
            NewGame();
        }

        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }
        Debug.Log("Loaded Time = " + gameData.days + " - " + gameData.hours + ":" + gameData.minutes);
        Debug.Log("Loaded Cash = " + gameData.cashAmount);
        Debug.Log("Loaded Stamina = " + gameData.stamina);
        Debug.Log("Loaded Fuel = " + gameData.fuel);
        Debug.Log("Loaded Seed = " + gameData.seed);
        Debug.Log("Loaded player pos = " + gameData.playerPosition + " and rot = " + gameData.playerRotation);
        Debug.Log("Loaded Camera rot = " + gameData.cameraRotation);
        Debug.Log("Loaded " + gameData.buildingStoredInfo.Count + " number of buildings");
        Debug.Log("Loaded " + gameData.treeInfomation.Count + " number of trees");
    }

    public void SaveGame()
    {
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceObjects)
        {
            dataPersistanceObj.SaveData(ref gameData);
        }
        Debug.Log("Saved Time = " + gameData.days + " - " + gameData.hours + ":" + gameData.minutes);
        Debug.Log("Saved Cash = " + gameData.cashAmount);
        Debug.Log("Saved Stamina = " + gameData.stamina);
        Debug.Log("Saved Fuel = " + gameData.fuel);
        Debug.Log("Saved Seed = " + gameData.seed);
        Debug.Log("Saved player pos = " + gameData.playerPosition + " and rot = " + gameData.playerRotation);
        Debug.Log("Saved Camera rot = " + gameData.cameraRotation);
        Debug.Log("Saved " + gameData.buildingStoredInfo.Count + " number of buildings");
        Debug.Log("Saved " + gameData.treeInfomation.Count + " number of trees");

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistancesObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistancesObjects);
    }
    
}
