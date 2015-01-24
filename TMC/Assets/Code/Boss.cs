using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartRoutine();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartRoutine() {
        StartCoroutine(Routine());
    }

    IEnumerator Routine() {
        yield return new WaitForSeconds(2f);

        Globals._.BOSS_Beams.SetActive(true);

        yield return new WaitForSeconds(1f);

        Globals._.BOSS_Beams.SetActive(false);

        yield return new WaitForSeconds(1f);

        var smash = Instantiate(Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation) as GameObject;
        smash.GetComponent<AttackInfo>().Owner = this.gameObject;

        Camera.main.GetComponent<AudioSource>().PlayOneShot(Globals._.SOUND_Smash);

        yield return new WaitForSeconds(2f);

        smash = Instantiate(Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation) as GameObject;
        smash.GetComponent<AttackInfo>().Owner = this.gameObject;

        Camera.main.GetComponent<AudioSource>().PlayOneShot(Globals._.SOUND_Smash);        

        yield return new WaitForSeconds(1f);

        Debug.Log("Finiiish");
    }
}
