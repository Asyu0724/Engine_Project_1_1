using UnityEngine;

public class UIManager : MonoBehaviour
{
    [field: SerializeField]
    public HealthUI healthUI { get; private set; }
    [field: SerializeField]
    public BombUI bombUI { get; private set; }
    [field: SerializeField]
    public ScoreUI scoreUI { get; private set; }
    public GameObject settingUI;    
    public GameObject TutorialUI;    
    

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
        settingUI.SetActive(false);
        TutorialUI.SetActive(false);
    }
}