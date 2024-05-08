using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descText;

    float width = 500;
    float height = 230;

    bool display = false;
    string currentTitle;
    string currentDesc;

    void Update()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(display);
        }

        titleText.text = currentTitle;
        descText.text = currentDesc;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;

        Vector3 offset = new Vector3(width / 2, -height / 2, 0);
        transform.position = mousePos + offset;
    }

    public void DisplayTooltip(Domino domino)
    {
        display = true;
        titleText.text = domino.leftNum + " - " + domino.rightNum;
    }

    public void CloseTooltip()
    {
        display = false;

        currentText = "";
    }
}
