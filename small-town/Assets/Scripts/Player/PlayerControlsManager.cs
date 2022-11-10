using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class PlayerControlsManager : MonoBehaviour
{

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Animator playerAnim;

    public GameObject[] playerObjects;
    public Transform[] weaponFollowTargets;
    public GameObject[] damageCollider;

    public bool canControl;

    public Vector2 movePosition;

    bool isAttacking;
    bool isEating;

    string lastRotation;

    private RaycastHit2D hit;
    private Vector2 rayDirection;

    [Space(10)]
    public LayerMask obstacleLayer;

    [Space(10)]
    public NavMeshAgent navMesh;

    void Start() {

        movePosition = transform.position;

        canControl = true;

        lastRotation = "front";

        rayDirection = Vector2.down;

    }

    void Update() {

        transform.eulerAngles = Vector3.zero;

        if (!canControl) { 

            playerAnim.Play("idle_" + lastRotation);

            movePosition = transform.position;

            if( navMesh.enabled ) navMesh.SetDestination(movePosition);

            return; 

        }

        Move();
        Attack();

    }

    private void Move() {

        if ( isAttacking || isEating ) return;

        if (Input.GetMouseButtonDown(0)) {

            SoundManager.SoundManagerInstance.PlayClick();

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

            navMesh.SetDestination( movePosition );

            playerAnim.Play("walk_" + lastRotation);

        } else {

            playerAnim.Play("idle_" + lastRotation);

        }

    }

    private void Attack() {

        if ( Input.GetMouseButtonDown(1) && !isAttacking ) {

            if (PlayerStats.PlayerStatInstance.stamina <= 0) {

                SimplePopUpManager.SPM_Instance.ShowPopUp("You need some rest. Rest in your bed to acquire more Stamina.");
                return;

            }

            PlayerStats.PlayerStatInstance.stamina -= 0.5f;

            SoundManager.SoundManagerInstance.PlayAttack();

            movePosition = transform.position;

            navMesh.SetDestination(movePosition);

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

        movePosition = transform.position;

        navMesh.SetDestination(movePosition);

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

}
