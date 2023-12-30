using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnClick_Play()
    {
        GameManager.Instance.StartGame();
    }

    public void OnClick_Settings()
    {
        //GameManager.Instance.Settings();
        // menu & gameplay
    }

    public void OnClick_Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
