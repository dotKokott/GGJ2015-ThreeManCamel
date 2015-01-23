using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionList : MonoBehaviour {

    private List<Action> actions;

    public int Count { get { return actions.Count; } }

    public Action Front { get { return (Count > 0) ? actions[0] : null; } }
    public Action Back { get { return (Count > 0) ? actions[Count - 1] : null; } }

    void Awake() {
        actions = new List<Action>();
    }

    // Update is called once per frame
    public void Update() {
        var actionsToUpdate = new List<Action>( actions );

        EActionLane mask = EActionLane._DoNotUse;

        foreach ( var item in actionsToUpdate ) {
            if ( (mask & item.Lanes) == item.Lanes ) {
                continue;
            }

            if ( item.IsComplete ) {
                item.End();
                int index = actions.IndexOf( item );
                actions.Remove( item );

                if ( item.IsBlocking ) {
                    for ( int i = index; i < Count; i++ ) {
                        var act = actions[i];

                        if ( (act.Lanes & item.Lanes) == act.Lanes && act.IsRunning && act.IsBlockedByOther ) {
                            act.Unblocked();
                            if ( act.IsBlocking ) break;
                        }
                    }
                }

                continue;
            } else if ( !item.IsRunning ) {
                item.Start();
                item.IsRunning = true;
            } else {
                item.Update();
            }

            if ( item.IsBlocking ) {
                mask |= item.Lanes;
                if ( (item.Lanes & EActionLane.Sync) == EActionLane.Sync ) {
                    break;
                } else {
                    BlockActions( actionsToUpdate.IndexOf( item ), item.Lanes );
                }
            }
        }
    }

    public void AddAction( Action action ) {
        actions.Add( action );

        action.Initialize( this );
    }

    public void AddActionAt( int index, Action action ) {
        actions.Insert( index, action );

        action.Initialize( this );

        BlockActions( index, action.Lanes );
    }

    public void RemoveAction( Action action ) {
        actions.Remove( action );
    }

    public void RemoveActionAt( int index ) {
        actions.RemoveAt( index );
    }

    private void BlockActions( int index, EActionLane lanes ) {
        for ( int i = index + 1; i < Count; i++ ) {
            var act = actions[i];

            if ( (act.Lanes & lanes) == act.Lanes && act.IsRunning && !act.IsBlockedByOther ) {
                act.Blocked();

                if ( act.IsBlocking ) break;
            }
        }
    }

    /// <summary>
    /// This is called by every iTweenAction.
    /// When the tween has been completed, it calls this method to remove itself from the actionlist
    /// </summary>
    /// <param name="action">The action that is going to be removed</param>
    private void RemoveTween( iTweenAction action ) {
        action.IsComplete = true;
    }
}
