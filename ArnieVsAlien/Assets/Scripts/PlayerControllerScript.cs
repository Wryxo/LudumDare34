using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControllerScript : MonoBehaviour {
    public float Acceleration, MaxSpeed, RotateSpeed;
    public float MaxSwarmDistance, GunCD;

    public Sprite LeftFireSprite;
    public Sprite RightFireSprite;
    public GameObject Blood;
    public GameObject Projectile;
    public bool Controls;

    private Rigidbody2D rb;
    private GameControllerScript gc;
    private AudioSource shot;
    private float currentCD;
    private bool fireLeft;
    private bool fireRight;
    private bool move;
    public int ammoLeft;
    public int ammoRight;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        shot = GetComponent<AudioSource>();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
    }

    void Update()
    {
        
        move = false;
        if (Input.GetButtonDown("Cancel"))
        {
            gc.ShowGameMenu(false);
        }
        if (Controls && !move)
        {
            rb.velocity = new Vector2(0, 0);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            fireLeft = true;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            fireRight = true;
        }
        if (Controls && Input.GetButton("Fire1") && Input.GetButton("Fire2"))
        {
            move = true;
            fireLeft = false;
            fireRight = false;
        }
        if (move)
        {
            Vector2 currPos = transform.position;
            Vector2 swarmPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (swarmPoint - (Vector2)transform.position).normalized;
            rb.AddForce(direction * MaxSpeed, ForceMode2D.Impulse);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
        } else
        {
            if (currentCD <= 0)
            {
                currentCD = 0;
                if (fireLeft && ammoLeft > 0)
                {
                    shot.Play();
                    Shoot(true);
                    fireLeft = false;
                    currentCD = GunCD;
                }
                else if (fireRight && ammoRight > 0)
                {
                    shot.Play();
                    Shoot(false);
                    fireRight = false;
                    currentCD = GunCD;
                }
            }
            currentCD -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Vector2 currPos = transform.position;
        Vector2 swarmPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (swarmPoint - (Vector2)transform.position).normalized;
        if (!Controls)
        {

            if (Vector2.Distance(currPos, swarmPoint) > MaxSwarmDistance)
            {
                rb.velocity *= 0.95f;
            }
         
            rb.AddForce(direction * Acceleration);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, RotateSpeed);
    }

    void Shoot(bool which)
    {
        var go = Instantiate(Projectile, transform.position + (which ? -transform.right : transform.right)*0.3f, transform.rotation) as GameObject;
        go.GetComponent<SpriteRenderer>().sprite = LeftFireSprite;
        go.GetComponent<SpriteRenderer>().color = which ? new Color(1, 1, 1) : new Color(0, 0, 0);
        go.GetComponent<ProjectileScript>().Attunement = which ? false : true;
        go.GetComponent<ProjectileScript>().Direction = transform.up;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Cell") { 
            gc.Defeat();
            Instantiate(Blood, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ammo0")
        {
            ammoLeft += 5;
            Destroy(other.gameObject);
        }
        if (other.tag == "Ammo1")
        {
            ammoRight += 5;
            Destroy(other.gameObject);
        }
    }
}
