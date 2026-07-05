using System.Collections.Generic;
using UnityEngine;

public class CircleShots : MonoBehaviour
{
    public void CircleShot()
    {
        ShotManager._sM.magicShotSounds[0].Play();
        // 총알 발사
        for (float j = 0; j < 360; j += 6.5f)
        {
            GameObject bullet;
            if (ShotManager._sM.cbulletPool.Count > 0) //스택에 총알이 있으면
            {
                bullet = ShotManager._sM.cbulletPool.Pop(); //스택에서 총알을 꺼낸다이
                bullet.SetActive(true); //총알을 활성화한다이
            }
            else
            {
                bullet = Instantiate(ShotManager._sM.cBullet); //총알이 없으면 새로 만든다이
            }

            //총알 생성 위치를 보스 좌표로 한다.
            bullet.transform.position = transform.position;

            //Z에 값이 변해야 회전이 이루어지므로, Z에 j를 대입한다.
            bullet.transform.rotation = Quaternion.Euler(0, 0, j);
        }
    }
    public void CircleShotForPatternD()
    {
        ShotManager._sM.magicShotSounds[0].Play();
        // 총알 발사
        for (float j = 0; j < 360; j += 12.5f)
        {
            GameObject bullet;
            if (ShotManager._sM.cbulletPool.Count > 0) //스택에 총알이 있으면
            {
                bullet = ShotManager._sM.cbulletPool.Pop(); //스택에서 총알을 꺼낸다이
                bullet.SetActive(true); //총알을 활성화한다이
            }
            else
            {
                bullet = Instantiate(ShotManager._sM.cBullet); //총알이 없으면 새로 만든다이
            }

            //총알 생성 위치를 보스 좌표로 한다.
            bullet.transform.position = transform.position;

            //Z에 값이 변해야 회전이 이루어지므로, Z에 j를 대입한다.
            bullet.transform.rotation = Quaternion.Euler(0, 0, j);
        }
    }

    public void CircleShot2()
    {
        ShotManager._sM.magicShotSounds[1].Play();
        for (float j = 0; j < 360; j += 6f)
        {
            GameObject bullet;
            if (ShotManager._sM.cbulletPool.Count > 0) //스택에 총알이 있으면
            {
                bullet = ShotManager._sM.cbulletPool.Pop(); //스택에서 총알을 꺼낸다이
                bullet.SetActive(true); //총알을 활성화한다이
            }
            else
            {
                bullet = Instantiate(ShotManager._sM.cBullet); //총알이 없으면 새로 만든다이
            }

            //총알 생성 위치를 보스 좌표로 한다.
            bullet.transform.position = transform.position;

            //Z에 값이 변해야 회전이 이루어지므로, Z에 j를 대입한다.
            bullet.transform.rotation = Quaternion.Euler(0, 0, j);
        }
    }

    public void CircleShotGoto()
    {
        //Target방향으로 발사될 오브젝트 수록
        List<Transform> bullets = new List<Transform>();

        for (int i = 0; i < 360; i += 9)
        {
            //총알 생성
            GameObject cgbullet;
            if (ShotManager._sM.cgbulletPool.Count > 0) //스택에 총알이 있으면
            {
                cgbullet = ShotManager._sM.cgbulletPool.Pop(); //스택에서 총알을 꺼낸다이
                cgbullet.SetActive(true); //총알을 활성화한다이
            }
            else
            {
                cgbullet = Instantiate(ShotManager._sM.cgBullet); //총알이 없으면 새로 만든다이
            }

            //총알 생성 위치를 보스 좌표로 한다.
            cgbullet.transform.position = transform.position;

            //?초후에 Target에게 날아갈 오브젝트 수록
            bullets.Add(cgbullet.transform);

            //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
            cgbullet.transform.rotation = Quaternion.Euler(0, 0, i);
        }

        //총알을 Target 방향으로 이동시킨다.
        StartCoroutine(ShotManager._sM.BulletToTarget(bullets));
    }
}
