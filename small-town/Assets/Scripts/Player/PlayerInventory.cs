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

    [HideInInspector] public bool isShopOpen;

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

    private void Start() {

        inventoryContent = PlayerPrefs.GetString("inventory");

        if (inventoryContent == "") {

            AddItem("Basic Clothing", "Comfy cothes.", 1, 1, ItemType.COSMETICS, ConsumableEffect.NONE, 0f);

            inventoryContent = PlayerPrefs.GetString("inventory");

        } else {

            DisplayItem();

        }


    }

    public void AddItem( string name, string description , int quantity, int price, ItemType itemType, ConsumableEffect consumableEffect, float consumableEffectValue ) {

        string addingContent = name + "," + description + "," + quantity + "," + price + ","+ itemType + "," + consumableEffect + "," + consumableEffectValue + ";";
        PlayerPrefs.SetString("inventory", PlayerPrefs.GetString("inventory") + addingContent);
        inventoryContent = PlayerPrefs.GetString("inventory");

        SimplePopUpManager.SPM_Instance.ShowPopUp("Obtained " + name + " x " + quantity + ".");

        DestroyDisplay();
        StartCoroutine(DelayDisplay()); // INCASE MESSED UP

    }

    public void UseItem(Item item) {

        if (isShopOpen) {

            switch (item.itemType)  {

                case ItemType.CONSUMABLE:

                    SimplePopUpManager.SPM_Instance.ShowPopUp("Succesfully sold " + item.itemName + " x 1."  + "\nObtained <color=yellow>" + (item.sellPrice) + "</color> coins.");
                    RemoveItem((int)item.inventoryIndex);

                    break;

                case ItemType.MISC:

                    SimplePopUpManager.SPM_Instance.ShowPopUp("Succesfully sold " + item.itemName + " x 1." + "\nObtained <color=yellow>" + (item.sellPrice) + "</color> coins.");
                    RemoveItem((int)item.inventoryIndex);

                    break;

                default:

                    SimplePopUpManager.SPM_Instance.ShowPopUp("Cannot sell this item.");

                    break;

            }
            return;

        }

        switch (item.itemType) {

            case ItemType.CONSUMABLE:

                switch (item.consumableEffect) {

                    case ConsumableEffect.HEAL:
                        GetComponent<PlayerStats>().health += item.consumableEffectValue;
                        if (GetComponent<PlayerStats>().health >= 100) GetComponent<PlayerStats>().health = 100;

                        SimplePopUpManager.SPM_Instance.ShowPopUp("Used " + item.itemName + " x 1.");

                        break;

                    case ConsumableEffect.HUNGER:
                        GetComponent<PlayerStats>().hungerLevel += item.consumableEffectValue;
                        if (GetComponent<PlayerStats>().hungerLevel >= 100) GetComponent<PlayerStats>().hungerLevel = 100;

                        SimplePopUpManager.SPM_Instance.ShowPopUp("Used " + item.itemName + " x 1.");
                        break;

                    case ConsumableEffect.WATER:
                        GetComponent<PlayerStats>().waterLevel += item.consumableEffectValue;
                        if (GetComponent<PlayerStats>().waterLevel >= 100) GetComponent<PlayerStats>().waterLevel = 100;

                        SimplePopUpManager.SPM_Instance.ShowPopUp("Used " + item.itemName + " x 1.");
                        break;

                }

                GetComponent<PlayerControlsManager>().Eat();
                int parse_n = (int)item.inventoryIndex;
                RemoveItem(parse_n);

                break;

            case ItemType.COSMETICS:

                UtilityManager.UtilityInstance.SetEquipedSkin( item.itemName );

                GetComponent<SkinsManager>().ChangeSkin( item.itemName );

                SimplePopUpManager.SPM_Instance.ShowPopUp("Equiped <color=yellow>" + UtilityManager.UtilityInstance.EquipedSkin() + "</color>.");

                break;

            case ItemType.WEAPON:

                UtilityManager.UtilityInstance.SetEquipedWeapon(item.itemName);

                SimplePopUpManager.SPM_Instance.ShowPopUp("Equiped <color=yellow>" + item.itemName + "</color>.");

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
        StartCoroutine(DelayDisplay()); // INCASE MESSED UP

    }

    IEnumerator DelayDisplay() { // INCASE MESSED UP

        yield return new WaitForSeconds(0.1f);
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

        bool isExisting = false; // INCASE MESSED UP

        string[] individualItems = inventoryContent.Split(char.Parse(";"));
        
        for (int i = 0; i <= individualItems.Length - 1; i++) {

            isExisting = false; // INCASE MESSED UP

            string[] itemContent = individualItems[i].Split(char.Parse(","));

            if (individualItems[i] == "") break;

            #region IN CASE I MESSED UP

            foreach (Transform child in inventoryContentContainer) {

                if (child.gameObject.GetComponent<Item>().itemName == itemContent[0]) {

                    child.gameObject.GetComponent<Item>().quantity += int.Parse(itemContent[2]);
                    child.gameObject.GetComponent<Item>().quantityDisplay.text = "" + child.gameObject.GetComponent<Item>().quantity;
                    isExisting = true;

                }

            }

            #endregion

            if (!isExisting) // INCASE MESSED UP
            {

                GameObject obj = Instantiate(itemDisplayPrefab);
                obj.transform.SetParent(inventoryContentContainer);
                obj.transform.localPosition = Vector2.zero;
                obj.transform.localScale = Vector2.one;

                Item item = obj.GetComponent<Item>();

                item.itemName = itemContent[0];

                item.description = itemContent[1];

                item.quantity = int.Parse(itemContent[2]);

                item.quantityDisplay.text = itemContent[2];

                item.sellPrice = int.Parse(itemContent[3]);

                ItemType parsedItemType = (ItemType)System.Enum.Parse(typeof(ItemType), itemContent[4]);
                item.itemType = parsedItemType;

                ConsumableEffect parsedConsumableEffect = (ConsumableEffect)System.Enum.Parse(typeof(ConsumableEffect), itemContent[5]);
                item.consumableEffect = parsedConsumableEffect;

                item.consumableEffectValue = int.Parse(itemContent[6]);

                foreach (GameObject iconObject in icons)
                {

                    if (iconObject.name == item.itemName)
                    {

                        item.itemIconDisplay.sprite = iconObject.GetComponent<Image>().sprite;

                    }

                }

                item.inventoryIndex = i;
            }

        }

    }

    private void DestroyDisplay() {

        for (int i = 0; i <= inventoryContentContainer.childCount - 1; i++) {

            Destroy(inventoryContentContainer.GetChild(i).gameObject);

        }

    }

    #region ITEM SORTING -- WILL CLEAN THIS IF I HAVE MORE TIME LOL.
    public void DisplayItemSorted(string itemType) {


        DestroyDisplay();

        if (itemType != "") {
            
            StartCoroutine(DelayDisplaySorted(itemType));

        } else {

            StartCoroutine(DelayDisplay());

        }

    }


    public void DisplayItemSortedMethod( string a ) {

        if (inventoryContent == "") return;

        bool isExisting = false; 

        string[] individualItems = inventoryContent.Split(char.Parse(";"));

        for (int i = 0; i <= individualItems.Length - 1; i++) {

            isExisting = false;

            string[] itemContent = individualItems[i].Split(char.Parse(","));

            if (individualItems[i] == "") break;

            print(itemContent[4] + " " + a);

            if (itemContent[4] != a) {

            } else {

                foreach (Transform child in inventoryContentContainer) {

                    if (child.gameObject.GetComponent<Item>().itemName == itemContent[0]) {

                        child.gameObject.GetComponent<Item>().quantity += int.Parse(itemContent[2]);
                        child.gameObject.GetComponent<Item>().quantityDisplay.text = "" + child.gameObject.GetComponent<Item>().quantity;
                        isExisting = true;


                    }

                }

                if (!isExisting) {

                    GameObject obj = Instantiate(itemDisplayPrefab);
                    obj.transform.SetParent(inventoryContentContainer);
                    obj.transform.localPosition = Vector2.zero;
                    obj.transform.localScale = Vector2.one;

                    Item item = obj.GetComponent<Item>();

                    item.itemName = itemContent[0];

                    item.description = itemContent[1];

                    item.quantity = int.Parse(itemContent[2]);

                    item.quantityDisplay.text = itemContent[2];

                    item.sellPrice = int.Parse(itemContent[3]);

                    ItemType parsedItemType = (ItemType)System.Enum.Parse(typeof(ItemType), itemContent[4]);
                    item.itemType = parsedItemType;

                    ConsumableEffect parsedConsumableEffect = (ConsumableEffect)System.Enum.Parse(typeof(ConsumableEffect), itemContent[5]);
                    item.consumableEffect = parsedConsumableEffect;

                    item.consumableEffectValue = int.Parse(itemContent[6]);

                    foreach (GameObject iconObject in icons)
                    {

                        if (iconObject.name == item.itemName)
                        {

                            item.itemIconDisplay.sprite = iconObject.GetComponent<Image>().sprite;

                        }

                    }

                    item.inventoryIndex = i;
                }
            }
        }

    }


    IEnumerator DelayDisplaySorted( string a ) { 

        yield return new WaitForSeconds(0.1f);
        DisplayItemSortedMethod(a);

    }

#endregion

}
