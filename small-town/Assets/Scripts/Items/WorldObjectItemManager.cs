using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectItemManager : MonoBehaviour
{

    Item item;

    private void Start() {

        item = GetComponent<Item>();

    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.tag == "Player") {

            PlayerInventory.InventoryInstance.AddItem(item.itemName, item.description, item.quantity, item.sellPrice, item.itemType, item.consumableEffect, item.consumableEffectValue);
            Destroy(gameObject);

        }

    }

}
