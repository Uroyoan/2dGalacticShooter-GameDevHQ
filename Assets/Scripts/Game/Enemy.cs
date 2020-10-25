using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  [SerializeField]
  private float _speed = 4f;
  private AudioSource _enemySounds;
  [SerializeField]
  private GameObject _LaserPrefab;

  private Player _player;
  private Animator _anim;
  private bool _death = false;
  private float _fireRate = 3.0f;
  private float _canFire = -1;


	private void Start()
	{
    _player = GameObject.Find("Player").GetComponent<Player>();
    if (_player == null)
    {
      Debug.LogError("Enemy::The Player is NULL");
		}

    _anim = gameObject.GetComponent<Animator>();
    if (_anim == null)
    {
      Debug.LogError("Enemy::The Animator is NULL");
    }

    _enemySounds = gameObject.GetComponent<AudioSource>();
    if (_enemySounds == null)
    {
      Debug.LogError("Enemy::THE ENEMY'S SOUND IS NULL");
    }
  }


	private void Update()
    {
      CalculateEnemyMovement();

      if (Time.time > _canFire)
      {
      _fireRate = Random.Range(3f, 9f);
      _canFire = Time.time + _fireRate;
      EnemyShooting();
      }

    }

  
  public void EnemyShooting()
  {
    GameObject enemyLaser = Instantiate(_LaserPrefab, transform.position, Quaternion.identity);
    foreach (Transform _laserShot in enemyLaser.transform)
    {
      _laserShot.gameObject.tag = "EnemyLaser";
		}

    //Debug.Break();
	}


  void CalculateEnemyMovement ()
  { //Movement
    Vector3 direction = new Vector3(0,-1,0);
    transform.Translate(direction * _speed * Time.deltaTime);

    // Boundries X
    transform.position = new Vector3(Mathf.Clamp(transform.position.x, -11.1f, 10.3f), transform.position.y, 0);

    // Boundries Y
    if (transform.position.y >= 7f && _death == false)
    {
      transform.position = new Vector3(Random.Range(-9f, 9f), -5.5f, 0);
    }
    else if (transform.position.y <= -5.5f && _death == false)
    {
      transform.position = new Vector3(Random.Range(-9f, 9f), 7f, 0);
    }
    else
    {
      
      return;
		}
  }


  public void DeathSequence()
  {
    _death = true;
    _anim.SetTrigger("OnEnemyDeath");
    _enemySounds.Play();
    Destroy(GetComponent<Collider2D>());

    Destroy(this.gameObject, 2.6f);
	}


	private void OnTriggerEnter2D(Collider2D other)
	{

    if (other.tag == "Laser")
    {
      Destroy(other.gameObject);
      if (_player != null)
      {
        _player.AddScore(10);
        DeathSequence();
      }
    }

    if (other.tag == "EnemyLaser")
    {
      Destroy(other.gameObject);
      if (_player != null)
      {
        _player.AddScore(5);
        DeathSequence();
      }
    }
  }
}
