using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsLogicManager : MonoBehaviour
{

    public float hungerIncreaseRate;
    public float waterIncreaseRate;

    [Header("UI")]
    public Slider healthSlider;
    public Slider hungerSlider;
    public Slider waterSlider;
    public Slider staminaSlider;
    public TextMeshProUGUI moneyDisplayText;

    void Start() {


    }

    void Update() {

        Health();
        Hunger();
        Water();
        Stamina();

        moneyDisplayText.text = UtilityManager.UtilityInstance.Money().ToString("n0");

    }

    bool warningOnce;
    private void Health() {

        healthSlider.value = PlayerStats.PlayerStatInstance.health;
        UtilityManager.UtilityInstance.SetHP(healthSlider.value);

    }

    private void Stamina(){

        staminaSlider.value = PlayerStats.PlayerStatInstance.stamina;
        UtilityManager.UtilityInstance.SetStamina(staminaSlider.value);

    }

    private void Hunger() {

        if (PlayerStats.PlayerStatInstance.hungerLevel >= 0)  {

            PlayerStats.PlayerStatInstance.hungerLevel -= (hungerIncreaseRate * Time.deltaTime);
            hungerSlider.value = PlayerStats.PlayerStatInstance.hungerLevel;

            UtilityManager.UtilityInstance.SetHunger(hungerSlider.value);

        } else {

            if (PlayerStats.PlayerStatInstance.health >= 0)
                PlayerStats.PlayerStatInstance.health -= (hungerIncreaseRate * Time.deltaTime);

            if (!warningOnce) {

                SimplePopUpManager.SPM_Instance.ShowPopUp("Hunger level too low. Eat food to avoid dying from starvation.");
                StartCoroutine(AlertPlayerCooldown());
                warningOnce = true;

            }

        }

    }

    private void Water() {

        PlayerStats.PlayerStatInstance.waterLevel -= (waterIncreaseRate * Time.deltaTime);
        waterSlider.value = PlayerStats.PlayerStatInstance.waterLevel;

        UtilityManager.UtilityInstance.SetWater(waterSlider.value);

    }

    IEnumerator AlertPlayerCooldown() {

        yield return new WaitForSeconds(2f);
        warningOnce = false;

    }

}
