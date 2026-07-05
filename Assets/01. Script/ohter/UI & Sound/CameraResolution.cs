using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    void Start()
    {
        // 1. 고정할 타겟 비율 설정 (16:9)
        float targetWidth = 16.0f;
        float targetHeight = 9.0f;
        float targetRatio = targetWidth / targetHeight;

        // 2. 현재 실행 중인 모니터(창)의 비율 계산
        float windowRatio = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowRatio / targetRatio;

        Camera camera = GetComponent<Camera>();

        // 3. 화면 비율에 맞춰 카메라의 렌더링 영역(Rect) 조절
        if (scaleHeight < 1.0f)
        {
            // 기기 화면이 목표 비율보다 세로가 길 때 (위아래 레터박스)
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            camera.rect = rect;
        }
        else
        {
            // 기기 화면이 목표 비율보다 가로가 길 때 (양옆 레터박스 - 보통 와이드 모니터)
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = camera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }
    }
}