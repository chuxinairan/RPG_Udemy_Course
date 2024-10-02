using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;

    public SerializableDictionary<string, int> inventory;
    public SerializableDictionary<string, bool> skillTree;
    public List<string> equipmentId;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointID;

    public int lostCurrencyAmount;
    public float lostCurrencyX;
    public float lostCurrencyY;

    public SerializableDictionary<string, float> volumeSettings;

    public GameData()
    {
        currency = 1000;

        inventory = new SerializableDictionary<string, int>();
        skillTree = new SerializableDictionary<string, bool>();
        equipmentId = new List<string>();

        checkpoints = new SerializableDictionary<string, bool>();
        closestCheckpointID = string.Empty;

        lostCurrencyAmount = 0;
        lostCurrencyX = 0;
        lostCurrencyY = 0;

        volumeSettings = new SerializableDictionary<string, float>();
    }
}
