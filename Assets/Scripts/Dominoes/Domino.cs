using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domino
{
    public int num1; // The first number on the domino, matched with the previous domino
    public int num2; // The second number on the domino, matched with the next domino

    public Vector3 targetPos;
    [SerializeField] float accel;

    public void OnUpdate()
    {
        GameObject.GetComponent<Transform>().position = Vector3.Lerp(GameObject.GetComponent<Transform>().position, targetPos, accel);
    }
}
