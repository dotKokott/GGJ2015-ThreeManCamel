using UnityEngine;
using System.Collections;

public class WaitAllAction : WaitAction {

    public WaitAllAction( float seconds )
        : base( seconds, EActionLane.Sync ) { }

    public override void Update() {
        if ( parent.Front == this ) {
            base.Update();
        }
    }
}
