using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  private Vector3 _direction = new Vector3(0, -1, 0);
  [SerializeField]
  private float _speed = 4f;
  [SerializeField]
  private GameObject _laserPrefab;
  [SerializeField]
  private GameObject _missilePrefab;
  [SerializeField]
  private GameObject _laserBackPrefab;

  private AudioSource _enemySounds;
  [SerializeField]
  private AudioClip _enemyExplosion;
  [SerializeField]
  private AudioClip _enemyLockingOn;
  [SerializeField]
  private AudioClip _enemyLaser;
  [SerializeField]
  private AudioClip _enemyMissile;

  private Player _player;
  private Animator _anim;
  private bool _death = false;
  private float _fireRate = 7f;
  private float _canFireLaser = -1;
  private float _canFireMissile = -1;
  private float _sporaticMovement = -1;
  private bool _shieldActive = false;
  [SerializeField]
  private GameObject _shieldVisualizer;

  private Transform _playerLocation;
  Vector3 rightTriangle;
  private float _xValueofShip;
  private float _yValueofShip;
  private float _tangente;
  private float _radianes;
  private float _angulo;
  private float _horizontalMove;

  private SideDetection _horizontalDetect;
  [SerializeField]
  private GameObject _detectHorizontal;
  private VerticalDetection _verticalDetect;
  [SerializeField]
  private GameObject _detectVertical;

  private void Start()
  {
    _horizontalDetect = _detectHorizontal.GetComponent<SideDetection>();
    _verticalDetect = _detectVertical.GetComponent<VerticalDetection>();
    _player = GameObject.Find("Player").GetComponent<Player>();
    if (_player == null)
    {
      Debug.LogError("Enemy::The Player is NULL");
    }
    _playerLocation = GameObject.Find("Player").transform;
    if (_playerLocation == null)
    {
      Debug.LogError("Enemy::The PlayerLocation is NULL");
    }

    _anim = gameObject.GetComponent<Animator>();
    if (_anim == null)
    {
      Debug.LogError("Enemy::The Animator is NULL");
    }

    _enemySounds = gameObject.GetComponent<AudioSource>();
    if (_enemySounds == null)
    {
      Debug.LogError("Enemy::AUDIO IS NULL");
    }

    _canFireMissile = Time.time + _fireRate;
    _canFireLaser = Time.time + 0.5f;

    if (Random.Range(0, 4) == 3)
    {
      GiveShield();
    }
  }
  private void Update()
  {
    CalculateEnemyMovement();

    if (Time.time > _canFireLaser && (gameObject.tag == "BasicEnemy" || gameObject.tag == "WaveEnemy") && _death == false)
    {
      _fireRate = Random.Range(3f, 9f);
      _canFireLaser = Time.time + _fireRate;
      EnemyShootingLaser();
    }
    else if (Time.time > _canFireMissile && gameObject.tag == "MissileEnemy" && _death == false)
    {
      _canFireMissile = Time.time + _fireRate;
      EnemyShootingMissile();
    }
    else if (Time.time > _canFireLaser && gameObject.tag == "SmartEnemy" && _verticalDetect._directiontoShoot == 1)
    {
      _fireRate = Random.Range(3f, 9f);
      _canFireLaser = Time.time + _fireRate;
      EnemyShootingLaser();
      Debug.Log("Front");
    }
    else if (Time.time > _canFireLaser && gameObject.tag == "SmartEnemy" && _verticalDetect._directiontoShoot == -1)
    {
      _fireRate = Random.Range(3f, 9f);
      _canFireLaser = Time.time + _fireRate;
      EnemyShootingLaserBack();
      Debug.Log("Back");
    }
  }


  public void EnemyShootingMissile()
  {
    _enemySounds.PlayOneShot(_enemyMissile);
    GameObject missileOfEnemy = Instantiate(_missilePrefab, transform.position + new Vector3(0, -1.5f, 0), Quaternion.identity);
  }
  public void EnemyShootingLaser()
  {
    GameObject laserOfEnemy = Instantiate(_laserPrefab, transform.localPosition, transform.rotation);
    _enemySounds.PlayOneShot(_enemyLaser);
  }
  public void EnemyShootingLaserBack()
  {
    GameObject laserOfEnemy = Instantiate(_laserBackPrefab, transform.localPosition, transform.rotation);
    _enemySounds.PlayOneShot(_enemyLaser);
  }


  private void LookAtPlayer()
  {
    rightTriangle = _playerLocation.position - transform.position;
    _xValueofShip = rightTriangle.x;
    _yValueofShip = rightTriangle.y;
    _tangente = (float)System.Math.Atan2(_yValueofShip, _xValueofShip);
    _radianes = _tangente;
    _angulo = _radianes * (180 / 3.1415f);
    gameObject.transform.rotation = Quaternion.Euler(0, 0, (_angulo -= 270));
  }
  void CalculateEnemyMovement()
  {
    switch (gameObject.tag)
    {
      case "BasicEnemy":
        if (Time.time > _sporaticMovement && _death == false)
        {
          _direction.x = Random.Range(-1, 2);
          _sporaticMovement = Time.time + 3f;
        }
        break;
      case "SmartEnemy":
        if (Time.time > _sporaticMovement && _death == false)
        {
          _direction.x = Random.Range(-1, 2);
          _sporaticMovement = Time.time + 3f;
        }
        break;
      case "MissileEnemy":
        if (transform.position.y <= 4.5f)
        {
          _direction.y = 0;
          LookAtPlayer();
          if (Time.time > (_canFireMissile - 6.5f))
          {
            _enemySounds.PlayOneShot(_enemyLockingOn);
          }
        }
        break;

      case "WaveEnemy":
        if (transform.position.y > 5.5f && _death == false)
        {
          gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
        }
        else if (transform.position.y < -4f && _death == false)
        {
          gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 135f);
        }
        break;

      default:
        Debug.Log("Enemy::CalculateEnemyMovement TAG ERROR");
        break;
    }
    if (_shieldActive == true && tag == "BasicEnemy")
    {
      _horizontalMove = _horizontalDetect._directiontoMove;
      if (_horizontalMove != 0)
      {
        _direction.x = _horizontalMove;
      }
		}

    transform.Translate(_direction * _speed * Time.deltaTime);

    // Boundries X
    if (transform.position.x >= 11f && _death == false)
    {
      transform.position = new Vector3(-11f, Random.Range(-5f, 6f), 0);
    }
    else if (transform.position.x <= -11f && _death == false)
    {
      transform.position = new Vector3(11f, Random.Range(-5f, 6f), 0);
    }

    // Boundries Y
    if (transform.position.y >= 7f && _death == false)
    {
      transform.position = new Vector3(Random.Range(-9f, 9f), -5.5f, 0);
    }
    else if (transform.position.y <= -5.5f && _death == false)
    {
      transform.position = new Vector3(Random.Range(-9f, 9f), 7f, 0);
    }

  }


  public void DeathSequence()
  {
    if (_shieldActive == true)
    {
      RemoveShield();
    }
    else
    {
      _enemySounds.PlayOneShot(_enemyExplosion);
      _death = true;
      transform.parent = null;
      _anim.SetTrigger("OnEnemyDeath");
      Destroy(GetComponent<Collider2D>());
      Destroy(this.gameObject, 2.6f);
    }
	}
	private void OnTriggerEnter2D(Collider2D other)
	{

    if (other.tag == "Laser" || other.tag == "Missile")
    {
      Destroy(other.gameObject);
      if (_player != null)
      {
        _player.AddScore(10);
        DeathSequence();
      }
    }
    else if (other.tag == "LaserOfEnemy" || other.tag == "MissileOfEnemy" && other.tag != "MissileEnemy" || other.tag == "LaserOfEnemyBack")
    {
      Destroy(other.gameObject);
      if (other.transform.parent != null)
      {
        Destroy(other.transform.parent.gameObject);
      }
      if (_player != null)
      {
        _player.AddScore(5);
        DeathSequence();
      }
    }
  }

  private void GiveShield()
  {
    if (tag == "WaveEnemy" || tag == "BasicEnemy")
    {
      _shieldActive = true;
      _shieldVisualizer.SetActive(true);
    }
  }
  public void RemoveShield()
  {
      _shieldActive = false;
      _shieldVisualizer.SetActive(false);
  }


}