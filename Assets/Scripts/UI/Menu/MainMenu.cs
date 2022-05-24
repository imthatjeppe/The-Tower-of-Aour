using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu:MonoBehaviour {
    private PauseMenu pause;
    public Button loadButton;

    private void Start() {
        pause = GetComponent<PauseMenu>();
        float checkLoad = PlayerPrefs.GetFloat("PlayerPosX");
        if(checkLoad == 0) {
            PlayerPrefs.SetFloat("PlayerPosX", -21.2f);
            PlayerPrefs.SetFloat("PlayerPosY", 3.55f);
            PlayerPrefs.SetFloat("PlayerPosZ", 1.61f);
            PlayerPrefs.SetInt("CurrentFloor", 0);
            loadButton.GetComponent<Image>().color = new Color(100, 100, 100);
        }

        if(checkLoad != -21.2f) {
            loadButton.GetComponent<Image>().color = Color.white;
        }
    }

    public void PlayGame() {
        PlayerPrefs.SetFloat("PlayerPosX", -21.2f);
        PlayerPrefs.SetFloat("PlayerPosY", 3.55f);
        PlayerPrefs.SetFloat("PlayerPosZ", 1.61f);
        PlayerPrefs.SetInt("CurrentFloor", 0);
        SceneManager.LoadScene(2);
    }

    public void LoadGame() {
        float checkLoad = PlayerPrefs.GetFloat("PlayerPosX");
        if(checkLoad == -21.2f)
            return;

        SceneManager.LoadScene(2);
    }

    public void backToMainMenu() {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void BackInGame() {
        pause.PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        pause.GameIsPaused = false;
    }
    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

}
