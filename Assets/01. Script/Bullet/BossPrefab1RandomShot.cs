using System.Collections;
using UnityEngine;

public class BossPrefab1RandomShot : MonoBehaviour
{
    public void BossPrefab1ShotForFinalPattern()
    {
        int willShot = Random.Range(0, 5);

        if (willShot <= 1)
        {
            for (float rotation = 0; rotation < 360; rotation += 12f)
            {
                GameObject bullet;
                if (ShotManager._sM.cbulletPool.Count > 0) //스택에 적이 있으면
                {
                    bullet = ShotManager._sM.cbulletPool.Pop(); //스택에서 적을 꺼낸다
                    bullet.SetActive(true); //적을 활성화한다
                }
                else
                {
                    bullet = Instantiate(ShotManager._sM.cBullet); //적이 없으면 새로 만든다
                }
                //총알 생성 위치를 보스 좌표로 한다.
                bullet.transform.position = transform.position;
                //Z에 값이 변해야 회전이 이루어지므로, Z에 j를 대입한다.
                bullet.transform.rotation = Quaternion.Euler(0, 0, rotation);
            }
        }
        if (willShot == 2)
        {
            for (float rotation = 0; rotation < 360; rotation += 11.5f)
            {
                GameObject bullet;
                if (ShotManager._sM.cbulletPool.Count > 0) //스택에 적이 있으면
                {
                    bullet = ShotManager._sM.cbulletPool.Pop(); //스택에서 적을 꺼낸다
                    bullet.SetActive(true); //적을 활성화한다
                }
                else
                {
                    bullet = Instantiate(ShotManager._sM.cBullet); //적이 없으면 새로 만든다
                }
                //총알 생성 위치를 보스 좌표로 한다.
                bullet.transform.position = transform.position;
                //Z에 값이 변해야 회전이 이루어지므로, Z에 j를 대입한다.
                bullet.transform.rotation = Quaternion.Euler(0, 0, rotation);
            }
        }
        if (willShot == 3)
        {
            StartCoroutine(PrefabSpinShot());
        }
        if (willShot == 4)
        {
            StartCoroutine(PrefabReSpinShot());
        }
    }

    private IEnumerator PrefabReSpinShot()
    {
        for (float m = 180; m < 540; m += 10f)
        {
            GameObject spbullet;
            if (ShotManager._sM.spbulletPool.Count > 0) //스택에 총알이 있으면
            {
                spbullet = ShotManager._sM.spbulletPool.Pop(); //스택에서 총알을 꺼낸다
                spbullet.SetActive(true); //총알을 활성화한다
            }
            else
            {
                spbullet = Instantiate(ShotManager._sM.spBullet); //총알이 없으면 새로 만든다이
            }

            //총알 생성 위치를 보스 좌표로 한다.
            spbullet.transform.position = transform.position;

            //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
            spbullet.transform.rotation = Quaternion.Euler(0, 0, m);

            yield return ShotManager.Wait_0_0_25;
        }
    }
    private IEnumerator PrefabSpinShot()
    {
        for (float i = 0; i < 360; i += 10f)
        {
            GameObject spbullet;
            if (ShotManager._sM.spbulletPool.Count > 0) //스택에 총알이 있으면
            {
                spbullet = ShotManager._sM.spbulletPool.Pop(); //스택에서 총알을 꺼낸다
                spbullet.SetActive(true); //총알을 활성화한다
            }
            else
            {
                spbullet = Instantiate(ShotManager._sM.spBullet); //총알이 없으면 새로 만든다이
            }

            //총알 생성 위치를 보스 좌표로 한다.
            spbullet.transform.position = transform.position;

            //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
            spbullet.transform.rotation = Quaternion.Euler(0, 0, i);

            yield return ShotManager.Wait_0_0_25;
        }
    }
}
