using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats PlayerStatInstance { get; private set; }

    public string weapon;
    public float health;
    public float hungerLevel;
    public float waterLevel;
    public float stamina;
    public float moveSpeed;
    public float attackTime;
    public float eatTime;
    public int money;

    private void Awake() {

        if (PlayerStatInstance == null) {

            PlayerStatInstance = this;

        } else {

            Destroy(this.gameObject);

        }

    }

}
