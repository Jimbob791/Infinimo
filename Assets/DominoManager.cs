using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoManager : MonoBehaviour
{
    [SerializeField] GameObject dominoPrefab;

    List<Domino> deck = new List<Domino>();

    List<Domino> queue = new List<Domino>();
    List<Domino> active = new List<Domino>();

    int deckSize;

    private void Start()
    {
        deckSize = CreateDeck(6);
        ShuffleDeck();

        for (int i = 0; i < deck.Count; i++)
        {
            Domino domino = deck[i];
            Domino newDomino = Instantiate(dominoPrefab).GetComponent<Domino>();
            newDomino.num1 = domino.num1;
            newDomino.num2 = domino.num2;
            newDomino.targetPos = new Vector3(6, -3.5f - (0.25f * i), 0);
            queue.Add(newDomino);
        }
    }

    private void Update()
    {
        foreach (Domino domino in queue)
        {
            domino.OnUpdate();
        }
    }

    private int CreateDeck(int deckSize)
    {
        int count = 0;
        for (int i = 0; i < deckSize + 1; i++)
        {
            for (int k = 0; k < i + 1; k++)
            {
                Domino newDomino = new Domino();
            
                newDomino.num1 = i;
                newDomino.num2 = k;

                deck.Add(newDomino);
                count++;
            }
        }
        return count;
    }

    private void ShuffleDeck()
    {  
        int i = 0;
        int t = deck.Count;
        int r = 0;
        Domino p = null;
        List<Domino> tempList = new List<Domino>();
        tempList.AddRange(deck);
     
        while (i < t)
        {
            r = Random.Range(i,tempList.Count);
            p = tempList[i];
            tempList[i] = tempList[r];
            tempList[r] = p;
            i++;
        }
     
        deck = tempList;
    }
}
