using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField]
	private float _speed = 8f;
	private float _modifiedSpeed;
	private float _slowSpeed = 2;
	private bool _slowMovement = false;
	[SerializeField]
	private int _lives = 3;
	[SerializeField]
	private float _fuel = 100;
	[SerializeField]
	private float _fuelBurnTime = 5;

	private AudioSource _playerSounds;
	[SerializeField]
	private AudioClip _deathClip;
	[SerializeField]
	private AudioClip _laserClip;
	[SerializeField]
	private AudioClip _powerupClip;
	[SerializeField]
	private AudioClip _noAmmoClip;
	[SerializeField]
	private AudioClip _missileClip;

	[SerializeField]
	private float _fireRate = 0.2f;
	private float _canFire = 0f;
	[SerializeField]
	private int _ammoMagazine = 6;
	[SerializeField]
	private float _ammoClip = 1;
	[SerializeField]
	private GameObject _laserPrefab;
	[SerializeField]
	private GameObject _tripleShotPrefab;
	[SerializeField]
	private GameObject _MissilePrefab;


	private UiManager _uiManager;
	[SerializeField]
	private int _score = 0;
	[SerializeField]
	private GameObject _leftEngineVisualizer;
	[SerializeField]
	private GameObject _rightEngineVisualizer;
	private Animator _anim;
	private bool _death = false;
	private bool _diedOnce = false;
	[SerializeField]
	private CameraShake _cameraShake;

	private SpawnManager _spawnManager;
	[SerializeField]
	private float _powerTimer = 5f;

	private bool _tripleShotActive = false;
	private bool _missileActive = false;

	[SerializeField]
	private int _shieldStrength = 3;
	[SerializeField]
	private GameObject _shieldVisualizer;
	[SerializeField]
	private bool _shieldActive = false;
	private SpriteRenderer _shieldColor;

	[SerializeField]
	private GameObject _speedVisualizer;
	[SerializeField]
	private float _speedMultiplier = 2;

	private bool _speedActive = false;


	void Start()
	{ 

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

		_playerSounds = gameObject.GetComponent<AudioSource>();
		if (_playerSounds == null)
		{
			Debug.LogError("Player::AUDIO IS NULL");
		}

		_uiManager.UpdateAmmo(_ammoMagazine);
		_uiManager.UpdateClip(_ammoClip / 4);
		transform.position = new Vector3(0, -3, 0);
		_modifiedSpeed = _speed;

	}


	void Update()
	{
		if (_death == false)
		{
			CalculatePlayerMovement();
			Thrusters();

			if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
			{
				_canFire = Time.time + _fireRate;
				PlayerShooting();
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
		_playerSounds.PlayOneShot(_powerupClip);
		_shieldActive = true;
		_shieldStrength = 3;
		_shieldColor.material.color = new Color(1f, 1f, 1f, 1f);
		_shieldVisualizer.SetActive(true);
	}


	public void AddFuel()
	{
		_playerSounds.PlayOneShot(_powerupClip);
		_fuel = 100;
		_uiManager.UpdateThrusters(_fuel / 100);
	}
	private void Thrusters()
	{
		if (Input.GetKey(KeyCode.LeftShift) && _fuel > 0 && _slowMovement == false)
		{
			_modifiedSpeed = _speed *_speedMultiplier;
			_fuel -= (_fuelBurnTime * 5) * Time.deltaTime;
			_speedVisualizer.SetActive(true);
		}
		else
		{
			if (_slowMovement == false)
			{
				_modifiedSpeed = _speed;
			}
			else 
			{
				_modifiedSpeed = _speed / _slowSpeed;
			}
			_speedVisualizer.SetActive(false);
		}
		_uiManager.UpdateThrusters(_fuel / 100);
	}


	public void SlowDownActive()
	{
		_playerSounds.PlayOneShot(_noAmmoClip);
		_modifiedSpeed += _speed / _slowSpeed;
		_slowMovement = true;
		StartCoroutine(RemoveSlowdownRoutine());
	}
	IEnumerator RemoveSlowdownRoutine()
	{
		yield return new WaitForSeconds(_slowSpeed * 2);
		_slowMovement = false;
	}


	public void TripleShotActive()
	{
		_playerSounds.PlayOneShot(_powerupClip);
		_tripleShotActive = true;
		_missileActive = false;

		StartCoroutine(TripleShotPowerdownRoutine());
	}
	IEnumerator TripleShotPowerdownRoutine()
	{
		yield return new WaitForSeconds(_powerTimer);
		_tripleShotActive = false;
	}


	public void MissileActive()
	{
		_playerSounds.PlayOneShot(_powerupClip);
		_missileActive = true;
		_tripleShotActive = false;

		StartCoroutine(MissilePowerdownRoutine());
	}
	IEnumerator MissilePowerdownRoutine()
	{
		yield return new WaitForSeconds(_powerTimer);
		_missileActive = false;
	}


	public void AddAmmo()
	{
		_playerSounds.PlayOneShot(_powerupClip);
		if (_ammoClip <= 3 && _ammoMagazine != 0)
		{
			_ammoClip++;
			_uiManager.UpdateClip(_ammoClip / 4);
		}
		else if (_ammoClip == 0 && _ammoMagazine == 0)
		{
			_ammoMagazine = 10;
			_uiManager.UpdateAmmo(_ammoMagazine);
		}
		else 
		{
			_ammoMagazine = 10;
			_uiManager.UpdateAmmo(_ammoMagazine);
		}
	}
	public void AddHealth()
	{
		_playerSounds.PlayOneShot(_powerupClip);
		_lives++;
		if (_lives > 3)
		{
			_lives = 3;
		}
		DamageVisualiser();
		_uiManager.UpdateLives(_lives);
	}


	private void CameraShaking()
	{
		StartCoroutine(_cameraShake.Shake());
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
			case 3:
				break;
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
			if (_diedOnce == false)
			{
				_diedOnce = true;
				Destroy(GetComponent<Collider2D>());
				_anim.SetTrigger("OnPlayerDeath");
				_uiManager.UpdateLives(_lives);
				_playerSounds.PlayOneShot(_deathClip);
				_spawnManager.OnPlayerDeath();
				Destroy(this.gameObject, 2.6f);
			}
		}
		CameraShaking();

	}

	void PlayerShooting()
	{
		if (_ammoMagazine > 0)
		{
			if (_tripleShotActive == true && _missileActive == false)
			{
				Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
				_playerSounds.PlayOneShot(_laserClip);
			}
			else if (_tripleShotActive == false && _missileActive == true)
			{
				Instantiate(_MissilePrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
				_playerSounds.PlayOneShot(_missileClip);
			}
			else
			{
				Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
				_playerSounds.PlayOneShot(_laserClip);
			}
			_ammoMagazine--;
			if (_ammoMagazine <= 0 && _ammoClip > 0)
			{
				_ammoMagazine = 10;
				_ammoClip--;
			}
		}
		else
		{
			AudioSource.PlayClipAtPoint(_noAmmoClip, transform.position);
		}
		_uiManager.UpdateAmmo(_ammoMagazine);
		_uiManager.UpdateClip(_ammoClip / 4);
	}


	void CalculatePlayerMovement()
	{

		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
		transform.Translate(direction * _modifiedSpeed * Time.deltaTime);

		if (horizontalInput <= -0.2)
		{
			_anim.SetBool("OnPlayerStill", false);
			_anim.SetFloat("OnPlayerRight", 0);
			_anim.SetFloat("OnPlayerLeft", horizontalInput);

		}
		if (horizontalInput >= 0.2)
		{
			_anim.SetBool("OnPlayerStill", false);
			_anim.SetFloat("OnPlayerLeft", 0);
			_anim.SetFloat("OnPlayerRight", horizontalInput);


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
		if (other.tag == "LaserOfEnemy" || other.tag == "MissileOfEnemy" || other.tag == "LaserOfEnemyBack")
		{
			Destroy(other.gameObject);
			if (other.transform.parent != null)
			{
				Destroy(other.transform.parent.gameObject);
			}
			Damage();
		}

		else if (other.tag == "BasicEnemy" || other.tag == "MissileEnemy" || other.tag == "WaveEnemy" || other.tag == "SmartEnemy")
		{
			other.gameObject.GetComponent<Enemy>().DeathSequence();
			DamageCollision();
		}

	}


}