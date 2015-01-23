using UnityEngine;
using System.Collections;

public class WaitAction : Action {

    private float duration;

    public WaitAction( float seconds, EActionLane lanes ) {
        duration = seconds;
        Lanes = lanes;
        IsBlocking = true;
    }

    public override void Update() {
        if ( duration <= 0 )
            IsComplete = true;

        duration -= Time.deltaTime;
    }
}
