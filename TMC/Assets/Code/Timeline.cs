﻿using UnityEngine;
using System.Collections;

public class Timeline : MonoBehaviour {

    private GameObject[] players;
    private int index = 0;

    public float TurnTime = 7f;
    public float MarkerSpawnTime = 2f;

    public GameObject tlBarPrefab;
    private GameObject tlStart;
    private GameObject tlEnd;
    private GameObject tlBar;
    private bool isRewinding;


    // Use this for initialization
    void Start() {
        tlStart = GameObject.Find( "TL_Start" );
        tlEnd = GameObject.Find( "TL_End" );
        tlBar = GameObject.Find( "TL_Bar" );

        tlBar.transform.position = tlStart.transform.position;

        players = GameObject.FindGameObjectsWithTag( "Player" );
        foreach ( var item in players ) {
            item.GetComponent<Journal>().OnRewindFinished += J_OnRewindFinished;
        }

    }

    // Update is called once per frame
    void Update() {
        if ( Input.GetKeyDown( KeyCode.O ) || InputManager.GetButtonDown( 0, ButtonMapping.BUTTON_Y ) ) {
            StartCoroutine( RecordObject() );
        }
    }


    IEnumerator RecordObject() {
        var go = GameObject.FindGameObjectsWithTag( "TimelineMarker" );
        foreach ( var item in go ) {
            Destroy( item );
        }
        tlStart.transform.position = tlStart.transform.position;

        iTween.MoveTo( tlBar.gameObject, iTween.Hash( "position", tlEnd.transform.position, "time", 7, "easetype", iTween.EaseType.linear ) );
        isRewinding = false;
        StartCoroutine( PutMarker() );

        var p = players[index];
        var j = p.GetComponent<Journal>();

        j.Record();

        yield return new WaitForSeconds( TurnTime );
        j.Idle();
        isRewinding = true;
        //GetComponent<AudioSource>().PlayOneShot(Globals._.SOUND_Rewind);
        j.Play( true );
        iTween.MoveTo( tlBar.gameObject, iTween.Hash( "position", tlStart.transform.position, "time", 3, "easetype", iTween.EaseType.easeInOutExpo ) );
    }

    IEnumerator PutMarker() {
        yield return new WaitForSeconds( MarkerSpawnTime );

        if ( !isRewinding ) {
            var g = Instantiate( tlBarPrefab, tlBar.transform.position, tlBar.transform.rotation ) as GameObject;
            g.tag = "TimelineMarker";
            g.transform.localScale = new Vector3( 0.2f, 1, 1 );

            StartCoroutine( PutMarker() );
        }
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