using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChipManager : MonoBehaviour
{
    public static ChipManager instance;

    public double chips;
    
    [SerializeField] TextMeshProUGUI chipsDisplay;
    [SerializeField] TextMeshProUGUI nextChipText;
    [SerializeField] Slider largeProgressSlider;
    [SerializeField] Slider smallProgressSlider;

    [SerializeField] PrestigeUpgrade chipBonus;

    private double currentChips;
    public double totalChips;

    public double currentPips;
    private double neededPips;

    void Awake()
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

    void Start()
    {
        neededPips = GetNextPips();
    }

    void FixedUpdate()
    {
        if (System.Math.Abs(chips - currentChips) < 0.5f)
        {
            currentChips = chips;
        }
        double chipsToAdd = (chips - currentChips) / 30;
        currentChips += chipsToAdd;
        chipsDisplay.text = DominoScore.instance.FormatLargeNumber(currentChips);

        float targetSliderValue = System.Convert.ToSingle(currentPips / neededPips);
        smallProgressSlider.value = Mathf.Lerp(smallProgressSlider.value, targetSliderValue, 0.05f);
        largeProgressSlider.value = Mathf.Lerp(largeProgressSlider.value, targetSliderValue, 0.05f);
        if (smallProgressSlider.value >= 1 || largeProgressSlider.value >= 1)
        {
            smallProgressSlider.value = 0;
            largeProgressSlider.value = 0;

            chips += 1;
            totalChips += 1;
            currentPips -= neededPips;
            neededPips = GetNextPips();
        }

        nextChipText.text = "NEXT CHIP    -     " + DominoScore.instance.FormatLargeNumber(currentPips) + "/"  + DominoScore.instance.FormatLargeNumber(neededPips);
    }

    double GetNextPips()
    {
        return 100 * System.Math.Pow(totalChips + 1, 4);
    }

    public void AddProgress(double pips)
    {
        currentPips += pips * Mathf.Pow(2, chipBonus.level);
    }
}
