using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoManager : MonoBehaviour
{
    public float playMulti = 1;

    [SerializeField] GameObject dominoPrefab;
    [SerializeField] GameObject queueParent;
    [SerializeField] GameObject lineParent;

    List<Domino> deck = new List<Domino>();

    List<Domino> queue = new List<Domino>();
    List<Domino> line = new List<Domino>();

    int deckSize;

    bool active = false;
    Domino activeDomino;

    private void Start()
    {
        deckSize = CreateDeck(3);
        queue = deck;
        ShuffleQueue();
    }

    private void Update()
    {
        UpdateDominoes();

        if (Input.GetKeyDown(KeyCode.Space) && !active)
        {
            if (queue.Count == 0)
            {
                for (int i = line.Count - 1; i > -1; i--)
                {
                    queue.Add(line[i]);
                    line.RemoveAt(i);
                    ShuffleQueue();
                    foreach (Domino domino in queue)
                    {
                        domino.obj.transform.SetParent(queueParent.transform);
                    }
                }

                return;
            }

            active = true;
            activeDomino = queue[0];
            activeDomino.obj.GetComponent<DominoController>().move = false;
            queue.RemoveAt(0);
            StartCoroutine(MoveActiveDomino());
        }
    }

    IEnumerator MoveActiveDomino()
    {
        line.Insert(0, activeDomino);
        GameObject dominoObj = activeDomino.obj;
        dominoObj.GetComponent<Animator>().enabled = true;

        dominoObj.GetComponent<Animator>().speed = playMulti;

        yield return new WaitForSeconds(1 / playMulti);
        dominoObj.transform.SetParent(lineParent.transform);
        yield return new WaitForSeconds(1 / playMulti);

        dominoObj.GetComponent<Animator>().enabled = false;
        dominoObj.transform.position = Vector3.zero;
        if (line.Count > 1)
            GetComponent<DominoScore>().ScoreDominoes(line[0], line[1]);

        activeDomino.obj.GetComponent<DominoController>().move = true;
        activeDomino = null;
        active = false;
    }

    private void UpdateDominoes()
    {
        for (int i = 0; i < queue.Count; i++)
        {
            Domino domino = queue[i];
            domino.obj.GetComponent<DominoController>().targetPos = new Vector3(0, -0.25f * i, 0);

            domino.obj.GetComponent<DominoRenderer>().isInQueue = i == 0 ? false : true;
        }

        for (int i = 0; i < line.Count; i++)
        {
            Domino domino = line[i];
            domino.obj.GetComponent<DominoController>().targetPos = new Vector3(-2.25f * i, 0, 0);

            domino.obj.GetComponent<DominoRenderer>().isInQueue = false;
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

                newDomino.leftNum = i;
                newDomino.rightNum = k;
                newDomino.obj = Instantiate(dominoPrefab, queueParent.transform);

                deck.Add(newDomino);
                count++;
            }
        }
        return count;
    }

    private void ShuffleQueue()
    {  
        int i = 0;
        int t = queue.Count;
        int r = 0;
        Domino p = null;
        List<Domino> tempList = new List<Domino>();
        tempList.AddRange(queue);
     
        while (i < t)
        {
            r = Random.Range(i, tempList.Count);
            p = tempList[i];
            tempList[i] = tempList[r];
            tempList[r] = p;
            i++;
        }

        queue = tempList;

        for (int k = 0; k < queue.Count; k++)
        {
            if (Random.value <= 0.5f)
            {
                Domino flippedDomino = new Domino();
                flippedDomino.leftNum = queue[k].rightNum;
                flippedDomino.rightNum = queue[k].leftNum;
                flippedDomino.obj = queue[k].obj;
                queue[k] = flippedDomino;
            }
        }

        foreach (Domino domino in queue)
        {
            domino.obj.GetComponent<DominoRenderer>().leftNum = domino.leftNum;
            domino.obj.GetComponent<DominoRenderer>().rightNum = domino.rightNum;
        }
    }
}

public class Domino
{
    public int leftNum;
    public int rightNum;
    public GameObject obj;
}