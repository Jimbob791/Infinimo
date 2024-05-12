using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class ScoreDisplay : MonoBehaviour
{
    public int index;
    public int leftPoints;
    public int rightPoints;
    public float scoreMultiplier;
    public double superBonus;
    public double superMulti;
    public bool match = false;

    public Vector3 linePos;

    [SerializeField] TextMeshProUGUI leftText;
    [SerializeField] TextMeshProUGUI rightText;
    [SerializeField] TextMeshProUGUI multiText;
    [SerializeField] TextMeshProUGUI superBonusText;
    [SerializeField] TextMeshProUGUI superMultiText;
    [SerializeField] TextMeshProUGUI plusText;

    private IEnumerator Start()
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

        yield return new WaitForSeconds(0.9f);
        if (match)
            GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(index * 0.02f);

        yield return new WaitForSeconds(0.2f);
        if (superBonus >= 10)
            GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(index * 0.03f);

        yield return new WaitForSeconds(0.15f);
        if (superMulti > 1)
            GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(index * 0.04f);
    }

    void Update()
    {
        transform.localPosition = new Vector3(400, linePos.y * 75, 0);
    }
}
