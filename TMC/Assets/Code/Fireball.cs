using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

    public Vector3 Direction;
    public float Speed = 20;

    private float timer = 0;
    void Start() {
        StartCoroutine(Kill());
    }

    void FixedUpdate() {
        transform.position += Direction * Speed * Time.deltaTime;
    }

    IEnumerator Kill() {
        yield return new WaitForSeconds(5);

        Destroy(this.gameObject);
    }
}
