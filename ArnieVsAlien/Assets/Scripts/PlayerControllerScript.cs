using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour {
    public float Acceleration, MaxSpeed, RotateSpeed;
    public float MaxSwarmDistance, GunCD;

    public Sprite LeftFireSprite;
    public Sprite RightFireSprite;
    public GameObject Projectile;

    private Rigidbody2D rb;
    private float currentCD;
    private bool fireLeft;
    private bool fireRight;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
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
            if (fireLeft)
            {
                Shoot(true);
                fireLeft = false;
                currentCD = GunCD;
            } else if (fireRight)
            {
                Shoot(false);
                fireRight = false;
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
        go.GetComponent<ProjectileScript>().Direction = transform.up;
    }
}
