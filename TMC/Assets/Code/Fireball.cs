using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

    public Vector3 Direction;
    public float Speed = 20;
    public float AliveTime = 5;

    private float timer = 0;
    void Start() {
        StartCoroutine(Kill());
    }

    void FixedUpdate() {
        transform.position += Direction * Speed * Time.deltaTime;
    }

    IEnumerator Kill() {
        yield return new WaitForSeconds(AliveTime);

        Destroy(this.gameObject);
    }
}
