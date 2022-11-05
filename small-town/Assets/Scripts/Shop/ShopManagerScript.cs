using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManagerScript : MonoBehaviour
{

    [System.Serializable]
    public class ShopDisplayItems{

        public string itemName;

        [TextArea]
        public string itemDescription;

        public int itemQuantity;
        public int itemSellPrice;
        public ItemType itemType;
        public ConsumableEffect consumableEffect;
        public float consumableValue;

        [Header("Price")]
        public int price;

    }

    [Header("Shopkeeper Details")]
    public string shopkeeperName;

    [Header("UI")]
    public GameObject shopUiObject;
    public Transform shopDisplayContainer;
    public GameObject shopDisplayUiPrefab;

    [Header("Items")]
    public ShopDisplayItems[] shopDisplayItems;

    private void OnMouseDown() {

        for (int i = 0; i <= shopDisplayItems.Length - 1; i++) {

            GameObject obj = Instantiate(shopDisplayUiPrefab);
            obj.transform.SetParent(shopDisplayContainer);
            obj.transform.localScale = Vector2.one;

            Item item = obj.GetComponent<Item>();

            item.itemName = shopDisplayItems[i].itemName;
            item.description = shopDisplayItems[i].itemDescription;
            item.quantity = shopDisplayItems[i].itemQuantity;
            item.sellPrice = shopDisplayItems[i].itemSellPrice;
            item.itemType = shopDisplayItems[i].itemType;
            item.consumableEffect = shopDisplayItems[i].consumableEffect;
            item.consumableEffectValue = shopDisplayItems[i].consumableValue;

            obj.GetComponent<ItemShopDisplay>().price = shopDisplayItems[i].price;

        }

        shopUiObject.SetActive(true);

    }

}
