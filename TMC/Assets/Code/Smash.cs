using UnityEngine;
using System.Collections;

public class Smash : MonoBehaviour {

    bool destroyed = false;
    float dTime = 0;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!destroyed) {
            transform.localScale += new Vector3(20 * Time.deltaTime, 20 * Time.deltaTime);

            if (transform.localScale.x > 3) {
                destroyed = true;                
            }
        } else {
            dTime += Time.deltaTime;

            if (dTime > 1) {
                Destroy(this.gameObject);
            }
        }
        
	}
}
