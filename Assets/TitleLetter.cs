using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLetter : MonoBehaviour
{
    float timer;
    [SerializeField] float magnitude;
    [SerializeField] float period;

    Vector3 startPos;

    void Start()
    {
        timer = Random.Range(0f, 10.5f);
        magnitude *= Random.Range(0.8f, 1.2f);
        startPos = transform.localPosition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        transform.localPosition = new Vector3(startPos.x, startPos.y + magnitude * Mathf.Sin(period * timer), 0);
    }
}
