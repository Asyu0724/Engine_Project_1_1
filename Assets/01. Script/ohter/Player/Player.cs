using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player playerInstance;
    public bool cannotDie;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _playerVisual;
    private Vector2 _moveDir;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float offset = 0.165f;
    private Vector2 MaxBounds;
    private Vector2 MinBounds;

    private void Awake()
    {
        playerInstance = this;
        cannotDie = false;
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        MaxBounds = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1));
        MinBounds = Camera.main.ViewportToWorldPoint(Vector2.zero);
    }

    private void FixedUpdate()
    {
        if (ShotManager._sM.story) return;
        _rb.linearVelocity = _moveDir * speed;
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            speed = 1.2f;
        }
        else
        {
            speed = 3f;
        }
    }

    public IEnumerator CannotDie()
    {
        if (cannotDie == true)
            yield break;
        cannotDie = true;
        StartCoroutine(CannotDieEffect());
        yield return new WaitForSeconds(2f);
        cannotDie = false;
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, MinBounds.x + offset, MaxBounds.x - offset);
        pos.y = Mathf.Clamp(pos.y, MinBounds.y + offset, MaxBounds.y - offset);

        transform.position = pos;
    }

    private IEnumerator CannotDieEffect()
    {
        Hitted.instance.OnHitEffect();
        while (cannotDie)
        {
            _playerVisual.GetComponent<SpriteRenderer>().color = Color.lightPink;
            yield return new WaitForSeconds(0.1f);
            _playerVisual.GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        _playerVisual.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>();
    }
}
