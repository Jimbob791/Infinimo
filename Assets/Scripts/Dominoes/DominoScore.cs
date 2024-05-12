using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DominoScore : MonoBehaviour
{
    public static DominoScore instance;

    public int scoreCombo;

    public double score;
    public TextMeshProUGUI scoreText;
    [SerializeField] GameObject scoreDisplay;
    [SerializeField] GameObject canvasParent;
    [SerializeField] GameObject scorePrefab;
    [SerializeField] PrestigeUpgrade superMulti;
    [SerializeField] PrestigeUpgrade superBonus;

    List<SuffixInfo> numberInfo = new List<SuffixInfo>();

    double currentScore;
    bool updateScore = true;

    float multi;
    double scoreToAdd;
    double superMultiValue;
    double superBonusValue;

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

    private void FixedUpdate()
    {
        UpdateScore();
    }
    
    public void ScoreDominoes(Domino played, Domino active, Line line)
    {
        Vector3 linePos;
        GameObject display;
        ScoreDisplay displayScript;

        if (!CheckMatch(played, active))
        {
            superBonusValue = Mathf.Pow(10, superBonus.level);
            superMultiValue = Mathf.Pow(2, superMulti.level);
            multi = 1;
            scoreToAdd = 0;
            if (superBonusValue == 1)
            {
                StatManager.instance.SaveData();
                return;
            }

            linePos = line.lineObject.transform.position;
            display = Instantiate(scoreDisplay);
            display.transform.SetParent(canvasParent.transform, false);
            displayScript = display.GetComponent<ScoreDisplay>();
            displayScript.match = false;
            displayScript.leftPoints = 0;
            displayScript.rightPoints = 0;
            displayScript.scoreMultiplier = 1;
            displayScript.superBonus = System.Math.Pow(10, superBonus.level);
            displayScript.superMulti = System.Math.Pow(2, superMulti.level);
            displayScript.linePos = linePos;
            displayScript.index = line.index + 1;

            StartCoroutine(PauseScoreUpdate(scoreToAdd, multi, superBonusValue, superMultiValue, played, false));
            return;
        }

        superMultiValue = System.Math.Pow(2, superMulti.level);
        superBonusValue = System.Math.Pow(10, superBonus.level);
        multi = GetMulti(line.multiplier, line);
        scoreToAdd = played.leftNum + GetAdditive(line.additive, line) + played.rightNum + GetAdditive(line.additive, line);
        if (played.leftNum == played.rightNum)
        {
            multi *= 2;
        }

        linePos = line.lineObject.transform.position;
        display = Instantiate(scoreDisplay, new Vector3(linePos.x + 300, linePos.y, 0), Quaternion.identity, canvasParent.transform);
        displayScript = display.GetComponent<ScoreDisplay>();
        displayScript.match = true;

        if (played.material == "Wooden" && active.material == "Wooden")
        {
            scoreToAdd *= 5;
            displayScript.leftPoints = (played.leftNum + GetAdditive(line.additive, line)) * 5;
            displayScript.rightPoints = (played.rightNum + GetAdditive(line.additive, line)) * 5;
        }
        else
        {
            displayScript.leftPoints = played.leftNum + GetAdditive(line.additive, line);
            displayScript.rightPoints = played.rightNum + GetAdditive(line.additive, line);
        }
    
        
        displayScript.superBonus = superBonusValue;
        if (played.material == "Marbled")
            multi *= 1.5f;

        displayScript.scoreMultiplier = multi;

        displayScript.superMulti = superMultiValue;
        displayScript.linePos = linePos;
        displayScript.index = line.index + 1;

        StartCoroutine(PauseScoreUpdate(scoreToAdd, multi, superBonusValue, superMultiValue, played, true));
    }

    public bool CheckMatch(Domino played, Domino active)
    {
        return active.rightNum == played.leftNum;
    }

    private IEnumerator PauseScoreUpdate(double bonus, double multi, double superBonus, double superMulti, Domino played, bool match)
    {
        if (match)
        {
            StartCoroutine(ScoreSound(1 + scoreCombo * 0.2f));
            scoreCombo += 1;
        }
        yield return new WaitForSeconds(1.2f);
        score += (bonus * multi + superBonus) * superMulti;
        
        if (played.material == "Golden")
            ChipManager.instance.AddProgress((bonus * multi + superBonus) * superMulti * 2f);
        else
            ChipManager.instance.AddProgress((bonus * multi + superBonus) * superMulti);
        StatManager.instance.SaveData();
    }

    IEnumerator ScoreSound(float pitch)
    {
        yield return new WaitForSeconds(0.8f);
        GameObject newObj = Instantiate(scorePrefab);
        newObj.GetComponent<AudioSource>().pitch = pitch;
    }

    private void UpdateScore()
    {
        if (!updateScore)
        {
            return;
        }

        if (System.Math.Abs(score - currentScore) < 0.5f)
        {
            currentScore = score;
        }
        double scoreToAdd = (score - currentScore) / 30;
        currentScore += scoreToAdd;
        scoreText.text = FormatLargeNumber(currentScore);
    }

    public string FormatLargeNumber(double num)
    {
        if (numberInfo.Count == 0)
        {
            numberInfo.Add(new SuffixInfo(27, "Oc"));
            numberInfo.Add(new SuffixInfo(24, "Sp"));
            numberInfo.Add(new SuffixInfo(21, "Sx"));
            numberInfo.Add(new SuffixInfo(18, "Qi"));
            numberInfo.Add(new SuffixInfo(15, "Qa"));
            numberInfo.Add(new SuffixInfo(12, "T"));
            numberInfo.Add(new SuffixInfo(9, "B"));
            numberInfo.Add(new SuffixInfo(6, "M"));
            numberInfo.Add(new SuffixInfo(3, "K"));
        }

        foreach (SuffixInfo info in numberInfo)
        {
            if (num >= Mathf.Pow(10, info.power)) return (num / Mathf.Pow(10, info.power)).ToString("F2") + info.suffix;
        }

        return num.ToString().Split(".")[0];
    }

    public float GetMulti(int level, Line line)
    {
        return level * Mathf.Pow(4, line.index) * (line.prestige + 1);
    }

    public int GetAdditive(int level, Line line)
    {
        return level + line.prestige;
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