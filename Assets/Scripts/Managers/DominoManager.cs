using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    [SerializeField] PrestigeUpgrade autoplayUpgrade;
    [SerializeField] PrestigeUpgrade scoreSpeed;

    [SerializeField] GameObject slideSoundHand;
    [SerializeField] GameObject slideSoundLine;
    [SerializeField] GameObject hitSound;
    [SerializeField] GameObject shuffleSound;

    public List<Domino> deck = new List<Domino>();

    public List<Domino> queue = new List<Domino>();
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
        if (deck.Count == 0)
        {
            CreateDeck();

            for (int i = 0; i < 1; i++)
            {
                CreateLine(i, 1, 0, 0);
            }
        }
        else
        {
            ShuffleQueue();
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

        if (timeSincePlayed >= autoplayTime * (10f / (autoplayUpgrade.level + 10f)) && !active)
        {
            AttemptPlay();
        }
    }

    public void TapPlay()
    {
        if (!active)
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
                domino.obj.transform.localPosition = new Vector3(0, -5, 0);
            }
            active = true;
            StartCoroutine(ResetShuffle());
            Instantiate(shuffleSound);
            return;
        }

        active = true;
        DominoScore.instance.scoreCombo = 0;
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
            Instantiate(slideSoundHand);
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
        if (domino.obj == null)
            yield break;
        line.dominoes.Insert(0, domino);
        GameObject dominoObj = domino.obj;
        dominoObj.GetComponent<Animator>().enabled = true;
        dominoObj.GetComponent<Animator>().Play("Base Layer.DominoExit");
        dominoObj.GetComponent<Animator>().speed = playMulti;
        bool dominoMatch = false;
        if (line.dominoes.Count > 1)
        {
            dominoMatch = GetComponent<DominoScore>().CheckMatch(line.dominoes[0], line.dominoes[1]);
        }
        dominoObj.GetComponent<Animator>().SetBool("scored", dominoMatch);

        if (domino.obj == null)
            yield break;
        yield return new WaitForSeconds(1 / playMulti);
        dominoObj.transform.SetParent(line.lineObject.transform);
        //Instantiate(slideSoundLine);
        if (domino.obj == null)
            yield break;
        yield return new WaitForSeconds((1 / playMulti) - 0.05f);

        Instantiate(hitSound);
        GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(1);

        yield return new WaitForSeconds(0.05f);

        TutorialController.instance.playedDomino = true;

        if (!dominoMatch)
        {
            dominoObj.GetComponent<Animator>().enabled = false;
            if (line.dominoes.Count > 1)
                GetComponent<DominoScore>().ScoreDominoes(line.dominoes[0], line.dominoes[1], line);
        }
        else
        {
            GetComponent<DominoScore>().ScoreDominoes(line.dominoes[0], line.dominoes[1], line);
            if (domino.obj == null)
                yield break;
            yield return new WaitForSeconds(1.5f);
            TutorialController.instance.dominoMatched = true;
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
        }
    }

    private void UpdateRing()
    {
        autoplayRing.GetComponent<RingController>().UpdateRing(autoplayTime * (10f / (autoplayUpgrade.level + 10f)), timeSincePlayed);
    }

    private void CreateDeck()
    {
        AddDomino(0, 0, "Plastic");
        AddDomino(0, 1, "Plastic");
        AddDomino(1, 1, "Plastic");
        AddDomino(2, 1, "Plastic");
        AddDomino(0, 2, "Plastic");
        AddDomino(2, 2, "Plastic");
    }

    public void AddDomino(int leftNum, int rightNum, string material)
    {
        AddDomino(leftNum, rightNum, material, true);
    }

    public void AddDomino(int leftNum, int rightNum, string material, bool addToDeck)
    {
        Domino newDomino = new Domino();

        newDomino.leftNum = leftNum;
        newDomino.rightNum = rightNum;
        newDomino.material = material;
        newDomino.obj = Instantiate(dominoPrefab, queueParent.transform);

        newDomino.obj.GetComponent<DominoRenderer>().leftNum = leftNum;
        newDomino.obj.GetComponent<DominoRenderer>().rightNum = rightNum;
        newDomino.obj.GetComponent<DominoRenderer>().material = material;

        queue.Add(newDomino);

        if (addToDeck)
            deck.Add(newDomino);

        if (DeckMenuController.instance != null)
            DeckMenuController.instance.UpdateDeckDisplay();
    }

    public void RemoveDomino(int index)
    {
        if (deck.Count <= 2)
            return;

        Domino removedDomino = deck[index];
        deck.RemoveAt(index);

        foreach (Line line in lines)
        {
            for (int i = line.dominoes.Count - 1; i >= 0; i--)
            {
                Destroy(line.dominoes[i].obj);
                line.dominoes.RemoveAt(i);
            }
        }

        for (int i = queue.Count - 1; i >= 0; i--)
        {
            Destroy(queue[i].obj);
            queue.RemoveAt(i);
        }

        foreach(Domino domino in deck)
        {
            AddDomino(domino.leftNum, domino.rightNum, domino.material, false);
        }

        if (DeckMenuController.instance != null)
            DeckMenuController.instance.UpdateDeckDisplay();

        StatManager.instance.SaveData();
    }

    public void CreateLine(int index, int multiLevel, int bonusLevel, int prestigeLevel)
    {
        Line newLine = new Line();
        newLine.lineObject = Instantiate(linePrefab);
        newLine.multiplier = multiLevel;
        newLine.additive = bonusLevel;
        newLine.prestige = prestigeLevel;
        newLine.index = index;
        lines.Add(newLine);

        GameObject newButtons = Instantiate(buttonsPrefab);
        newButtons.transform.SetParent(GameObject.Find("TextCanvas").transform, false);
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

    public string material;
}

public class Line
{
    public int index;
    public List<Domino> dominoes = new List<Domino>();
    public GameObject lineObject;
    public int multiplier;
    public int additive;
    public int prestige;
}