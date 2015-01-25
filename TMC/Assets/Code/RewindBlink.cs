using UnityEngine;
using System.Collections;

public class RewindBlink : MonoBehaviour {

    private SpriteRenderer r;

    // Use this for initialization
    void Start() {
        r = GetComponent<SpriteRenderer>();
        r.enabled = false;
    }

    public void DoBlink() {
        Globals._.ccc.enabled = true;
        Globals._.nas.enabled = true;
        r.enabled = true;
        StartCoroutine( Blink( Mathf.RoundToInt( Globals._.TIME_TURN / 2f ) - 1 ) );
    }

    IEnumerator Blink( int times ) {
        var r = GetComponent<Renderer>();
        r.enabled = true;
        yield return new WaitForSeconds( 0.5f );
        r.enabled = false;

        if ( times > 0 ) {
            yield return new WaitForSeconds( 0.5f );
            StartCoroutine( Blink( times - 1 ) );
        } else {
            Globals._.ccc.enabled = false;
            Globals._.nas.enabled = false;
            r.enabled = false;
        }
    }
}
