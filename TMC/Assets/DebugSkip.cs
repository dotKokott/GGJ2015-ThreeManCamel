using UnityEngine;
using System.Collections;

public class DebugSkip : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.G))
		{
			Application.LoadLevel(Application.loadedLevel + 1);
		}
		//NOW that I have your attention, C or C, please disable this script before shipping.
	}
}
