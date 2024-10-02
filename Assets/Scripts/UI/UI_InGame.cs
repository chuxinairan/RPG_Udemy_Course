using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InGame : MonoBehaviour
{
    [Header("Health status")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private PlayerStats playerStats;

    [Header("Currency Soul")]
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private int soulAmount;
    [SerializeField] private int soulIncreaceRate;

    [Header("Skill cooldowns")]
    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;
    //[SerializeField] private Image armorImage;

    private SkillManager skill;
    void Start()
    {
        playerStats.onHealthChanged += UpdateHealthUI;
        UpdateHealthUI();
        skill = SkillManager.instance;
    }

    private void Update()
    {
        UpdateSoulsUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && skill.dashSkill.dashUnlocked && dashImage.fillAmount <= 0)
            dashImage.fillAmount = 1;
        if (Input.GetKeyDown(KeyCode.Mouse1) && skill.parrySkill.parryUnlocked && parryImage.fillAmount <= 0 && PlayerManager.instance.player.IsGroundDetected())
            parryImage.fillAmount = 1;
        if (Input.GetKeyDown(KeyCode.F) && skill.crystalSkill.crystalUnlocked && crystalImage.fillAmount <= 0)
            crystalImage.fillAmount = 1;
        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmengType.Flask) != null && flaskImage.fillAmount <= 0)
            flaskImage.fillAmount = 1;

        CheckCooldownOf(dashImage, skill.dashSkill.cooldown);
        CheckCooldownOf(parryImage, skill.parrySkill.cooldown);
        CheckCooldownOf(crystalImage, skill.crystalSkill.cooldown);
        CheckCooldownOf(swordImage, skill.swordSkill.cooldown);
        CheckCooldownOf(blackholeImage, skill.blackholeSkill.cooldown);
        CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    private void UpdateSoulsUI()
    {
        int newSoulAmount = (int)(soulIncreaceRate * Time.deltaTime) + soulAmount;
        if (newSoulAmount < PlayerManager.instance.currency)
            soulAmount = newSoulAmount;
        else
            soulAmount = PlayerManager.instance.currency;

        currencyText.text = soulAmount.ToString();
    }

    private void UpdateHealthUI()
    {
        healthSlider.maxValue = playerStats.GetMaxHealthValue();
        healthSlider.value = playerStats.currentHealth;
    }

    public void SetCooldownOf(Image _image)
    {
        _image.fillAmount = 1;
    }

    public void SetSwordCooldown()
    {
        swordImage.fillAmount = 1;
    }

    public void SetBlackholeCooldown()
    {
        blackholeImage.fillAmount = 1;
    }

    public void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}
