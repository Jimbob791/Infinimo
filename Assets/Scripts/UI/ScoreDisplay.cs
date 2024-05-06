using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public int leftPoints;
    public int rightPoints;
    public float scoreMultiplier;
    public double superBonus;
    public double superMulti;
    public bool match = false;

    [SerializeField] TextMeshProUGUI leftText;
    [SerializeField] TextMeshProUGUI rightText;
    [SerializeField] TextMeshProUGUI multiText;
    [SerializeField] TextMeshProUGUI superBonusText;
    [SerializeField] TextMeshProUGUI superMultiText;
    [SerializeField] TextMeshProUGUI plusText;

    private void Start()
    {
        leftText.text = leftPoints.ToString();
        rightText.text = rightPoints.ToString();
        superBonusText.text = "+" + superBonus.ToString();

        superMultiText.text = "x" + superMulti.ToString();
        multiText.text = "x" + scoreMultiplier.ToString();

        Destroy(this.gameObject, 5f);

        if (superBonus == 1)
        {
            superBonusText.enabled = false;
        }
        if (superMulti == 1)
        {
            superMultiText.enabled = false;
        }

        if (!match)
        {
            GetComponent<Animator>().Play("Base Layer.ScoreUpEnter", 0, (65 / 140));
            leftText.enabled = false;
            rightText.enabled = false;
            multiText.enabled = false;
            plusText.enabled = false;
        }
    }
}
