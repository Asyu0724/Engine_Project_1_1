using UnityEngine;
using static ShotManager;

public class BossPrefabManager : MonoBehaviour
{
    public void MakeBossPrefabForPatternB()
    {
        _sM.Pos = transform.position;
        _sM.Pos.x = Random.Range(_sM.MinBounds.x + _sM.OffsetX, _sM.MaxBounds.x - _sM.OffsetX);
        _sM._boss.transform.position = _sM.Pos;
        for (int repeat = 0; repeat < 3; repeat++)
        {
            _sM._bossPrefab[repeat].gameObject.SetActive(true);
            _sM.Pos.x = Random.Range(_sM.MinBounds.x + _sM.OffsetX, _sM.MaxBounds.x - _sM.OffsetX);
            _sM.Pos.y = Random.Range(_sM.MinBounds.y + _sM.OffsetY, _sM.MaxBounds.y - 0.6f);
            _sM._bossPrefab[repeat].transform.position = _sM.Pos;
            _sM._bossPrefab[repeat].gameObject.SetActive(true);
        }
    }

    public void BossPrefabShotForPatternB()
    {
        for (int j = 0; j < 3; j++)
        {
            for (float rotation = 0; rotation < 360; rotation += 12.5f)
            {
                GameObject bullet;
                if (_sM.cbulletPool.Count > 0) //스택에 적이 있으면
                {
                    bullet = _sM.cbulletPool.Pop(); //스택에서 적을 꺼낸다
                    bullet.SetActive(true); //적을 활성화한다
                }
                else
                {
                    bullet = Instantiate(_sM.cBullet); //적이 없으면 새로 만든다
                }

                //총알 생성 위치를 보스 좌표로 한다.
                bullet.transform.position = _sM._bossPrefab[j].transform.position;

                //Z에 값이 변해야 회전이 이루어지므로, Z에 j를 대입한다.
                bullet.transform.rotation = Quaternion.Euler(0, 0, rotation);
            }
        }
    }

    public void MakeBossPrefabForPatternD()
    {
        for (int repeat = 0; repeat < 3; repeat++)
        {
            _sM._bossPrefab[repeat].gameObject.SetActive(true);
            _sM._bossPrefab[repeat].transform.position = _sM._boss.transform.position;
        }
    }

    public void BossPrefabShotForPatternD(Vector3 firePosition)
    {
        for (float rotation = 0; rotation < 360; rotation += 12.5f)
        {
            GameObject bullet;
            if (_sM.cbulletPool.Count > 0)
            {
                bullet = _sM.cbulletPool.Pop();
                bullet.SetActive(true);
            }
            else
            {
                bullet = Instantiate(_sM.cBullet);
            }

            // 핵심: i 번호가 아니라, 넘겨받은 실제 위치에서 총알을 생성함!
            bullet.transform.position = firePosition;
            bullet.transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
    }
}
