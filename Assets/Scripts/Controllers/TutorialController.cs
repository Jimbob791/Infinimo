using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance;

    private bool tutorial;

    int phase = 0;
    float timeSinceLastPhase;

    public bool playedDomino = false;
    public bool dominoMatched = false;

    Animator anim;

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

    void Start()
    {
        anim = GetComponent<Animator>();
        tutorial = StatManager.instance.firstPlay;
    }

    void Update()
    {
        anim.SetInteger("phase", phase);
        timeSinceLastPhase += Time.unscaledDeltaTime;

        if (phase == 0)
        {
            if (tutorial && timeSinceLastPhase > 2f)
            {
                timeSinceLastPhase = 0;
                phase = 1;
                Time.timeScale = 0;
                return;
            }
        }
        else if (phase == 1)
        {
            if (Input.anyKeyDown && timeSinceLastPhase > 2f)
            {
                timeSinceLastPhase = 0;
                phase = 2;
                return;
            }
        }
        else if (phase == 2)
        {
            if (Input.anyKeyDown && timeSinceLastPhase > 2f)
            {
                timeSinceLastPhase = 0;
                StartCoroutine(ResetTimeScale());
                phase = 3;
                return;
            }
        }
        else if (phase == 3)
        {
            if (playedDomino)
            {
                timeSinceLastPhase = 0;
                Time.timeScale = 0;
                phase = 4;
                return;
            }
        }
        else if (phase == 4)
        {
            if (Input.anyKeyDown && timeSinceLastPhase > 2f)
            {
                timeSinceLastPhase = 0;
                StartCoroutine(ResetTimeScale());
                phase = 5;
                return;
            }
        }
        else if (phase == 5)
        {
            if (dominoMatched)
            {
                timeSinceLastPhase = 0;
                Time.timeScale = 0;
                phase = 6;
                return;
            }
        }
        else if (phase == 6)
        {
            if (Input.anyKeyDown && timeSinceLastPhase > 2f)
            {
                timeSinceLastPhase = 0;
                StartCoroutine(ResetTimeScale());
                phase = 7;
                return;
            }
        }
        else if (phase == 7)
        {
            if (DominoScore.instance.score > 0 && timeSinceLastPhase > 2f)
            {
                timeSinceLastPhase = 0;
                Time.timeScale = 0;
                phase = 8;
                return;
            }
        }
        else if (phase == 8)
        {
            if (Input.anyKeyDown && timeSinceLastPhase > 2f)
            {
                timeSinceLastPhase = 0;
                StartCoroutine(ResetTimeScale());
                phase = 9;
                return;
            }
        }
        else if (phase == 9)
        {
            if (DominoScore.instance.score > 20 && timeSinceLastPhase > 2f)
            {
                timeSinceLastPhase = 0;
                Time.timeScale = 0;
                phase = 10;
                return;
            }
        }
        else if (phase == 10)
        {
            if (Input.anyKeyDown && timeSinceLastPhase > 2f)
            {
                timeSinceLastPhase = 0;
                StartCoroutine(ResetTimeScale());
                phase = 11;
                return;
            }
        }
        else if (phase == 11)
        {
            if (DominoScore.instance.score > 50 && timeSinceLastPhase > 2f)
            {
                timeSinceLastPhase = 0;
                Time.timeScale = 0;
                phase = 12;
                return;
            }
        }
        else if (phase == 12)
        {
            if (Input.anyKeyDown && timeSinceLastPhase > 2f)
            {
                timeSinceLastPhase = 0;
                StartCoroutine(ResetTimeScale());
                phase = 13;
                return;
            }
        }
        else if (phase == 13)
        {
            if (Input.anyKeyDown && timeSinceLastPhase > 2f)
            {
                timeSinceLastPhase = 0;
                StartCoroutine(ResetTimeScale());
                phase = 14;
                return;
            }
        }
    }

    IEnumerator ResetTimeScale()
    {
        yield return new WaitForSecondsRealtime(0.75f);
        Time.timeScale = 1;
    }
}
