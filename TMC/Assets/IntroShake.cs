using UnityEngine;
using System.Collections;

public class IntroShake : MonoBehaviour {
	Vector3 amount = new Vector3(0.1f, 0.1f, 0f);
	public Animation animation;

	public MonoBehaviour[] behaviours;
	public CanvasRenderer[] canvases;
	float timer = 0f;
	public AudioSource stomp;
	public AudioClip aoeClip;

	bool dirtyCode;
	bool dirtyCode2;
	bool dirtyCode3;
	bool started;

	// Use this for initialization
	void Start () {
		StartCoroutine(Shake());
	}

	void FirstLevel() {
		Application.LoadLevel(Application.loadedLevel + 1);
	}

	void Update() {
		if (started)
		{
			timer += Time.deltaTime;

			foreach ( var canvas in canvases)
			{
				canvas.SetAlpha(canvas.GetAlpha() - Time.deltaTime * 2f);
			}

			if (timer >= 1f && !dirtyCode)
			{
				dirtyCode = true;

				iTween.ShakePosition(gameObject, amount * 7f, 2f); 
			}

			if (timer >= 1.04f & !dirtyCode3)
			{
				AudioSource.PlayClipAtPoint(aoeClip, Camera.main.transform.position);
				dirtyCode3 = true;
			}

			if (timer >= 1.4f & !dirtyCode2)
			{

				dirtyCode2 = true;
	
				iTween.CameraFadeAdd ();
				iTween.CameraFadeTo(iTween.Hash ("amount", 1f, "time", 1f, "oncompletetarget", this.gameObject, "oncomplete", "FirstLevel"));
			}
			return;

		}

		if (InputManager.GetButtonDown(0, ButtonMapping.BUTTON_Y))
		{
			StopAllCoroutines();

			animation.Play ("AOE");


			foreach (MonoBehaviour mb in behaviours)
			{
				mb.enabled = false;
				iTween.Stop (mb.gameObject, "MoveTo");
			}

			started = true;

			stomp.Stop ();
		}
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
