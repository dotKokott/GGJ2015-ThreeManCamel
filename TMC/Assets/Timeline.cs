using UnityEngine;
using System.Collections;

public class Timeline : MonoBehaviour {

    private GameObject[] players;
    private int index = 0;

    // Use this for initialization
    void Start() {
        players = GameObject.FindGameObjectsWithTag( "Player" );
        foreach ( var item in players ) {
            item.GetComponent<Journal>().OnRewindFinished += J_OnRewindFinished;
        }
    }

    // Update is called once per frame
    void Update() {
        if ( Input.GetKeyDown( KeyCode.O ) ) {
            StartCoroutine( RecordObject() );
        }
    }

    IEnumerator RecordObject() {
        var p = players[index];
        var j = p.GetComponent<Journal>();

        j.Record();

        yield return new WaitForSeconds( 2.5f );
        j.Idle();
        yield return new WaitForSeconds( 1 );
        j.Play( true );
    }

    private void J_OnRewindFinished( object sender, System.EventArgs e ) {
        if ( index < players.Length - 1 ) {
            index++;
            StartCoroutine( RecordObject() );
        } else {
            foreach ( var item in players ) {
                var j = item.GetComponent<Journal>();
                j.Play();
            }

            index = 0;
        }
    }
}
