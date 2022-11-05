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

}
