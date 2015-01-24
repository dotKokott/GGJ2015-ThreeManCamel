using UnityEngine;
using System.Collections;

public class DeactivateAfter : MonoBehaviour {

    public float TimeToDeactivate = 0;

    // Use this for initialization
    void Start() {
        StartCoroutine( Execute() );
    }

    IEnumerator Execute() {
        if ( TimeToDeactivate != 0 ) {
            yield return new WaitForSeconds( TimeToDeactivate );
            gameObject.SetActive( false );
        }
    }
}
