using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLineButton : MonoBehaviour
{
    [SerializeField] GameObject newLineSfx;

    private void Update()
    {
        transform.localPosition = new Vector3(-180, 320 - (DominoManager.instance.lines.Count * 160), 0);

        if (DominoManager.instance.lines.Count == 5)
        {
            Destroy(this.gameObject);
        }
    }

    public void HoverNewLine(string info)
    {
        if (info != "none")
        {
            UpgradeManager.instance.over = true;
            UpgradeManager.instance.upgradeString = "newLine";
            UpgradeManager.instance.hoveredLine = DominoManager.instance.lines[DominoManager.instance.lines.Count - 1];
        }   
        else
        {
            UpgradeManager.instance.over = false;
            UpgradeManager.instance.upgradeString = "none";
            UpgradeManager.instance.hoveredLine = DominoManager.instance.lines[DominoManager.instance.lines.Count - 1];
        }
    }

    public void BuyNewLine()
    {
        Line line = DominoManager.instance.lines[DominoManager.instance.lines.Count - 1];
        double cost = UpgradeManager.instance.GetCost("newLine", line.index, line.multiplier, line.additive, line.prestige);
        if (cost <= DominoScore.instance.score)
        {
            DominoScore.instance.score -= cost;
            Instantiate(newLineSfx);

            DominoManager.instance.CreateLine(DominoManager.instance.lines.Count, 1, 0, 0);
        }
    }
}
