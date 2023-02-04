using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NodeClickSounds : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    private AudioSource _audioSource;
    public static NodeClickSounds instance; 

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    public void PlaySound()
    {
        _audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
    }
}