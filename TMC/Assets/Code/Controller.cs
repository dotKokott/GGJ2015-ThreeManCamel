using UnityEngine;
using System.Collections;

public class Controller : JournalObject {

    private GameObject smash;
    private GameObject fireball;
    private GameObject beam;

    void Start() {
        renderer = GetComponent<Renderer>();

        IsAlive = true;

        Journal.OnFrame += Journal_OnFrame;
    }

    private Vector3 prevpos;
    private Vector3 direction;

    void Journal_OnFrame( object sender, Journal.JournalEventArgs e ) {
        if ( e.Mode == Journal.JournalMode.Playing ) {
            var npos = e.Frame.Position - prevpos;
            npos.Normalize();

            direction = npos;

            if ( e.Frame.PrimaryAttack ) {
                Debug.Log( "Firing a" );
                smash = Instantiate( Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation ) as GameObject;
                Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Smash );
            }

            if ( e.Frame.SecondaryAttack ) {
                Debug.Log( "Firing b" );
                fireball = Instantiate( Globals._.PREFAB_FIREBALL, transform.position, Globals._.PREFAB_FIREBALL.transform.rotation ) as GameObject;
                var fire = fireball.GetComponent<Fireball>();

                fire.Direction = direction;
                Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Fireball );
            }

            if ( e.Frame.ThirdiraryAttack ) {
                Debug.Log( "Firing c" );
                beam = Instantiate( Globals._.PREFAB_BEAM, transform.position, Globals._.PREFAB_BEAM.transform.rotation ) as GameObject;
                beam.transform.rotation = transform.rotation;
                //beam.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) + 90));
                beam.transform.position += -transform.up * beam.transform.localScale.y / 2;
                Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Beam );
            }

            prevpos = e.Frame.Position;
        }

    }

    void FixedUpdate() {
        if ( Journal.Mode == Journal.JournalMode.Recording ) {
            if ( !IsAlive ) return;

            if ( beam != null ) return;

            var v = InputManager.GetAxisValue( 0, AxesMapping.LEFT_Y_AXIS );
            var h = InputManager.GetAxisValue( 0, AxesMapping.LEFT_X_AXIS );

            var vel = new Vector3( h * 5, -v * 5 ) * Time.deltaTime;

            transform.position += vel;

            var dir = vel.normalized;

            if ( dir != Vector3.zero ) {
                direction = vel.normalized;
                transform.rotation = Quaternion.Euler( new Vector3( 0, 0, Mathf.Atan2( direction.y, direction.x ) * Mathf.Rad2Deg + 90 ) );
            }

            if ( InputManager.GetButtonDown( 0, ButtonMapping.BUTTON_A ) ) {
                smash = Instantiate( Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation ) as GameObject;
                Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Smash );
            }

            if ( InputManager.GetButtonDown( 0, ButtonMapping.BUTTON_X ) ) {
                fireball = Instantiate( Globals._.PREFAB_FIREBALL, transform.position, Globals._.PREFAB_FIREBALL.transform.rotation ) as GameObject;
                var fire = fireball.GetComponent<Fireball>();

                fire.Direction = direction;
                Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Fireball );
            }

            if ( InputManager.GetButtonDown( 0, ButtonMapping.BUTTON_B ) ) {
                beam = Instantiate( Globals._.PREFAB_BEAM, transform.position, Globals._.PREFAB_BEAM.transform.rotation ) as GameObject;
                beam.transform.rotation = transform.rotation;
                //beam.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) + 90));
                beam.transform.position += -transform.up * beam.transform.localScale.y / 2;
                Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Beam );
            }
        }
    }

    void OnTriggerEnter( Collider other ) {

        if ( other.gameObject == smash ) return;
        if ( other.gameObject == fireball ) return;
        if ( other.gameObject == beam ) return;

        if ( other.gameObject.tag == "Attack" ) {
            Instantiate( Globals._.PREFAB_EXPLOSION, transform.position, Quaternion.identity );
            Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Explosion );
            Destroy( gameObject );
        }
    }
}
