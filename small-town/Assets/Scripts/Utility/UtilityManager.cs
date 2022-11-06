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

}
