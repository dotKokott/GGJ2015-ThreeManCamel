using UnityEngine;
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
