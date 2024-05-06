using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [SerializeField] GameObject prestigePrefab;
    [SerializeField] TextMeshProUGUI upgradeTitle;
    [SerializeField] TextMeshProUGUI upgradeDescription;
    [SerializeField] TextMeshProUGUI upgradeCost;

    [SerializeField] PrestigeUpgrade bonusDiscount;
    [SerializeField] PrestigeUpgrade multiDiscount;

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
            if (upgrade == "multi" && line.multiplier < 50 * (line.prestige + 1))
            {
                line.multiplier += 1;
                Debug.Log("Line Multiplier Upgraded x" + line.multiplier);
            }
            else if (upgrade == "add" && line.additive < 50 * (line.prestige + 1))
            {
                line.additive += 1;
                Debug.Log("Line Additive Upgraded +" + line.additive);
            }
            else if (upgrade == "prestige")
            {
                line.additive = 0;
                line.multiplier = 1;
                line.prestige += 1;

                GameObject newObj = Instantiate(prestigePrefab, new Vector3(1 + ((line.prestige - 1) * 0.3f), -0.15f, 0), Quaternion.identity);
                newObj.transform.SetParent(line.lineObject.transform, false);
                newObj.GetComponent<SpriteRenderer>().sortingOrder = -line.prestige;

                Debug.Log("Line Prestiged " + line.prestige);

                over = false;
                DisplayUpgradeInfo("none", line);
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
            if (line.multiplier == 50 * (line.prestige + 1))
            {
                upgradeTitle.text = "MULTIPLIER AT MAX LVL" + line.multiplier;
                upgradeDescription.text = "x" + DominoScore.instance.GetMulti(line.multiplier, line);
            }
            else
            {
                upgradeTitle.text = "UPGRADE MULTI LVL" + line.multiplier;
                upgradeDescription.text = "x" + DominoScore.instance.GetMulti(line.multiplier, line) + " => x" + DominoScore.instance.GetMulti(line.multiplier + 1, line);
            }
        }
        else if (upgrade == "add")
        {
            if (line.additive == 50 * (line.prestige + 1))
            {
                upgradeTitle.text = "BONUS AT MAX LVL" + line.additive;
                upgradeDescription.text = "+" + DominoScore.instance.GetAdditive(line.additive, line);
            }
            else
            {
                upgradeTitle.text = "UPGRADE BONUS LVL" + line.additive;
                upgradeDescription.text = "+" + DominoScore.instance.GetAdditive(line.additive, line) + " => +" + DominoScore.instance.GetAdditive(line.additive + 1, line);
            }
        }
        else if (upgrade == "newLine")
        {
            upgradeTitle.text = "BUY NEW LINE";
            upgradeDescription.text = "";
        }
        else if (upgrade == "prestige")
        {
            upgradeTitle.text = "PRESTIGE LINE";
            upgradeDescription.text = "NEW MAX LVL " + ((line.prestige + 2) * 50) + "  |  +1 BONUS  |  x2 MULTI";
        }

        upgradeCost.text = DominoScore.instance.FormatLargeNumber(GetCost(upgrade, line)) + " Pips";
    }

    public double GetCost(string upgrade, Line line)
    {
        cost = 0;
        if (upgrade == "multi")
        {
            cost = Mathf.Pow(10, line.index + 1) + Mathf.Pow(line.multiplier, line.index + 2);
            cost *= (10f / (multiDiscount.level + 10f));
        }
        else if (upgrade == "add")
        {
            cost = Mathf.Pow(10, line.index) + Mathf.Pow(line.additive, line.index + 4);
            cost *= (10f / (bonusDiscount.level + 10f));
        }
        else if (upgrade == "newLine")
        {
            cost = Mathf.Pow(10, line.index * 2 + 2);
        }
        else if (upgrade == "prestige")
        {
            for (int i = 0; i < 50 * (line.prestige + 1); i++)
            {
                cost += Mathf.Pow(10, line.index + 1) + Mathf.Pow(i, line.index + 2);
            }
        }

        return System.Math.Floor(cost);
    }
}