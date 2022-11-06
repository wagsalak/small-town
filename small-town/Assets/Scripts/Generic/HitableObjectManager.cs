using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableObjectManager : MonoBehaviour
{

    public Item item;
    public Sprite displaySprite;
    public GameObject itemDropPrefab;
    public float objectHitPoint;

    [Header("Animation")]
    public Animator anim;
    public string stateName;

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.tag == "Attack") {

            objectHitPoint -= 1f;
            anim.Play(stateName);

            if (objectHitPoint <= 0) {

                GameObject obj = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);

                foreach (Transform child in transform)   {

                    if (child.name == "Item") {

                        child.gameObject.GetComponent<SpriteRenderer>().sprite = displaySprite;

                    }

                }

                obj.GetComponent<Item>().itemName = item.itemName;
                obj.GetComponent<Item>().description = item.description;
                obj.GetComponent<Item>().quantity = item.quantity;
                obj.GetComponent<Item>().sellPrice = item.sellPrice;
                obj.GetComponent<Item>().itemType = item.itemType;
                obj.GetComponent<Item>().consumableEffect = item.consumableEffect;
                obj.GetComponent<Item>().consumableEffectValue = item.consumableEffectValue;

                Destroy(gameObject);

            }

        }

    }

}
