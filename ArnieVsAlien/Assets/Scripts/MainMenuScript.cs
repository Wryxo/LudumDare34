using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public GameObject MainMenu, Help, GameController;

    // Use this for initialization
    void Start () {
        if (GameControllerScript.Instance == null)
        {
            Instantiate(GameController);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
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
    }
}
