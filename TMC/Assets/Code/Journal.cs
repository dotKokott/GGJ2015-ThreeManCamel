using UnityEngine;
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
        public bool Attacked;
    }

    [HideInInspector]
    public JournalMode Mode;

    private List<Frame> frames;
    private int frameIndex = 0;

    private JournalObject jObject;

    public event EventHandler OnStartRecording;
    public event EventHandler OnStartPlaying;

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
            f.Attacked = jObject.Attacked;
            if ( f.Attacked ) {
                jObject.Attacked = false;
            }

            frames.Add( f );
        } else if ( Mode == JournalMode.Playing ) {
            if ( frameIndex >= frames.Count ) {
                Mode = JournalMode.Idling;

                if ( OnPlayFinished != null ) {
                    OnPlayFinished.Invoke( this, new EventArgs() );
                }

                return;
            }

            if ( frameIndex == -1 ) {
                iTween.Stop( gameObject );
                frameIndex = 0;
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
                frameIndex = 0;
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
        }
    }

    public void Record( bool clear = true ) {
        Mode = JournalMode.Recording;
        frames = new List<Frame>();

        if ( OnStartRecording != null ) {
            OnStartRecording.Invoke( this, new EventArgs() );
        }
    }

    public void Play( bool reversed = false ) {
        if ( reversed ) {
            iTween.ValueTo( gameObject, iTween.Hash( "from", frames.Count - 1, "to", -1,
                "time", Timeline.TTime / 2,
                "onupdate", "reverseUpdate", "easetype", iTween.EaseType.easeInOutExpo ) );
        }

        Mode = reversed ? JournalMode.Reversing : JournalMode.Playing;
        frameIndex = reversed ? frames.Count - 1 : 0;

        if ( OnStartPlaying != null ) {
            OnStartPlaying.Invoke( this, new EventArgs() );
        }
    }

    private void reverseUpdate( float v ) {
        frameIndex = Mathf.RoundToInt( v );
    }

    public void Idle() {
        Mode = JournalMode.Idling;
    }
}
