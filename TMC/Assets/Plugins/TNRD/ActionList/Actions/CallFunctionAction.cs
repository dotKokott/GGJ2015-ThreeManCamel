using UnityEngine;
using System.Collections;

public class CallFunctionAction : Action {

    private System.Action function;

    public CallFunctionAction( System.Action function )
        : this( function, false ) {

    }

    public CallFunctionAction( System.Action function, bool isBlocking )
        : this( function, isBlocking, EActionLane.Utility ) {
    }

    public CallFunctionAction( System.Action function, bool isBlocking, EActionLane lanes ) {
        this.function = function;
        IsBlocking = isBlocking;
        Lanes = lanes;
    }

    public override void Update() {
        base.Update();

        function();
        IsComplete = true;
    }
}
