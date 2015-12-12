using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

    public GameObject wall1;
    public GameObject wall2;
    public float playerSpeed;

    private PathGenerator pathGen;
    private GameObject activeWall;
    private bool activeType; // True = two way, False = one way
    private bool[] activeCenter;
    private bool[] activeLeft;
    private bool[] activeRight;
    private byte goodCenter;
    private byte goodLeft;
    private byte goodRight;
    private Transform player;
    private Vector3 lastPosition;
    private Vector3 newPosition;
    private float startTime;
    private float journeyLength;
    private bool lastHigh;

    private bool win;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        pathGen = gameObject.GetComponent<PathGenerator>();
        win = false;
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        float distCovered = (Time.time - startTime) * playerSpeed;
        float fracJourney = distCovered / journeyLength;
        player.position = Vector3.Lerp(lastPosition, newPosition, fracJourney);
    }

    public void Try(bool guess)
    {
        if (activeType)
        {
            if (activeLeft[goodLeft] == guess)
            {
                activeWall.GetComponent<Wall>().Good2Way(true, goodLeft);
                goodLeft++;
                if (goodLeft >= goodRight && !lastHigh)
                {
                    lastPosition = player.position;
                    newPosition = new Vector3(-4.75f, -8.5f, 0);
                    journeyLength = Vector3.Distance(lastPosition, newPosition) * ((float)goodLeft / 6);
                    startTime = Time.time;
                    lastHigh = true;
                }
                if (goodLeft == activeLeft.Length)
                {
                    goodLeft = 0;
                    Destroy(activeWall);
                    Spawn();
                }
            }
            else
            {
                activeWall.GetComponent<Wall>().ResetColor(true);
                goodLeft = 0;
                if (goodLeft < goodRight && lastHigh)
                {
                    lastPosition = player.position;
                    newPosition = new Vector3(3.25f, -8.5f, 0);
                    journeyLength = Vector3.Distance(lastPosition, newPosition) * ((float)goodRight / 6);
                    startTime = Time.time;
                    lastHigh = false;
                }
            }
            if (activeRight[goodRight] == guess)
            {
                activeWall.GetComponent<Wall>().Good2Way(false, goodRight);
                goodRight++;
                if (goodLeft <= goodRight && lastHigh)
                {
                    lastPosition = player.position;
                    newPosition = new Vector3(3.25f, -8.5f, 0);
                    journeyLength = Vector3.Distance(lastPosition, newPosition) * ((float)goodRight / 6);
                    startTime = Time.time;
                    lastHigh = false;
                }
                if (goodRight == activeRight.Length)
                {
                    goodRight = 0;
                    Destroy(activeWall);
                    Spawn();
                }
            }
            else
            {
                activeWall.GetComponent<Wall>().ResetColor(false);
                goodRight = 0;
                if (goodLeft > goodRight && !lastHigh)
                {
                    lastPosition = player.position;
                    newPosition = new Vector3(-4.75f, -8.5f, 0);
                    journeyLength = Vector3.Distance(lastPosition, newPosition) * ((float)goodLeft / 6);
                    startTime = Time.time;
                    lastHigh = true;
                }
            }
        }
        else
        {
            if (activeCenter[goodCenter] == guess)
            {
                activeWall.GetComponent<Wall>().Good(goodCenter);
                goodCenter++;
                if (goodCenter == activeCenter.Length)
                {
                    goodCenter = 0;
                    Destroy(activeWall);
                    Spawn();
                }
                if (!lastHigh)
                {
                    journeyLength = Vector3.Distance(lastPosition, newPosition) * ((float)goodCenter / 6);
                    lastPosition = player.position;
                    newPosition = new Vector3(0, -8.5f, 0);
                    startTime = Time.time;
                    lastHigh = true;
                }
            }
            else
            {
                activeWall.GetComponent<Wall>().ResetColor();
                goodCenter = 0;
                player.position = lastPosition;
            }
        }
    }

    public void Spawn()
    {
        if (win)
        {
            return;
        }
        lastPosition = player.position;
        newPosition = new Vector3(0, -8.5f, 0);
        var p = pathGen.GenerateNext();
        if (p == null)
        {
            win = true;
            Debug.Log("Victory!");
            return;
        }
        if (p.l == p.u)
        {
            activeCenter = new bool[6];
            for (int i = 0; i < 6; i++)
            {
                activeCenter[i] = (p.l & (1 << i)) != 0 ? true : false;
            }
            activeWall = Instantiate(wall1);
            activeWall.GetComponent<Wall>().SetNumber(activeCenter);
            goodCenter = 0;
            activeType = false;
        }
        else
        {
            activeLeft = new bool[6];
            for (int i = 0; i < 6; i++)
            {
                activeLeft[i] = (p.l & (1 << i)) != 0 ? true : false;
            }
            activeRight = new bool[6];
            for (int i = 0; i < 6; i++)
            {
                activeRight[i] = (p.u & (1 << i)) != 0 ? true : false;
            }
            activeWall = Instantiate(wall2);
            if (Random.value > 0.5)
            {
                var ttmp = activeLeft;
                activeLeft = activeRight;
                activeRight = ttmp;
            }
            activeWall.GetComponent<Wall>().SetNumber2(activeLeft, activeRight);
            activeType = true;
            goodLeft = 0;
            goodRight = 0;
        }
    }
}
