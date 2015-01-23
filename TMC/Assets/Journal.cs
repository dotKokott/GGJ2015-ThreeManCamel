using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Journal : MonoBehaviour {

    public enum JournalMode {
        Recording,
        Playing,
        Reversing,
        Idling
    }

    public struct Frame {
        public Vector3 Position;
        public bool IsAlive;
    }

    [HideInInspector]
    public JournalMode Mode;

    private List<Frame> frames;
    private int frameIndex = 0;

    private JournalObject jObject;

    public event EventHandler OnPlayFinished;
    public event EventHandler OnRewindFinished;

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
            f.IsAlive = jObject.IsAlive;

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
            jObject.IsAlive = f.IsAlive;

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
            jObject.IsAlive = f.IsAlive;

            frameIndex--;
        }
    }

    public void Record( bool clear = true ) {
        Mode = JournalMode.Recording;
        frames = new List<Frame>();
    }

    public void Play( bool reversed = false ) {
        Mode = reversed ? JournalMode.Reversing : JournalMode.Playing;
        frameIndex = reversed ? frames.Count - 1 : 0;
    }

    public void Idle() {
        Mode = JournalMode.Idling;
    }
}
