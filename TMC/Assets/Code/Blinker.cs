using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class Blinker : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine( Blink( 7 ) );
	}

    IEnumerator Blink( int times ) {
        var r = transform.Find( "Model" ).GetComponentsInChildren<Renderer>();

        foreach ( var item in r ) {
            item.enabled = false;
        }
        yield return new WaitForSeconds( 0.01f );
        foreach ( var item in r ) {
            item.enabled = true;
        }

        if ( times > 0 ) {
            yield return new WaitForSeconds( 0.08f );
            StartCoroutine( Blink( times - 1 ) );
        } else {
            Destroy( gameObject );
        }
    }
}
