using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool isEncrypt;
    private GameData gameData;
    private List<ISaveManager> saveManagerList;
    private FileDataHandler dataHandler;

    [ContextMenu("Delete saved file")]
    public void DeleteSavedFile()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, isEncrypt);
        dataHandler.Delete();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, isEncrypt);
        saveManagerList = FindAllSaveManager();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();
        if(this.gameData == null)
        {
            Debug.Log("No saved data found");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagerList)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach(ISaveManager saveManager in saveManagerList)
        {
            saveManager.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManager()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }

    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
            return true;

        return false;
    }
}
