using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUiManager : MonoBehaviour
{

    public Transform shopDisplayContainer;

    public void CloseShop() {

        for (int i = 0; i <= shopDisplayContainer.childCount - 1; i++){

            Destroy(shopDisplayContainer.GetChild(i).gameObject);

        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControlsManager>().canControl = true;

        PlayerInventory.InventoryInstance.isShopOpen = false;

        gameObject.SetActive(false);

    }
}
