using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SimplePopUpManager : MonoBehaviour
{

    public static SimplePopUpManager SPM_Instance { get; private set; }

    public GameObject propUpUiMain;
    public TextMeshProUGUI displayText;

    private CanvasGroup canvasGroup;
    private bool isShowing;

    private void Awake() {

        if (SPM_Instance == null) {

            SPM_Instance = this;

        } else {

            Destroy(this.gameObject);

        }

        canvasGroup = propUpUiMain.GetComponent<CanvasGroup>();
    }

    private void Update() {

        HidePopUp();

    }

    public void ShowPopUp( string content ) {

        propUpUiMain.SetActive(true);
        displayText.text += content + "\n";
        canvasGroup.alpha = 1;
        isShowing = true;

    }

    private void HidePopUp() {

        if (isShowing) {

            canvasGroup.alpha -= 0.5f * Time.deltaTime;

            if (canvasGroup.alpha <= 0.1f) {

                propUpUiMain.SetActive(false);
                displayText.text = "";
                isShowing = false;
            }

        }

    }
}
