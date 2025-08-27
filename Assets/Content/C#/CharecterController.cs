using UnityEngine;
using UnityEngine.InputSystem.XR;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class CharecterController : MonoBehaviour
{
    public float _moveSpeed = 5f;          // скорость передвижения
    public float _rotationSpeed = 10f;     // скорость поворота
    public float _gravity = -9.81f;        // сила гравитации
    public Transform _cameraTransform;     // камера (если null → MainCamera)

    private CharacterController _controller;
    private Animator _animator;
    private Vector3 _velocity;             // текущая скорость (Vector3)
    private bool _isGrounded;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        if (_cameraTransform == null && Camera.main != null)
            _cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        //проверка земли
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; //сброс при касании земли
        }

        //ввод (WASD)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //направление относительно камеры
        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = (camForward * vertical + camRight * horizontal).normalized;

        //анимация
        _animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
        _animator.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);

        //движение по XZ
        if (moveDir.magnitude >= 0.1f)
        {
            _controller.Move(moveDir * _moveSpeed * Time.deltaTime);

            // поворот к направлению движения
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        //гравитация
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
} 