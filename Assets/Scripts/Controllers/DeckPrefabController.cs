using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckPrefabController : MonoBehaviour
{
    [SerializeField] GameObject hoverDisplay;
    [SerializeField] GameObject selectedText;

    [SerializeField] DominoUIRenderer dominoRenderer;

    public Domino domino;

    bool display;
    public bool selected;

    void Update()
    {
        hoverDisplay.SetActive(display || selected);
        selectedText.SetActive(selected);

        dominoRenderer.leftNum = domino.leftNum;
        dominoRenderer.rightNum = domino.rightNum;
        dominoRenderer.material = domino.material;
    }

    public void SetHover()
    {
        display = true;
        DeckMenuController.instance.HoverDomino(this.gameObject);
    }

    public void RemoveHover()
    {
        display = false;
        DeckMenuController.instance.ExitDomino(this.gameObject);
    }

    public void ToggleSelected()
    {
        DeckMenuController.instance.SetSelected(this.gameObject);
    }
}
