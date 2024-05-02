using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DominoScore : MonoBehaviour
{
    public static DominoScore instance;

    public double score;
    public TextMeshProUGUI scoreText;
    [SerializeField] GameObject scoreDisplay;
    [SerializeField] GameObject canvasParent;

    List<SuffixInfo> numberInfo = new List<SuffixInfo>();

    double currentScore;
    bool updateScore = true;

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

    private void Start()
    {
        numberInfo.Add(new SuffixInfo(24, "Sp"));
        numberInfo.Add(new SuffixInfo(21, "Sx"));
        numberInfo.Add(new SuffixInfo(18, "Qi"));
        numberInfo.Add(new SuffixInfo(15, "Qa"));
        numberInfo.Add(new SuffixInfo(12, "T"));
        numberInfo.Add(new SuffixInfo(9, "B"));
        numberInfo.Add(new SuffixInfo(6, "M"));
        numberInfo.Add(new SuffixInfo(3, "K"));
    }

    private void FixedUpdate()
    {
        UpdateScore();
    }
    
    public void ScoreDominoes(Domino played, Domino active, Line line)
    {
        float multi = line.multiplier * Mathf.Pow(8, line.index);
        double scoreToAdd = played.leftNum + line.additive + played.rightNum + line.additive;
        if (played.leftNum == played.rightNum)
        {
            multi *= 2;
        }
        score += scoreToAdd * multi;

        Vector3 linePos = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(line.lineObject.transform.position);
        GameObject display = Instantiate(scoreDisplay, new Vector3(linePos.x + 400, linePos.y, 0), Quaternion.identity, canvasParent.transform);
        ScoreDisplay displayScript = display.GetComponent<ScoreDisplay>();
        displayScript.leftPoints = played.leftNum + line.additive;
        displayScript.rightPoints = played.rightNum + line.additive;
        displayScript.scoreMultiplier = multi;

        StartCoroutine(PauseScoreUpdate());
    }

    public bool CheckMatch(Domino played, Domino active)
    {
        return active.rightNum == played.leftNum;
    }

    private IEnumerator PauseScoreUpdate()
    {
        updateScore = false;
        yield return new WaitForSeconds(1.2f);
        updateScore = true;
    }

    private void UpdateScore()
    {
        if (!updateScore)
        {
            return;
        }

        if (score - currentScore < 0.5f)
        {
            currentScore = score;
        }
        double scoreToAdd = (score - currentScore) / 30;
        currentScore += scoreToAdd;
        scoreText.text = FormatLargeNumber(currentScore);
    }

    public string FormatLargeNumber(double num)
    {
        foreach (SuffixInfo info in numberInfo)
        {
            if (num > Mathf.Pow(10, info.power)) return (num / Mathf.Pow(10, info.power)).ToString("F2") + info.suffix;
        }

        return num.ToString().Split(".")[0];
    }
}

public class SuffixInfo
{
    public int power;
    public string suffix;

    public SuffixInfo(int newPower, string newSuffix)
    {
        power = newPower;
        suffix = newSuffix;
    }
}