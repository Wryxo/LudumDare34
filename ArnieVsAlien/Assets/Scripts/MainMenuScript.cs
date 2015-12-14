using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public GameObject MainMenu, Help, Difficulty, GameController, Credits;

    private GameControllerScript gc;

    // Use this for initialization
    void Start () {
        if (GameControllerScript.Instance == null)
        {
            Instantiate(GameController);
        }
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerScript>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }

    public void SetMultiplier0(int x)
    {
        gc.Multipliers0 = x;
        var diff = GameObject.Find("CurrentDifficulty").GetComponent<Text>();
        var multiplier = gc.Multipliers0 + gc.Multipliers1 + gc.Multipliers2 + gc.Multipliers3;
        diff.text = string.Format("current modifier - {0} {1} {2} {3} => {4}x", gc.Multipliers0, gc.Multipliers1, gc.Multipliers2, gc.Multipliers3, multiplier);
    }

    public void SetMultiplier1(int x)
    {
        gc.Multipliers1 = x;
        var diff = GameObject.Find("CurrentDifficulty").GetComponent<Text>();
        var multiplier = gc.Multipliers0 + gc.Multipliers1 + gc.Multipliers2 + gc.Multipliers3;
        diff.text = string.Format("current modifier - {0} {1} {2} {3} => {4}x", gc.Multipliers0, gc.Multipliers1, gc.Multipliers2, gc.Multipliers3, multiplier);
    }
    public void SetMultiplier2(int x)
    {
        gc.Multipliers2 = x;
        var diff = GameObject.Find("CurrentDifficulty").GetComponent<Text>();
        var multiplier = gc.Multipliers0 + gc.Multipliers1 + gc.Multipliers2 + gc.Multipliers3;
        diff.text = string.Format("current modifier - {0} {1} {2} {3} => {4}x", gc.Multipliers0, gc.Multipliers1, gc.Multipliers2, gc.Multipliers3, multiplier);
    }
    public void SetMultiplier3(int x)
    {
        gc.Multipliers3 = x;
        var diff = GameObject.Find("CurrentDifficulty").GetComponent<Text>();
        var multiplier = gc.Multipliers0 + gc.Multipliers1 + gc.Multipliers2 + gc.Multipliers3;
        diff.text = string.Format("current modifier - {0} {1} {2} {3} => {4}x", gc.Multipliers0, gc.Multipliers1, gc.Multipliers2, gc.Multipliers3, multiplier);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowHelp()
    {
        MainMenu.SetActive(false);
        Help.SetActive(true);
    }

    public void ShowMainMenu()
    {
        MainMenu.SetActive(true);
        Help.SetActive(false);
        Difficulty.SetActive(false);
        Credits.SetActive(false);
    }

    public void ShowCredits()
    {
        MainMenu.SetActive(false);
        Credits.SetActive(true);
    }

    public void ShowDifficulty()
    {
        MainMenu.SetActive(false);
        Difficulty.SetActive(true);
        var diff = GameObject.Find("CurrentDifficulty").GetComponent<Text>();
        var multiplier = gc.Multipliers0 + gc.Multipliers1 + gc.Multipliers2 + gc.Multipliers3;
        diff.text = string.Format("current modifier - {0} {1} {2} {3} => {4}x", gc.Multipliers0, gc.Multipliers1, gc.Multipliers2, gc.Multipliers3, multiplier);
    }
}
