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
    AreaOfEffect,
    RotatingBeams,
    SingleRotatingBeam
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

    public int Health = 5;

	public Animation animation;

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
        } else if ( Journal.Mode == Journal.JournalMode.Playing ) {
            protectionTimer -= Time.deltaTime;
        }
    }

    float protectionTimer = 0;

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
				animation.Play ("Cone Attack", AnimationPlayMode.Stop);
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
			animation.Play ("Line Attack", AnimationPlayMode.Stop);
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
				animation.Play ("AOE", AnimationPlayMode.Stop);
                break;
            case AttackType.RotatingBeams:
                Globals._.BOSS_BeamUp.SetActive( true );
                Globals._.BOSS_BeamDown.SetActive( true );
                Globals._.BOSS_BeamLeft.SetActive( true );
                Globals._.BOSS_BeamRight.SetActive( true );

                StartCoroutine( rotateBeams() );

                break;
            case AttackType.SingleRotatingBeam:
                Globals._.BOSS_BeamUp.SetActive( true );
                StartCoroutine( rotateBeams() );

                break;
            default:
                break;
        }
    }

    private IEnumerator rotateBeams() {
        yield return new WaitForSeconds( 1f );

        var beams = GameObject.Find( "Beams" );
        var rotator = beams.GetComponent<RotateBeams>();
        rotator.Rotate = true;

        yield return new WaitForSeconds( 6f );

        rotator.Reset();

        Globals._.BOSS_BeamUp.SetActive( false );
        Globals._.BOSS_BeamDown.SetActive( false );
        Globals._.BOSS_BeamLeft.SetActive( false );
        Globals._.BOSS_BeamRight.SetActive( false );
    }

    private IEnumerator PrepAttack( BossAttack attack ) {
        yield return new WaitForSeconds( attack.TimeFromStart );
        // Omg, we're still GGJ'ing. It didn't work with just saying "Attacked = true"...
        GetComponent<JournalObject>().Attacked = true;
        DoAttack( attack );
    }

    private List<GameObject> collidersAlreadyHit = new List<GameObject>();

    void OnTriggerStay( Collider collider ) {
        if (collider.name.Contains("Protector")) {
            protectionTimer = 0.2f;
        } else {
            var comp = collider.GetComponent<AttackInfo>();
            if ( comp == null || comp.Owner == this.gameObject ) return;

            if ( Journal.Mode == Journal.JournalMode.Recording || Journal.Mode == Journal.JournalMode.Idling ) return;

            if ( collidersAlreadyHit.Contains( collider.gameObject ) ) return;

            if ( collider.gameObject.tag == "Attack" ) {
                collidersAlreadyHit.Add( collider.gameObject );

                if ( --Health <= 0 ) {
                    Instantiate( Globals._.PREFAB_EXPLOSION, transform.position, Quaternion.identity );
                    Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Explosion );
                    Destroy( gameObject );
                } else {
                    StartCoroutine( Blink( 3 ) );
                }
            }
        }
    }

    IEnumerator Blink( int times ) {
        var r = transform.Find( "Model" ).GetComponentInChildren<Renderer>();
        r.enabled = false;
        yield return new WaitForSeconds( 0.02f );
        r.enabled = true;

        if ( times > 0 ) {
            yield return new WaitForSeconds( 0.1f );
            StartCoroutine( Blink( times - 1 ) );
        }
    }

}
