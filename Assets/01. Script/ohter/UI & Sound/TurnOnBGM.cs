using UnityEngine;

public class TurnOnBGM : MonoBehaviour
{
    [SerializeField] private AudioSource BGM;
    public static TurnOnBGM instance;
    private void Awake()
    {
        instance = this;
    }
    public void TurnOn()
    {
        BGM.Play();
    }
}
