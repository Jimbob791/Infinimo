using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RingController : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TextMeshProUGUI timeLeft;

    public void UpdateRing(float maxTime, float currentTime)
    {
        img.fillAmount = currentTime / maxTime;

        timeLeft.text = (maxTime - currentTime).ToString("F1");
    }
}
