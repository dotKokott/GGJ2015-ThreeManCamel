using UnityEngine;
using System.Collections;

public enum CharacterType {
    Tank,
    Knight,
    Mage
}

public class Controller : JournalObject {

    public CharacterType Type;

    public GameObject beam;
    public GameObject smash;

    public int health = 0;

    int originalAttackLimit;
    public int attackLimit = 1;

    private Vector3 velocity = new Vector3();
    public float acceleration;
    public float friction;

    public float StrikeCooldown = 1f;
    private float strikeTimer;

    public float BeamChargeTime = 1f;
    private float beamChargeTimer = 0f;
    public float BeamStayTime = 1f;

	public GameObject animationContainer;
	Animation animation;

    public int Order = 0;

    private TextMesh orderText;

    void Start() {
        strikeTimer = StrikeCooldown;

        renderer = GetComponent<Renderer>();

        IsAlive = true;

        Journal.OnFrame += Journal_OnFrame;

        originalAttackLimit = attackLimit;

        orderText = gameObject.transform.FindChild( "Order" ).gameObject.GetComponent<TextMesh>();
        orderText.text = ( Order + 1 ).ToString() + ".";
		animation = animationContainer.GetComponent<Animation>();
    }

    private Vector3 prevpos;
    private Vector3 direction;

