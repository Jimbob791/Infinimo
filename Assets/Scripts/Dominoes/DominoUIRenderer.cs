using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DominoUIRenderer : MonoBehaviour
{
    [Header("Sprite References")]
    [SerializeField] Image mainBacking;
    [SerializeField] Image shadowBacking;
    [SerializeField] List<MaterialInfo> materialInfo = new List<MaterialInfo>();
    [SerializeField] List<Image> images1 = new List<Image>();
    [SerializeField] List<Image> images2 = new List<Image>();

    [Header("Number Info")]
    public List<DominoNumber> numberInfo = new List<DominoNumber>();

    Domino dominoNumbers;
    [HideInInspector] public bool isInQueue = false;
    public int leftNum;
    public int rightNum;
    public string material;

    void Awake()
    {
        SetDots(images1, numberInfo[leftNum]);
        SetDots(images2, numberInfo[rightNum]);
        SetMaterial();
    }

    void Update()
    {
        SetDots(images1, numberInfo[leftNum]);
        SetDots(images2, numberInfo[rightNum]);
        SetMaterial();
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