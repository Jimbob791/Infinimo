using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineButtons : MonoBehaviour
{
    public Line line;

    [SerializeField] GameObject prestigeButton;
    [SerializeField] GameObject multiButton;
    [SerializeField] GameObject upgradeButton;

    private void Update()
    {
        Vector3 linePos = line.lineObject.transform.position;
        transform.localPosition = new Vector3(-800, linePos.y * 75, 0);

        prestigeButton.SetActive(line.multiplier == 50 * (line.prestige + 1));
    }

    public void UpgradePrestige()
    {
        UpgradeManager.instance.AttemptBuy("prestige", line);
        prestigeButton.GetComponent<Animator>().Play("Base Layer.InfoPress");
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
