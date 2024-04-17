using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoRenderer : MonoBehaviour
{
    [Header("Sprite References")]
    [SerializeField] List<SpriteRenderer> sprites1 = new List<SpriteRenderer>();
    [SerializeField] List<SpriteRenderer> sprites2 = new List<SpriteRenderer>();

    [Header("Number Info")]
    public List<DominoNumber> numberInfo = new List<DominoNumber>();

    Domino dominoNumbers;
    [HideInInspector] public bool isInDeck;

    void Start()
    {
        dominoNumbers = GetComponent<Domino>();
    }

    void Update()
    {
        if (isInDeck)
        {
            for (int i = 0; i < sprites1.Count; i++)
            {
                sprites1[i].enabled = false;
            
            }
            for (int i = 0; i < sprites2.Count; i++)
            {
                sprites2[i].enabled = false;
            }
            return;
        }

        SetDots(sprites1, numberInfo[dominoNumbers.num1]);
        SetDots(sprites2, numberInfo[dominoNumbers.num2]);
    }

    void SetDots(List<SpriteRenderer> renderers, DominoNumber info)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            SpriteRenderer renderer = renderers[i];
            renderer.color = info.dotColor;
            renderer.enabled = info.dots[i];
        }
    }
}

[System.Serializable]
public class DominoNumber
{
    public Color dotColor;
    public bool[] dots = new bool[9];
}