using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InGameESC : MonoBehaviour
{
    [Header("ESC UI")]
    [SerializeField] private GameObject escCanvasGroupObject;
    [SerializeField] private CanvasGroup escCanvasGroup;

    [Header("Audio")]
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource endingBackgroundMusic;
    [SerializeField] private AudioSource escSound;

    public bool isPaused { get; set; }
    
    public static InGameESC instance;

    private void Awake()
    {
        HidePauseUI();
        Time.timeScale = 1f;
        isPaused = false;
        instance = this;
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
                escSound.Play();
            }
        }
    }

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;

        if (backgroundMusic != null)
        {
            backgroundMusic.Pause();
        }

        if (endingBackgroundMusic != null)
        {
            endingBackgroundMusic.Pause();
        }

        ShowPauseUI();
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f;

        if (backgroundMusic != null)
        {
            backgroundMusic.UnPause();
        }

        if (endingBackgroundMusic != null)
        {
            endingBackgroundMusic.UnPause();
        }

        HidePauseUI();
    }

    public void ReturnToMain()
    {
        Time.timeScale = 1f;

        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
        }
        if (endingBackgroundMusic != null)
        {
            endingBackgroundMusic.Stop();
        }

        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    private void ShowPauseUI()
    {
        if (escCanvasGroupObject != null)
        {
            escCanvasGroupObject.SetActive(true);
        }

        if (escCanvasGroup == null && escCanvasGroupObject != null)
        {
            escCanvasGroup = escCanvasGroupObject.GetComponent<CanvasGroup>();
        }

        if (escCanvasGroup == null) return;

        escCanvasGroup.alpha = 1f;
        escCanvasGroup.interactable = true;
        escCanvasGroup.blocksRaycasts = true;
    }

    private void HidePauseUI()
    {
        if (escCanvasGroup == null && escCanvasGroupObject != null)
        {
            escCanvasGroup = escCanvasGroupObject.GetComponent<CanvasGroup>();
        }

        if (escCanvasGroup != null)
        {
            escCanvasGroup.alpha = 0f;
            escCanvasGroup.interactable = false;
            escCanvasGroup.blocksRaycasts = false;
        }

        if (escCanvasGroupObject != null)
        {
            escCanvasGroupObject.SetActive(false);
        }
    }
}
