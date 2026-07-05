using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public int healthCount = 5;
    public int currentHealth = 5;
    [SerializeField] private GameObject lifePrefab;
    private List<GameObject> hearts = new List<GameObject>();

    public void InitHealth()
    {
        //목숨개수만큼 life ui 생성해서 배치하기
        for (int i = 0; i < healthCount; i++)
        {
            GameObject heart = Instantiate(lifePrefab, transform);
            hearts.Add(heart); // 생성된 복제본을 리스트에 저장
        }
    }

    public void DestroyHealth()
    {
        // 리스트의 마지막 요소를 가져옴
        GameObject target = hearts[hearts.Count - 1];

        // 리스트에서 제거하고
        hearts.Remove(target);

        // 실제 게임 화면에서 삭제
        Destroy(target);
    }
}
