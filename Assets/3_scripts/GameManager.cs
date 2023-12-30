using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

#region STATES
public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver,
    Quit
}
#endregion

#region SUB-STATES
public enum GameSubState
{
    StandingUp,
    Crouching,
    Prone
    // These are just examples, add as needed
}

#endregion

public class GameManager : Singleton<GameManager>
{
    [Header("Current States")]
    [SerializeField] private GameState gameState;
    [SerializeField] private GameSubState currentGameSubState;

    protected override void Awake()
    {
        // Check for duplicates of this Singleton.
        base.Awake();

        // Make the Game Manager persistent across all scenes.
        DontDestroyOnLoad(gameObject);

        // Subscribe to the scene loaded event.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This function was made for debugging on the Editor, allowing the GameManager to start from the main menu or from the game scene.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Handle the GameManager's behavior based on the loaded scene.
        if (scene.name == "Menu")
        {
            gameState = GameState.MainMenu;
        }
        else if (scene.name == "Main")
        {
            gameState = GameState.Playing;
            //currentArenaState = ArenaState.StoneAgeArena; //Set inicial arena state
        }
    }

    private void Start()
    {
        SetGameState(gameState);
    }

    public void SetGameState(GameState newState)
    {
        gameState = newState;
        HandleNewState();
    }

    private void HandleNewState()
    {
        switch (gameState)
        {
            case GameState.MainMenu:
                if (SceneManager.GetActiveScene().name != "Menu")
                {
                   SceneManager.LoadScene("Menu"); 
                }
                break;
            
            case GameState.Playing:
                if (SceneManager.GetActiveScene().name != "Main")
                {
                    SceneManager.LoadScene("Main");
                }
                HandleGameSubState();
                break;

            case GameState.Paused:
                Time.timeScale = 0;
                break;

            case GameState.GameOver:
                // LOAD SCENE GAME OVER
                break;

            case GameState.Quit:
                Application.Quit();
                break; 
        }
    }

    public void SetArenaState(GameSubState newSubState)
    {
        currentGameSubState = newSubState;
        HandleGameSubState();
    }

    private void HandleGameSubState()
    {
        switch (currentGameSubState)
        {
            case GameSubState.StandingUp:

                break;

            case GameSubState.Crouching:

                break;

            case GameSubState.Prone:

                break;
        }
    }

    public void StartGame()
    {
        SetGameState(GameState.Playing);
    }

    public void PauseGame(GameObject pausePanel)
    {
        pausePanel.SetActive(true);
        SetGameState(GameState.Paused);
    }

    public void ResumeGame(GameObject pausePanel)
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        SetGameState(GameState.Playing);
    }

    public void GameOver()
    {
        SetGameState(GameState.GameOver);
    }

    public void ReturnToMenu()
    {
        SetGameState(GameState.MainMenu);
    }

    public void QuitGame()
    {
        SetGameState(GameState.Quit);
    }
}
