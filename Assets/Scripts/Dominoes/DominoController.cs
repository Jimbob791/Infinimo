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
            transform.position = Vector3.Lerp(transform.position, targetPos, accel * Time.deltaTime);
    }
}