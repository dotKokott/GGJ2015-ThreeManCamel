using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour {

    public float TimeToDestroy;
    public bool DestroyScript = true;

    // Use this for initialization
    void Start() {
        StartCoroutine( Execute() );
    }

    IEnumerator Execute() {
        if ( TimeToDestroy != 0 ) {
            yield return new WaitForSeconds( TimeToDestroy );
            if ( DestroyScript ) {
                Destroy( this );
            } else {
                Destroy( gameObject );
            }
        }
    }
}
