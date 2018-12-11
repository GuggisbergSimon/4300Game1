using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public enum Sound
    {
        HIT,
        JUMP,
        FIRE,
        BUBBLING,
        BUBBLE_POP,
        PICKUP
    }

    [SerializeField] private bool playMusic = true;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource[] sounds;

    // Self reference.
    private static SoundManager instance = null;
    public static SoundManager Instance => instance;

    public void PlaySound(Sound sound)
    {
        sounds[(int)sound].PlayDelayed(0);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (playMusic)
        {
            music.PlayDelayed(0);
        }
    }
}
