using UnityEngine;
using System.Collections;

public class Controller : JournalObject {

    private GameObject smash;
    private GameObject fireball;
    private GameObject beam;

    public int health = 0;

    int originalAttackLimit;
    public int attackLimit = 1;

    public bool isBoss;

    private Vector3 velocity = new Vector3();
    public float acceleration;
    public float friction; 

    void Start() {
        renderer = GetComponent<Renderer>();

        IsAlive = true;

        Journal.OnFrame += Journal_OnFrame;

        originalAttackLimit = attackLimit;
    }

    private Vector3 prevpos;
    private Vector3 direction;

    void Journal_OnFrame( object sender, Journal.JournalEventArgs e ) {
        if ( e.Mode == Journal.JournalMode.Playing ) {
            var npos = e.Frame.Position - prevpos;
            npos.Normalize();

            direction = npos;

            attackLimit = originalAttackLimit;
            if (attackLimit > 0 || isBoss) {

                if ( e.Frame.PrimaryAttack ) {
                    Debug.Log( "Firing a" );
                    smash = Instantiate( Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation ) as GameObject;
                    smash.GetComponent<AttackInfo>().Owner = this.gameObject;
                    Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Smash );
                }

                if ( e.Frame.SecondaryAttack ) {
                    Debug.Log( "Firing b" );
                    fireball = Instantiate( Globals._.PREFAB_FIREBALL, transform.position, Globals._.PREFAB_FIREBALL.transform.rotation ) as GameObject;
                    var fire = fireball.GetComponent<Fireball>();
                    fireball.GetComponent<AttackInfo>().Owner = this.gameObject;
                    fire.Direction = direction;
                    Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Fireball );
                }

                if ( e.Frame.ThirdiraryAttack ) {
                    Debug.Log( "Firing c" );
                    beam = Instantiate( Globals._.PREFAB_BEAM, transform.position, Globals._.PREFAB_BEAM.transform.rotation ) as GameObject;
                    beam.GetComponent<AttackInfo>().Owner = this.gameObject;
                
                    beam.transform.rotation = transform.rotation;

                    //beam.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) + 90));
                    beam.transform.position += -transform.up * beam.transform.localScale.y / 2;
                    Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Beam );
                }
            }

            prevpos = e.Frame.Position;
        }

    }

    void FixedUpdate() {
        if ( Journal.Mode == Journal.JournalMode.Recording ) {
            if ( !IsAlive ) return;

            if ( beam != null ) return;


            //var v = InputManager.GetAxisValue( 0, AxesMapping.LEFT_Y_AXIS );
            //var h = InputManager.GetAxisValue( 0, AxesMapping.LEFT_X_AXIS );
            var h = Input.GetAxis( "Horizontal" );
            var v = -Input.GetAxis( "Vertical" );

            var vel = new Vector3( h * 5, -v * 5 ) * Time.deltaTime;
           // velocity.x += acceleration * Time.deltaTime * InputManager.GetAxisValue(0, AxesMapping.LEFT_X_AXIS) - velocity.x * friction * Time.deltaTime;
            //velocity.y += acceleration * Time.deltaTime * -InputManager.GetAxisValue(0, AxesMapping.LEFT_Y_AXIS) - velocity.y * friction * Time.deltaTime;

            //Vector3 vel = velocity;

           // GetComponent<Rigidbody>().MovePosition(transform.position + velocity);

            transform.position += vel;

            var dir = vel.normalized;

            if ( dir != Vector3.zero ) {
                direction = vel.normalized;
                transform.rotation = Quaternion.Euler( new Vector3( 0, 0, Mathf.Atan2( direction.y, direction.x ) * Mathf.Rad2Deg + 90 ) );
            }

            if (attackLimit <= 0 && !isBoss)
                return;

            if ( InputManager.GetButtonDown( 0, ButtonMapping.BUTTON_A )) {
                smash = Instantiate( Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation ) as GameObject;
                smash.GetComponent<AttackInfo>().Owner = this.gameObject;

                Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Smash );
                attackLimit--;
            }

            if (InputManager.GetButtonDown(0, ButtonMapping.BUTTON_X)) {
                fireball = Instantiate( Globals._.PREFAB_FIREBALL, transform.position, Globals._.PREFAB_FIREBALL.transform.rotation ) as GameObject;
                var fire = fireball.GetComponent<Fireball>();
                fireball.GetComponent<AttackInfo>().Owner = this.gameObject;

                fire.Direction = direction;
                Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Fireball );
                attackLimit--;
            }

            if (InputManager.GetButtonDown(0, ButtonMapping.BUTTON_B)) {
                beam = Instantiate( Globals._.PREFAB_BEAM, transform.position, Globals._.PREFAB_BEAM.transform.rotation ) as GameObject;
                beam.transform.rotation = transform.rotation;
                beam.GetComponent<AttackInfo>().Owner = this.gameObject;
                //beam.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x) + 90));
                beam.transform.position += -transform.up * beam.transform.localScale.y / 2;
                Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Beam );
                attackLimit--;
            }
        }
    }

    void OnTriggerEnter( Collider other ) {
        var comp = other.GetComponent<AttackInfo>();
        if (comp == null || comp.Owner == this.gameObject) return;        

        if ( other.gameObject.tag == "Attack" ) {
            print("THIS IS HAPPENING TOO OFTEN");
            if (--health <= 0 && GetComponent<Journal>().Mode == global::Journal.JournalMode.Playing)
             { 
                Instantiate( Globals._.PREFAB_EXPLOSION, transform.position, Quaternion.identity );
                Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Explosion );
                Destroy( gameObject );
            }
        }
    }
}
