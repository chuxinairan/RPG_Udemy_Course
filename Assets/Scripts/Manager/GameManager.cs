using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    [SerializeField] private Checkpoint[] checkpoints;

    private Transform player;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    private float lostCurrencyX;
    private float lostCurrencyY;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        player = PlayerManager.instance.player.transform;
        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
    
    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }


    public void LoadData(GameData _gameData)
    {
        LoadLostCurrency(_gameData);

        foreach (KeyValuePair<string, bool> pair in _gameData.checkpoints)
        {
            foreach (Checkpoint cp in checkpoints)
            {
                if (cp.id == pair.Key && pair.Value == true)
                    cp.ActivateCheckpoint();
            }
        }

        foreach (Checkpoint cp in checkpoints)
        {
            if (cp.id == _gameData.closestCheckpointID)
               player.position = cp.transform.position;
        }
    }

    private void LoadLostCurrency(GameData _gameData)
    {
        lostCurrencyAmount = _gameData.lostCurrencyAmount;
        lostCurrencyX = _gameData.lostCurrencyX;
        lostCurrencyY = _gameData.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject lostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            lostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.lostCurrencyAmount = lostCurrencyAmount;
        _gameData.lostCurrencyX = player.position.x;
        _gameData.lostCurrencyY = player.position.y;

        _gameData.closestCheckpointID = FindClosestCheckpoint().id;
        _gameData.checkpoints.Clear();
        foreach(Checkpoint cp in checkpoints)
        {
            _gameData.checkpoints.Add(cp.id, cp.activated);
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        Checkpoint closestCheckpoint = null;
        float distanceToClosestCheckpoint = Mathf.Infinity;
        foreach(Checkpoint cp in checkpoints)
        {
            float distanceToCP = Vector2.Distance(player.position, cp.transform.position);
            if (distanceToCP < distanceToClosestCheckpoint && cp.activated == true)
            {
                distanceToClosestCheckpoint = distanceToCP;
                closestCheckpoint = cp;
            }
        }
        return closestCheckpoint;
    }
}
