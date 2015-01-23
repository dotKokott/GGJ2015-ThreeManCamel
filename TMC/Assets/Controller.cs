using UnityEngine;
using System.Collections;

public class Controller : JournalObject {

    // Use this for initialization
    void Start() {
        renderer = GetComponent<Renderer>();

        IsAlive = true;
    }

    // Update is called once per frame
    void Update() {
        if ( Input.GetKeyDown( KeyCode.Space ) ) {
            Journal.Play();
        } else if ( Input.GetKeyDown( KeyCode.LeftShift ) ) {
            Journal.Play( true );
        } else if ( Input.GetKeyDown( KeyCode.LeftControl ) ) {
            Journal.Record( true );
        } else if ( Input.GetKeyDown( KeyCode.RightShift ) ) {
            Journal.Mode = Journal.JournalMode.Idling;
        }

        if ( Journal.Mode == Journal.JournalMode.Recording ) {
            if ( !IsAlive ) return;

            var v = Input.GetAxis( "Vertical" );
            var h = Input.GetAxis( "Horizontal" );

            transform.position += new Vector3( h * 5, v * 5 ) * Time.deltaTime;
        }
    }
}
