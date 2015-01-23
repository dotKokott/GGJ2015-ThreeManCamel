using UnityEngine;
using System.Collections;

public class ConditionalAction : Action {

    private System.Func<bool> function;

    public ConditionalAction( System.Func<bool> function )
        : this( function, true ) {
    }

    public ConditionalAction( System.Func<bool> function, bool isBlocking )
        : this( function, isBlocking, EActionLane.Utility ) {
    }

    public ConditionalAction( System.Func<bool> function, bool isBlocking, EActionLane lanes ) {
        this.function = function;
        IsBlocking = isBlocking;
        Lanes = lanes;
    }

    public override void Update() {
        base.Update();

        if ( function() ) {
            IsComplete = true;
        }
    }
}
