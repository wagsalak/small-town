using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { MISC, COSMETICS, WEAPON, CONSUMABLE }
public enum ConsumableEffect { NONE, HEAL, HUNGER, WATER }

public class PlayerInventory : MonoBehaviour
{

    public static PlayerInventory InventoryInstance { get; private set; }

    public string inventoryContent;

    [Header("UI Display")]
    public Transform inventoryContentContainer;
    public GameObject itemDisplayPrefab;
    public List<GameObject> icons;

    private void Awake() {

        if (InventoryInstance == null) {

            InventoryInstance = this;

        } else {

            Destroy(this.gameObject);

        }

    }

    public void AddDummyItem() {

        AddItem("Dummy", "Lorem ipsum. this is a test text.", 1, 1, ItemType.CONSUMABLE, ConsumableEffect.HEAL, 120f);

    }

    private void Start() {

        inventoryContent = PlayerPrefs.GetString("inventory");

        if (inventoryContent == "") {

            AddItem("WoodenStick", "Part of a tree branch. Can be use in combat", 1, 1, ItemType.WEAPON, ConsumableEffect.NONE, 0f);
            inventoryContent = PlayerPrefs.GetString("inventory");

        } else {

            DisplayItem();

        }


    }

    public void AddItem( string name, string description , int quantity, int price, ItemType itemType, ConsumableEffect consumableEffect, float consumableEffectValue ) {

        string addingContent = name + "," + description + "," + quantity + "," + price + ","+ itemType + "," + consumableEffect + "," + consumableEffectValue + ";";
        PlayerPrefs.SetString("inventory", PlayerPrefs.GetString("inventory") + addingContent);
        inventoryContent = PlayerPrefs.GetString("inventory");

        DestroyDisplay();
        DisplayItem();

    }

    public void UseItem(Item item) {

        switch (item.itemType) {

            case ItemType.CONSUMABLE:

                switch (item.consumableEffect) {

                    case ConsumableEffect.HEAL:
                        GetComponent<PlayerStats>().health += item.consumableEffectValue;
                        break;

                    case ConsumableEffect.HUNGER:
                        GetComponent<PlayerStats>().hungerLevel += item.consumableEffectValue;
                        break;

                    case ConsumableEffect.WATER:
                        GetComponent<PlayerStats>().waterLevel += item.consumableEffectValue;
                        break;

                }

                int parse_n = (int)item.inventoryIndex;
                RemoveItem(parse_n);

                break;

            case ItemType.COSMETICS:



                break;

            case ItemType.WEAPON:



                break;
        }

    }

    public void RemoveItem( int index ) {
        
        string[] individualItems = inventoryContent.Split(char.Parse(";"));

        string finalReplacement = "";

        for (int i = 0; i <= individualItems.Length - 1; i++) {

            if (i != index)
                finalReplacement += individualItems[i] + ";";

        }

        for (int i = 0; i <= finalReplacement.Length - 1; i++) {

            if ( finalReplacement[i] == ';' ) {

                if ((i + 1) < finalReplacement.Length) {

                    if (finalReplacement[i + 1] == ';') {

                        finalReplacement = finalReplacement.Remove( i , 1 ); 
                    }

                }
                
            }

        }

        PlayerPrefs.SetString("inventory", finalReplacement);

        inventoryContent = PlayerPrefs.GetString("inventory");

        DestroyDisplay();

        DisplayItem();

    }

    public void DisplayItem() {

        /* PARAMETER
         * 0: NAME
         * 1: DESCRIPTION
         * 2: QUANTITY
         * 3: PRICE
         * 4: ITEM TYPE
         * 5: CONSUMABLE EFFECT
         * 6: COSUMABLE EFFECT VALUE
         * */

        if (inventoryContent == "") return;

        string[] individualItems = inventoryContent.Split(char.Parse(";"));
        
        for (int i = 0; i <= individualItems.Length - 1; i++) {

            string[] itemContent = individualItems[i].Split(char.Parse(","));

            if (individualItems[i] == "") break;

            GameObject obj = Instantiate(itemDisplayPrefab);
            obj.transform.SetParent(inventoryContentContainer);
            obj.transform.localPosition = Vector2.zero;
            obj.transform.localScale = Vector2.one;

            Item item = obj.GetComponent<Item>();

            item.itemName = itemContent[0];

            item.description = itemContent[1];

            item.quantityDisplay.text = itemContent[2];

            item.quantity = int.Parse(itemContent[2]);

            item.sellPrice = int.Parse(itemContent[3]);

            ItemType parsedItemType = (ItemType)System.Enum.Parse(typeof(ItemType), itemContent[4]);
            item.itemType = parsedItemType;

            ConsumableEffect parsedConsumableEffect = (ConsumableEffect)System.Enum.Parse(typeof(ConsumableEffect), itemContent[5]);
            item.consumableEffect = parsedConsumableEffect;

            item.consumableEffectValue = int.Parse(itemContent[6]);

            foreach (GameObject iconObject in icons) {

                if (iconObject.name == item.itemName) {

                    item.itemIconDisplay.sprite = iconObject.GetComponent<Image>().sprite;

                }

            } 

            item.inventoryIndex = i;

        }

    }

    private void DestroyDisplay() {

        for (int i = 0; i <= inventoryContentContainer.childCount - 1; i++) {

            Destroy(inventoryContentContainer.GetChild(i).gameObject);

        }

    }

}
