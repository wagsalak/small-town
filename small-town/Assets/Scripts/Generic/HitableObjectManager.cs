using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableObjectManager : MonoBehaviour
{

    public string requiredTool; // Name of tool that can do damage.

    public Item item;
    public Sprite displaySprite;
    public GameObject itemDropPrefab;
    public float objectHitPoint;

    [Header("Effects")]
    public GameObject hitParticle;
    public float offSetX;
    public float offSetY;

    [Header("Animation")]
    public Animator anim;
    public string stateName;

    private void Update() {



    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (requiredTool == UtilityManager.UtilityInstance.EquipedWeapon()) {

            if (collision.tag == "Attack") {

                objectHitPoint -= 1f;
                anim.Play(stateName);

                Instantiate(hitParticle, new Vector3(transform.position.x + offSetX, transform.position.y + offSetY, 0f), Quaternion.identity);

                if (objectHitPoint <= 0) {

                    GameObject obj = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);

                    obj.transform.GetChild(0).transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = displaySprite;

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

        } else {

            SimplePopUpManager.SPM_Instance.ShowPopUp( requiredTool + " needed." );

        }

        

    }

}
