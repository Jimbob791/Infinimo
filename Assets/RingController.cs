using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingController : MonoBehaviour
{
    [SerializeField] Image img;

    public void UpdateRing(float maxTime, float currentTime)
    {
        img.fillAmount = currentTime / maxTime;
    }
}
