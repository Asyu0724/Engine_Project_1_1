using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class PlayerShot : MonoBehaviour
{
    private Rigidbody2D _rb;
    private bool _shot;
    [SerializeField] private Transform _shot1;
    [SerializeField] private Transform _shot2;
    [SerializeField] private GameObject playerBullet;
    public Stack<GameObject> bulletPool = new();
    public static PlayerShot instance;
    private int _bulletCount = 150;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CreateBullet();
    }

    private void FixedUpdate()
    {
        if (ShotManager._sM.story) return;
        if (_shot) return;
        if (Keyboard.current.ctrlKey.isPressed)
            StartCoroutine(DelayTime());
    }
    IEnumerator DelayTime()
    {
        _shot = true;
        Shot();
        Shot2();
        yield return new WaitForSeconds(0.1f);
        _shot = false;
    }

    private void CreateBullet()
    {
        for (int i = 0; i < _bulletCount; i++)
        {
            GameObject bullet = Instantiate(playerBullet); //총알을 생성한다
            bulletPool.Push(bullet); //총알을 스택에 넣는다
            bullet.SetActive(false); //총알을 비활성화한다
        }
    }

    private void Shot()
    {
        GameObject bullet;
        if (bulletPool.Count > 0) //스택에 총알이 있으면
        {
            bullet = bulletPool.Pop(); //스택에서 총알을 꺼낸다
            bullet.SetActive(true); //총알을 활성화한다
        }
        else
        {
            bullet = Instantiate(playerBullet); //총알이 없으면 새로 만든다
        }
        bullet.transform.position = _shot1.transform.position;
    }
    private void Shot2()
    {
        GameObject bullet;
        if (bulletPool.Count > 0) //스택에 총알이 있으면
        {
            bullet = bulletPool.Pop(); //스택에서 총알을 꺼낸다
            bullet.SetActive(true); //총알을 활성화한다
        }
        else
        {
            bullet = Instantiate(playerBullet); //총알이 없으면 새로 만든다
        }
        bullet.transform.position = _shot2.transform.position;
    }
}
