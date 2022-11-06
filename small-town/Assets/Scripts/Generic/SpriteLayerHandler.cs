using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpriteLayerHandler : MonoBehaviour
{

    public Transform playerTransform;

    void Update() {

        CheckPlayer();

    }

    private void CheckPlayer() {

        if (playerTransform.position.y > transform.position.y) {

            GetComponent<SortingGroup>().sortingOrder = 1;

        }  else {

            GetComponent<SortingGroup>().sortingOrder = -1;

        }
    
    }

}
