using UnityEngine;
using System.Collections;

public class SyncLanesAction : Action {

    public SyncLanesAction() {
        IsBlocking = true;
        Lanes = EActionLane.Sync;
    }

    public override void Update() {
        if ( parent.Front == this )
            IsComplete = true;
    }
}
