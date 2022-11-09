using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGenericBehaviour : MonoBehaviour
{

    public NavMeshAgent navMesh;
    public float moveDistance;
    public float moveInterval;
    private float dummyInterval;

    private void Start() {

        dummyInterval = moveInterval;

    }

    void Update() {

        transform.eulerAngles = Vector3.zero;
        Move();

    }

    private void Move() {

        if (moveInterval > 0) {

            moveInterval -= Time.deltaTime;

        } else {

            float rand_posX = Random.Range(transform.position.x - moveDistance, transform.position.x + moveDistance);
            float rand_posY = Random.Range(transform.position.y - moveDistance, transform.position.y + moveDistance);

            Vector3 newDestination = new Vector3(rand_posX, rand_posY, 0f);

            navMesh.SetDestination(newDestination);

            if (transform.position == newDestination) {

                moveInterval = dummyInterval;

            }

        } 

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, moveDistance);
    }
}
