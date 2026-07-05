using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenSystemUI : MonoBehaviour
{
    public AudioSource selectSound;
    //재시작
    public void GameStart()
    {
        selectSound.Play();
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
    public void ReturnToStart()
    {
        selectSound.Play();
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void Settings()
    {
        selectSound.Play();
        UIManager.Instance.settingUI.SetActive(true);
    }
    public void QuitSettings()
    {
        selectSound.Play();
        UIManager.Instance.settingUI.SetActive(false);
    }
    public void Tutorial()
    {
        selectSound.Play();
        UIManager.Instance.TutorialUI.SetActive(true);
    }
    public void QuitTutorial()
    {
        selectSound.Play();
        UIManager.Instance.TutorialUI.SetActive(false);
    }

    //나가기
    public void Exit()
    {
        Application.Quit();
    }
}
