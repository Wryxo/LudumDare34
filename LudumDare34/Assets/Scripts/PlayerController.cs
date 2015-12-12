using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private byte pressed;
	private bool processed;
    private GameController gc;

	// Use this for initialization
	void Start () {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update () {
		if (pressed == 0)
		{
			processed = false;
		}
		if (Input.GetButtonDown("Left")) 
		{
			pressed |= 1;
		}
		if (Input.GetButtonDown("Right")) 
		{
			pressed |= 2;
		}
		if (Input.GetButtonUp("Left")) 
		{
			pressed &= 2;
		}
		if (Input.GetButtonUp("Right")) 
		{
			pressed &= 1;
		}
		if (!processed){
			var pos = transform.position;
			switch (pressed) {
				case 1:
                    //pos += new Vector3(-1.5f, 0);
                    //transform.position = pos;
                    gc.Try(true);
					processed = true;
					break;
				case 2:
                    //pos += new Vector3(1.5f, 0);
                    //transform.position = pos;
                    gc.Try(false);
                    processed = true;
					break;
			}
		}
	}
}
