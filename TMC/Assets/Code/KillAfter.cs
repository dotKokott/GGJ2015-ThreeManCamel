using UnityEngine;
using System.Collections;

public class KillAfter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine( Kill() );
	}

    IEnumerator Kill() {
        yield return new WaitForSeconds( 1 );
        Destroy( gameObject );
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
