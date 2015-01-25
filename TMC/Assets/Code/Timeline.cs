using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Timeline : MonoBehaviour {

    private GameObject[] players;
    private GameObject boss;
    private int index = 0;

    public float TurnTime = 7f;
    public float MarkerSpawnTime = 2f;

    public GameObject tlBarPrefab;
    private GameObject tlStart;
    private GameObject tlEnd;
    private GameObject tlBar;
    private bool isRewinding;

    private int playersInLevel = 0;
    private bool gameOver = false;
    private bool won = false;

    private int playIndex = -1;

    // Use this for initialization
    void Start() {
        Globals._.TIME_MARKER_SPAWN = MarkerSpawnTime;
        Globals._.TIME_TURN = TurnTime;

        tlStart = GameObject.Find( "TL_Start" );
        tlEnd = GameObject.Find( "TL_End" );
        tlBar = GameObject.Find( "TL_Bar" );

        tlBar.transform.position = tlStart.transform.position;

        //SUPER DIRTY
        var _players = GameObject.FindGameObjectsWithTag( "Player" );
        players = GameObject.FindGameObjectsWithTag( "Player" );

        boss = GameObject.Find( "Boss" );
        boss.GetComponent<Journal>().OnRewindFinished += Boss_OnRewindFinished;
        boss.GetComponent<Journal>().OnPlayFinished += Boss_OnPlayFinished;

        foreach ( var item in _players ) {
            var order = item.GetComponent<Controller>().Order;
            players[order] = item;
        }

        foreach ( var item in players ) {
            item.GetComponent<Journal>().OnRewindFinished += J_OnRewindFinished;
            item.SetActive( false );
        }

        playersInLevel = players.Length;

        EnableUIObject( "Panel" );
        EnableUIObject( "Next Turn" );
        EnableUIObject( "Image" );
        EnableUIObject( "Title 2" );
    }

    bool isRecording = false;

    // Update is called once per frame
    void Update() {
        if ( Input.GetKeyDown( KeyCode.O ) || InputManager.GetButtonDown( 0, ButtonMapping.BUTTON_Y ) ) {
            if ( gameOver ) {
                if ( won ) {
                    Application.LoadLevel( Application.loadedLevel + 1 );
                } else {
                    Application.LoadLevel( Application.loadedLevel );
                }
            } else {
                DisableUI();

                if ( !isRecording ) {
                    if ( playIndex == -1 ) {
                        StartCoroutine( RecordBoss() );
                    } else {
                        StartCoroutine( RecordObject() );
                    }
                }
            }
        } else if ( InputManager.GetButtonDown( 0, ButtonMapping.BUTTON_BACK ) ) {
            Application.LoadLevel( Application.loadedLevel );
        }
    }

    private void MoveTimelineForward() {
        tlBar.transform.position = tlStart.transform.position;
        iTween.MoveTo( tlBar, iTween.Hash( "position", tlEnd.transform.position, "time", TurnTime, "easetype", iTween.EaseType.linear ) );
    }

    private void MoveTimelineBack() {
        iTween.Stop( tlBar );
        iTween.MoveTo( tlBar, iTween.Hash( "position", tlStart.transform.position, "time", TurnTime / 2, "easetype", iTween.EaseType.easeInOutExpo ) );
    }

    IEnumerator RecordBoss() {
        isRecording = true;

        MoveTimelineForward();
        var j = boss.GetComponent<Journal>();

        isRewinding = false;
        //StartCoroutine( PutMarker() );

        var music = GameObject.Find( "Music" ).GetComponent<AudioSource>();
        music.clip = Globals._.MUSIC_Boss;
        music.Play();

        j.Record();
        yield return new WaitForSeconds( TurnTime );

        isRewinding = true;
        music.clip = Globals._.MUSIC_Reverse;
        music.Play();
        j.Play( true );
        MoveTimelineBack();
    }

    IEnumerator RecordObject() {
        isRecording = true;

        MoveTimelineForward();

        var p = players[index];
        var j = p.GetComponent<Journal>();

        var music = GameObject.Find( "Music" ).GetComponent<AudioSource>();
        music.clip = Globals._.MUSIC_SinglePlay;
        music.Play();

        j.Record();
        yield return new WaitForSeconds( TurnTime );

        music.clip = Globals._.MUSIC_Reverse;
        music.Play();
        j.Play( true );
        MoveTimelineBack();
    }

    public void PutMarker() {
        //yield return new WaitForSeconds( MarkerSpawnTime );

        //if ( !isRewinding ) {
        var g = Instantiate( tlBarPrefab, tlBar.transform.position, tlBar.transform.rotation ) as GameObject;
        g.tag = "TimelineMarker";
        g.transform.localScale = new Vector3( 0.2f, 1, 1 );

        //StartCoroutine( PutMarker() );
        //}

        //return null;
    }

    private void DoRewind() {

    }

    private void EnableUIObject( string name ) {
        foreach ( var item in Globals._.UI ) {
            if ( item.name == name ) {
                item.SetActive( true );
                return;
            }
        }
    }

    private void DisableUI() {
        foreach ( var item in Globals._.UI ) {
            item.SetActive( false );
        }
    }

    private void Boss_OnPlayFinished( object sender, System.EventArgs e ) {
        StartCoroutine( BossASD() );
    }

    private IEnumerator BossASD() {
        yield return new WaitForSeconds( 1.3f );
        gameOver = true;

        EnableUIObject( "Panel" );
        EnableUIObject( "Image 1" );
        if ( Application.loadedLevelName == "Level3" ) {
            if ( GameObject.FindGameObjectsWithTag( "Player" ).Length > 0 ) {
                won = true;
                EnableUIObject( "Victory" );
                EnableUIObject( "Victory 1" );
            } else {
                won = false;
                if ( UnityEngine.Random.Range( 0, 100 ) < 50 ) {
                    EnableUIObject( "NoBoss" );
                    EnableUIObject( "NoBoss 1" );
                } else {
                    EnableUIObject( "Death" );
                    EnableUIObject( "Death 1" );
                }
            }
        } else {
            if ( boss.GetComponentInChildren<BossController>().Health == 0 ) {
                won = true;
                EnableUIObject( "Victory" );
                EnableUIObject( "Victory 1" );
            } else {
                won = false;
                if ( UnityEngine.Random.Range( 0, 100 ) < 50 ) {
                    EnableUIObject( "NoBoss" );
                    EnableUIObject( "NoBoss 1" );
                } else {
                    EnableUIObject( "Death" );
                    EnableUIObject( "Death 1" );
                }
            }
        }

    }

    private void Boss_OnRewindFinished( object sender, System.EventArgs e ) {
        isRecording = false;

        var music = GameObject.Find( "Music" ).GetComponent<AudioSource>();
        music.Stop();

        boss.SetActive( false );
        players[0].SetActive( true );
        playIndex++;

        EnableUIObject( "Panel" );
        EnableUIObject( "Next Turn" );
        EnableUIObject( "Image" );
        EnableUIObject( "Title" );
        EnableUIObject( "Title 1" );

        if ( players[index].name == "Player1" ) {
            GameObject.Find( "Title" ).GetComponent<Text>().text = "Wizard";
        } else if ( players[index].name == "Player2" ) {
            GameObject.Find( "Title" ).GetComponent<Text>().text = "Knight";
        } else if ( players[index].name == "Player3" ) {
            GameObject.Find( "Title" ).GetComponent<Text>().text = "Protector";
        }
    }

    private void J_OnRewindFinished( object sender, System.EventArgs e ) {
        isRecording = false;

        var music = GameObject.Find( "Music" ).GetComponent<AudioSource>();
        music.Stop();

        if ( index < players.Length - 1 ) {
            players[index].SetActive( false );

            index++;
            playIndex++;

            players[index].SetActive( true );


            EnableUIObject( "Panel" );
            EnableUIObject( "Next Turn" );
            EnableUIObject( "Image" );
            EnableUIObject( "Title" );
            EnableUIObject( "Title 1" );

            if ( players[index].name == "Player1" ) {
                GameObject.Find( "Title" ).GetComponent<Text>().text = "Wizard";
            } else if ( players[index].name == "Player2" ) {
                GameObject.Find( "Title" ).GetComponent<Text>().text = "Knight";
            } else if ( players[index].name == "Player3" ) {
                GameObject.Find( "Title" ).GetComponent<Text>().text = "Protector";
            }
        } else {
            foreach ( var item in players ) {
                item.SetActive( true );
                boss.SetActive( true );

                //var music = GameObject.Find( "Music" ).GetComponent<AudioSource>();
                music.clip = Globals._.MUSIC_AllPlay;
                music.Play();

                var j = item.GetComponent<Journal>();
                j.Play();
            }

            boss.GetComponent<Journal>().Play();
            MoveTimelineForward();

            index = 0;
        }
    }
}
