using UnityEngine;
using System.Collections;

public class RotateBeams : MonoBehaviour {

    public bool Rotate;
    public float RotationSpeed = 5f;
    private Quaternion originalRotation;

	// Use this for initialization
	void Start () {
        originalRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (Rotate) {
            transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
        }
	}

    public void Reset() {
        Rotate = false;
        transform.rotation = originalRotation;
    }
}
