using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DominoScore : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI scoreText;
    [SerializeField] GameObject scoreDisplay;
    [SerializeField] GameObject canvasParent;

    float currentScore;
    bool updateScore = true;

    private void FixedUpdate()
    {
        UpdateScore();
    }
    
    public void ScoreDominoes(Domino played, Domino active)
    {
        if (active.rightNum == played.leftNum) // MATCH!!!!
        {
            float multi = 1;
            int scoreToAdd = played.leftNum + played.rightNum;
            if (played.leftNum == played.rightNum)
            {
                multi = 2;
            }
            score += Mathf.RoundToInt(scoreToAdd * multi);

            GameObject display = Instantiate(scoreDisplay, new Vector3(Screen.width / 2, Screen.height / 2 + 200, 0), Quaternion.identity, canvasParent.transform);
            ScoreDisplay displayScript = display.GetComponent<ScoreDisplay>();
            displayScript.leftPoints = played.leftNum;
            displayScript.rightPoints = played.rightNum;
            displayScript.scoreMultiplier = multi;

            StartCoroutine(PauseScoreUpdate());
        }
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
        float scoreToAdd = (score - currentScore) / 30;
        currentScore += scoreToAdd;
        scoreText.text = FormatLargeNumber(Mathf.RoundToInt(currentScore));
    }

    private string FormatLargeNumber(int num)
    {
        if (num >= 1000.0f) return (num/1000.0f).ToString("F3")+ "K";
        if (num >= 1000000.0f) return (num/1000000.0f).ToString("F3") + "M";
        if (num >= 1000000000.0f) return (num/1000000000.0f).ToString("F3") + "B";
        if (num >= 1000000000000.0f) return (num/1000000000000.0f).ToString("F3") + "T";

        return num.ToString();
    }
}
