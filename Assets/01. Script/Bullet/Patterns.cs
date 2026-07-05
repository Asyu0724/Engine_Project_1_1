using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using static ShotManager;
using Random = UnityEngine.Random;

public class Patterns : MonoBehaviour
{

    public bool cannotDie;
    
    public static  Patterns instance;

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator PatternA()
    {
        _sM.Pos = transform.position;
        _sM.Pos.x = Random.Range(_sM.MinBounds.x + _sM.BossOffsetX, _sM.MaxBounds.x - _sM.BossOffsetX);
        _sM._boss.transform.position = _sM.Pos;
        _circleShots.CircleShot();
        
        _sM.IsWaiting = true;
        yield return Wait_0_3; 

        if (_sM.firstPatternActivated)
        {
            yield return StartCoroutine(PatternA2()); 
            yield return Wait_0_3; 
        }

        if (_sM.secondPatternActivated)
        {
            _circleShots.CircleShot2();
        }
        _sM.IsWaiting = false;
        _sM.Timer = Random.Range(_sM._MinTime, _sM._MaxTime);
    }

    private IEnumerator PatternA2()
    {
        if (_sM.thirdPatternActivated)
        {
            yield break; 
        }
        
        _circleShots.CircleShotGoto();
        _sM.magicShotSounds[2].Play();
        yield return Wait_0_5; 
    }
    private IEnumerator BulletToTarget(IList<Transform> objects)
    {
        //0.5초 후에 시작
        yield return Wait_0_8;

        foreach (var t in objects)
        {
            //현재 총알의 위치에서 플레이의 위치의 벡터값을 뻴셈하여 방향을 구함
            Vector3 targetDirection = (_sM.Target.transform.position - t.position).normalized;

            //x,y의 값을 조합하여 Z방향 값으로 변형함. -> ~도 단위로 변형
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            //Target 방향으로 이동
            t.rotation = Quaternion.Euler(0, 0, angle);
        }

        //데이터 해제
        objects.Clear();
    }
    public IEnumerator PatternB()
    {
        EffectEnabled effect = _sM._visuals[0].GetComponent<EffectEnabled>();
        _sM._visuals[0].SetActive(true);
        effect.PlayReady();
        _sM.IsWaiting = true;
        yield return Wait_3; //3초 동안 대기
        Tween castTween = effect.PlayCast();
        yield return castTween.WaitForCompletion();
        for (int i = 0; i < 20; i++)
        {
            _bossPrefabManager.MakeBossPrefabForPatternB();
            yield return Wait_0_2;
            _sM.cloneSound.Play();
            _bossPrefabManager.BossPrefabShotForPatternB();
            yield return Wait_0_5; // 0.5초 동안 대기
        }
        yield return Wait_1; // 1초 동안 대기
        _sM._bossPrefab[0].gameObject.SetActive(false);
        _sM._bossPrefab[1].gameObject.SetActive(false);
        _sM._bossPrefab[2].gameObject.SetActive(false);
        _sM.IsWaiting = false;
        _sM.Timer = Random.Range(_sM._MinTime, _sM._MaxTime);
    }

    public IEnumerator PatternC()
    {
        EffectEnabled effect = _sM._visuals[1].GetComponent<EffectEnabled>();
        _sM._visuals[1].SetActive(true);
        effect.PlayReady();
        _sM.IsWaiting = true;
        yield return Wait_3;

        Tween castTween = effect.PlayCast();
        yield return castTween.WaitForCompletion();
        _sM.Pos = _sM._moveTransforms[0].transform.position;
        _sM._skillVisual[0].SetActive(true);
        yield return Wait_0_5;
        
        _sM._boss.transform.position = _sM.Pos;

        // ✅ 수정됨: 코루틴을 변수에 담아서 실행합니다.
        Coroutine spShot = StartCoroutine(_spinShots.SpShot());
        Coroutine reverseSpShot = StartCoroutine(_spinShots.ReverseSpShot());
        
        yield return Wait_0_2;
        for (int m = 0; m < 3; m++)
        {
            yield return Wait_1;
            _circleShots.CircleShot();
            yield return Wait_1;
            _circleShots.CircleShot2();
        }
        yield return Wait_2;

        // ✅ 수정됨: 패턴 연출이 끝났으므로 발사 코루틴을 강제로 정지시킵니다!
        if (spShot != null) StopCoroutine(spShot);
        if (reverseSpShot != null) StopCoroutine(reverseSpShot);

        _sM._skillVisual[0].SetActive(false);
        _sM.Pos = _sM._bossTransform.transform.position;
        _sM._boss.transform.position = _sM.Pos;
        
        _sM.Timer = Random.Range(_sM._MinTime, _sM._MaxTime);
        _sM.IsWaiting = false;
    }

    public Transform realBossTransform;

