using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domino : MonoBehaviour
{
    public int num1; // The first number on the domino, matched with the previous domino
    public int num2; // The second number on the domino, matched with the next domino

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.1f)
        {
            num1 = Random.Range(0, 9);
            num2 = Random.Range(0, 9);
            timer = 0;
        }
    }
}
