using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sp;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;
    private Transform myEnemy;
    private Blackhole_Skill_Controller myBlackhole;

    public void SetuoHotKey(KeyCode _myHotKey, Transform _myEnemy, Blackhole_Skill_Controller _myBlackhole)
    {
        sp = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myHotKey = _myHotKey;
        myText.text = myHotKey.ToString();
        myEnemy = _myEnemy;
        myBlackhole = _myBlackhole;
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            myBlackhole.AddEnemyToList(myEnemy);
            myText.color = Color.clear;
        }
    }
}
