using UnityEngine;
using System.Collections;

public static class _GameObjectE {

    public static void DeactivateAfter( this GameObject gameObject, float time ) {
        var c = gameObject.AddComponent<DeactivateAfter>();
        c.TimeToDeactivate = time;
    }
}
