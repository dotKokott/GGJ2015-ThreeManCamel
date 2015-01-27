using UnityEngine;
using System.Collections;
using UnitySampleAssets.ImageEffects;

public class Globals : MonoBehaviour {

    public GameObject PREFAB_SMASH;    
    public GameObject PREFAB_STRIKE;
    public GameObject PREFAB_BEAM;
    public GameObject PREFAB_EXPLOSION;
    public GameObject PREFAB_PROTECTOR;

    public AudioClip SOUND_Rewind;
    public AudioClip SOUND_Beam;
    public AudioClip SOUND_Smash;
    public AudioClip SOUND_Fireball;
    public AudioClip SOUND_Explosion;

    public AudioClip MUSIC_SinglePlay;
    public AudioClip MUSIC_AllPlay;
    public AudioClip MUSIC_Reverse;
    public AudioClip MUSIC_Boss;

    public GameObject BOSS_BeamUp;
    public GameObject BOSS_BeamLeft;
    public GameObject BOSS_BeamRight;
    public GameObject BOSS_BeamDown;

    public GameObject BOSS_Smash;

    public GameObject BOSS_ConeUp;
    public GameObject BOSS_ConeLeft;
    public GameObject BOSS_ConeRight;
    public GameObject BOSS_ConeDown;

    public GameObject[] UI;

    public float TIME_TURN;
    public float TIME_MARKER_SPAWN;

    public AudioClip SFX_BIG_WOOSH;
    public AudioClip SFX_MAGE_ATTACK;
    public AudioClip SFX_AEO_SLAM;
    public AudioClip SFX_SWOOSH;
    public AudioClip SFX_SWOOSH2;
    public AudioClip SFX_JUMP;

    public AudioClip SFX_DIE;

    [HideInInspector]
    public NoiseAndScratches nas;
    [HideInInspector]
    public ColorCorrectionCurves ccc;

    [HideInInspector]
    public static Globals _;

	// Use this for initialization
	void Start () {
        _ = this;

        nas = Camera.main.GetComponent<NoiseAndScratches>();
        ccc = Camera.main.GetComponent<ColorCorrectionCurves>();
        nas.enabled = false;
        ccc.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
