using UnityEngine;
using System.Collections;
using System;

public class Action {

    public EActionLane Lanes;

    public bool IsBlockedByOther { get; private set; }
    public bool IsBlocking { get; set; }
    public bool IsComplete { get; set; }
    public bool IsRunning { get; set; }

    protected GameObject gameObject;
    protected ActionList parent;

    public void Initialize( ActionList parent ) {
        this.parent = parent;

        gameObject = parent.gameObject;

        IsBlockedByOther = false;
        IsComplete = false;
        IsRunning = false;
    }

    /// <summary>
    /// Terminates the Action; End method will be called
    /// </summary>
    public void Cancel() {
        IsComplete = true;
    }

    public virtual void Update() { }

    /// <summary>
    /// Called the first time before Update is called
    /// </summary>
    public virtual void Start() {
        if ( OnStart != null )
            OnStart.Invoke( this, new EventArgs() );
    }

    /// <summary>
    /// Called after IsComplete is set to true
    /// </summary>
    public virtual void End() {
        if ( OnEnd != null )
            OnEnd.Invoke( this, new EventArgs() );
    }

    /// <summary>
    /// Always call base.OnBlocked
    /// </summary>
    public virtual void Blocked() {
        IsBlockedByOther = true;
        if ( OnBlocked != null )
            OnBlocked.Invoke( this, new EventArgs() );
    }

    /// <summary>
    /// Always call base.OnUnblocked
    /// </summary>
    public virtual void Unblocked() {
        IsBlockedByOther = false;
        if ( OnUnblocked != null )
            OnUnblocked.Invoke( this, new EventArgs() );
    }

    public event EventHandler OnStart;
    public event EventHandler OnEnd;
    public event EventHandler OnBlocked;
    public event EventHandler OnUnblocked;

}
