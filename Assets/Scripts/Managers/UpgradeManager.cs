using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [SerializeField] TextMeshProUGUI upgradeTitle;
    [SerializeField] TextMeshProUGUI upgradeDescription;
    [SerializeField] TextMeshProUGUI upgradeCost;

    double cost;

    [HideInInspector] public bool over;
    [HideInInspector] public string upgradeString;
    [HideInInspector] public Line hoveredLine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (over)
        {
            DisplayUpgradeInfo(upgradeString, hoveredLine);
        }
    }

    public void AttemptBuy(string upgrade, Line line)
    {
        if (DominoScore.instance.score >= GetCost(upgrade, line))
        {
            DominoScore.instance.score -= GetCost(upgrade, line);
            if (upgrade == "multi" && line.multiplier < 50)
            {
                line.multiplier += 1;
                Debug.Log("Line Multiplier Upgraded x" + line.multiplier);
            }
            else if (upgrade == "add" && line.additive < 50)
            {
                line.additive += 1;
                Debug.Log("Line Additive Upgraded +" + line.additive);
            }
        }
    }

    public void DisplayUpgradeInfo(string upgrade, Line line)
    {   
        if (upgrade == "none")
        {
            upgradeTitle.text = "";
            upgradeDescription.text = "";
            upgradeCost.text = "";
        }

        if (upgrade == "multi")
        {
            if (line.multiplier == 50)
            {
                upgradeTitle.text = "MULTIPLIER AT MAX LVL" + line.multiplier;
                upgradeDescription.text = "x" + (line.multiplier * Mathf.Pow(8, line.index));
            }
            else
            {
                upgradeTitle.text = "UPGRADE MULTIPLIER";
                upgradeDescription.text = "x" + (line.multiplier * Mathf.Pow(8, line.index)) + " => x" + ((line.multiplier + 1) * Mathf.Pow(8, line.index));
            }
        }
        else if (upgrade == "add")
        {
            if (line.additive == 50)
            {
                upgradeTitle.text = "BONUS AT MAX LVL" + line.additive;
                upgradeDescription.text = "+" + line.additive;
            }
            else
            {
                upgradeTitle.text = "UPGRADE BONUS";
                upgradeDescription.text = "+" + line.additive + " => +" + (line.additive + 1);
            }
        }
        else if (upgrade == "newLine")
        {
            upgradeTitle.text = "BUY NEW LINE";
            upgradeDescription.text = "";
        }

        upgradeCost.text = DominoScore.instance.FormatLargeNumber(GetCost(upgrade, line)) + " Dots";
    }

    public double GetCost(string upgrade, Line line)
    {
        cost = 0;
        if (upgrade == "multi")
        {
            cost = Mathf.Pow(10, line.index + 1) + Mathf.Pow(line.multiplier, line.index + 2);
        }
        else if (upgrade == "add")
        {
            cost = Mathf.Pow(10, line.index) + Mathf.Pow(line.additive, line.index + 4);
        }
        else if (upgrade == "newLine")
        {
            cost = Mathf.Pow(10, (line.index + 1) * 2 + 1);
        }

        return cost;
    }
}