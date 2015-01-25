using UnityEngine;
using System.Collections;

public class MovingStage : MonoBehaviour {
	public GameObject target;
	public GameObject restartTarget;

	Vector3 targetPosition;
	Vector3 originalPosition;

	// Use this for initialization
	void Start () {
		targetPosition = target.transform.position;
		originalPosition = transform.position;

		ResetFunction();
	}

	void ResetFunction()
	{
		this.transform.position = originalPosition;

		iTween.MoveTo (gameObject, iTween.Hash ("position", targetPosition, "time", 18f, "oncompletetarget", this.gameObject, "oncomplete", "ResetFunction", "easetype", iTween.EaseType.linear));
	}
}
