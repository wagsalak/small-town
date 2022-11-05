using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControlsManager : MonoBehaviour
{

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Animator playerAnim;

    public GameObject[] playerObjects;
    public Transform[] weaponFollowTargets;
    public GameObject[] damageCollider;

    public bool canControl;

    Vector2 movePosition;

    bool isAttacking;

    string lastRotation;

    void Start() {

        canControl = true;
        lastRotation = "front";

    }

    void Update() {

        if (!canControl) return;

        Move();
        Attack();

    }

    private void Move() {

        if ( isAttacking ) return;

        if (Input.GetMouseButtonDown(0)) {

            if (IsPointerOverUIElement(GetEventSystemRaycastResults())) return;

            movePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            SetPlayerRotation(movePosition.x, movePosition.y);

            foreach (GameObject obj in playerObjects){

                if (obj.activeInHierarchy) {

                    lastRotation = obj.name.ToLower();

                }

            }

        }

        if ((Vector2)transform.position != movePosition) {

            transform.position = Vector2.MoveTowards(transform.position, movePosition, playerStats.moveSpeed * Time.deltaTime);

            playerAnim.Play("walk_" + lastRotation);

        } else {

            playerAnim.Play("idle_" + lastRotation);

        }
       

    }

    private void Attack() {

        if ( Input.GetMouseButtonDown(1) && !isAttacking ) {

            playerAnim.Play("attack_" + lastRotation);

            isAttacking = true;

            switch (lastRotation) {

                case "front":

                    damageCollider[0].SetActive(true);

                    break;

                case "back":

                    damageCollider[1].SetActive(true);

                    break;

                case "side":

                    if ( playerObjects[2].transform.localEulerAngles.y < 180 ){

                        damageCollider[2].SetActive(true);

                    } else {

                        damageCollider[3].SetActive(true);

                    }

                    break;

            }

            StartCoroutine(StopAttack());

        }

    }

    IEnumerator StopAttack() {

        yield return new WaitForSeconds(playerStats.attackTime);

        isAttacking = false;

        foreach (GameObject obj in damageCollider) {

            obj.SetActive(false);

        }

    }

    private void ChangePlayerDisplay(int index) {

        playerObjects[index].SetActive(true);

        for (int i = 0; i <= playerObjects.Length - 1; i++) {

            if ( i != index ) {

                playerObjects[i].SetActive(false);

            }

        }

    }

    private void SetPlayerRotation(float x, float y) {

        if ( Mathf.Abs(transform.position.x - x) > Mathf.Abs(transform.position.y - y) ) {

            if (transform.position.x > x)  { playerObjects[2].transform.eulerAngles = new Vector2(0f, 180f); } else { playerObjects[2].transform.eulerAngles = new Vector2(0f, 0f); }

            ChangePlayerDisplay(2);

        } else {

            if (transform.position.y < y) {

                ChangePlayerDisplay(1);

            } else {

                ChangePlayerDisplay(0);

            }

        }

    }

    public void SetPlayerControlCondition(bool val) {

        canControl = val;

    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults) {

        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if ( curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI") )
                return true;
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults() {

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;

    }

}