    public IEnumerator PatternD()
    {
        // --- 1. 연출 및 준비 과정 ---
        EffectEnabled effect = _sM._visuals[2].GetComponent<EffectEnabled>();
        _sM._visuals[2].SetActive(true);
        effect.PlayReady();
        _sM.IsWaiting = true;
        yield return Wait_3;

        Tween castTween = effect.PlayCast();
        yield return castTween.WaitForCompletion();
        _sM._skillVisual[1].SetActive(true);
        _sM._skillVisual[2].SetActive(true);
        _sM._skillVisual[3].SetActive(true);
        _sM._skillVisual[4].SetActive(true);

        _bossPrefabManager.MakeBossPrefabForPatternD();
        yield return Wait_2;

        // --- 2. 보스 배열 묶기 ---
        Transform[] allBosses = new Transform[4];
        // this.transform 대신 진짜 보스 오브젝트를 넣어서 확실하게 움직이게 함!
        allBosses[0] = realBossTransform;
        allBosses[1] = _sM._bossPrefab[0].transform;
        allBosses[2] = _sM._bossPrefab[1].transform;
        allBosses[3] = _sM._bossPrefab[2].transform;

        // 사방을 뜻하는 4곳의 위치만 묶어주기 (0번 제외)
        Transform[] circlePoints = new Transform[4];
        circlePoints[0] = _sM._moveTransforms[1];
        circlePoints[1] = _sM._moveTransforms[2];
        circlePoints[2] = _sM._moveTransforms[3];
        circlePoints[3] = _sM._moveTransforms[4];

        int headPosIndex = 0;

        //시계방향 이동 및 꼬리물기 발사 + 추가 반복

        // 처음 1~4명 모이는 4번 + 4명이서 다 같이 도는 6번 = 총 10번 반복
        int totalSteps = 10;

        for (int step = 0; step < totalSteps; step++)
        {
            //step이 아무리 커져도 activeCount는 최대 4까지만 올라가게 제한
            int activeCount = Mathf.Min(step + 1, 4);

            // 이동 처리, activeCount가 4로 고정된 이후부터는 4명 모두 같이 이동
            for (int i = 0; i < activeCount; i++)
            {
                int targetPosIndex = (headPosIndex - i + 4) % 4;
                allBosses[i].position = circlePoints[targetPosIndex].position;
            }

            //발사
            _sM.Pos = allBosses[0].position;
            _circleShots.CircleShotForPatternD();

            for (int i = 1; i < activeCount; i++)
            {
                _bossPrefabManager.BossPrefabShotForPatternD(allBosses[i].position);
            }

            //대기 및 인덱스 이동
            yield return Wait_1;
            headPosIndex = (headPosIndex + 1) % 4;
        }
        yield return Wait_2;

        for (int hide = 0; hide < 3; hide++)
        {
            _sM._bossPrefab[hide].gameObject.SetActive(false);
        }
        _sM._skillVisual[3].SetActive(false);
        _sM._skillVisual[4].SetActive(false);
        _sM.IsWaiting = false;
        EffectsShot.Instance.StartCoroutine(EffectsShot.Instance.Shooting());
        EffectsShot2.Instance.StartCoroutine(EffectsShot2.Instance.Shooting());
    }

    public IEnumerator FinalPattern()
    {
        EffectEnabled effect = _sM._visuals[3].GetComponent<EffectEnabled>();
        _sM._visuals[3].SetActive(true);
        effect.PlayReady();
        _sM.IsWaiting = true;
        cannotDie = true;
        yield return Wait_3;

        Tween castTween = effect.PlayCast();
        yield return castTween.WaitForCompletion();
        _sM._skillVisual[0].SetActive(true);
        yield return Wait_0_5; //0.5초 동안 대기
        StartCoroutine(_spinShots.SpShot2());
        StartCoroutine(_spinShots.ReverseSpShot2());
        for (int i = 0; i < 40; i++)
        {
            _sM.cloneSound.Play();
            _bossPrefabManager.MakeBossPrefabForPatternB();
            yield return Wait_0_2; // 0.2초 동안 대기
            _sM.bossPrefab1RandomShot.BossPrefab1ShotForFinalPattern();
            _sM.bossPrefab2RandomShot.BossPrefab2ShotForFinalPattern();
            _sM.bossPrefab3RandomShot.BossPrefab3ShotForFinalPattern();
            yield return Wait_0_5; // 0.5초 동안 대기
        }
        StopCoroutine(_spinShots.SpShot2());
        StopCoroutine(_spinShots.ReverseSpShot2());
        yield return Wait_1; // 1초 동안 대기
        _sM._bossPrefab[0].gameObject.SetActive(false);
        _sM._bossPrefab[1].gameObject.SetActive(false);
        _sM._bossPrefab[2].gameObject.SetActive(false);
        FinalSpinPattern();
        _sM.IsWaiting = false;
        cannotDie = false;
        _sM.Timer = Random.Range(_sM._MinTime, _sM._MaxTime);
    }

    private void FinalSpinPattern()
    {
        StartCoroutine(_spinShots.SpShot2());
        StartCoroutine(_spinShots.ReverseSpShot2());
        StartCoroutine(_spinShots.FinSpinShot());
        StartCoroutine(_spinShots.FinSpinShot());
    }
}
