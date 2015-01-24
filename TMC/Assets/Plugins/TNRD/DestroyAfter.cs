using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour {

    public float TimeToDestroy;

    // Use this for initialization
    void Start() {
        StartCoroutine( Execute() );
    }

    IEnumerator Execute() {
        if ( TimeToDestroy != 0 ) {
            yield return new WaitForSeconds( TimeToDestroy );
            Destroy( gameObject );
        }
    }
}
