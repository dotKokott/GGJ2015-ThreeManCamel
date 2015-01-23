using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Journal))]
public class JournalObject : MonoBehaviour {

    protected Renderer renderer;

    private bool isAlive = true;
    public bool IsAlive {
        get {
            return isAlive;
        }
        set {
            if ( value != isAlive ) {
                renderer.enabled = value;
            }

            isAlive = value;
        }
    }

    private Journal journal;
    public Journal Journal {
        get {
            if ( journal == null ) {
                journal = GetComponent<Journal>();
            }

            return journal;
        }
    }


}
