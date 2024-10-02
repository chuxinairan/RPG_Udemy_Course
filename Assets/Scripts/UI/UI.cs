using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    public static UI instance;

    [Header("End region")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject youDeadText;
    [SerializeField] private GameObject restartButton;
    
    [Header("Main UI")]
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] public GameObject inGameUI;

    [Header("Main UI")]
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_SkillToolTip skillToolTip;
    public UI_CraftWindow craftWindow;

    [Header("Settings")]
    [SerializeField] private UI_VolumeSlier[] volumeSettings;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        SwitchTo(skillTreeUI);
    }

    private void Start()
    {
        SwitchTo(null);
        SwitchTo(inGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        skillToolTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchToWithKey(characterUI);
        if (Input.GetKeyDown(KeyCode.B))
            SwitchToWithKey(craftUI);
        if (Input.GetKeyDown(KeyCode.K))
            SwitchToWithKey(skillTreeUI);
        if (Input.GetKeyDown(KeyCode.O))
            SwitchToWithKey(optionsUI);
    }

    public void SwitchTo(GameObject _menu)
    {
        for(int i=0; i<transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
            if(!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu != null)
        {
            AudioManager.instance.PlaySFX(7, null);
            _menu.SetActive(true);
        }

        if (_menu == inGameUI)
            GameManager.instance.PauseGame(false);
        else
            GameManager.instance.PauseGame(true);
    }

    public void SwitchToWithKey(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            inGameUI.SetActive(true);
            GameManager.instance.PauseGame(false);
            return;
        }
        SwitchTo(_menu);
    }

    public void SwitchToYouDead()
    {
        StartCoroutine(YouDeadCoroutine(1));
    }

    private IEnumerator YouDeadCoroutine(float _second)
    {
        yield return new WaitForSeconds(_second);
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(1.0f);
        youDeadText.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        restartButton.SetActive(true);
    }

    public void LoadData(GameData _gameData)
    {
        foreach(KeyValuePair<string, float> pair in _gameData.volumeSettings)
        {
            foreach(UI_VolumeSlier slider in volumeSettings)
            {
                if (slider.parameter == pair.Key)
                    slider.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData _gameData)
    {
        _gameData.volumeSettings.Clear();
        foreach(UI_VolumeSlier slider in volumeSettings)
        {
            _gameData.volumeSettings.Add(slider.parameter, slider.slider.value);
        }
    }
}
