using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NodeClickSounds : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private List<AudioClip> blackClips;
    [SerializeField] private AudioClip connectedClip;
    [SerializeField] private AudioClip disconnectedClip;
    [SerializeField] private AudioClip flux;
    
    private AudioSource _audioSource;
    public static NodeClickSounds instance;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        instance = this;
    }

    public void PlayBlackSound(int left)
    {
        _audioSource.PlayOneShot(blackClips[3-left]);

    }

    public void PlayRedToGreenSound()
    {
        _audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
    }
}