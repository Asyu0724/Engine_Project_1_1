using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotManager : MonoBehaviour
{
    [Header("Transform")]
    public GameObject _boss;
    public Transform[] _moveTransforms;
    public Transform _bossTransform;
    public float OffsetX { get; set; } = 3f;
    public float BossOffsetX { get; set; } = 6f;
    public float OffsetY { get; set; } = 6f;
    public Vector2 MinBounds { get; set; }
    public Vector2 MaxBounds { get; set; }
    public float Timer { get; set; }
    public float _MinTime { get; set; } = 1.5f;
    public float _MaxTime { get; set; } = 2.3f;
    public bool IsWaiting { get; set; } = false;
    public Vector3 Pos;


    [Header("Patterns And Shots")]

    // 1. 인스펙터 창에 띄워서 드래그 앤 드롭으로 연결할 인스턴스 변수들 (static 아님)
    [SerializeField] private Patterns patternsInstance;
    [SerializeField] private CircleShots circleShotsInstance;
    [SerializeField] private SpinShots spinShotsInstance;
    [SerializeField] private BossPrefabManager bossPrefabInstance;

    // 2. 다른 스크립트에서 편하게 가져다 쓸 static 프로퍼티들
    public static Patterns _patterns { get; private set; }
    public static CircleShots _circleShots { get; private set; }
    public static SpinShots _spinShots { get; private set; }
    public static BossPrefabManager _bossPrefabManager { get; private set; }

    [Header("Health")]
    [SerializeField] private BossHealth _bossHealth;

    [Header("Visuals")]
    [SerializeField] public GameObject[] _bossPrefab;
    [SerializeField] public GameObject[] _visuals;
    [SerializeField] public GameObject[] _skillVisual;

    [Header("Skill Activated")]
    [SerializeField] public bool firstPatternActivated;
    [SerializeField] public bool secondPatternActivated;
    [SerializeField] public bool thirdPatternActivated;
    [SerializeField] public bool finalPatternActivated;
    public bool story {  get; set; }
    public bool startShot = false;

    public Stack<GameObject> cbulletPool = new();
    public Stack<GameObject> cpbulletPool = new();
    public Stack<GameObject> cgbulletPool = new();
    public Stack<GameObject> spbulletPool = new();
    [Header("Prefabs")]
    [SerializeField] public GameObject cBullet;
    [SerializeField] public GameObject cpBullet;
    [SerializeField] public GameObject cgBullet;
    [SerializeField] public GameObject spBullet;
    private int cbulletCount = 700;
    private int cpbulletCount = 100;
    private int cgbulletCount = 300;
    private int spbulletCount = 300;
    public static ShotManager _sM;

    [Header("Sounds")]
    public AudioSource cloneSound;
    public AudioSource[] magicShotSounds;

    [Header("Final Pattern")]
    [field: SerializeField] public BossPrefab1RandomShot bossPrefab1RandomShot { get; private set; }
    [field: SerializeField] public BossPrefab2RandomShot bossPrefab2RandomShot { get; private set; }
    [field: SerializeField] public BossPrefab3RandomShot bossPrefab3RandomShot { get; private set; }

    // 캐시된 WaitForSeconds 인스턴스
    public static readonly WaitForSeconds Wait_0_3 = new(0.3f);
    public static readonly WaitForSeconds Wait_0_5 = new(0.5f);
    public static readonly WaitForSeconds Wait_0_7 = new(0.7f);
    public static readonly WaitForSeconds Wait_0_8 = new(0.8f);
    public static readonly WaitForSeconds Wait_0_2 = new(0.2f);
    public static readonly WaitForSeconds Wait_0_0_25 = new(0.025f);
    public static readonly WaitForSeconds Wait_0_0_5 = new(0.05f);
    public static readonly WaitForSeconds Wait_1 = new(1f);
    public static readonly WaitForSeconds Wait_2 = new(2f);
    public static readonly WaitForSeconds Wait_3 = new(3f);

    [Header("Target")]
    //총알을 생성후 Target에게 날아갈 변수
    public Transform Target;

    private void Awake()
    {
        _sM = this;
        // 3. 게임이 시작될 때(Awake), 인스펙터에 넣은 값을 static 프로퍼티에 덮어씌워줌
        _patterns = patternsInstance;
        _circleShots = circleShotsInstance;
        _spinShots = spinShotsInstance;
        _bossPrefabManager = bossPrefabInstance;
    }
    private void Start()
    {
        Timer = UnityEngine.Random.Range(_MinTime, _MaxTime);
        MinBounds = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        MaxBounds = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1));
        for (int i = 0; i < _visuals.Length; i++)
        {
            _visuals[i].SetActive(false);
        }
        firstPatternActivated = false;
        secondPatternActivated = false;
        thirdPatternActivated = false;
        CreateBullet();
    }

    private void Update()
    {
        if (story) return;
        if (IsWaiting) return;
        Timer -= Time.deltaTime;


        if (Timer <= 0)
        {
            StartCoroutine(_patterns.PatternA());
        }

        if (_bossHealth.Health <= 800)
        {
            if (finalPatternActivated)
                return;
            if (!finalPatternActivated)
            {
                for (int i = BulletMove.AllBullets.Count - 1; i >= 0; i--)
                {
                    if (BulletMove.AllBullets[i] != null)
                    {
                        BulletMove.AllBullets[i].ExplodeByBomb();
                    }
                }
                StartCoroutine(_patterns.FinalPattern());
                _skillVisual[1].SetActive(false);
                _skillVisual[2].SetActive(false);
            }
            finalPatternActivated = true;
        }

        if (_bossHealth.Health <= 1600)
        {
            if (thirdPatternActivated)
                return;
            if (!thirdPatternActivated)
            {
                for (int i = BulletMove.AllBullets.Count - 1; i >= 0; i--)
                {
                    if (BulletMove.AllBullets[i] != null)
                    {
                        BulletMove.AllBullets[i].ExplodeByBomb();
                    }
                }
                StartCoroutine(_patterns.PatternD());
            }
            thirdPatternActivated = true;
        }

        if (_bossHealth.Health <= 2400)
        {
            if (secondPatternActivated)
                return;
            if (!secondPatternActivated)
            {
                for (int i = BulletMove.AllBullets.Count - 1; i >= 0; i--)
                {
                    if (BulletMove.AllBullets[i] != null)
                    {
                        BulletMove.AllBullets[i].ExplodeByBomb();
                    }
                }
                StartCoroutine(_patterns.PatternC());
            }
            secondPatternActivated = true;
        }

        if (_bossHealth.Health <= 3200)
        {
            if (firstPatternActivated) 
                return;
            if (!firstPatternActivated)
            {
                for (int i = BulletMove.AllBullets.Count - 1; i >= 0; i--)
                {
                    if (BulletMove.AllBullets[i] != null)
                    {
                        BulletMove.AllBullets[i].ExplodeByBomb();
                    }
                }
                StartCoroutine(_patterns.PatternB());
            }
            firstPatternActivated = true;
        }
        

    }
   private void CreateBullet()
    {
        for (int i = 0; i < cbulletCount; i++)
        {
            GameObject bullet = Instantiate(cBullet); //총알을 생성한다
            cbulletPool.Push(bullet); //총알을 스택에 넣는다
            bullet.SetActive(false); //총알을 비활성화한다
        }
        for (int i = 0; i < cpbulletCount; i++)
        {
            GameObject bullet = Instantiate(cpBullet); //총알을 생성한다
            cpbulletPool.Push(bullet); //총알을 스택에 넣는다
            bullet.SetActive(false); //총알을 비활성화한다
        }
        for (int i = 0; i < cgbulletCount; i++)
        {
            GameObject bullet = Instantiate(cgBullet); //총알을 생성한다
            cgbulletPool.Push(bullet); //총알을 스택에 넣는다
            bullet.SetActive(false); //총알을 비활성화한다
        }
        for (int i = 0; i < spbulletCount; i++)
        {
            GameObject bullet = Instantiate(spBullet); //총알을 생성한다
            spbulletPool.Push(bullet); //총알을 스택에 넣는다
            bullet.SetActive(false); //총알을 비활성화한다
        }
    }

    public IEnumerator BulletToTarget(IList<Transform> objects)
    {
        //0.5초 후에 시작
        yield return Wait_0_8;

        for (int i = 0; i < objects.Count; i++)
        {
            //현재 총알의 위치에서 플레이의 위치의 벡터값을 뻴셈하여 방향을 구함
            Vector3 targetDirection = (Target.transform.position - objects[i].position).normalized;

            //x,y의 값을 조합하여 Z방향 값으로 변형함. -> ~도 단위로 변형
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            //Target 방향으로 이동
            objects[i].rotation = Quaternion.Euler(0, 0, angle);
        }

        //데이터 해제
        objects.Clear();
    }
}
