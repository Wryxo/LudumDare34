using UnityEngine;
using System.Collections;

public class AlienCellScript : MonoBehaviour {

    public bool Attunement, IsCore;
    public Sprite OneSprite, ZeroSprite, CoreSprite, CellSprite;
    public float MitosisCD, DeadCD;
    public GameObject Cell;

    private float currentCD;
    public bool mitosisEnabled, dead;
    public int numAdj, currAdj;
    private GameObject[] adjacent = new GameObject[4]; // left, up, right, down

	// Use this for initialization
	void Start () {
        dead = false;
        adjacent[0] = null;
        adjacent[1] = null;
        adjacent[2] = null;
        adjacent[3] = null;
        numAdj = 1;
        currAdj = 0;
        IsCore = false;
        if (Random.value > 0.9f)
        {
            IsCore = true;
        }
        if (!IsCore) { 
            Attunement = false;
            if (Random.value > 0.5f)
            {
                Attunement = true;
            }
            GetComponent<SpriteRenderer>().sprite = CellSprite;
            GetComponent<BoxCollider2D>().enabled = true;
            transform.Find("Attunement").GetComponent<SpriteRenderer>().sprite = Attunement ? OneSprite : ZeroSprite;
            transform.Find("Attunement").GetComponent<SpriteRenderer>().color = new Color(255,0,0,255);
            numAdj = 2;
        } else
        {
            GetComponent<SpriteRenderer>().sprite = CoreSprite;
            GetComponent<BoxCollider2D>().enabled = false;
            transform.Find("Attunement").GetComponent<SpriteRenderer>().sprite = null;
            transform.Find("Attunement").GetComponent<SpriteRenderer>().color = new Color(255,255,255,0);
            numAdj = 4;
        }
        mitosisEnabled = true;
        currentCD = IsCore ? 0 : MitosisCD;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!dead) {
            currAdj = 0;
            for (int i=0; i < 4; i++)
            {
                if (adjacent[i] != null)
                    currAdj++;
            }
	        if (currentCD <= 0)
            {
                if (mitosisEnabled) { 
                    Mitosis();
                    mitosisEnabled = false;
                }
            }
            if (mitosisEnabled)
                currentCD -= Time.deltaTime;
            if (currAdj <= 0 && IsCore)
            {
                dead = true;
                GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
                currentCD = DeadCD;
                return;
            }
            if (currAdj < numAdj && !mitosisEnabled)
            {
                mitosisEnabled = true;
                currentCD = MitosisCD;
            }
        } else {
            currentCD -= Time.deltaTime;
            if (currentCD <= 0)
                Destroy(gameObject);
        }
    }

    void SetAdjacent(int index, GameObject adj)
    {
        adjacent[index] = adj;
    }

    // GENERATE NEXT CELL, POTENTIAL CORE
    void Mitosis()
    {
        for (int i = 0; i < 4; i++)
        {
            if (currAdj >= numAdj)
                break;
            if (Random.value < (0.25f + currAdj*0.25f ))
            {
                var offset = new Vector3(0, 0, 0);
                switch(i)
                {
                    case 0:
                        offset.x = -2f;
                        //offset.y = -0.5f + Random.value;
                        break;
                    case 1:
                        offset.y = 2f;
                        //offset.x = -0.5f + Random.value;
                        break;
                    case 2:
                        offset.x = 2f;
                        //offset.y = -0.5f + Random.value;
                        break;
                    case 3:
                        offset.y = -2f;
                        //offset.x = -0.5f + Random.value;
                        break;
                }
                adjacent[i] = Instantiate(Cell, transform.position + offset, Quaternion.identity) as GameObject;
                switch (i)
                {
                    case 0:
                        adjacent[i].GetComponent<AlienCellScript>().SetAdjacent(2, gameObject);
                        break;
                    case 1:
                        adjacent[i].GetComponent<AlienCellScript>().SetAdjacent(3, gameObject);
                        break;
                    case 2:
                        adjacent[i].GetComponent<AlienCellScript>().SetAdjacent(0, gameObject);
                        break;
                    case 3:
                        adjacent[i].GetComponent<AlienCellScript>().SetAdjacent(1, gameObject);
                        break;
                }
            }
        }
    }

    bool CheckCore(int from)
    {
        if (IsCore)
            return true;
        for (int i=0; i < 4; i++)
        {
            if (i == from) {
                Debug.Log("Koniec!");
                continue;
            }
            if (adjacent[i] != null)
            {
                switch (i) { 
                    case 0:
                        if (adjacent[i].GetComponent<AlienCellScript>().CheckCore(2))
                            return true;
                        break;
                    case 1:
                        if (adjacent[i].GetComponent<AlienCellScript>().CheckCore(3))
                            return true;
                        break;
                    case 2:
                        if (adjacent[i].GetComponent<AlienCellScript>().CheckCore(0))
                            return true;
                        break;
                    case 3:
                        if (adjacent[i].GetComponent<AlienCellScript>().CheckCore(1))
                            return true;
                        break;
                }
            }
        }
        if (Random.value < 0.25f)
        {
            Mutate();
            return true;
        }
        return false;
    }

    //CELL BECOMES CORE
    void Mutate()
    {
        IsCore = true;
        GetComponent<SpriteRenderer>().sprite = CoreSprite;
        transform.Find("Attunement").GetComponent<SpriteRenderer>().sprite = null;
        GetComponent<BoxCollider2D>().enabled = false;
        mitosisEnabled = true;
        currentCD = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            if (other.GetComponent<ProjectileScript>().Attunement != Attunement) {
                for (int i = 0; i < 4; i++)
                {
                    if (adjacent[i] != null)
                    {
                        switch (i)
                        {
                            case 0:
                                adjacent[i].GetComponent<AlienCellScript>().CheckCore(2);
                                adjacent[i].GetComponent<AlienCellScript>().SetAdjacent(2, null);
                                break;
                            case 1:
                                adjacent[i].GetComponent<AlienCellScript>().CheckCore(3);
                                adjacent[i].GetComponent<AlienCellScript>().SetAdjacent(3, null);
                                break;
                            case 2:
                                adjacent[i].GetComponent<AlienCellScript>().CheckCore(0);
                                adjacent[i].GetComponent<AlienCellScript>().SetAdjacent(0, null);
                                break;
                            case 3:
                                adjacent[i].GetComponent<AlienCellScript>().CheckCore(1);
                                adjacent[i].GetComponent<AlienCellScript>().SetAdjacent(1, null);
                                break;
                        }
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}
