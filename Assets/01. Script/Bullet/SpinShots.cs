using System.Collections;
using UnityEngine;
using static ShotManager;

public class SpinShots : MonoBehaviour
{
    public IEnumerator SpShot()
    {
        for (int n = 0; n < 8; n++)
        {
            for (float i = 0; i < 360; i += 10f)
            {
                GameObject spbullet;
                if (_sM.spbulletPool.Count > 0) //스택에 총알이 있으면
                {
                    spbullet = _sM.spbulletPool.Pop(); //스택에서 총알을 꺼낸다
                    spbullet.SetActive(true); //총알을 활성화한다
                }
                else
                {
                    spbullet = Instantiate(_sM.spBullet); //총알이 없으면 새로 만든다이
                }

                //총알 생성 위치를 보스 좌표로 한다.
                spbullet.transform.position = transform.position;

                //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
                spbullet.transform.rotation = Quaternion.Euler(0, 0, i);

                yield return Wait_0_0_25;
            }
        }
    }
    public IEnumerator ReverseSpShot()
    {
        for (int k = 0; k < 8; k++)
        {
            for (float m = 180; m < 540; m += 10f)
            {
                GameObject spbullet;
                if (_sM.spbulletPool.Count > 0) //스택에 총알이 있으면
                {
                    spbullet = _sM.spbulletPool.Pop(); //스택에서 총알을 꺼낸다
                    spbullet.SetActive(true); //총알을 활성화한다
                }
                else
                {
                    spbullet = Instantiate(_sM.spBullet); //총알이 없으면 새로 만든다이
                }

                //총알 생성 위치를 보스 좌표로 한다.
                spbullet.transform.position = transform.position;

                //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
                spbullet.transform.rotation = Quaternion.Euler(0, 0, m);

                yield return Wait_0_0_25;
            }
        }
    }

    public IEnumerator SpShot2()
    {
        while (!ShotManager._sM.story)
        {
            for (float i = -90; i < 270; i += 10f)
            {
                GameObject spbullet;
                if (_sM.spbulletPool.Count > 0) //스택에 총알이 있으면
                {
                    spbullet = _sM.spbulletPool.Pop(); //스택에서 총알을 꺼낸다
                    spbullet.SetActive(true); //총알을 활성화한다
                }
                else
                {
                    spbullet = Instantiate(_sM.spBullet); //총알이 없으면 새로 만든다이
                }

                //총알 생성 위치를 보스 좌표로 한다.
                spbullet.transform.position = _sM._skillVisual[0].transform.position;

                //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
                spbullet.transform.rotation = Quaternion.Euler(0, 0, i);

                yield return Wait_0_0_5;
            }
        }
    }

    public IEnumerator ReverseSpShot2()
    {
        while (!ShotManager._sM.story)
        {
            for (float m = 250f; m >= -90f; m -= 10f)
            {
                GameObject spbullet;
                if (_sM.spbulletPool.Count > 0) //스택에 총알이 있으면
                {
                    spbullet = _sM.spbulletPool.Pop(); //스택에서 총알을 꺼낸다
                    spbullet.SetActive(true); //총알을 활성화한다
                }
                else
                {
                    spbullet = Instantiate(_sM.spBullet); //총알이 없으면 새로 만든다이
                }

                //총알 생성 위치를 보스 좌표로 한다.
                spbullet.transform.position = _sM._skillVisual[0].transform.position;

                //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
                spbullet.transform.rotation = Quaternion.Euler(0, 0, m);

                yield return Wait_0_0_5;
            }
        }
    }
    public IEnumerator FinSpinShot()
    {
        while (!ShotManager._sM.story)
        {
            for (float i = 90; i < 450; i += 10f)
            {
                GameObject spbullet;
                if (_sM.spbulletPool.Count > 0) //스택에 총알이 있으면
                {
                    spbullet = _sM.spbulletPool.Pop(); //스택에서 총알을 꺼낸다
                    spbullet.SetActive(true); //총알을 활성화한다
                }
                else
                {
                    spbullet = Instantiate(_sM.spBullet); //총알이 없으면 새로 만든다이
                }

                //총알 생성 위치를 보스 좌표로 한다.
                spbullet.transform.position = _sM._skillVisual[0].transform.position;

                //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
                spbullet.transform.rotation = Quaternion.Euler(0, 0, i);

                yield return Wait_0_0_5;
            }
        }
    }
    public IEnumerator FinRESpinShot()
    {
        while (!ShotManager._sM.story)
        {
            for (float i = 270; i >= -90; i -= 10f)
            {
                GameObject spbullet;
                if (_sM.spbulletPool.Count > 0) //스택에 총알이 있으면
                {
                    spbullet = _sM.spbulletPool.Pop(); //스택에서 총알을 꺼낸다
                    spbullet.SetActive(true); //총알을 활성화한다
                }
                else
                {
                    spbullet = Instantiate(_sM.spBullet); //총알이 없으면 새로 만든다이
                }

                //총알 생성 위치를 보스 좌표로 한다.
                spbullet.transform.position = _sM._skillVisual[0].transform.position;

                //Z에 값이 변해야 회전이 이루어지므로, Z에 i를 대입한다.
                spbullet.transform.rotation = Quaternion.Euler(0, 0, i);

                yield return Wait_0_0_5;
            }
        }
    }
}
