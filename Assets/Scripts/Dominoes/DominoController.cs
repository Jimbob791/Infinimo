using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoController : MonoBehaviour
{
    public Vector3 targetPos;
    public float accel;

    [HideInInspector] public bool move = true;

    private void Update()
    {
        if (move)
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, accel * Time.deltaTime);

        if (transform.localPosition.x < -4.5f)
        {
            float scaleFactor = transform.localPosition.x + 5.5f;
            scaleFactor = scaleFactor < 0 ? 0 : scaleFactor;
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
        else
        {
            float scaleFactor = transform.localScale.x + 2 * Time.deltaTime;
            if (scaleFactor > 1)
                scaleFactor = 1;
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
    }
}