using UnityEngine;
using System.Collections;

public class Controller : JournalObject {

    private GameObject smash;
    private GameObject fireball;
    void Start() {
        renderer = GetComponent<Renderer>();

        IsAlive = true;

        Journal.OnFrame += Journal_OnFrame;
    }

    private Vector3 prevpos;
    private Vector3 direction;

    void Journal_OnFrame(object sender, Journal.JournalEventArgs e) {
        if (e.Mode == Journal.JournalMode.Playing) {
            var npos = e.Frame.Position - prevpos;
            npos.Normalize();

            direction = npos;

            if (e.Frame.PrimaryAttack) {
                smash = Instantiate(Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation) as GameObject;
            }

            if (e.Frame.SecondaryAttack) {
                fireball = Instantiate(Globals._.PREFAB_FIREBALL, transform.position, Globals._.PREFAB_FIREBALL.transform.rotation) as GameObject;
                var fire = fireball.GetComponent<Fireball>();

                fire.Direction = direction;
            }

            prevpos = e.Frame.Position;
        }

    }

    // Update is called once per frame
    void Update() {
        //if ( Input.GetKeyDown( KeyCode.Space ) ) {
        //    Journal.Play();
        //} else if ( Input.GetKeyDown( KeyCode.LeftShift ) ) {
        //    Journal.Play( true );
        //} else if ( Input.GetKeyDown( KeyCode.LeftControl ) ) {
        //    Journal.Record( true );
        //} else if ( Input.GetKeyDown( KeyCode.RightShift ) ) {
        //    Journal.Mode = Journal.JournalMode.Idling;
        //}

        if ( Journal.Mode == Journal.JournalMode.Recording ) {
            if ( !IsAlive ) return;

            var v = InputManager.GetAxisValue(0, AxesMapping.LEFT_Y_AXIS);
            var h = InputManager.GetAxisValue(0, AxesMapping.LEFT_X_AXIS);

            var vel = new Vector3( h * 5, -v * 5 ) * Time.deltaTime;

            transform.position += vel;

            var dir = vel.normalized;

            if (dir != Vector3.zero) {
                direction = vel.normalized;
            }            

            if (InputManager.GetButtonDown(0, ButtonMapping.BUTTON_A)) {
                smash = Instantiate(Globals._.PREFAB_SMASH, transform.position, Globals._.PREFAB_SMASH.transform.rotation) as GameObject;
            }

            if (InputManager.GetButtonDown(0, ButtonMapping.BUTTON_X)) {
                fireball = Instantiate(Globals._.PREFAB_FIREBALL, transform.position, Globals._.PREFAB_FIREBALL.transform.rotation) as GameObject;
                var fire = fireball.GetComponent<Fireball>();

                fire.Direction = direction;
            }
        }        
    }

    void OnTriggerEnter(Collider other) {

        //Debug.Log("Trigger enter");
        if (other.gameObject == smash) return;
        if (other.gameObject == fireball) return;

        if (other.gameObject.tag == "Attack") {
            Debug.Log("HIHTIHTIHTIHTIHTIHT");
        }
    }
}
