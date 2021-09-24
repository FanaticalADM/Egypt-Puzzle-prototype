using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _mouseSesitivity;
    [SerializeField] private GameObject _vision;
    [Range(0,1)] [SerializeField] private float _walkSoundVolume;

    private float _horizontalInput;
    private float _verticalInput;
    private float _mouseMovementX;
    private float _mouseMovementY;
    private float _verticalLookRotation;
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
    }

    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");

            if (_verticalInput + _horizontalInput != 0)
                _audioSource.volume = _walkSoundVolume;
            else
                _audioSource.volume = 0;


            _mouseMovementX = Input.GetAxis("Mouse X");
            _mouseMovementY = Input.GetAxis("Mouse Y");
        }
        else
        {
            _audioSource.volume = 0;
        }

    }

    private void FixedUpdate()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            _verticalLookRotation += _mouseMovementY * _mouseSesitivity;
            _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -90f, 90f);
            _vision.transform.localEulerAngles = Vector3.left * _verticalLookRotation;
       

            transform.Rotate(_mouseMovementX * Vector3.up * _mouseSesitivity);
            Vector3 velocity = new Vector3(_horizontalInput, 0, _verticalInput);
            velocity.Normalize();
            velocity *= _speed * Time.deltaTime;
            Vector3 offset = transform.rotation * velocity;
            _rigidbody.MovePosition(transform.position + offset);
        }
    }
}
