using UnityEngine;
using System.Collections;

public class IntroShake : MonoBehaviour {
	Vector3 amount = new Vector3(0.1f, 0.1f, 0f);
	// Use this for initialization
	void Start () {
		StartCoroutine(Shake());
	}

	IEnumerator Shake() 
	{
		while (true)
		{
			iTween.ShakePosition(gameObject, amount, 0.16f); 
			yield return new WaitForSeconds(0.14f);
		}

	}
}
