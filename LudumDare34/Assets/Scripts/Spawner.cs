using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    private GameController gc;

	// Use this for initialization
	void Start () {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
		
	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Wall") {
			Destroy(other.gameObject);
            gc.Spawn();
		}
	}
}
