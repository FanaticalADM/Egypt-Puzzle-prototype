using UnityEngine;

public class Connection : MonoBehaviour
{
    public Scarab[] _scarabs;

    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _inactiveColor;

    private bool _isActive;
    private PuzzleWall _puzzleWall;
    private Scarab _currentScarab;
    private SpriteRenderer _spriteRenderer;

    public bool IsActive => _isActive;

    public void ResetConnection()
    {
        _currentScarab = null;
        _isActive = false;
        _spriteRenderer.color = _inactiveColor;
    }

    private void ScarabsSetup()
    {
        foreach (var scarab in _scarabs)
        {
            scarab._connections.Add(this);
        }        
    }

    private void OnDisable()
    {
        _puzzleWall.UpdateCurrentScarab -= Check;
    }

    private void Start()
    {
        ScarabsSetup();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = _inactiveColor;
        _puzzleWall = GetComponentInParent<PuzzleWall>();
        _puzzleWall.UpdateCurrentScarab += Check;
    }

    private void Activate()
    {
        _isActive = true;
        _spriteRenderer.color = _activeColor;
    }

    private void Check(Scarab currentScarab)
    {
        if (_puzzleWall._GameStatus == GameStatus.NotStarted)
        {
            _currentScarab = currentScarab;
            return;
        }

        Scarab previousScarab = _currentScarab;
        _currentScarab = currentScarab;

        int activatedScarabs = 0;
        foreach (var scarab in _scarabs)
        {
            if(scarab == previousScarab || scarab == _currentScarab)
            {
                activatedScarabs++;
            }
        }

        if(activatedScarabs == _scarabs.Length)
        {
            Activate();
        }
    }
}
