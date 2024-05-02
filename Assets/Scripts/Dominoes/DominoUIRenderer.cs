using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DominoUIRenderer : MonoBehaviour
{
    [Header("Sprite References")]
    [SerializeField] List<Image> images1 = new List<Image>();
    [SerializeField] List<Image> images2 = new List<Image>();

    [Header("Number Info")]
    public List<DominoNumber> numberInfo = new List<DominoNumber>();

    Domino dominoNumbers;
    [HideInInspector] public bool isInQueue = false;
    public int leftNum;
    public int rightNum;

    void Awake()
    {
        SetDots(images1, numberInfo[leftNum]);
        SetDots(images2, numberInfo[rightNum]);
    }

    void Update()
    {
        SetDots(images1, numberInfo[leftNum]);
        SetDots(images2, numberInfo[rightNum]);
    }

    void SetDots(List<Image> renderers, DominoNumber info)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            Image renderer = renderers[i];
            renderer.color = info.dotColor;
            renderer.enabled = info.dots[i];

            if (isInQueue)
            {
                renderer.enabled = false;
            }
        }
    }
}