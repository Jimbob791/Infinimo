using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPrefabController : MonoBehaviour
{
    [SerializeField] List<AudioClip> clips = new List<AudioClip>();
    [SerializeField] float minPitch = 1.0f;
    [SerializeField] float maxPitch = 1.0f;

    void Start()
    {
        GetComponent<AudioSource>().clip = clips[Random.Range(0, clips.Count)];
        GetComponent<AudioSource>().pitch = GetComponent<AudioSource>().pitch * Random.Range(minPitch, maxPitch);
        GetComponent<AudioSource>().Play();

        Destroy(this.gameObject, 10f);
    }
}
