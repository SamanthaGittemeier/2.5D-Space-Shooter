using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoCountText;
    [SerializeField]
    private Text _maxAmmoText;
    [SerializeField]
    private Text _youWonText;

    [SerializeField]
    private Sprite[] _livesSprite;

    [SerializeField]
    private Image _livesImage;

    [SerializeField]
    private GameObject _ammoCountWhite;
    [SerializeField]
    private GameObject _ammoCountRed;

    [SerializeField]
    private bool _bossDead;

    [SerializeField]
    private Button _quitButton;
    [SerializeField]
    private Button _playAgainButton;

    void Start()
    {
        _livesImage = GameObject.Find("Lives Display").GetComponent<Image>();
        _scoreText = GameObject.Find("Score Text").GetComponent<Text>();
        _scoreText.text = "Score:" + 0;
        _gameOverText = GameObject.Find("Game Over Text").GetComponent<Text>();
        _gameOverText.gameObject.SetActive(false);
        _restartText = GameObject.Find("Restart Text").GetComponent<Text>();
        _restartText.gameObject.SetActive(false);
        _ammoCountText = GameObject.Find("Ammo Count Text").GetComponent<Text>();
        _ammoCountText.text = "15";
        _ammoCountWhite = GameObject.Find("Ammo Count White");
        _ammoCountRed = GameObject.Find("Ammo Count Red");
        _ammoCountRed.SetActive(false);
        _maxAmmoText = GameObject.Find("Max Ammo Text").GetComponent<Text>();
        _youWonText = GameObject.Find("You Won Text").GetComponent<Text>();
        _youWonText.gameObject.SetActive(false);
        _quitButton = GameObject.Find("Quit Button").GetComponent<Button>();
        _quitButton.gameObject.SetActive(false);
        _playAgainButton = GameObject.Find("Play Again Button").GetComponent<Button>();
        _playAgainButton.gameObject.SetActive(false);
    }

    public void UpdateScore(int _playerScore)
    {
        _scoreText.text = "Score: " + _playerScore;
    }

    public void UpdateAmmo(int _currentAmmoCount)
    {
        _ammoCountText.text = _currentAmmoCount.ToString();
        if (_currentAmmoCount == 0)
        {
            _ammoCountWhite.SetActive(false);
            _ammoCountRed.SetActive(true);
        }
        else if (_currentAmmoCount != 0)
        {
            _ammoCountWhite.SetActive(true);
            _ammoCountRed.SetActive(false);
        }
    }

    public void UpdateMaxAmmo(int _maxAmmo)
    {
        _maxAmmoText.text = "/ " + _maxAmmo.ToString();
    }

    public void UpdateLives(int _currentLives)
    {
        _livesImage.sprite = _livesSprite[_currentLives];
        if (_currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void IsBossDead(bool _bossIsDead)
    {
        _bossDead = _bossIsDead;
        GameWonSequence();
    }

    void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(Flicker());
    }

    public IEnumerator Flicker()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(.5f);
        }
    }

    void GameWonSequence()
    {
        _youWonText.gameObject.SetActive(true);
        _playAgainButton.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
        StartCoroutine(WinFlicker());
    }

    IEnumerator WinFlicker()
    {
        while (true)
        {
            _youWonText.text = "YOU WON";
            yield return new WaitForSeconds(.5f);
            _youWonText.text = "";
            yield return new WaitForSeconds(.5f);
        }
    }
}
