using UnityEngine;
using System.Collections;

public class TemporaryHealthFeedback : MonoBehaviour {
    public TextMesh text;
    Controller c;

	void Start () {
        c = GetComponent<Controller>();
	}
	
	// Update is called once per frame
	void Update () {
        //text.text = ""+c.health;
	}
}
