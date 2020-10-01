using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool gameIsPaused = false;

    public void Start()
    {
        //the pause menu must not be active by default when we launch the game
        pauseMenu.SetActive(false);
    }

    public void Update()
    {
        //if the user presses on escape :
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //if the game is not paused already 
            if (gameIsPaused == false) {
                //open the pause menu
                pauseMenu.SetActive(true);
                gameIsPaused = true;
            } 
            //if the game is already paused
            else
            {
                //close the pause menu
                ResumeButton();
            }
            
        }
    }

    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
        gameIsPaused = false;
    }

    public void MainMenuButton()
    {
        //takes the user to the main menu
        SceneManager.LoadScene("MainMenu");
    }
}
