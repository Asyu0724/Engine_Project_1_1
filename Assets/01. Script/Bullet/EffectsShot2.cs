using System.Collections;
using UnityEngine;

public class EffectsShot2 : MonoBehaviour
{
    private static readonly WaitForSeconds Wait_3 = new(3f);
    public static EffectsShot2 Instance;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator Shooting()
    {
        LastPatternCircleShot();
        yield return Wait_3;
        StartCoroutine(Shooting());
    }

    public void LastPatternCircleShot()
    {
        ShotManager._sM.magicShotSounds[0].Play();
        // 총알 발사
        for (float j = 0; j < 360; j += 18f)
        {
            GameObject bullet;
            if (ShotManager._sM.cpbulletPool.Count > 0) //스택에 총알이 있으면
            {
                bullet = ShotManager._sM.cpbulletPool.Pop(); //스택에서 총알을 꺼낸다이
                bullet.SetActive(true); //총알을 활성화한다이
            }
            else
            {
                bullet = Instantiate(ShotManager._sM.cpBullet); //총알이 없으면 새로 만든다이
            }

            //총알 생성 위치를 비쥬얼 좌표로 한다.
            bullet.transform.position = transform.position;

            //Z에 값이 변해야 회전이 이루어지므로, Z에 j를 대입한다.
            bullet.transform.rotation = Quaternion.Euler(0, 0, j);
        }
    }
}
