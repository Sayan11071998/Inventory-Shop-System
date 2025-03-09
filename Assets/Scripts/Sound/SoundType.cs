using System;
using UnityEngine;

[Serializable]
public class SoundType
{
    public Sounds sound;
    public AudioClip soundClip;

    [Range(1, 100)]
    public int volume;
}