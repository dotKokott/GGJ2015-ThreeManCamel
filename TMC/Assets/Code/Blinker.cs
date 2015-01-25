using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class Blinker : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine( Blink( 7 ) );
	}

    IEnumerator Blink( int times ) {
        var r = transform.Find( "Model" ).GetComponentInChildren<Renderer>();
        r.enabled = false;
        yield return new WaitForSeconds( 0.02f );
        r.enabled = true;

        if ( times > 0 ) {
            yield return new WaitForSeconds( 0.1f );
            StartCoroutine( Blink( times - 1 ) );
        } else {
            Destroy( gameObject );
        }
    }
}
