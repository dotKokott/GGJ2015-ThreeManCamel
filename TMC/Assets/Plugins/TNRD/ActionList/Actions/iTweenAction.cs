using UnityEngine;
using System.Collections;

public class iTweenAction : Action {

    #region Types

    #region Audio

    public static iTweenAction AudioFrom( GameObject gameObject, float volume, float pitch, float time ) {
        return AudioFrom( gameObject, iTween.Hash( "volume", volume, "pitch", pitch, "time", time ) );
    }

    public static iTweenAction AudioFrom( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.AudioFrom );
    }

    public static iTweenAction AudioTo( GameObject gameObject, float volume, float pitch, float time ) {
        return AudioTo( gameObject, iTween.Hash( "volume", volume, "pitch", pitch, "time", time ) );
    }

    public static iTweenAction AudioTo( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.AudioTo );
    }

    #endregion

    #region Color

    public static iTweenAction ColorFrom( GameObject gameObject, Color color, float time ) {
        return ColorFrom( gameObject, iTween.Hash( "color", color, "time", time ) );
    }

    public static iTweenAction ColorFrom( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.ColorFrom );
    }

    public static iTweenAction ColorTo( GameObject gameObject, Color color, float time ) {
        return ColorTo( gameObject, iTween.Hash( "color", color, "time", time ) );
    }

    public static iTweenAction ColorTo( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.ColorTo );
    }

    #endregion

    #region Fade

    public static iTweenAction FadeFrom( GameObject gameObject, float alpha, float time ) {
        return FadeFrom( gameObject, iTween.Hash( "alpha", alpha, "time", time ) );
    }

    public static iTweenAction FadeFrom( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.FadeFrom );
    }

    public static iTweenAction FadeTo( GameObject gameObject, float alpha, float time ) {
        return FadeTo( gameObject, iTween.Hash( "alpha", alpha, "time", time ) );
    }

    public static iTweenAction FadeTo( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.FadeTo );
    }

    #endregion

    #region Look

    public static iTweenAction LookFrom( GameObject gameObject, Vector3 lookTarget, float time ) {
        return LookFrom( gameObject, iTween.Hash( "looktarget", lookTarget, "time", time ) );
    }

    public static iTweenAction LookFrom( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.LookFrom );
    }

    public static iTweenAction LookTo( GameObject gameObject, Vector3 lookTarget, float time ) {
        return LookTo( gameObject, iTween.Hash( "looktarget", lookTarget, "time", time ) );
    }

    public static iTweenAction LookTo( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.LookTo );
    }

    #endregion

    #region Move

    public static iTweenAction MoveFrom( GameObject gameObject, Vector3 position, float time ) {
        return MoveFrom( gameObject, iTween.Hash( "position", position, "time", time ) );
    }

    public static iTweenAction MoveFrom( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.MoveFrom );
    }

    public static iTweenAction MoveTo( GameObject gameObject, Vector3 position, float time ) {
        return MoveTo( gameObject, iTween.Hash( "position", position, "time", time ) );
    }

    public static iTweenAction MoveTo( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.MoveTo );
    }

    #endregion

    #region Rotate

    public static iTweenAction RotateFrom( GameObject gameObject, Vector3 rotation, float time ) {
        return RotateFrom( gameObject, iTween.Hash( "rotation", rotation, "time", time ) );
    }

    public static iTweenAction RotateFrom( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.RotateFrom );
    }

    public static iTweenAction RotateTo( GameObject gameObject, Vector3 rotation, float time ) {
        return RotateTo( gameObject, iTween.Hash( "rotation", rotation, "time", time ) );
    }

    public static iTweenAction RotateTo( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.RotateTo );
    }

    #endregion

    #region Scale

    public static iTweenAction ScaleFrom( GameObject gameObject, Vector3 scale, float time ) {
        return ScaleFrom( gameObject, iTween.Hash( "scale", scale, "time", time ) );
    }

    public static iTweenAction ScaleFrom( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.ScaleFrom );
    }

    public static iTweenAction ScaleTo( GameObject gameObject, Vector3 scale, float time ) {
        return ScaleTo( gameObject, iTween.Hash( "scale", scale, "time", time ) );
    }

    public static iTweenAction ScaleTo( GameObject gameObject, Hashtable arguments ) {
        return Tween( gameObject, arguments, iTween.ScaleTo );
    }

    #endregion

    #endregion    

    private static iTweenAction Tween( GameObject gameObject, Hashtable arguments, System.Action<GameObject, Hashtable> tween ) {
        var act = new iTweenAction( gameObject, arguments, tween );
        return act;
    }

    private GameObject target;
    private Hashtable arguments;
    private System.Action<GameObject, Hashtable> tween;

    private iTweenAction( GameObject gameObject, Hashtable arguments, System.Action<GameObject, Hashtable> tween ) {
        this.target = gameObject;
        this.arguments = arguments;
        this.tween = tween;

        this.Lanes = EActionLane.Tween;

        if ( arguments.ContainsKey( "oncomplete" ) )
            arguments.Remove( "oncomplete" );
        if ( arguments.ContainsKey( "oncompletetarget" ) )
            arguments.Remove( "oncompletetarget" );
        if ( arguments.ContainsKey( "oncompletearguments" ) )
            arguments.Remove( "oncompletearguments" );

        this.arguments = arguments;
    }

    public override void Start() {
        arguments.Add( "oncompletetarget", target );
        arguments.Add( "oncomplete", "RemoveTween" );
        arguments.Add( "oncompleteparams", this );

        tween( target, arguments );
    }
}
