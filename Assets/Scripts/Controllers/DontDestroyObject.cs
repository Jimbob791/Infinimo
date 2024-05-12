using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    public static DontDestroyObject instance;

    [SerializeField] AudioClip song1;
    [SerializeField] AudioClip song2;

    float timer;
    int song = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            song = Random.Range(0, 2);
            song = song == 0 ? 0 : 1;
            GetComponent<AudioSource>().clip = song == 0 ? song1 : song2;
            GetComponent<AudioSource>().Play();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= GetComponent<AudioSource>().clip.length)
        {
            song = song == 0 ? 1 : 0;
            timer = 0;
            GetComponent<AudioSource>().clip = song == 0 ? song1 : song2;
            GetComponent<AudioSource>().Play();
        }
    }
}
