﻿using System.Collections;
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
    private Sprite[] _livesSprite;

    [SerializeField]
    private Image _livesImage;

    // Start is called before the first frame update
    void Start()
    {
        _livesImage = GameObject.Find("Lives Display").GetComponent<Image>();
        _scoreText = GameObject.Find("Score Text").GetComponent<Text>();
        _scoreText.text = "Score:" + 0;
        _gameOverText = GameObject.Find("Game Over Text").GetComponent<Text>();
        _gameOverText.gameObject.SetActive(false);
    }

    public void UpdateScore(int _playerScore)
    {
        _scoreText.text = "Score: " + _playerScore;
    }

    public void UpdateLives(int _currentLives)
    {
        _livesImage.sprite = _livesSprite[_currentLives];
        //while (_currentLives <= 0)
        //{
        //    //StartCoroutine(Flicker());
        //}
    }

    //public void ShowGameOver()
    //{
    //    StartCoroutine(Flicker());
    //}

    public IEnumerator Flicker()
    {
        _gameOverText.gameObject.SetActive(true);
        yield return new WaitForSeconds(.25f);
        _gameOverText.gameObject.SetActive(false);
        yield return new WaitForSeconds(.25f);
    }
}
