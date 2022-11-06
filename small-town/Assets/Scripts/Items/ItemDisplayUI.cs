using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDisplayUI : MonoBehaviour
{

    public Item item;

    private void Start() {

        GetComponent<Button>().onClick.AddListener(delegate { FindObjectOfType<PlayerInventory>().UseItem(item); });

    }

    private void Update() {

        if (item.itemType == ItemType.WEAPON) {

            if (UtilityManager.UtilityInstance.EquipedWeapon() == item.itemName) {

                item.quantityDisplay.text = "E";

            } else {

                item.quantityDisplay.text = "";

            }

        }

    }

}
