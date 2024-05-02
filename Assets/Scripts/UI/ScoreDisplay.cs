using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public int leftPoints;
    public int rightPoints;
    public float scoreMultiplier;

    [SerializeField] TextMeshProUGUI leftText;
    [SerializeField] TextMeshProUGUI rightText;
    [SerializeField] TextMeshProUGUI multiText;

    private void Start()
    {
        leftText.text = leftPoints.ToString();
        rightText.text = rightPoints.ToString();
    
        multiText.text = "x" + scoreMultiplier.ToString();

        Destroy(this.gameObject, 5f);
    }
}
