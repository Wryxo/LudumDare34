using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript Instance = null;

    public GameObject Cell, Arnie, VictoryGO, DefeatGO, GameMenu, GameUI;
    public GameObject[] Tiles;
    public float currentCDMitosis;
    public int CoreValue, CellValue;

    public int Multipliers0 { get { return controls ? 1 : 2; } set { controls = value == 1 ? true : false;} }
    public int Multipliers1 { get { return 20 - mitosisCD; }  set { mitosisCD = value; } }
    public int Multipliers2 { get { return maxCore; } set { maxCore = value; } }
    public int Multipliers3 { get { return maxCell; } set { maxCell = value; } }
    private bool controls;
    private int maxCore, maxCell, multiplier, mitosisCD;

    public int numCores;

    private long score;
    private int vertExtent, horzExtent;
    private AlienCellScript[][] map;
    private short[][] bfsMap;
    private Slider MitosisCDSlider;
    private Text scoreText;
    private AudioSource slime;
    private GameObject ui;
    private bool menu;
    private bool menuInited;

    void Awake()
    {
        //Check if instance already exists
        if (Instance == null)

            //if not, set instance to this
            Instance = this;

        //If instance already exists and it's not this:
        else if (Instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        menu = true;
        InitMenu();
        menuInited = true;
    }

    // Use this for initialization
    void OnLevelWasLoaded(int level)
    {
        switch(level)
        {
            case 0:
                menu = true;
                if (!menuInited) InitMenu();
                break;
            case 1:
                menu = false;
                InitGame();
                break;
        }

    }

    void InitGame()
    {
        ui = GameObject.Find("UI");
        var go = Instantiate(GameUI) as GameObject;
        go.transform.SetParent(ui.transform, false);

        slime = transform.Find("SlimeAudio").GetComponent<AudioSource>();
        MitosisCDSlider = GameObject.Find("MitosisCDSlider").GetComponent<Slider>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

        vertExtent = (int)Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
        vertExtent -= 2;
        score = 0;
        multiplier = Multipliers0 + Multipliers1 + Multipliers2 + Multipliers3;

        GenerateBackground();
        GenerateMap();
        go = Instantiate(Arnie) as GameObject;
        go.GetComponent<PlayerControllerScript>().Controls = controls;

        currentCDMitosis = mitosisCD;
    }

    void InitMenu()
    {
        Multipliers0 = 2;
        Multipliers1 = 12;
        Multipliers2 = 3;
        Multipliers3 = 1;
        multiplier = Multipliers0 + Multipliers1 + Multipliers2 + Multipliers3;
    }

    // Update is called once per frame
    void Update()
    {
        if (!menu)
        {
            LevelUpdate();
        }
    }

    void LevelUpdate()
    {
        scoreText.text = score.ToString();
        currentCDMitosis -= Time.deltaTime;
        MitosisCDSlider.normalizedValue = currentCDMitosis / mitosisCD;
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
                            if (CheckAlive(i, j))
                            {
                                Mitosis(i, j);
                                numCores++;
                            }
                        }
                    }
                }
            }
            if (numCores == 0) Victory();
            currentCDMitosis = mitosisCD;
        }
    }

    void GenerateBackground()
    {
        var up = GameObject.Find("StenaUp").transform;
        up.position = new Vector3(0, vertExtent-2);
        var down = GameObject.Find("StenaDown").transform;
        down.position = new Vector3(0, -vertExtent-2);
        var left = GameObject.Find("StenaLeft").transform;
        left.position = new Vector3(horzExtent+0.5f, 0);
        var right = GameObject.Find("StenaRight").transform;
        right.position = new Vector3(-horzExtent-0.5f, 0);
        for (int i = 0; i < horzExtent+1; i++)
        {
            for (int j = 0; j < vertExtent; j++)
            {
                GameObject tile;
                if (Random.value > 0.96)
                {
                    tile = Tiles[2];
                } else
                {
                    tile = Random.value > 0.5 ? Tiles[0] : Tiles[1];
                }
                var go = Instantiate(tile, new Vector3(-horzExtent + i * 2, -vertExtent-1 + j * 2, 10), Quaternion.identity) as GameObject;
            }
        }
    }

    void GenerateMap()
    {
        map = new AlienCellScript[horzExtent][];
        for (int i = 0; i < horzExtent; i++)
        {
            map[i] = new AlienCellScript[vertExtent];
            for (int j = 0; j < vertExtent; j++)
            {
                map[i][j] = null;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            int x = Random.Range(0, horzExtent);
            int y = Random.Range(0, vertExtent);
            SpawnAlien(x, y, 0, 0);
            map[x][y].SetCore(true);
            numCores++;
            Mitosis(x, y);
        }
    }
    
    public void IncreaseScore(bool core)
    {
        score += (core ? CoreValue : CellValue) * numCores * multiplier;
    }

    public void ShowGameMenu(bool end)
    {
        if (GameObject.FindGameObjectWithTag("GameMenu") == null) { 
            PauseGame(true);
            var go = Instantiate(GameMenu) as GameObject;
            go.transform.SetParent(ui.transform, false);
            if (end)
            {
                GameObject.Find("BackButtonT").GetComponent<Button>().interactable = false;
            } else
            {
                GameObject.Find("BackButtonT").GetComponent<Button>().interactable = true;
            }
        }
        //GameUI.SetActive(false);
    }

    public void HideGameMenu()
    {
        Destroy(GameObject.FindGameObjectWithTag("GameMenu"));
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
        var go = Instantiate(VictoryGO) as GameObject;
        go.transform.SetParent(ui.transform, false);
        ShowGameMenu(true);
    }

    public void Defeat()
    {
        var go = Instantiate(DefeatGO) as GameObject;
        go.transform.SetParent(ui.transform, false);
        ShowGameMenu(true);
    }

    bool SpawnAlien(int x, int y, int ox, int oy)
    {
        if (x >= 0 && x < horzExtent && y >= 0 && y < vertExtent && map[x][y] == null)
        {
            var go = Instantiate(Cell, new Vector3(-horzExtent+1 + x * 2, -vertExtent-1 + y * 2, 0), Quaternion.identity) as GameObject;
            map[x][y] = go.GetComponent<AlienCellScript>();
            map[x][y].GetComponent<Animator>().speed = 10 / mitosisCD;
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