    void Journal_OnFrame( object sender, Journal.JournalEventArgs e ) {
        if ( e.Mode == Journal.JournalMode.Playing ) {
            var npos = e.Frame.Position - prevpos;

            if (!animation.IsPlaying("Attack")) {
                if (Vector3.Distance(npos, Vector3.zero) < 0.01f) {
                    animation.Play("Idle");
                } else {
                    animation.Play("Walk", AnimationPlayMode.Stop);
                }
            }            
            
            npos.Normalize();

            direction = npos;



            attackLimit = originalAttackLimit;
            if ( attackLimit > 0 ) {
                if ( e.Frame.Attacked ) {
                    Debug.Log( "Firing a" );
                    animation.Play("Attack");
                    switch ( Type ) {
                        case CharacterType.Tank:
                            smash = Instantiate( Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation ) as GameObject;
                            smash.GetComponent<AttackInfo>().Owner = this.gameObject;
                            Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Smash );

                            break;
                        case CharacterType.Knight:
                            var strike = Instantiate( Globals._.PREFAB_STRIKE, transform.position, Globals._.PREFAB_STRIKE.transform.rotation ) as GameObject;
                            strike.GetComponent<AttackInfo>().Owner = this.gameObject;

                            var strikeComp = strike.GetComponent<Strike>();
                            strikeComp.Direction = direction;

                            Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Smash );

                            break;
                        case CharacterType.Mage:
                            beam = Instantiate( Globals._.PREFAB_BEAM, transform.position, Globals._.PREFAB_BEAM.transform.rotation ) as GameObject;
                            beam.transform.rotation = transform.rotation;
                            beam.transform.Rotate( Vector3.right, -90 );
                            beam.AddComponent<AttackInfo>().Owner = this.gameObject;

                            beam.transform.position += -transform.up * beam.transform.localScale.y / 2;
                            Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Beam );

                            beam.transform.localScale = new Vector3( 5, 72, 145 );
                            iTween.ScaleTo(beam, new Vector3(36, 72, 145), 0.2f);
                            beam.DestroyAfter( BeamStayTime );                        

                            beam.transform.position = new Vector3( beam.transform.position.x, beam.transform.position.y, -0.15f );

                            Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Beam );

                            break;
                        default:
                            break;
                    }
                }
            }

            prevpos = e.Frame.Position;
        }

    }

    void Update() {
		//animation.Play ("Idle");
		print(animation.IsPlaying("Attack"));

        if ( Journal.Mode == Journal.JournalMode.Recording ) {
            orderText.gameObject.SetActive( false );
            if ( !IsAlive ) return;

            strikeTimer += Time.deltaTime;

            if ( beam != null ) {
                if ( !beam.activeInHierarchy ) {
                    beamChargeTimer += Time.deltaTime;

                    if ( beamChargeTimer >= BeamChargeTime ) {
                        beam.SetActive( true );

                        beamChargeTimer = 0;
                    }
                }

                return;
            }

            if (smash != null) {
                return;
            }

            var v = -InputManager.GetAxisValue( 0, AxesMapping.LEFT_Y_AXIS );
            var h = InputManager.GetAxisValue( 0, AxesMapping.LEFT_X_AXIS );
            //var h = Input.GetAxis( "Horizontal" );
            //var v = Input.GetAxis( "Vertical" );

            velocity.x += acceleration * Time.deltaTime * h - velocity.x * friction * Time.deltaTime;
            velocity.y += acceleration * Time.deltaTime * v - velocity.y * friction * Time.deltaTime;

            Vector3 vel = velocity;

            //GetComponent<Rigidbody>().MovePosition(transform.position + velocity);
			//Should change with a constant treshold

			if (!animation.IsPlaying("Attack"))
			{
				if (Vector3.Distance(vel, Vector3.zero) < 0.01f)
				{
					print ("THIS");
					animation.Play ("Idle");
				}

				else
					animation.Play("Walk", AnimationPlayMode.Stop);
			}

            transform.position += vel;

            var dir = vel.normalized;

            if ( dir != Vector3.zero ) {
                direction = vel.normalized;
                transform.rotation = Quaternion.Euler( new Vector3( 0, 0, Mathf.Atan2( direction.y, direction.x ) * Mathf.Rad2Deg + 90 ) );
            }

            if ( attackLimit <= 0 ) {
                return;
            }

            if ( InputManager.GetButtonDown( 0, ButtonMapping.BUTTON_A )) {  
				animation.Play("Attack");

                switch (Type) {
                    case CharacterType.Tank:
                        Attacked = true;

                        smash = Instantiate( Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation ) as GameObject;
                        smash.GetComponent<AttackInfo>().Owner = this.gameObject;

                        Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Smash );

                        attackLimit--;

                        break;
                    case CharacterType.Knight:
                        if ( strikeTimer >= StrikeCooldown ) {
                            Attacked = true;

                            var strike = Instantiate( Globals._.PREFAB_STRIKE, transform.position, Globals._.PREFAB_STRIKE.transform.rotation ) as GameObject;
                            strike.GetComponent<AttackInfo>().Owner = this.gameObject;

                            var strikeComp = strike.GetComponent<Strike>();
                            strikeComp.Direction = direction;

                            Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Smash );

                            strikeTimer = 0;
                        }

                        break;
                    case CharacterType.Mage:
                        Attacked = true;

                        beam = Instantiate( Globals._.PREFAB_BEAM, transform.position, Globals._.PREFAB_BEAM.transform.rotation ) as GameObject;
                        beam.transform.rotation = transform.rotation;
                        beam.transform.Rotate( Vector3.right, -90 );
                        beam.AddComponent<AttackInfo>().Owner = this.gameObject;

                        beam.transform.position += -transform.up * beam.transform.localScale.y / 2;
                        Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Beam );
                        beam.transform.position = new Vector3( beam.transform.position.x, beam.transform.position.y, -0.15f );
                        
                        beam.transform.localScale = new Vector3( 5, 72, 145 );
                        iTween.ScaleTo(beam, new Vector3(36, 72, 145), 0.2f);

                        beam.DestroyAfter( BeamStayTime );

                        beam.SetActive( false );

                        attackLimit--;

                        break;
                    default:
                        break;
                }

            }
        } else if ( Journal.Mode == Journal.JournalMode.Playing ) {
            protectionTimer -= Time.deltaTime;
        }
    }

    float protectionTimer = 0;

    void OnTriggerEnter( Collider other ) {
        
    }

    void OnTriggerStay( Collider collider ) {
        if ( collider.name.Contains( "Protector" ) ) {
            protectionTimer = 0.2f;
        } else {
            var comp = collider.GetComponent<AttackInfo>();
            if ( comp == null || comp.Owner == this.gameObject )
                return;

            if ( Journal.Mode == Journal.JournalMode.Recording || Journal.Mode == Journal.JournalMode.Idling ) return;

            if ( collider.gameObject.tag == "Attack" ) {
                if ( protectionTimer <= 0 ) {
                    if ( --health <= 0 ) {
                        Instantiate( Globals._.PREFAB_EXPLOSION, transform.position, Quaternion.identity );
                        Camera.main.GetComponent<AudioSource>().PlayOneShot( Globals._.SOUND_Explosion );
                        Destroy( gameObject );
                    }
                } else {
                    Debug.Log( "Protected" );
                }
            }
        }
    }
}
