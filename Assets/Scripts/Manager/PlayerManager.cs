using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    [SerializeField] public Player player;
    [SerializeField] public int currency;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public bool HaveEnoughMoney(int _cost)
    {
        if (currency < _cost)
        {
            Debug.Log("Has no enough money!");
            return false;
        }
        currency -= _cost;
        return true;
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.currency = this.currency;
    }

    public void LoadData(GameData _gameData)
    {
        this.currency = _gameData.currency;
    }
}
