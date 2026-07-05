using System;
using TMPro;
using UnityEngine;

public class RestPhase : MonoBehaviour
{
    public static RestPhase instance;
    [SerializeField] private TextMeshProUGUI restPhases;
    private int remainPhasesNum = 4;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        restPhases.text = $"{remainPhasesNum}";
    }

    public void UpdateRestPhases()
    {
        remainPhasesNum--;
        restPhases.text = $"{remainPhasesNum}";
    }
}
