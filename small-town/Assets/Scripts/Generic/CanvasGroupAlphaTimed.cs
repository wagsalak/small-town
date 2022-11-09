using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupAlphaTimed : MonoBehaviour
{
    public float fadeTime;

    void Update() {

        if (GetComponent<CanvasGroup>().alpha >= 0) {

            GetComponent<CanvasGroup>().alpha -= fadeTime * Time.deltaTime;

        } else {

            gameObject.SetActive(false);

        }

    }
}
