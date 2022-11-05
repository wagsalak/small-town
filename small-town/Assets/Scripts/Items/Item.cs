using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour {

    public ItemType itemType;

    public ConsumableEffect consumableEffect;

    public string itemName;

    [TextArea]
    public string description;

    public int quantity;

    public int sellPrice;

    public float consumableEffectValue;

    public float inventoryIndex;

    public TextMeshProUGUI quantityDisplay;

    public Image itemIconDisplay;

}
