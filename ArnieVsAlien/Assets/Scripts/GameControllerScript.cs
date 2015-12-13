using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{

    public GameObject Cell, Ammo0, Ammo1, Arnie, VictoryGO, DefeatGO, GameMenu, GameUI, BackButton;
    public float MitosisCD, currentCDMitosis;
    public float AmmoCD, currectCDAmmo;
    public int CoreValue, CellValue;

    public int numCores;

    private long score;
    private int vertExtent, horzExtent;
    private AlienCellScript[][] map;
    private short[][] bfsMap;
    private int maxCore, maxCell;
    private Slider MitosisCDSlider;
    private Text scoreText;
    private AudioSource slime;

    // Use this for initialization
    void Start()
    {
        slime = transform.Find("SlimeAudio").GetComponent<AudioSource>();
        MitosisCDSlider = GameObject.Find("MitosisCDSlider").GetComponent<Slider>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        score = 0;
        vertExtent = (int)Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
        vertExtent -= 2;
        map = new AlienCellScript[horzExtent][];
        for (int i = 0; i < horzExtent; i++)
        {
            map[i] = new AlienCellScript[vertExtent];
            for (int j = 0; j < vertExtent; j++)
            {
                map[i][j] = null;
            }
        }
        maxCore = 4;
        maxCell = 1;
        for (int i = 0; i < 4; i++)
        {
            int x = Random.Range(0, horzExtent);
            int y = Random.Range(0, vertExtent);
            SpawnAlien(x, y, 0, 0);
            map[x][y].SetCore(true);
            numCores++;
            Mitosis(x, y);
        }
        currentCDMitosis = MitosisCD;
        currectCDAmmo = AmmoCD / 2;
        Instantiate(Arnie);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        currectCDAmmo -= Time.deltaTime;
        currentCDMitosis -= Time.deltaTime;
        MitosisCDSlider.normalizedValue = currentCDMitosis / MitosisCD;
        if (currentCDMitosis <= 0)
        {
            if (!slime.isPlaying) slime.Play();
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
            currentCDMitosis = MitosisCD;
        }
        if (currectCDAmmo <= 0)
        {
            int x = Random.Range(0, horzExtent);
            int y = Random.Range(0, vertExtent);
            SpawnAmmo(x, y, true);
            x = Random.Range(0, horzExtent);
            y = Random.Range(0, vertExtent);
            SpawnAmmo(x, y, false);
            currectCDAmmo = AmmoCD;
        }
    }

    public void IncreaseScore(bool core)
    {
        Debug.Log("score " + numCores);
        score += (core ? CoreValue : CellValue) * numCores;
    }

    public void ShowGameMenu(bool end)
    {
        if (end)
        {
            BackButton.GetComponent<Button>().interactable = false;
        } else
        {
            BackButton.GetComponent<Button>().interactable = true;
        }
        GameMenu.SetActive(true);
        //GameUI.SetActive(false);
        PauseGame(true);
    }

    public void HideGameMenu()
    {
        GameMenu.SetActive(false);
        //GameUI.SetActive(true);
        PauseGame(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        Destroy(GameObject.Find("DJ"));
        SceneManager.LoadScene("Menu");
    }

    void PauseGame(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0.0f;
        } else
        {
            Time.timeScale = 1.0f;
        }
    }

    void Victory()
    {
        VictoryGO.SetActive(true);
        ShowGameMenu(true);
    }

    public void Defeat()
    {
        DefeatGO.SetActive(true);
        ShowGameMenu(true);
    }

    void SpawnAmmo(int x, int y, bool type)
    {
        if (type)
        {
            Instantiate(Ammo0, new Vector3(-horzExtent+1 + x * 2, -vertExtent-1 + y * 2, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(Ammo1, new Vector3(-horzExtent+1 + x * 2, -vertExtent-1 + y * 2, 0), Quaternion.identity);
        }
    }

    bool SpawnAlien(int x, int y, int ox, int oy)
    {
        if (x >= 0 && x < horzExtent && y >= 0 && y < vertExtent && map[x][y] == null)
        {
            var go = Instantiate(Cell, new Vector3(-horzExtent+1 + x * 2, -vertExtent-1 + y * 2, 0), Quaternion.identity) as GameObject;
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
            map[x][y].UsedMitosis();
            for (int i = 0; i < maxCore; i++)
            {
                int offx = Random.Range(-1, 2);
                int offy = Random.Range(-1, 2);
                if (x+offx < 0 || x+offx >= horzExtent || y+offy < 0 || y+offy >= vertExtent) continue;
                if (map[x+offx][y+offy] == null)
                {
                    if (SpawnAlien(x + offx, y + offy, offx, offy))
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
                    if (SpawnAlien(x + offx, y + offy, offx, offy))
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
                if (map[i][j] != null)
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
