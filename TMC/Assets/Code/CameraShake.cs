using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	//Thanks to mdjasper for the original class!
	Vector3 originPosition;
	Quaternion originRotation;
	
	float shake_decay;
	float shake_intensity;
	
//	void OnGUI () {
//		if (GUI.Button (new Rect (20,40,80,20), "Shake")) {
//			Shake();
//		}
//	} 

	void Start() {
		originPosition = transform.position;
		originRotation = transform.rotation;
	}
	
	void Update(){
		if(shake_intensity > 0){
			transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
			transform.rotation =  new Quaternion(
				originRotation.x + Random.Range(-shake_intensity,shake_intensity)*.2f,
				originRotation.y + Random.Range(-shake_intensity,shake_intensity)*.2f,
				originRotation.z + Random.Range(-shake_intensity,shake_intensity)*.2f,
				originRotation.w + Random.Range(-shake_intensity,shake_intensity)*.2f);
			shake_intensity -= shake_decay;
		}

		else if (shake_intensity <= 0.1f)
		{
			transform.position = originPosition;
			transform.rotation = originRotation;
		}
	}
	
	void Shake(float intensity = .15f, float decay = .004f){
		shake_intensity = intensity;
		shake_decay = decay;
	}
}
