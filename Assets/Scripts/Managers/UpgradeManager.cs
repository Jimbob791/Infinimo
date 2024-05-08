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
        double cost = 0;
        int levelAmount = 1;
        if (Input.GetKey(KeyCode.LeftShift) && upgrade != "prestige")
        {
            levelAmount = 10;
            for (int i = 0; i < 10; i++)
            {
                cost += GetCost(upgrade, line.index, line.multiplier + i, line.additive + i, line.prestige);
            }
        }
        else if (Input.GetKey(KeyCode.LeftControl) && upgrade != "prestige")
        {
            levelAmount = 50;
            for (int i = 0; i < 50; i++)
            {
                cost += GetCost(upgrade, line.index, line.multiplier + i, line.additive + i, line.prestige);
            }
        }
        else
        {
            cost += GetCost(upgrade, line.index, line.multiplier, line.additive, line.prestige);
        }

        if (DominoScore.instance.score >= cost)
        {
            if (upgrade == "multi" && line.multiplier + levelAmount - 1 < 50 * (line.prestige + 1))
            {
                DominoScore.instance.score -= cost;
                line.multiplier += levelAmount;
                StatManager.instance.SaveData();
            }
            else if (upgrade == "add" && line.additive + levelAmount - 1 < 50 * (line.prestige + 1))
            {
                DominoScore.instance.score -= cost;
                line.additive += levelAmount;
                StatManager.instance.SaveData();
            }
            else if (upgrade == "prestige")
            {
                DominoScore.instance.score -= cost;
                line.multiplier = 1;
                line.prestige += 1;

                AddPrestigeIcon(line, line.prestige);

                Debug.Log("Line Prestiged " + line.prestige);

                over = false;
                DisplayUpgradeInfo("none", line);
                StatManager.instance.SaveData();
            }
        }
    }

    public void AddPrestigeIcon(Line line, int prestige)
    {
        GameObject newObj = Instantiate(prestigePrefab, new Vector3(1 + ((prestige - 1) * 0.3f), -0.15f, 0), Quaternion.identity);
        newObj.transform.SetParent(line.lineObject.transform, false);
        newObj.GetComponent<SpriteRenderer>().sortingOrder = -prestige;
    }

    public void DisplayUpgradeInfo(string upgrade, Line line)
    {   
        double cost = 0;
        int levelAmount = 1;
        if (Input.GetKey(KeyCode.LeftShift) && upgrade != "prestige")
        {
            levelAmount = 10;
            for (int i = 0; i < 10; i++)
            {
                cost += GetCost(upgrade, line.index, line.multiplier + i, line.additive + i, line.prestige);
            }
        }
        else if (Input.GetKey(KeyCode.LeftControl) && upgrade != "prestige")
        {
            levelAmount = 50;
            for (int i = 0; i < 50; i++)
            {
                cost += GetCost(upgrade, line.index, line.multiplier + i, line.additive + i, line.prestige);
            }
        }
        else
        {
            cost += GetCost(upgrade, line.index, line.multiplier, line.additive, line.prestige);
        }

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
                upgradeDescription.text = "x" + DominoScore.instance.GetMulti(line.multiplier, line) + " => x" + DominoScore.instance.GetMulti(line.multiplier + levelAmount, line);
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
                upgradeDescription.text = "+" + DominoScore.instance.GetAdditive(line.additive, line) + " => +" + DominoScore.instance.GetAdditive(line.additive + levelAmount, line);
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

        upgradeCost.text = DominoScore.instance.FormatLargeNumber(cost) + " Pips";
    }

    public double GetCost(string upgrade, int index, int multiplier, int additive, int prestige)
    {
        cost = 0;
        if (upgrade == "multi")
        {
            cost = Mathf.Pow(10, index + 1) + Mathf.Pow(multiplier, index + 2);
            cost *= (10f / (multiDiscount.level + 10f));
        }
        else if (upgrade == "add")
        {
            cost = Mathf.Pow(10, index) + Mathf.Pow(additive, index + 4);
            cost *= (10f / (bonusDiscount.level + 10f));
        }
        else if (upgrade == "newLine")
        {
            cost = Mathf.Pow(10, index * 3 + 2);
        }
        else if (upgrade == "prestige")
        {
            for (int i = 0; i < 50 * (prestige + 1); i++)
            {
                cost += Mathf.Pow(10, index + 1) + Mathf.Pow(i, index + 2);
            }
        }

        return System.Math.Floor(cost);
    }
}