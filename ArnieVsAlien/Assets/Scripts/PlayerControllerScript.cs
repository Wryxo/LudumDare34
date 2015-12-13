﻿using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {
    public float Acceleration, MaxSpeed, RotateSpeed;
    public float MaxSwarmDistance, GunCD;

    public Sprite LeftFireSprite;
    public Sprite RightFireSprite;
    public GameObject Projectile;

    private Rigidbody2D rb;
    private GameControllerScript gc;
    private float currentCD;
    private bool fireLeft;
    private bool fireRight;
    public int ammoLeft;
    public int ammoRight;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
        ammoLeft = 5;
        ammoRight = 5;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            fireLeft = true;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            fireRight = true;
        }
        if (currentCD <= 0)
        {
            currentCD = 0;
            if (fireLeft && ammoLeft > 0)
            {
                Shoot(true);
                fireLeft = false;
                ammoLeft--;
                currentCD = GunCD;
            } else if (fireRight && ammoRight > 0)
            {
                Shoot(false);
                fireRight = false;
                ammoRight--;
                currentCD = GunCD;
            }
        }
        currentCD -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        Vector2 currPos = transform.position;
        Vector2 swarmPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(currPos, swarmPoint) > MaxSwarmDistance)
        {
            rb.velocity *= 0.95f;
        }

        Vector2 direction = (swarmPoint - (Vector2)transform.position).normalized;
        rb.AddForce(direction * Acceleration * rb.mass);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed * rb.mass);
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        var q = Quaternion.AngleAxis(angle-90, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, RotateSpeed);
    }

    void Shoot(bool which)
    {
        var go = Instantiate(Projectile, transform.position, transform.rotation) as GameObject;
        go.GetComponent<SpriteRenderer>().sprite = which ? LeftFireSprite : RightFireSprite;
        go.GetComponent<ProjectileScript>().Attunement = which ? false : true;
        go.GetComponent<ProjectileScript>().Direction = transform.up;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collision" + other.gameObject.tag);
        if (other.gameObject.tag == "Cell")
            gc.Defeat();
        if (other.gameObject.tag == "Ammo0") { 
            ammoLeft += 5;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Ammo1") { 
            ammoRight += 5;
            Destroy(other.gameObject);
        }
    }
}
