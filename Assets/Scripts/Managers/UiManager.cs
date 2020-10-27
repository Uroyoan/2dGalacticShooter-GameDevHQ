using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
  [SerializeField]
  private Text _scoreText;
  [SerializeField]
  private Image _livesImg;
  [SerializeField]
  private Image _thrusterBarImg;
  [SerializeField]
  private Sprite[] _livesSprite;
  [SerializeField]
  private Text _gameOverText;
  [SerializeField]
  private Text _restartText;
  [SerializeField]
  private Text _ammoText;

  private GameManager _gameManager;


  void Start()
  {
    _scoreText.text = "Score: " + 0;
    _gameOverText.gameObject.SetActive(false);
    _restartText.gameObject.SetActive(false);
    _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

    if (_gameManager == null)
    {
      Debug.Log("GameManager is NULL");
		}
  }
  
  public void UpdateThrusters(float ThrusterFuel)
  {
    _thrusterBarImg.fillAmount = ThrusterFuel;
	}


  public void UpdateAmmo(int playerAmmo)
  {
    _ammoText.text = "Ammo: " + playerAmmo;
  }


  public void UpdateScore(int playerScore)
  {
    _scoreText.text = "Score: " + playerScore;
	}


  public void UpdateLives(int currentLives)
  { 
    if (currentLives <= 0)
    {
      currentLives = 0;
      GameoverSequence();
		}
    else
    {
      _livesImg.sprite = _livesSprite[currentLives];
    }
	}


  IEnumerator GameOverFlickerRoutine()
  {
    while(true)
    {
      _gameOverText.text = "GAME OVER";
      yield return new WaitForSeconds(1f);
      _gameOverText.text = "";
      yield return new WaitForSeconds(1f);
		}
	}

  void GameoverSequence()
  {
    _gameManager.Gameover();
    _gameOverText.gameObject.SetActive(true);
    _restartText.gameObject.SetActive(true);
    _restartText.text = "Press F to Pay Respects";
    StartCoroutine(GameOverFlickerRoutine());
  }


}
