using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour {

    public float StayTime;

	// Use this for initialization
	void Start () {
        StartCoroutine(Kill());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator Kill() {
        yield return new WaitForSeconds(StayTime);

        Destroy(this.gameObject);
    }
}
