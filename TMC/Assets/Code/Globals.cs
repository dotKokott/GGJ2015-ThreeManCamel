using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {

    public GameObject PREFAB_SMASH;
    public GameObject PREFAB_FIREBALL;

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
