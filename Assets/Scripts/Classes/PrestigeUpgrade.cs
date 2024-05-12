using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrestigeUpgrade : MonoBehaviour
{
    public int level = 0;
    public UpgradeType type;

    private double cost;

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI costText;

    [SerializeField] PrestigeUpgrade prestigeDiscount;

    void Start()
    {
        cost = GetUpgradeCost(type);
    }

    void FixedUpdate()
    {
        levelText.text = "Lv" + level;
        costText.text = DominoScore.instance.FormatLargeNumber(cost);
    }

    public void AttemptUpgrade()
    {
        double tempCost = GetUpgradeCost(type);
        if (tempCost > ChipManager.instance.chips)
            return;

        ChipManager.instance.chips -= tempCost;
        level += 1;
        UpgradeManager.instance.PrestigeSound();
        cost = GetUpgradeCost(type);
        StatManager.instance.SaveData();
    }

    private double GetUpgradeCost(UpgradeType type)
    {
        double cost = 0;
        switch (type)
        {
            case UpgradeType.SuperMulti:
                cost = 1 + level / 2;
                break;
            case UpgradeType.SuperBonus:
                cost = 1 + level / 2;
                break;
            case UpgradeType.ScoreSpeed:
                cost = 1 + level / 3;
                break;
            case UpgradeType.AutoplaySpeed:
                cost = 1 + level / 4;
                break;
            case UpgradeType.BoneyardDiscount:
                cost = 1 + level / 5;
                break;
            case UpgradeType.UpgradeDiscount:
                cost = 1 + level / 4;
                break;
            case UpgradeType.BonusDiscount:
                cost = 1 + level / 4;
                break;
            case UpgradeType.ChipBonus:
                cost = 1 + level / 3;
                break;
            case UpgradeType.PrestigeDiscount:
                cost = 1 + level / 4;
                break;
            default:
                cost = 0;
                break;
        }

        cost = System.Math.Floor(cost * (10f / (prestigeDiscount.level + 10f)));

        if (cost < 1)
        {
            cost = 1;
        }
        return cost;
    }
}

public enum UpgradeType
{
    SuperMulti,
    SuperBonus,
    ScoreSpeed,
    AutoplaySpeed,
    BoneyardDiscount,
    UpgradeDiscount,
    BonusDiscount,
    ChipBonus,
    PrestigeDiscount
}