using UnityEngine;
using System.Collections;

public class HierarchicalAction : Action {

    private ActionList actions;

    public int Count { get { return actions.Count; } }

    public Action Front { get { return actions.Front; } }
    public Action Back { get { return actions.Back; } }

    public HierarchicalAction() {
        actions = new ActionList();
    }

    public HierarchicalAction( params Action[] actions )
        : this() {
        foreach ( var item in actions ) {
            this.actions.AddAction( item );
        }
    }

    public HierarchicalAction( bool isBlocking, params Action[] actions )
        : this( actions ) {
        IsBlocking = IsBlocking;
    }

    public override void Update() {
        actions.Update();
        if ( actions.Count == 0 )
            IsComplete = true;
    }

    public void AddAction( Action action ) {
        actions.AddAction( action );
    }

    public void InsertAction( int index, Action action ) {
        actions.AddActionAt( index, action );
    }
}
