using UnityEngine;
using System;
using System.Collections.Generic;

public partial class Scarab : MonoBehaviour
{
    [SerializeField] public List<Connection> _connections;
    [SerializeField] private List<Sprite> _scarabSprites;
    [SerializeField] private ScarabForm _scarabForm;
    [SerializeField] private AudioClip _activationSound;
    [SerializeField] private AudioClip _wrongSound;

    private PuzzleWall _puzzleWall;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private bool _isTrappted;

    public bool IsTrapped => _isTrappted;
    public ScarabForm _ScarabForm => _scarabForm;

    public event Action<Scarab> ScarabActive;
    public event Action<Scarab> ScarabTraped;

    public void ResetScarab()
    {
        _isTrappted = false;
        _scarabForm = ScarabForm.Disabled;
        _spriteRenderer.sprite = _scarabSprites[_scarabForm.GetHashCode()];
    }

    public void SetInactive()
    {
        _scarabForm = ScarabForm.Inactive;
        _spriteRenderer.sprite = _scarabSprites[_scarabForm.GetHashCode()];
    }

    private void Start()
    {
        _scarabForm = ScarabForm.Disabled;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _puzzleWall = GetComponentInParent<PuzzleWall>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (_scarabForm == ScarabForm.Disabled || _scarabForm == ScarabForm.Inactive)
        {
           if (_puzzleWall._GameStatus == GameStatus.NotStarted)
           {
                Activate();
                return;
           }

            foreach (var connection in _connections)
            {
                foreach (var scarab in connection._scarabs)
                {
                    if(scarab._ScarabForm == ScarabForm.Active)
                    {
                        if (connection.IsActive == false)
                        {
                            Activate();
                            CheckStatus();
                            return;
                        }
                    }
                        
                }
            }
            _audioSource.PlayOneShot(_wrongSound);
        }
    }
    private void Activate()
    {
        _audioSource.PlayOneShot(_activationSound);
        _scarabForm = ScarabForm.Active;
        _spriteRenderer.sprite = _scarabSprites[_scarabForm.GetHashCode()];
        ScarabActive?.Invoke(this);
    }

    private void CheckStatus()
    {
        int connectionsCount = 0;
        foreach (var connection in _connections)
        {
            if (connection.IsActive)
                connectionsCount++;
        }

        if (connectionsCount == _connections.Count)
        {
            _isTrappted = true;
            ScarabTraped?.Invoke(this);
        }
    }
}
