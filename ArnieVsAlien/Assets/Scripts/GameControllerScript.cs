using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameControllerScript : MonoBehaviour
{

    public GameObject Cell;
    public float MitosisCD;
    public float currentCD;

    public int numCores;
    private int vertExtent, horzExtent;
    private AlienCellScript[][] map;
    private short[][] bfsMap;
    private int maxCore, maxCell;

    // Use this for initialization
    void Start()
    {
        vertExtent = (int)Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
        map = new AlienCellScript[horzExtent][];
        for (int i = 0; i < horzExtent; i++)
        {
            map[i] = new AlienCellScript[vertExtent];
            for (int j = 0; j < vertExtent; j++)
            {
                map[i][j] = null;
            }
        }
        maxCore = 3;
        maxCell = 1;
        for (int i = 0; i < 3; i++)
        {
            int x = Random.Range(0, horzExtent);
            int y = Random.Range(0, vertExtent);
            SpawnAlien(x, y);
            map[x][y].SetCore(true);
            Mitosis(x, y);
        }
        currentCD = MitosisCD / 2;
    }

    // Update is called once per frame
    void Update()
    {
        currentCD -= Time.deltaTime;
        if (currentCD <= 0)
        {
            numCores = 0;
            for (int i = 0; i < horzExtent; i++)
            {
                for (int j = 0; j < vertExtent; j++)
                {
                    if (map[i][j] != null)
                    {
                        if (!map[i][j].IsCore)
                            Mitosis(i, j);
                        else
                        {
                            if (CheckAlive(i,j))
                            {
                                Mitosis(i, j);
                                numCores++;
                            }
                        }
                    }
                }
            }
            if (numCores == 0) Victory();
            currentCD = MitosisCD;
        }
    }

    void Victory()
    {
        Debug.Log("VICTORY!");
    }

    bool SpawnAlien(int x, int y)
    {
        if (x >= 0 && x < horzExtent && y >= 0 && y < vertExtent && map[x][y] == null)
        {
            var go = Instantiate(Cell, new Vector3(-19 + x * 2, -14 + y * 2, 0), Quaternion.identity) as GameObject;
            map[x][y] = go.GetComponent<AlienCellScript>();
            map[x][y].SetCoord(x, y);
            if (Random.value > 0.9)
            {
                map[x][y].SetCore(true);
            }
            else
            {
                map[x][y].SetCore(false);
            }
            return true;
        }
        return false;
    }

    void Mitosis(int x, int y)
    {
        int numAdj = 0;
        for (int i = x-1; i < x+2; i++)
        {
            for (int j = y-1; j < y+2; j++)
            {
                if (i == x && j == y) continue;
                if (i < 0 || i >= horzExtent || j < 0 || j >= vertExtent) continue;
                if (map[i][j] != null) numAdj++;
            }
        }
        if (map[x][y].IsCore)
        {
            for (int i = 0; i < maxCore; i++)
            {
                int offx = Random.Range(-1, 2);
                int offy = Random.Range(-1, 2);
                if (x+offx < 0 || x+offx >= horzExtent || y+offy < 0 || y+offy >= vertExtent) continue;
                if (map[x+offx][y+offy] == null)
                {
                    if (SpawnAlien(x + offx, y + offy))
                    {
                        numAdj++;
                    }
                }
            }

        } else
        {
            for (int i = 0; i < maxCell; i++)
            {
                int offx = Random.Range(-1, 2);
                int offy = Random.Range(-1, 2);
                if (x+offx < 0 || x+offx >= horzExtent || y+offy < 0 || y+offy >= vertExtent) continue;
                if (map[x + offx][y + offy] == null)
                {
                    if (SpawnAlien(x+offx, y+offy))
                    {
                        numAdj++;
                    }
                }
            }
            /*for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {
                    if (i == x && j == y) continue;
                    if (i < 0 || i >= horzExtent || j < 0 || j >= vertExtent) continue;
                    if (SpawnAlien(i, j))
                    {
                        numAdj++;
                    }
                    if (numAdj >= maxCell) break;
                }
                if (numAdj >= maxCell) break;
            }*/
        }
    }

    public void DestroyAlien(int x, int y)
    {
        bfsMap = new short[horzExtent][];
        for (int i = 0; i < horzExtent; i++)
        {
            bfsMap[i] = new short[vertExtent];
            for (int j = 0; j < vertExtent; j++)
            {
                bfsMap[i][j] = 0;
            }
        }
        /*if (map[x + 1][y] != null)
        {
            if (!CheckCore(x + 1, y))
            {
                map[x + 1][y].SetCore(true);
            }
        }*/
        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = y - 1; j < y + 2; j++)
            {
                if (i == x && j == y) continue;
                if (i < 0 || i >= horzExtent || j < 0 || j >= vertExtent) continue;
                if (map[i][j] != null)
                {
                    if (!CheckCore(i, j))
                    {
                        map[i][j].SetCore(true);
                    }
                }
            }
        }
    }

    bool CheckAlive(int x, int y)
    {
        var alive = false;
        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = y - 1; j < y + 2; j++)
            {
                if (i == x && j == y) continue;
                if (i < 0 || i >= horzExtent || j < 0 || j >= vertExtent) continue;
                if (map[i][j] != null && !map[i][j].IsCore)
                {
                    alive = true;
                }
            }
        }
        if (!alive) { 
            Destroy(map[x][y].gameObject);
            return false;
        }
        return true;
    }

    bool CheckCore(int x, int y)
    {
        if (bfsMap[x][y] != 0)
            return bfsMap[x][y] > 0;
        bfsMap[x][y] = -1;
        if (map[x][y].IsCore)
        {
            bfsMap[x][y] = 1;
            return true;
        }
        for (int i = x - 1; i < x + 2; i++)
        {
            for (int j = y - 1; j < y + 2; j++)
            {
                if (i == x && j == y) continue;
                if (i < 0 || i >= horzExtent || j < 0 || j >= vertExtent) continue;
                if (map[i][j] != null && bfsMap[i][j] == 0)
                {
                    if (CheckCore(i, j))
                    {
                        bfsMap[x][y] = 1;
                        return true;
                    }
                }
            }
        }
        //if (map[x + 1][y] != null) if (CheckCore(x + 1, y)) return true;
        return false;
    }
}

public class Pair
{
    int x;
    int y;

    public Pair (int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return x + " " + y;
    }
}
