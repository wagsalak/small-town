using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForTutorialScript : MonoBehaviour
{

    public GameObject tutorialMainPanel;
    public GameObject[] tutorials;

    int index = 1;
    public void Next() {

        if (index < tutorials.Length){

            foreach (GameObject obj in tutorials) obj.SetActive(false);

            tutorials[index].SetActive(true);

            index += 1;

        } else {
            index = 0;
            tutorialMainPanel.SetActive(false);
        }

    }

}
