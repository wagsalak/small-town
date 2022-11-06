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


    [Header("Player")]
    public float playerMaxDistance;

    private GameObject player;
    private float distance;
    private bool isCliked;

    private void Start() {

        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Update() {

        CheckPlayerDisptance();

    }

    private void OnMouseDown() {

        isCliked = true;

    }

    private void CheckPlayerDisptance() {

        if (!isCliked) return;

        distance = Vector2.Distance(transform.position, player.transform.position);

        if (playerMaxDistance >= distance) {

            DisplayShopContent();
            player.GetComponent<PlayerControlsManager>().canControl = false;
            isCliked = false;

        }


    }

    private void DisplayShopContent() {

        for (int i = 0; i <= shopDisplayItems.Length - 1; i++) {

            GameObject obj = Instantiate(shopDisplayUiPrefab);
            obj.transform.SetParent(shopDisplayContainer);
            obj.transform.localScale = Vector2.one;
            obj.transform.localPosition = Vector2.one;

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


    private void OnDrawGizmos() {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerMaxDistance);

    }

}
