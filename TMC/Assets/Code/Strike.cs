using UnityEngine;
using System.Collections;

public class Strike : MonoBehaviour {
    
    public float StayTime = 0.1f;
    public Vector3 Direction;

    void Start() {
        StartCoroutine(Kill());
    }

    IEnumerator Kill() {
        yield return new WaitForSeconds(StayTime);
        Destroy(this.gameObject);
    }

    void Update() {
        transform.position += Direction * 20 * Time.deltaTime;
    }
}
