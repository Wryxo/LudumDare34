using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    public Vector3 Direction;
    public float Speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position = transform.position + (Direction * Speed);
	}
}
