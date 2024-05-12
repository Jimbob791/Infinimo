using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descText;
    [SerializeField] List<UpgradeTooltipInfo> information = new List<UpgradeTooltipInfo>();

    [SerializeField] float width = 500;
    [SerializeField] float height = 230;

    bool display = false;
    string currentText;

    void Update()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(display);
        }

        descText.text = currentText;

        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(screenPoint);

        Vector3 offset = new Vector3(width, height, 0);
        transform.position = mousePos + offset;
    }

    public void DisplayTooltip(string type)
    {
        display = true;
        foreach (UpgradeTooltipInfo info in information)
        {
            if (info.type == StatManager.instance.GetUpgradeType(type))
            {
                currentText = info.description;
            }
        }
    }

    public void CloseTooltip()
    {
        display = false;

        currentText = "";
    }
}

[System.Serializable]
public class UpgradeTooltipInfo
{
    public UpgradeType type;
    public string description;
}