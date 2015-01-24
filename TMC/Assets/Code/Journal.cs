﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Journal : MonoBehaviour {
    int reverseFrameSpeed = 1;

    public class JournalEventArgs : EventArgs {
        public Frame Frame;
        public JournalMode Mode;

        public JournalEventArgs( Frame frame, JournalMode mode )
            : base() {
            Frame = frame;
            Mode = mode;
        }
    }

    public enum JournalMode {
        Recording,
        Playing,
        Reversing,
        Idling
    }

    public struct Frame {
        public Vector3 Position;
        public Quaternion Rotation;
        public bool IsAlive;

        public bool PrimaryAttack;
        public bool SecondaryAttack;
        public bool ThirdiraryAttack;
    }

    [HideInInspector]
    public JournalMode Mode;

    private List<Frame> frames;
    private int frameIndex = 0;

    private JournalObject jObject;

    public event EventHandler OnPlayFinished;
    public event EventHandler OnRewindFinished;

    public event EventHandler<JournalEventArgs> OnFrame;

    // Use this for initialization
    void Start() {
        jObject = GetComponent<JournalObject>();

        frames = new List<Frame>();
        Mode = JournalMode.Idling;
    }

    void FixedUpdate() {
        if ( Mode == JournalMode.Recording ) {
            var f = new Frame();

            f.Position = jObject.transform.position;
            f.Rotation = jObject.transform.rotation;
            f.IsAlive = jObject.IsAlive;
            f.PrimaryAttack = InputManager.GetButtonDown( 0, ButtonMapping.BUTTON_A );
            f.SecondaryAttack = InputManager.GetButtonDown( 0, ButtonMapping.BUTTON_X );
            f.ThirdiraryAttack = InputManager.GetButtonDown( 0, ButtonMapping.BUTTON_B );

            frames.Add( f );
        } else if ( Mode == JournalMode.Playing ) {
            if ( frameIndex >= frames.Count ) {
                Mode = JournalMode.Idling;

                if ( OnPlayFinished != null ) {
                    OnPlayFinished.Invoke( this, new EventArgs() );
                }

                return;
            }

            var f = frames[frameIndex];

            jObject.transform.position = f.Position;
            jObject.transform.rotation = f.Rotation;
            jObject.IsAlive = f.IsAlive;

            if ( OnFrame != null ) {
                OnFrame.Invoke( this, new JournalEventArgs( f, Mode ) );
            }

            frameIndex++;
        } else if ( Mode == JournalMode.Reversing ) {
            if ( frameIndex < 0 ) {
                Mode = JournalMode.Idling;

                if ( OnRewindFinished != null ) {
                    OnRewindFinished.Invoke( this, new EventArgs() );
                }

                return;
            }

            var f = frames[frameIndex];

            jObject.transform.position = f.Position;
            jObject.transform.rotation = f.Rotation;
            jObject.IsAlive = f.IsAlive;

            if ( OnFrame != null ) {
                OnFrame.Invoke( this, new JournalEventArgs( f, Mode ) );
            }

            //frameIndex -= reverseFrameSpeed;
        }
    }

    public void Record( bool clear = true ) {
        Mode = JournalMode.Recording;
        frames = new List<Frame>();
    }

    public void Play( bool reversed = false ) {
        if ( reversed ) {
            iTween.ValueTo( gameObject, iTween.Hash( "from", frames.Count - 1, "to", 0,
                "time", 3,
                "onupdate", "reverseUpdate", "easetype", iTween.EaseType.easeInOutExpo ) );
        }
        Mode = reversed ? JournalMode.Reversing : JournalMode.Playing;
        frameIndex = reversed ? frames.Count - 1 : 0;
    }

    private void reverseUpdate( float v ) {
        frameIndex = Mathf.RoundToInt( v );
    }

    public void Idle() {
        Mode = JournalMode.Idling;
    }
}
