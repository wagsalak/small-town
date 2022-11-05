using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Item))]
public class ItemShopDisplay : MonoBehaviour
{

    Item item;
    public int price;

    void Start() {

        item = GetComponent<Item>();

        Display();

        GetComponent<Button>().onClick.AddListener(delegate { BuyItem(); });

    }

    private void Display() {

        item.quantityDisplay.text = "" + price;

        foreach (GameObject iconObject in PlayerInventory.InventoryInstance.icons) {

            if (iconObject.name == item.itemName) {

                item.itemIconDisplay.sprite = iconObject.GetComponent<Image>().sprite;

            }

        }

    }

    private void BuyItem() {

        if ( PlayerStats.PlayerStatInstance.money > price ) {

            PlayerInventory.InventoryInstance.AddItem(item.itemName, item.description, 1, item.sellPrice, item.itemType, item.consumableEffect, item.consumableEffectValue);

        }
        
    }

}
