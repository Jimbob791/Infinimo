using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineButtons : MonoBehaviour
{
    public Line line;

    [SerializeField] GameObject infoButton;
    [SerializeField] GameObject multiButton;
    [SerializeField] GameObject upgradeButton;

    private void Update()
    {
        Vector3 linePos = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(line.lineObject.transform.position);
        transform.position = new Vector3(135, linePos.y, 0);
    }

    public void ShowInfo()
    {
        infoButton.GetComponent<Animator>().Play("Base Layer.InfoPress");
    }

    public void UpgradeAdditive()
    {
        UpgradeManager.instance.AttemptBuy("add", line);
        upgradeButton.GetComponent<Animator>().Play("Base Layer.AdditivePress");
    }

    public void UpgradeMulti()
    {
        UpgradeManager.instance.AttemptBuy("multi", line);
        multiButton.GetComponent<Animator>().Play("Base Layer.MultiplierPress");
    }

    public void ButtonHover(string upgrade)
    {
        if (upgrade != "none")
        {
            UpgradeManager.instance.over = true;
            UpgradeManager.instance.upgradeString = upgrade;
            UpgradeManager.instance.hoveredLine = line;
        }   
        else
        {
            UpgradeManager.instance.over = false;
            UpgradeManager.instance.upgradeString = "none";
            UpgradeManager.instance.hoveredLine = line;
        }
    }
}
