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
    }

    private double GetUpgradeCost(UpgradeType type)
    {
        double cost = 0;
        switch (type)
        {
            case UpgradeType.SuperMulti:
                cost = 3 + Mathf.Pow(level, 2);
                break;
            case UpgradeType.SuperBonus:
                cost = 4 + Mathf.Pow(level, 2.5f);
                break;
            case UpgradeType.ScoreSpeed:
                cost = 2 + Mathf.Pow(5, level + 1);
                break;
            case UpgradeType.AutoplaySpeed:
                cost = 5 + Mathf.Pow(3, level + 1);
                break;
            case UpgradeType.BoneyardDiscount:
                cost = 2 * Mathf.Pow(level + 1, 3);
                break;
            case UpgradeType.UpgradeDiscount:
                cost = 4 + Mathf.Pow(1.5f, level);
                break;
            case UpgradeType.BonusDiscount:
                cost = 6 + Mathf.Pow(2, level);
                break;
            default:
                cost = 0;
                break;
        }
        
        return System.Math.Floor(cost);
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
    BonusDiscount
}