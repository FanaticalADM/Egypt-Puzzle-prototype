using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _descriptionCanvas;
    [SerializeField] private GameObject _playerCanvas;
    [SerializeField] private GameObject _exitMenuCanvas;

    private bool _isDescriptionOpen;

    //Start Button in Guide Canvas
    public void StartGame()
    {
        _isDescriptionOpen = false;
        _descriptionCanvas.SetActive(false);
        _playerCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Exit Game Button
    public void ExitGame()
    {
        Application.Quit();
    }

    //Menu Button
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void Awake()
    {
        _playerCanvas.SetActive(false);
        _exitMenuCanvas.SetActive(false);
        _descriptionCanvas.SetActive(true);
        _isDescriptionOpen = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _isDescriptionOpen == false)
            ToggleMenu();
    }

    private void ToggleMenu()
    {
        _exitMenuCanvas.SetActive(!_exitMenuCanvas.activeInHierarchy);
        if (_exitMenuCanvas.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
