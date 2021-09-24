using System;
using System.Collections.Generic;
using UnityEngine;

public partial class PuzzleWall : MonoBehaviour
{
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _lossSound;
    [SerializeField] private List<Scarab> _scarabs;
    [SerializeField] private List<Connection> _connections;
    [SerializeField] private string _winText;
    [SerializeField] private string _lossText;
    [SerializeField] private GameObject _winParticle;

    private GameStatus _gameStatus;
    private Scarab _currentScarab;
    private AudioSource _audioSource;

    public GameStatus _GameStatus => _gameStatus;

    public event Action<Scarab> UpdateCurrentScarab;
    public event Action<GameStatus, string> OnGameEnded;

    public void RestartPuzzle()
    {
        _gameStatus = GameStatus.NotStarted;
        _currentScarab = null;

        foreach (var connection in _connections)
        {
            connection.ResetConnection();
        }

        foreach (var scarab in _scarabs)
        {
            scarab.ResetScarab();
        }
    }

    private void Start()
    {
        _gameStatus = GameStatus.NotStarted;
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        foreach (var scarab in _scarabs)
        {
            scarab.ScarabActive += UpdateWall;
            scarab.ScarabTraped += CheckGameStatus;
        }
    }

    private void OnDisable()
    {
        foreach (var scarab in _scarabs)
        {
            scarab.ScarabActive -= UpdateWall;
            scarab.ScarabTraped -= CheckGameStatus;
        }
    }

    private void UpdateWall(Scarab currentScarab)
    {
        UpdateCurrentScarab?.Invoke(currentScarab);

        if (_gameStatus == GameStatus.NotStarted)
        {
            _currentScarab = currentScarab;
            _gameStatus = GameStatus.InProgress;
            return;
        }
        _currentScarab.SetInactive();
        _currentScarab = currentScarab;
    }

    private void CheckGameStatus(Scarab currentScarab)
    {
        if (_currentScarab.IsTrapped)
        {
            int numberOfActiveConnections = 0;
            foreach (var connection in _connections)
            {
                if(connection.IsActive == true)
                {
                    numberOfActiveConnections++;
                }
            }

            string gameStatusText;

            if (numberOfActiveConnections == _connections.Count)
            {
                _gameStatus = GameStatus.Win;
                _audioSource.PlayOneShot(_winSound);
                gameStatusText = _winText;
                _winParticle.SetActive(true);
            } 
            else
            {
                _gameStatus = GameStatus.Loss;
                _audioSource.PlayOneShot(_lossSound);
                gameStatusText = _lossText;
            }

            OnGameEnded?.Invoke(_gameStatus, gameStatusText);
        }
    }
}
