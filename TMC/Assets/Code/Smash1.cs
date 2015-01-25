﻿using UnityEngine;
using System.Collections;

public class Smash1 : MonoBehaviour {

    public float StayTime = 5;
    public float GrowSpeed = 20;

    void Start() {
        StartCoroutine( Kill() );
    }

    IEnumerator Kill() {
        yield return new WaitForSeconds( StayTime );
        Destroy( this.gameObject );
    }

    // Update is called once per frame
    void Update() {
        if ( transform.localScale.y < 20f ) {
            transform.localScale += new Vector3( GrowSpeed * Time.deltaTime, GrowSpeed * Time.deltaTime, GrowSpeed * Time.deltaTime );
        }
    }
}