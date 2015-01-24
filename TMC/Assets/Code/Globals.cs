﻿using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {

    public GameObject PREFAB_SMASH;
    public GameObject PREFAB_FIREBALL;
    public GameObject PREFAB_BEAM;
    public GameObject PREFAB_EXPLOSION;

    public AudioClip SOUND_Rewind;
    public AudioClip SOUND_Beam;
    public AudioClip SOUND_Smash;
    public AudioClip SOUND_Fireball;
    public AudioClip SOUND_Explosion;

    public AudioClip MUSIC_SinglePlay;
    public AudioClip MUSIC_AllPlay;
    public AudioClip MUSIC_Reverse;
    public AudioClip MUSIC_Boss;


    [HideInInspector]
    public static Globals _;

	// Use this for initialization
	void Start () {
        _ = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
