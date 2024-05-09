using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoRenderer : MonoBehaviour
{
    [Header("Sprite References")]
    [SerializeField] SpriteRenderer mainBacking;
    [SerializeField] SpriteRenderer shadowBacking;
    [SerializeField] List<MaterialInfo> materialInfo = new List<MaterialInfo>();
    [SerializeField] List<SpriteRenderer> sprites1 = new List<SpriteRenderer>();
    [SerializeField] List<SpriteRenderer> sprites2 = new List<SpriteRenderer>();

    [Header("Number Info")]
    public List<DominoNumber> numberInfo = new List<DominoNumber>();

    Domino dominoNumbers;
    [HideInInspector] public bool isInQueue;
    [HideInInspector] public int leftNum;
    [HideInInspector] public int rightNum;
    [HideInInspector] public string material;

    void Update()
    {
        SetDots(sprites1, numberInfo[leftNum]);
        SetDots(sprites2, numberInfo[rightNum]);
        SetMaterial();
    }

    void SetDots(List<SpriteRenderer> renderers, DominoNumber info)
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            SpriteRenderer renderer = renderers[i];
            renderer.color = info.dotColor;
            renderer.enabled = info.dots[i];

            if (isInQueue)
            {
                renderer.enabled = false;
            }
        }
    }

    void SetMaterial()
    {
        foreach (MaterialInfo info in materialInfo)
        {
            if (info.material == material)
            {
                mainBacking.sprite = info.sprite;
                shadowBacking.color = info.shadowColor;
            }
        }
    }
}

[System.Serializable]
public class DominoNumber
{
    public Color dotColor;
    public bool[] dots = new bool[9];
}

[System.Serializable]
public class MaterialInfo
{
    public Sprite sprite;
    public string material;
    public Color shadowColor;
}