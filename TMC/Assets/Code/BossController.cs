using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[Serializable]
public enum AttackType {
    ConeUp,
    ConeDown,
    ConeLeft,
    ConeRight,
    BeamUp,
    BeamDown,
    BeamLeft,
    BeamRight,
    AreaOfEffect
}

[Serializable]
public class BossAttack {
    public float TimeFromStart;
    public AttackType Type;
}

public class BossController : JournalObject {

    [HideInInspector]
    public List<BossAttack> Attacks = new List<BossAttack>();

    public float ConeTime = 1;
    public float BeamTime = 1;

    private int index = 0;

    // Use this for initialization
    void Start() {
        Journal.OnStartRecording += Journal_OnStartRecording;
        Journal.OnFrame += Journal_OnFrame;
    }

    private void Journal_OnStartRecording( object sender, EventArgs e ) {
        foreach ( var item in Attacks ) {
            StartCoroutine( PrepAttack( item ) );
        }
    }

    private void Journal_OnFrame( object sender, Journal.JournalEventArgs e ) {
        if ( e.Mode == Journal.JournalMode.Playing ) {
            if ( e.Frame.Attacked ) {
                float time = Attacks[index].TimeFromStart;
                for ( int i = 0; i < Attacks.Count; i++ ) {
                    if ( Attacks[i].TimeFromStart == time ) {
                        DoAttack( Attacks[i] );
                        index++;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if ( Input.GetKeyDown( KeyCode.Space ) ) {
            Journal.Record();
        } else if ( Input.GetKeyDown( KeyCode.A ) ) {
            Journal.Play();
        }

        if ( Journal.Mode == Journal.JournalMode.Recording ) {
            // Movement stuff by controller can go in here if we want to
        }
    }

    private void DoAttack( BossAttack attack ) {
        switch ( attack.Type ) {
            case AttackType.ConeUp:
                Globals._.BOSS_ConeUp.transform.localScale = new Vector3( 7, 70, 70 );
                Globals._.BOSS_ConeUp.SetActive( true );
                iTween.ScaleTo( Globals._.BOSS_ConeUp, new Vector3( 70, 70, 70 ), ConeTime * .75f );
                Globals._.BOSS_ConeUp.DeactivateAfter( ConeTime );
                break;
            case AttackType.ConeDown:
                Globals._.BOSS_ConeDown.transform.localScale = new Vector3( 7, 70, 70 );
                Globals._.BOSS_ConeDown.SetActive( true );
                iTween.ScaleTo( Globals._.BOSS_ConeDown, new Vector3( 70, 70, 70 ), ConeTime * .75f );
                Globals._.BOSS_ConeDown.DeactivateAfter( ConeTime );
                break;
            case AttackType.ConeLeft:
                Globals._.BOSS_ConeLeft.transform.localScale = new Vector3( 75, 75, 7 );
                Globals._.BOSS_ConeLeft.SetActive( true );
                iTween.ScaleTo( Globals._.BOSS_ConeLeft, new Vector3( 75, 75, 75 ), ConeTime * .75f );
                Globals._.BOSS_ConeLeft.DeactivateAfter( ConeTime );
                break;
            case AttackType.ConeRight:
                Globals._.BOSS_ConeRight.transform.localScale = new Vector3( 75, 75, 7 );
                Globals._.BOSS_ConeRight.SetActive( true );
                iTween.ScaleTo( Globals._.BOSS_ConeRight, new Vector3( 75, 75, 75 ), ConeTime * .75f );
                Globals._.BOSS_ConeRight.DeactivateAfter( ConeTime );
                break;
            case AttackType.BeamUp:
                Globals._.BOSS_BeamUp.SetActive( true );
                Globals._.BOSS_BeamUp.DeactivateAfter( BeamTime );
                break;
            case AttackType.BeamDown:
                Globals._.BOSS_BeamDown.SetActive( true );
                Globals._.BOSS_BeamDown.DeactivateAfter( BeamTime );
                break;
            case AttackType.BeamLeft:
                Globals._.BOSS_BeamLeft.SetActive( true );
                Globals._.BOSS_BeamLeft.DeactivateAfter( BeamTime );
                break;
            case AttackType.BeamRight:
                Globals._.BOSS_BeamRight.SetActive( true );
                Globals._.BOSS_BeamRight.DeactivateAfter( BeamTime );
                break;
            case AttackType.AreaOfEffect:
                var smash = Instantiate( Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation ) as GameObject;
                smash.GetComponent<AttackInfo>().Owner = this.gameObject;
                break;
            default:
                break;
        }
    }

    private IEnumerator PrepAttack( BossAttack attack ) {
        yield return new WaitForSeconds( attack.TimeFromStart );
        // Omg, we're still GGJ'ing. It didn't work with just saying "Attacked = true"...
        GetComponent<JournalObject>().Attacked = true;
        DoAttack( attack );
    }

}
