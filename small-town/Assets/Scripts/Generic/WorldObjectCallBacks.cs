using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectCallBacks : MonoBehaviour
{

    public GameObject player;

    [Header("Bed")]
    public Transform bedPlayerPosition;
    private Vector3 onClickedBedPosition;

    bool isResting;
    public void Bed() {

        if (isResting) return;

        onClickedBedPosition = player.transform.position;

        player.GetComponent<PlayerControlsManager>().canControl = false;

        player.GetComponent<PlayerControlsManager>().navMesh.enabled = false;

        player.transform.position = bedPlayerPosition.position;

        isResting = true;

        StartCoroutine(DelayRest());

    }

    IEnumerator DelayRest() {

        yield return new WaitForSeconds(2f);

        PlayerStats.PlayerStatInstance.stamina = 100f;

        player.GetComponent<PlayerControlsManager>().canControl = true;

        player.GetComponent<PlayerControlsManager>().navMesh.enabled = true;

        player.GetComponent<PlayerControlsManager>().movePosition = onClickedBedPosition;

        SimplePopUpManager.SPM_Instance.ShowPopUp("Stamina restored.");

        isResting = false;

    }

    public void Well() {

        PlayerInventory.InventoryInstance.AddItem("Water Container", "Drink 3 times a day kids.Restore water level by 20 points.", 1, 0, ItemType.CONSUMABLE, ConsumableEffect.WATER, 20f);

    }

}
