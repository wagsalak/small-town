using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectInteractDetection : MonoBehaviour
{

    public enum WorldObjectType { NONE,BED, CHAIR, TABLE, WELL }

    public DescriptionMouseFollow dmf;

    [TextArea]
    public string tempDetails;

    [Header("Player")]
    public float playerMaxDistance;

    public GameObject player;
    private float distance;

    [Header("Type")]
    public WorldObjectType objectType;

    private void Update() {

        CheckPlayerDisptance();

    }

    private void OnMouseDown() {

        if (playerMaxDistance >= distance)
        {

            switch (objectType) {

                case WorldObjectType.BED:
                    GetComponent<WorldObjectCallBacks>().Bed();
                    break;

                case WorldObjectType.WELL:
                    GetComponent<WorldObjectCallBacks>().Well();
                    break;

                default:
                    SimplePopUpManager.SPM_Instance.ShowPopUp("Nothing happened.");
                    break;

            }

        }

    }

    private void OnMouseOver() {

        if (playerMaxDistance >= distance) {

            dmf.hoveringOther = true;

            dmf.description.text = tempDetails;

        }

    }

    private void OnMouseExit() {

        dmf.hoveringOther = false;

    }

    private void CheckPlayerDisptance() {

        distance = Vector2.Distance(transform.position, player.transform.position);

    }

    private void OnDrawGizmos() {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, playerMaxDistance);

    }

    private void OnDestroy() {

        dmf.hoveringOther = false;

    }

}
