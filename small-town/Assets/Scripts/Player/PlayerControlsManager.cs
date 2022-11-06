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
    bool isEating;

    string lastRotation;

    private RaycastHit2D hit;
    private Vector2 rayDirection;

    [Space(10)]
    public LayerMask obstacleLayer;

    void Start() {

        canControl = true;
        lastRotation = "front";
        rayDirection = Vector2.down;

    }

    void Update() {

        if (!canControl) { 

            playerAnim.Play("idle_" + lastRotation);
            movePosition = transform.position;
            return; 

        }

        Move();
        Attack();

    }

    private void Move() {

        if ( isAttacking || isEating ) return;

        if (Input.GetMouseButtonDown(0)) {

            if (IsPointerOverUIElement(GetEventSystemRaycastResults())) return;

            movePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            SetPlayerRotation(movePosition.x, movePosition.y);

            foreach (GameObject obj in playerObjects){

                if (obj.activeInHierarchy) {

                    lastRotation = obj.name.ToLower();

                }

            }

            if (HasObstacle()) SimplePopUpManager.SPM_Instance.ShowPopUp("There's an obstacle in front of you. Try going another way.");
        }

        if ((Vector2)transform.position != movePosition) {

            SetRayCastDirection();

            if (HasObstacle()) {

                movePosition = transform.position;
                playerAnim.Play("idle_" + lastRotation);
                return;

            }

            transform.position = Vector2.MoveTowards(transform.position, movePosition, playerStats.moveSpeed * Time.deltaTime);

            playerAnim.Play("walk_" + lastRotation);

        } else {

            playerAnim.Play("idle_" + lastRotation);

        }
       

    }

    private void Attack() {

        if ( Input.GetMouseButtonDown(1) && !isAttacking ) {

            movePosition = transform.position;

            playerAnim.Play("attack_" + lastRotation);

            isAttacking = true;

            switch (lastRotation) {

                case "front":

                    damageCollider[0].SetActive(true);
                    InstanceWeapon(weaponFollowTargets[0], 1);

                    break;

                case "back":

                    damageCollider[1].SetActive(true);
                    InstanceWeapon(weaponFollowTargets[1], -1);

                    break;

                case "side":

                    if ( playerObjects[2].transform.localEulerAngles.y < 180 ){

                        damageCollider[2].SetActive(true);
                        InstanceWeapon(weaponFollowTargets[2], 1);

                    } else {

                        damageCollider[3].SetActive(true);
                        InstanceWeapon(weaponFollowTargets[2], 1);

                    }

                    break;

            }

            StartCoroutine(StopAttack());

        }

    }

    GameObject dummyWeapon;
    private void InstanceWeapon( Transform target, int layer ) {

        if (UtilityManager.UtilityInstance.EquipedWeapon() == "") return;

        GameObject weapon = Instantiate( Resources.Load("Weapons/" + UtilityManager.UtilityInstance.EquipedWeapon() ) as GameObject);
        weapon.transform.parent = target;
        weapon.transform.localPosition = Vector2.zero;
        weapon.transform.localScale = Vector2.one;
        weapon.transform.localEulerAngles = Vector2.zero;
        weapon.GetComponent<SpriteRenderer>().sortingOrder = layer;
        dummyWeapon = weapon;

    }

    IEnumerator StopAttack() {

        yield return new WaitForSeconds(playerStats.attackTime);

        isAttacking = false;

        foreach (GameObject obj in damageCollider) {

            obj.SetActive(false);

        }

        if (dummyWeapon != null) Destroy(dummyWeapon);

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

    public void Eat() {

        playerAnim.Play("eat_" + lastRotation);

        isEating = true;

        StartCoroutine( StopEating() );

    }

    IEnumerator StopEating() {

        yield return new WaitForSeconds(playerStats.eatTime);

        isEating = false;

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

    private bool HasObstacle() { 

        return Physics2D.Raycast(transform.position, rayDirection, 0.5f, obstacleLayer);

    }

    private void SetRayCastDirection() {

        switch (lastRotation) {

            case "front":

                rayDirection = Vector2.down;

                break;

            case "back":

                rayDirection = Vector2.up;

                break;

            case "side":

                if (playerObjects[2].transform.localEulerAngles.y < 180) {

                    rayDirection = Vector2.right;

                } else {

                    rayDirection = Vector2.left;

                }

                break;

        }

    }

}
