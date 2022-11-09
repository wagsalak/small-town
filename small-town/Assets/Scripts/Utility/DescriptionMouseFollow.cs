using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DescriptionMouseFollow : MonoBehaviour
{

    public TextMeshProUGUI description;
    public GameObject descprtionDisplayObject;

    public float offSetX;
    public float offSetY;

    public bool hoveringOther;

    void Start() {

        descprtionDisplayObject.SetActive(false);

    }

    void Update() {

        if (!IsPointerOverUIElement(GetEventSystemRaycastResults()) && !hoveringOther) {

            descprtionDisplayObject.SetActive(false);
            return;

        }

        descprtionDisplayObject.SetActive(true);
        FollowMouse();
        
    }

    public void Display(Item item) {

        description.text = "<color=yellow>" + item.itemName.ToUpper() + "</color>\n" +
            "Type: " + item.itemType.ToString() + "\n"
            + item.description + "\n\n"
            + "Quantity: " + item.quantity + "\n"
            + "Sell Price: " + item.sellPrice;

    }


    public void FollowMouse() {

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x + offSetX, transform.position.y + offSetY, 0f);

    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults) {

        for (int index = 0; index < eventSystemRaysastResults.Count; index++) {

            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.tag == "ItemDisplay") {

                Display( curRaysastResult.gameObject.GetComponent<Item>() );
                return true;
            }

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
