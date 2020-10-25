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
	private AudioClip _noAmmoClip;

	[SerializeField]
	private GameObject _laserPrefab;
	[SerializeField]
	private GameObject _tripleShotPrefab;
	[SerializeField]
	private float _fireRate = 0.2f;
	private float _canFire = 0f;
	[SerializeField]
	private int _ammoCount = 15;

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
	private int _shieldStrength = 3;
	[SerializeField]
	private GameObject _shieldVisualizer;
	//For Animation
	[SerializeField]
	private bool _shieldActive = false;
	private SpriteRenderer _shieldColor;

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

		_shieldColor = _shieldVisualizer.GetComponent<SpriteRenderer>();
		if (_shieldColor == null)
		{
			Debug.LogError("Player::THE SPRITE RENDERER IS NULL");
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
			}

			if (Input.GetKey(KeyCode.LeftShift))
			{
				_modifiedSpeed = _speed * _speedMultiplier;
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


	public void ShieldActive()
	{
		AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
		_shieldActive = true;
		_shieldStrength = 3;
		_shieldColor.material.color = new Color(1f, 1f, 1f, 1f);
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


	public void TripleShotActive()
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

	public void AddAmmo()
	{
		AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
		_ammoCount += 10;
		_uiManager.UpdateAmmo(_ammoCount);
	}


	public void AddHealth()
	{
		AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
		_lives++;
		if (_lives > 3)
		{
			_lives = 3;
		}
		DamageVisualiser();
		_uiManager.UpdateLives(_lives);
	}


	public void DamageVisualiser()
	{
		switch (_lives)
		{
			case 3:
				_leftEngineVisualizer.SetActive(false);
				_rightEngineVisualizer.SetActive(false);
				break;

			case 2:
				_rightEngineVisualizer.SetActive(true);
				_leftEngineVisualizer.SetActive(false);
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

			case -1:
				_leftEngineVisualizer.SetActive(false);
				_rightEngineVisualizer.SetActive(false);
				_shieldVisualizer.SetActive(false);
				_speedVisualizer.SetActive(false);
				break;

			default:
				_anim.SetTrigger("OnPlayerDeath");
				Debug.Log("Player::DamageVisualizer SwitchCase _lives ERROR");
				break;
		}
		switch (_shieldStrength)
		{
			case 2:
				_shieldColor.material.color = new Color(1f, 1f, 0f, 1f);
				break;
			case 1:
				_shieldColor.material.color = new Color(1f, 0f, 0f, 1f);
				break;
			case 0:
				_shieldVisualizer.SetActive(false);
				_shieldActive = false;
				break;
			case -1:
				_shieldVisualizer.SetActive(false);
				_shieldActive = false;
				break;
			default:
				Debug.Log("Player::DamageVisualiser SwitchCase _shieldStrength ERROR");
				_shieldActive = false;
				_shieldVisualizer.SetActive(false);
				break;
		}
	}

	public void DamageCollision()
	{
		if (_shieldActive == true)
		{
			Damage();
		}
		else
		{
			_lives--;
			Damage();
		}
	}

	public void Damage()
	{
		if (_shieldActive == true)
		{
			_shieldStrength--;
			DamageVisualiser();
		}
		else
		{
			_lives--;
			_uiManager.UpdateLives(_lives);
			DamageVisualiser();
		}
		if (_lives <= 0)
		{
			_death = true;
			Destroy(GetComponent<Collider2D>());
			_anim.SetTrigger("OnPlayerDeath");
			AudioSource.PlayClipAtPoint(_deathClip, transform.position);
			_spawnManager.OnPlayerDeath();
			Destroy(this.gameObject, 2.6f);
		}
	}


	void PlayerShooting()
	{
		if (_ammoCount > 0)
		{
			if (_tripleShotActive == true)
			{
				Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
			}
			else
			{
				Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
			}
			AudioSource.PlayClipAtPoint(_laserClip, transform.position);
			_ammoCount--;
			_uiManager.UpdateAmmo(_ammoCount);

		}
		else
		{
			AudioSource.PlayClipAtPoint(_noAmmoClip, transform.position);
		}
	}


	void CalculatePlayerMovement()
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

		if (other.tag == "Enemy")
		{
			other.gameObject.GetComponent<Enemy>().DeathSequence();
			DamageCollision();
		}

	}


}