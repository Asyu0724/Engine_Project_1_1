using UnityEngine;

public class ResolutionFixed : MonoBehaviour
{
    void Start()
    {
        // 1. 기본 해상도를 1920x1080 풀스크린으로 강제 설정
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);

        // 2. 카메라 컴포넌트 가져오기
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;

        // 목표로 하는 화면 비율 (16:9)
        float targetRatio = 1920f / 1080f; 
        // 현재 기기의 화면 비율
        float currentRatio = (float)Screen.width / (float)Screen.height;
        float scaleHeight = currentRatio / targetRatio;

        // 화면이 목표 비율(16:9)보다 세로로 길 때 (위아래 검은 레터박스)
        // 너의 2560x1600 모니터(16:10)가 여기에 해당돼!
        if (scaleHeight < 1f)
        {
            rect.height = scaleHeight;
            rect.width = 1f;
            rect.x = 0f;
            rect.y = (1f - scaleHeight) / 2f;
        }
        // 화면이 목표 비율(16:9)보다 가로로 길 때 (좌우 검은 필러박스)
        else
        {
            float scaleWidth = 1f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0f;
        }

        // 계산된 뷰포트 영역을 카메라에 적용
        camera.rect = rect;
    }
}