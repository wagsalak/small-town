using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityManager : MonoBehaviour
{

    public static UtilityManager UtilityInstance { get; private set; }

    public float debugVersion;

    private void Awake() {

        if (UtilityInstance == null)   {

            UtilityInstance = this;

        }  else {

            Destroy(this.gameObject);

        }

        HandlePlayerPrefs();

        InitializeStats();

        if (EquipedSkin() == "") { SetEquipedSkin("Basic Clothing"); }

    }

    private void Update() {

        //CHEAT CODE LOL -
        if (Input.GetKey("p")) {

            SetMoney( Money() + 5000 );

        }

    }

    private void HandlePlayerPrefs() {

        if (PlayerPrefs.GetString("build_demo_" + debugVersion) == "") {

            PlayerPrefs.SetString("inventory", "");

            PlayerPrefs.SetString("equiped_weapon", "");

            PlayerPrefs.SetString("build_demo_" + debugVersion, "done");

        }

    }

    public void SetEquipedWeapon( string weaponName ) {

        PlayerPrefs.SetString("equiped_weapon", weaponName);

    }

    public string EquipedWeapon() { 
        
        return PlayerPrefs.GetString("equiped_weapon");

    }

    public void SetEquipedSkin(string skinName) {

        PlayerPrefs.SetString("equiped_skin", skinName);

    }

    public string EquipedSkin() {

        return PlayerPrefs.GetString("equiped_skin");

    }

    public void SetHP( float hp ) {

        PlayerPrefs.SetFloat("player_health", hp);

    }

    public float Health() {

        return PlayerPrefs.GetFloat("player_health");

    }

    public void SetHunger( float hunger ) {

        PlayerPrefs.SetFloat("player_hunger", hunger);

    }

    public float Hunger() {

        return PlayerPrefs.GetFloat("player_hunger");

    }

    public void SetWater(float water) {

        PlayerPrefs.SetFloat("player_water", water);

    }

    public float Water() {

        return PlayerPrefs.GetFloat("player_water");

    }

    public void SetStamina(float stamina) {

        PlayerPrefs.SetFloat("player_stamina", stamina);

    }

    public float Stamina() {

        return PlayerPrefs.GetFloat("player_stamina");

    }

    public void SetMoney(int money) { 

        PlayerPrefs.SetInt("player_money", money);

    }

    public int Money() {

        return PlayerPrefs.GetInt("player_money");

    }

    private void InitializeStats() {

        if (PlayerPrefs.GetString("stats_init") == "") {

            SetHP(100f);
            SetHunger(100f);
            SetWater(100f);
            SetStamina(100f);
            SetMoney(1000);

            PlayerPrefs.SetString("stats_init", "DONE");

        }

        PlayerStats.PlayerStatInstance.health = UtilityInstance.Health();
        PlayerStats.PlayerStatInstance.hungerLevel = UtilityInstance.Hunger();
        PlayerStats.PlayerStatInstance.waterLevel = UtilityInstance.Water();
        PlayerStats.PlayerStatInstance.stamina = UtilityInstance.Stamina();
        PlayerStats.PlayerStatInstance.money = UtilityInstance.Money();

    }

}
