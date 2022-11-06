using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsLogicManager : MonoBehaviour
{

    public float hungerIncreaseRate;
    public float waterIncreaseRate;

    [Header("UI")]
    public Slider hungerSlider;
    public Slider waterSlider;

    void Start() {



    }

    void Update() {

        Hunger();
        Water();
    }

    private void Hunger() {

        PlayerStats.PlayerStatInstance.hungerLevel -= (hungerIncreaseRate * Time.deltaTime);
        hungerSlider.value = PlayerStats.PlayerStatInstance.hungerLevel;

    }

    private void Water() {

        PlayerStats.PlayerStatInstance.waterLevel -= (waterIncreaseRate * Time.deltaTime);
        waterSlider.value = PlayerStats.PlayerStatInstance.waterLevel;

    }

}
