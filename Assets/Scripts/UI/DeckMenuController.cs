using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckMenuController : MonoBehaviour
{
    public static DeckMenuController instance;

    [SerializeField] HeaderButtons headerButtons;

    [SerializeField] GameObject deckPrefab;
    [SerializeField] GameObject deckParent;
    [SerializeField] GameObject hoverSound;
    [SerializeField] GameObject deleteSound;
    [SerializeField] DominoTooltip dominoTooltip;
    List<GameObject> deckObjects = new List<GameObject>();
    int selectedIndex = -1;

    bool display;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        UpdateDeckDisplay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            headerButtons.CloseShop();
            headerButtons.CloseUpgrade();
            CloseDeck();
        }
    }

    public void UpdateDeckDisplay()
    {
        if (DominoManager.instance == null)
            return;

        selectedIndex = -1;

        for (int i = deckObjects.Count - 1; i >= 0; i--)
        {
            Destroy(deckObjects[i]);
            deckObjects.RemoveAt(i);
        }

        foreach (Domino domino in DominoManager.instance.deck)
        {
            Debug.Log(domino.leftNum + " - " + domino.rightNum + " " + domino.material);
            GameObject newObj = Instantiate(deckPrefab, deckParent.transform);
            newObj.GetComponent<DeckPrefabController>().domino = domino;

            deckObjects.Add(newObj);
        }
    }

    public void SetSelected(GameObject clicked)
    {
        for (int i = 0; i < deckObjects.Count; i++)
        {
            if (deckObjects[i] == clicked)
            {
                deckObjects[i].GetComponent<DeckPrefabController>().selected = true;
                selectedIndex = i;
            }
            else
            {
                deckObjects[i].GetComponent<DeckPrefabController>().selected = false;
            }
        }
    }

    public void HoverDomino(GameObject hoveredObj)
    {
        int selectedIndex = 0;

        for (int i = 0; i < deckObjects.Count; i++)
        {
            if (deckObjects[i] == hoveredObj)
            {
                selectedIndex = i;
                break;
            }
        }

        Instantiate(hoverSound);

        dominoTooltip.DisplayTooltip(DominoManager.instance.deck[selectedIndex]);
    }

    public void ExitDomino(GameObject hoveredObj)
    {
        dominoTooltip.CloseTooltip();
    }

    public void RemoveSelected()
    {
        if (selectedIndex == -1 || ChipManager.instance.chips < 1)
            return;

        ChipManager.instance.chips -= 1;
        Instantiate(deleteSound);
        DominoManager.instance.RemoveDomino(selectedIndex);
    }

    public void ToggleActive()
    {
        StatManager.instance.Click();
        display = !display;
        GetComponent<Animator>().SetBool("active", display);

        if (display == true)
        {
            headerButtons.CloseShop();
            headerButtons.CloseUpgrade();
        }

    }

    public void CloseDeck()
    {
        display = false;
        GetComponent<Animator>().SetBool("active", display);
    }
}
