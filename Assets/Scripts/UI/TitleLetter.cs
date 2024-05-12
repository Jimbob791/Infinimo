using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLetter : MonoBehaviour
{
    float timer;
    [SerializeField] float moveMagnitude;
    [SerializeField] float movePeriod;

    [SerializeField] float rotateMagnitude;
    [SerializeField] float rotatePeriod;

    Vector3 startPos;

    void Start()
    {
        timer = Random.Range(0f, 20f);
        moveMagnitude *= Random.Range(0.8f, 1.2f);
        startPos = transform.localPosition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        transform.localPosition = new Vector3(startPos.x, startPos.y + moveMagnitude * Mathf.Sin(movePeriod * timer), 0);

        Quaternion rotation = Quaternion.Euler(0, 0, rotateMagnitude * Mathf.Sin(rotatePeriod * timer));
        transform.rotation = rotation;
    }
}
