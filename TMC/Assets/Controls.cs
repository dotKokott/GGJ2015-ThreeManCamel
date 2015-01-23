using UnityEngine;
using System.Collections;

public enum CharType {
	Warrior,
	Mage,
	Defender,
	Healer
}

public class Controls : MonoBehaviour {
	//topspeed = ACCEL/FRICTION
	private Vector3 velocity = new Vector3();
	public float acceleration;
	public float friction;

	public CharType Type;

	Rigidbody rigidbody;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();

		//Do in editor to "fix" bounciness
		//Time.fixedTime = 0.015f;
	}

	void Update () {
		velocity.x += acceleration * Time.deltaTime * Input.GetAxis ("Horizontal") - velocity.x * friction * Time.deltaTime;
		velocity.z += acceleration * Time.deltaTime * Input.GetAxis ("Vertical") - velocity.z * friction * Time.deltaTime;

		rigidbody.MovePosition(transform.position + velocity);
	}
}
