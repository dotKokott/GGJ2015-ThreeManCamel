using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {


    public GameObject tlBarPrefab;
    private GameObject tlStart;
    private GameObject tlEnd;
    private GameObject tlBar;

	// Use this for initialization
	void Start () {

        tlStart = GameObject.Find("TL_Start");
        tlEnd = GameObject.Find("TL_End");
        tlBar = GameObject.Find("TL_Bar");

        StartRoutine();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartRoutine() {
        StartCoroutine(Routine());
        iTween.MoveTo(tlBar.gameObject, iTween.Hash("position", tlEnd.transform.position, "time", Globals._.TIME_TURN, "easetype", iTween.EaseType.linear));
        StartCoroutine(PutMarker());
    }
    private bool isDone = false;

    IEnumerator PutMarker() {
        yield return new WaitForSeconds(Globals._.TIME_MARKER_SPAWN);

        if (!isDone) {
            var g = Instantiate(tlBarPrefab, tlBar.transform.position, tlBar.transform.rotation) as GameObject;
            g.tag = "TimelineMarker";
            g.transform.localScale = new Vector3(0.2f, 1, 1);

            StartCoroutine(PutMarker());
        }
    }

    IEnumerator Routine() {
        yield return new WaitForSeconds(2f);

        //Globals._.BOSS_Beams.SetActive(true);

        yield return new WaitForSeconds(1f);

        //Globals._.BOSS_Beams.SetActive(false);

        yield return new WaitForSeconds(1f);

        var smash = Instantiate(Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation) as GameObject;
        smash.GetComponent<AttackInfo>().Owner = this.gameObject;

        Camera.main.GetComponent<AudioSource>().PlayOneShot(Globals._.SOUND_Smash);

        yield return new WaitForSeconds(2f);

        smash = Instantiate(Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation) as GameObject;
        smash.GetComponent<AttackInfo>().Owner = this.gameObject;

        Camera.main.GetComponent<AudioSource>().PlayOneShot(Globals._.SOUND_Smash);        

        yield return new WaitForSeconds(1f);

        isDone = true;
        Debug.Log("Finiiish");
    }
}
