using UnityEngine;
using TMPro;

public class PuzzleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _gameStatusText;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private AudioClip _clickSound;

    private PuzzleWall _puzzleWall;
    private CapsuleCollider _capsuleCollider;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;

    private void Awake()
    {
        _puzzleWall = GetComponentInParent<PuzzleWall>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _puzzleWall.OnGameEnded += ActivateUI;
    }
    private void OnDisable()
    {
        _puzzleWall.OnGameEnded -= ActivateUI;
    }

    private void Start()
    {
        DeactivateUI();
    }

    private void ActivateUI(GameStatus gameStatus, string statusText)
    {
        _capsuleCollider.enabled = true;
        _spriteRenderer.enabled = true;
        _canvas.gameObject.SetActive(true);

        _gameStatusText.text = statusText;

        if (gameStatus == GameStatus.Win)
        {
            _spriteRenderer.color = Color.green;
        }else if (gameStatus == GameStatus.Loss)
        {
            _spriteRenderer.color = Color.red;
        }
    }

    private void DeactivateUI()
    {
        _capsuleCollider.enabled = false;
        _spriteRenderer.enabled = false;
        _canvas.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if(_puzzleWall._GameStatus == GameStatus.Loss)
        {
            _audioSource.PlayOneShot(_clickSound);
            _puzzleWall.RestartPuzzle();
            DeactivateUI();
        }
    }
}
