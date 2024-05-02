using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoManager : MonoBehaviour
{
    public static DominoManager instance;

    public float playMulti = 1;
    public float autoplayTime = 5;

    [SerializeField] GameObject dominoPrefab;
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject queueParent;
    [SerializeField] GameObject buttonsPrefab;

    [SerializeField] GameObject autoplayRing;

    List<Domino> deck = new List<Domino>();

    List<Domino> queue = new List<Domino>();
    [HideInInspector] public List<Line> lines = new List<Line>();

    int deckSize;

    bool active = false;
    Domino activeDomino;
    float timeSincePlayed;

    private void Awake()
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

    private void Start()
    {
        deckSize = CreateDeck(3);
        queue = deck;
        ShuffleQueue();

        for (int i = 0; i < 1; i++)
        {
            CreateLine();
        }
    }

    private void Update()
    {
        timeSincePlayed += Time.deltaTime;

        UpdateDominoes();
        UpdateLines();

        UpdateRing();

        if (Input.GetKeyDown(KeyCode.Space) && !active)
        {
            AttemptPlay();
        }

        if (timeSincePlayed >= autoplayTime && !active)
        {
            AttemptPlay();
        }
    }

    private void AttemptPlay()
    {
        timeSincePlayed = 0;
        if (queue.Count == 0)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                for (int k = lines[i].dominoes.Count - 1; k > -1; k--)
                {
                    queue.Add(lines[i].dominoes[k]);
                    lines[i].dominoes.RemoveAt(k);
                }
            }
            
            ShuffleQueue();
            foreach (Domino domino in queue)
            {
                domino.obj.transform.SetParent(queueParent.transform);
            }
            active = true;
            StartCoroutine(ResetShuffle());
            return;
        }

        active = true;
        StartCoroutine(PlayLines());
    }

    IEnumerator ResetShuffle()
    {
        yield return new WaitForSeconds(1);
        active = false;
    }

    IEnumerator PlayLines()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            if (queue.Count == 0)
            {
                break;
            }
            activeDomino = queue[0];
            activeDomino.obj.GetComponent<DominoController>().move = false;
            queue.RemoveAt(0);
            StartCoroutine(PlayDomino(lines[i], activeDomino));

            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(2 / playMulti + 0.25f);
        active = false;
    }

    IEnumerator PlayDomino(Line line, Domino domino)
    {
        line.dominoes.Insert(0, domino);
        GameObject dominoObj = domino.obj;
        dominoObj.GetComponent<Animator>().enabled = true;
        dominoObj.GetComponent<Animator>().Play("Base Layer.DominoExit");
        dominoObj.GetComponent<Animator>().speed = playMulti;
        bool dominoMatch = false;
        if (line.dominoes.Count > 1)
            dominoMatch = GetComponent<DominoScore>().CheckMatch(line.dominoes[0], line.dominoes[1]);
        dominoObj.GetComponent<Animator>().SetBool("scored", dominoMatch);

        yield return new WaitForSeconds(1 / playMulti);
        dominoObj.transform.SetParent(line.lineObject.transform);
        yield return new WaitForSeconds(1 / playMulti);
        
        if (!dominoMatch)
            dominoObj.GetComponent<Animator>().enabled = false;
        else
        {
            GetComponent<DominoScore>().ScoreDominoes(line.dominoes[0], line.dominoes[1], line);
            yield return new WaitForSeconds(1.5f);
            dominoObj.GetComponent<Animator>().enabled = false;
        }

        dominoObj.transform.localPosition = Vector3.zero;
        dominoObj.GetComponent<DominoController>().move = true;
    }

    private void UpdateDominoes()
    {
        for (int i = 0; i < queue.Count; i++)
        {
            Domino domino = queue[i];
            domino.obj.GetComponent<DominoController>().targetPos = new Vector3(0, -0.25f * i, 0);

            domino.obj.GetComponent<DominoRenderer>().isInQueue = i == 0 ? false : true;
        }

        foreach (Line line in lines)
        {
            for (int i = 0; i < line.dominoes.Count; i++)
            {
                Domino domino = line.dominoes[i];
                domino.obj.GetComponent<DominoController>().targetPos = new Vector3(-2.25f * i, 0, 0);

                domino.obj.GetComponent<DominoRenderer>().isInQueue = false;
            }
        }
    }

    private void UpdateLines()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            Vector3 targetPos = new Vector3(0, 4 - (i * 2), 0);
            lines[i].lineObject.transform.localPosition = targetPos;
            //Vector3.Lerp(lines[i].lineObject.transform.position, targetPos, 2 * Time.deltaTime);
        }
    }

    private void UpdateRing()
    {
        autoplayRing.GetComponent<RingController>().UpdateRing(autoplayTime, timeSincePlayed);
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

    public void CreateLine()
    {
        Line newLine = new Line();
        newLine.lineObject = Instantiate(linePrefab);
        newLine.multiplier = 1;
        newLine.additive = 0;
        newLine.index = lines.Count;
        lines.Add(newLine);

        GameObject newButtons = Instantiate(buttonsPrefab);
        newButtons.transform.SetParent(GameObject.Find("MainCanvas").transform);
        newButtons.GetComponent<LineButtons>().line = newLine;
        UpdateLines();
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

public class Line
{
    public int index;
    public List<Domino> dominoes = new List<Domino>();
    public GameObject lineObject;
    public int multiplier;
    public int additive;
}