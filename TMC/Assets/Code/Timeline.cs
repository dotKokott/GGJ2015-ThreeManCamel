using UnityEngine;
using System.Collections;

public class Timeline : MonoBehaviour {

    //I needed a quick way to see how much time was left
    public GameObject timeLineGraphics;
    bool isDoing;
    Vector3 originalScale;
    ///

    private GameObject[] players;
    private int index = 0;

    public float TurnTime = 7f;

    // Use this for initialization
    void Start() {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in players) {
            item.GetComponent<Journal>().OnRewindFinished += J_OnRewindFinished;
        }

        //I needed a quick way to see how much time was left
        originalScale = timeLineGraphics.transform.localScale;
        //
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.O) || InputManager.GetButtonDown(0, ButtonMapping.BUTTON_Y)) {
            StartCoroutine(RecordObject());
        }

       
    }

    void FixedUpdate() {
        //I needed a quick way to see how much time was left
        if (isDoing) {
            timeLineGraphics.transform.localScale -= new Vector3(Time.deltaTime / (1.1f * TurnTime / 7f), 0f, 0f); // dont ask
        }

        else {
            if (timeLineGraphics.transform.localScale.x < originalScale.x)
                timeLineGraphics.transform.localScale += new Vector3(Time.deltaTime / (1.1f * TurnTime / 7f), 0f, 0f);
        }
    }


    IEnumerator RecordObject() {
        var p = players[index];
        var j = p.GetComponent<Journal>();

        var music = GameObject.Find("Music").GetComponent<AudioSource>();        
        music.clip = Globals._.MUSIC_SinglePlay;
        if (p.name == "Boss") {
            music.clip = Globals._.MUSIC_Boss;
        }
        music.Play();

        j.Record();

        SetUIActive(true);

        yield return new WaitForSeconds(TurnTime);
        j.Idle();
        //Nope
        //yield return new WaitForSeconds(1f);        
        music.clip = Globals._.MUSIC_Reverse;
        music.Play();

        j.Play(true);

        SetUIActive(false);
    }

    private void SetUIActive(bool isActive) {
        isDoing = isActive;

        if (!isDoing) {
            timeLineGraphics.GetComponent<Renderer>().material.color = Color.white;
        } else {
            timeLineGraphics.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    private void J_OnRewindFinished(object sender, System.EventArgs e) {
        if (index < players.Length - 1) {
            index++;
            StartCoroutine(RecordObject());
        } else {
            var music = GameObject.Find("Music").GetComponent<AudioSource>();
            music.clip = Globals._.MUSIC_AllPlay;
            music.Play();

            foreach (var item in players) {
                var j = item.GetComponent<Journal>();
                j.Play();
            }

            index = 0;
        }
    }
}
