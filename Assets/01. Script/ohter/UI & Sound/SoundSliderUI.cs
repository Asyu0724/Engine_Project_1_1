using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSliderUI : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource audioSource;
    public Slider scrollbar;

    public void SetBGMLevel(float value)
    {
        // float의 소수점 오차를 고려하여 0 대신 0.0001f 이하인지 체크합니다.
        if (value <= 0.0001f)
        {
            audioSource.mute = true;

            // AudioMixer의 볼륨도 최소치(보통 -80dB)로 명시적으로 낮춰줍니다.
            audioMixer.SetFloat("BGM", -80f);
        }
        else
        {
            audioSource.mute = false;

            // 값이 0보다 클 때만 Log10 연산을 수행하도록 else 문 안으로 이동시킵니다.
            audioMixer.SetFloat("BGM", Mathf.Log10(value) * 20f);
        }
    }
    public void SetSFXLevel(float value)
    {
        // float의 소수점 오차를 고려하여 0 대신 0.0001f 이하인지 체크합니다.
        if (value <= 0.0001f)
        {
            audioSource.mute = true;

            // AudioMixer의 볼륨도 최소치(보통 -80dB)로 명시적으로 낮춰줍니다.
            audioMixer.SetFloat("SFX", -80f);
        }
        else
        {
            audioSource.mute = false;

            // 값이 0보다 클 때만 Log10 연산을 수행하도록 else 문 안으로 이동시킵니다.
            audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20f);
        }
    }
    public void SetMasterLevel(float value)
    {
        // float의 소수점 오차를 고려하여 0 대신 0.0001f 이하인지 체크합니다.
        if (value <= 0.0001f)
        {
            audioSource.mute = true;

            // AudioMixer의 볼륨도 최소치(보통 -80dB)로 명시적으로 낮춰줍니다.
            audioMixer.SetFloat("Master", -80f);
        }
        else
        {
            audioSource.mute = false;

            // 값이 0보다 클 때만 Log10 연산을 수행하도록 else 문 안으로 이동시킵니다.
            audioMixer.SetFloat("Master", Mathf.Log10(value) * 20f);
        }
    }
}