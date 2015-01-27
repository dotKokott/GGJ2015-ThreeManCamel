using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum PromptType {
    Y,
    A,
    Back
}

public class YSwapper : MonoBehaviour {

    public Sprite controllerY;
    public Sprite controllerA;
    public Sprite controllerRestart;

    public Sprite keyboardY;
    public Sprite keyboardA;
    public Sprite keyboardRestart;

    public Image image;

    public PromptType Type;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        var controllerPluggedin = Input.GetJoystickNames().Length > 0;

        if (controllerPluggedin) {
            switch (Type) {
                case PromptType.Y:
                    image.sprite = controllerY;
                    break;
                case PromptType.A:
                    image.sprite = controllerA;
                    break;
                case PromptType.Back:
                    image.sprite = controllerRestart;

                    transform.localScale = new Vector3(0.6750634f, 0.6750634f, 0.6750634f);
                    break;
                default:
                    break;
            }
            
        } else {
            switch (Type) {
                case PromptType.Y:
                    image.sprite = keyboardY;
                    break;
                case PromptType.A:
                    image.sprite = keyboardA;
                    break;
                case PromptType.Back:
                    image.sprite = keyboardRestart;

                    transform.localScale = new Vector3(0.6750634f * 1.5f, 0.6750634f * 1.5f, 0.6750634f * 1.5f);
                    break;
                default:
                    break;
            }
        }
	}
}
