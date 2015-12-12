using UnityEngine;
using System.Collections;

public class AlienCellScript : MonoBehaviour {

    public bool Attunement, IsCore;
    public Sprite OneSprite, ZeroSprite, CoreSprite, CellSprite;

    private int x, y;
    private GameControllerScript gc;

	// Use this for initialization
	void Start () {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent <GameControllerScript>();
    }

    // Update is called once per frame
    void FixedUpdate () {
       
    }
    
    public void SetCoord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SetCore(bool value)
    {
        IsCore = value;
        if (!IsCore)
        {
            Attunement = false;
            if (Random.value > 0.5f)
            {
                Attunement = true;
            }
            GetComponent<SpriteRenderer>().sprite = CellSprite;
            GetComponent<BoxCollider2D>().enabled = true;
            transform.Find("Attunement").GetComponent<SpriteRenderer>().sprite = Attunement ? OneSprite : ZeroSprite;
            transform.Find("Attunement").GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = CoreSprite;
            GetComponent<BoxCollider2D>().enabled = false;
            transform.Find("Attunement").GetComponent<SpriteRenderer>().sprite = null;
            transform.Find("Attunement").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            if (other.GetComponent<ProjectileScript>().Attunement != Attunement) {
                gc.DestroyAlien(x, y);
                Destroy(gameObject);
            }
        }
    }
}
