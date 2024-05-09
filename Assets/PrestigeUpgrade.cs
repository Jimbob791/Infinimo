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
        cost = GetUpgradeCost(type);
        StatManager.instance.SaveData();
    }

    private double GetUpgradeCost(UpgradeType type)
    {
        double cost = 0;
        switch (type)
        {
            case UpgradeType.SuperMulti:
                cost = 2 + 2 * level;
                break;
            case UpgradeType.SuperBonus:
                cost = 2 + 2 * level;
                break;
            case UpgradeType.ScoreSpeed:
                cost = 2 + level;
                break;
            case UpgradeType.AutoplaySpeed:
                cost = 1 + level;
                break;
            case UpgradeType.BoneyardDiscount:
                cost = 1 + level;
                break;
            case UpgradeType.UpgradeDiscount:
                cost = 1 + level;
                break;
            case UpgradeType.BonusDiscount:
                cost = 2 + level;
                break;
            case UpgradeType.ChipBonus:
                cost = 2 + 2 * level;
                break;
            case UpgradeType.PrestigeDiscount:
                cost = 1 + level;
                break;
            default:
                cost = 0;
                break;
        }
        
        return System.Math.Ceiling(cost * (10f / (prestigeDiscount.level + 10f)));
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