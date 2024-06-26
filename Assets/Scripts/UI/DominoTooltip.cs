using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DominoTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descText;

    [SerializeField] float width = 500;
    [SerializeField] float height = 230;

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

        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(screenPoint);

        Vector3 offset = new Vector3(width, height, 0);
        transform.position = mousePos + offset;
    }

    public void DisplayTooltip(Domino domino)
    {
        display = true;
        currentTitle = domino.material + " " + domino.leftNum + " - " + domino.rightNum;
        if (domino.material == "Plastic")
        {
            currentDesc = "A simple, plastic domino. No extra bonus.";
        }
        else if (domino.material == "Wooden")
        {
            currentDesc = "Wooden dominoes get x5 their base if matched with another wooden domino.";
        }
        else if (domino.material == "Golden")
        {
            currentDesc = "Golden dominoes count as double towards the current chip progress.";
        }
        else if (domino.material == "Marbled")
        {
            currentDesc = "Marbled dominoes score 1.5x as many pips compared to normal.";
        }
    }

    public void CloseTooltip()
    {
        display = false;

        currentDesc = "";
    }
}
