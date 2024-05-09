using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckMenuController : MonoBehaviour
{
    public static DeckMenuController instance;

    [SerializeField] GameObject deckPrefab;
    [SerializeField] GameObject deckParent;
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

    public void RemoveSelected()
    {
        if (selectedIndex == -1)
            return;

        DominoManager.instance.RemoveDomino(selectedIndex);
    }

    public void ToggleActive()
    {
        display = !display;
        GetComponent<Animator>().SetBool("active", display);
    }
}
