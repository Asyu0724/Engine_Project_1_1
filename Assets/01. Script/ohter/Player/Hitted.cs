using System;
using UnityEngine;
using DG.Tweening; // DOTween 사용을 위해 반드시 추가해야 합니다.

public class Hitted : MonoBehaviour
{
    [Header("카메라 흔들림 설정 (Camera Shake Settings)")]
    [Tooltip("흔들리는 시간")]
    public float duration = 0.5f;   
    
    [Tooltip("흔들림의 강도")]
    public float strength = 0.5f;     
    
    [Tooltip("진동 횟수 (높을수록 덜덜 떨림)")]
    public int vibrato = 10;        
    
    [Tooltip("무작위성 (0~180)")]
    public float randomness = 90f;  
    
    [Tooltip("피격 사운드")]
    public AudioSource audioSource;
    
    public static Hitted instance;

    private void Awake()
    {
        instance = this;
    }

    // 다른 스크립트나 이벤트에서 이 메서드를 호출하면 화면이 흔들립니다.
    public void OnHitEffect()
    {
        // 1. 현재 씬의 메인 카메라를 찾아 흔듭니다.
        // 2. 이미 흔들리고 있는 중에 또 맞을 경우를 대비해 기존 트윈을 죽이고 새로 시작할 수도 있습니다.
        Camera.main.DOComplete(); // 진행 중인 카메라 트윈이 있다면 즉시 완료 처리 (선택 사항)
        Camera.main.DOShakePosition(duration, strength, vibrato, randomness);
        audioSource.Play();
        Debug.Log("피격! 화면이 흔들립니다.");
    }
}