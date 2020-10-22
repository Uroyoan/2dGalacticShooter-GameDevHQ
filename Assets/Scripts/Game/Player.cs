using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
  [SerializeField]
  private float _speed = 8f;
  private float _modifiedSpeed;
  [SerializeField]
  private int _lives = 3;

  [SerializeField]
  private AudioClip _deathClip;
  [SerializeField]
  private AudioClip _laserClip;
  [SerializeField]
  private AudioClip _powerupClip;

  [SerializeField]
  private GameObject _laserPrefab;
  [SerializeField]
  private GameObject _tripleShotPrefab;
  [SerializeField]
  private float _fireRate = 0.2f;
  private float _canFire = 0f;

  private UiManager _uiManager;
  [SerializeField]
  private int _score = 0;
  [SerializeField]
  private GameObject _leftEngineVisualizer;
  [SerializeField]
  private GameObject _rightEngineVisualizer;
  private Animator _anim;
  private bool _death = false;

  private SpawnManager _spawnManager;
  [SerializeField]
  private float _powerTimer = 5f;

  //For Animation
  private bool _tripleShotActive = false;

  [SerializeField]
  private GameObject _shieldVisualizer;
  //For Animation
  private bool _shieldActive = false;

  [SerializeField]
  private GameObject _speedVisualizer;
  [SerializeField]
  private float _speedMultiplier = 2;
  //For Animation
  private bool _speedActive = false;
  

  void Start()
  { // Starting Position
    transform.position = new Vector3(0, -3, 0);
    _modifiedSpeed = _speed;
    _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

    if (_spawnManager == null)
    {
      Debug.LogError("Player::SPAWN MANAGER IS NULL");
		}

    _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
    if (_uiManager == null)
    {
      Debug.LogError("Player::UI MANAGER IS NULL");
    }

    _anim = gameObject.GetComponent<Animator>();
    if (_anim == null)
    {
      Debug.LogError("Player::THE ANIMATOR IS NULL");
    }
  }


  void Update()
  {
    if (_death == false)
    {
      CalculatePlayerMovement();
      if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
      {
        _canFire = Time.time + _fireRate;
        PlayerShooting();
        AudioSource.PlayClipAtPoint(_laserClip, transform.position);
      }
      if (Input.GetKey(KeyCode.LeftShift))
      {
        _modifiedSpeed =  _speed * _speedMultiplier;
      }
      else
      {
        _modifiedSpeed = _speed;
			}

    }
    if (_lives <= 0)
    {
      _lives = 0;
      Damage();
    }
  }


  public void AddScore(int points)
  {
    _score += points;
    _uiManager.UpdateScore(_score);
	}


  public void ShieldActive ()
  {
    AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
    _shieldActive = true;
    _shieldVisualizer.SetActive(true);
  }

  public void SpeedActive()
  {
    AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
    _speedActive = true;
    _speedVisualizer.SetActive(true);
    _modifiedSpeed = _speed * _speedMultiplier;
    StartCoroutine(SpeedPowerdownRoutine());
  }
  IEnumerator SpeedPowerdownRoutine()
  {
    yield return new WaitForSeconds(_powerTimer);
    _speedVisualizer.SetActive(false);
    _modifiedSpeed = _speed;
    _speedActive = false;
  }


  public void TripleShotActive ()
  {
    AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
    _tripleShotActive = true;
    StartCoroutine(TripleShotPowerdownRoutine());
	}
  IEnumerator TripleShotPowerdownRoutine()
  {
    yield return new WaitForSeconds(_powerTimer);
    _tripleShotActive = false;
	}


  public void DamageVisualiser()
  {
    switch (_lives)
    {
      case 3:
        break;

      case 2:
        _rightEngineVisualizer.SetActive(true);
        break;

      case 1:
        _rightEngineVisualizer.SetActive(true);
        _leftEngineVisualizer.SetActive(true);
        break;

      case 0:
        _leftEngineVisualizer.SetActive(false);
        _rightEngineVisualizer.SetActive(false);
        _shieldVisualizer.SetActive(false);
        _speedVisualizer.SetActive(false);
        break;

      default:
        break;
    }
  }

  public void DamageCollision()
  {
    if (_shieldActive == false)
    {
      _lives -= 2;
      _uiManager.UpdateLives(_lives);
      Damage();
    }
    else
    {
      _shieldActive = false;
      _shieldVisualizer.SetActive(false);
      return;
    }
  }

  public void Damage()
  {
    if (_shieldActive == false)
    {
      _lives--;
      _uiManager.UpdateLives(_lives);
    }
    else
    {
      _shieldActive = false;
      _shieldVisualizer.SetActive(false);
      return;
		}
    switch (_lives)
    {
      case 3:
        break;
      case 2:
        DamageVisualiser();
        break;
      case 1:
        DamageVisualiser();
        break;
      case 0:
        DamageVisualiser();
        _death = true;
        Destroy(GetComponent<Collider2D>());
        _anim.SetTrigger("OnPlayerDeath");
        AudioSource.PlayClipAtPoint(_deathClip, transform.position);
        _spawnManager.OnPlayerDeath();
        Destroy(this.gameObject, 2.6f);

        break;
      default:
        _anim.SetTrigger("OnPlayerDeath");
        //Debug.Log("Player::Damage Error");
        break;
		}

	}


  void PlayerShooting ()
  {
    if (_tripleShotActive == true) {
      Instantiate( _tripleShotPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
    }
    else
    {
      Instantiate( _laserPrefab, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
    }
  }


  void CalculatePlayerMovement ()
  {
   // Basic Movement
    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");
    Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
    transform.Translate(direction * _modifiedSpeed * Time.deltaTime);
    //Debug.Log("Horizontal Input" + horizontalInput);
    if (horizontalInput <= -0.2)
    {
      _anim.SetBool("OnPlayerStill", false);
      _anim.SetFloat("OnPlayerRight", 0);
      _anim.SetFloat("OnPlayerLeft", horizontalInput);
      //_anim.SetTrigger("OnPlayerLeftMax");
    }
    if (horizontalInput >= 0.2)
    {
      _anim.SetBool("OnPlayerStill", false);
      _anim.SetFloat("OnPlayerLeft", 0);
      _anim.SetFloat("OnPlayerRight", horizontalInput);
      //_anim.SetTrigger("OnPlayerRightMax");

    }
    if (horizontalInput == Mathf.Clamp(horizontalInput, -0.19f, 0.19f))
    {
      _anim.SetBool("OnPlayerStill", true);
      horizontalInput = 0;
      _anim.SetFloat("OnPlayerLeft", 0);
      _anim.SetFloat("OnPlayerRight", 0);
    }

    // Boundries X
    if (transform.position.x >= 11f)
    {
      transform.position = new Vector3(-11.3f, transform.position.y, 0);
    }
    else if (transform.position.x <= -11.3f)
    {
      transform.position = new Vector3(11f, transform.position.y, 0);
    }

    // Boundries Y
    transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 5.8f), 0);
  }


  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "EnemyLaser")
    {
      Destroy(other.gameObject);
      Damage();
    }
  }


}