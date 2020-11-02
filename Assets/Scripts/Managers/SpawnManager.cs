using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _enemyPrefabs;
	[SerializeField]
	private GameObject _enemyContainer;
	private int _enemySelected;
	private int _enemySide;

	[SerializeField]
	private GameObject[] _powerupPrefabs;
	[SerializeField]
	private GameObject _powerupContainer;
	private int _powerSelected;

	[SerializeField]
	private GameObject _asteroidPrefab;
	[SerializeField]
	private bool _stopSpawning = false;
	[SerializeField]
	private int _enemySpawn = 5;
	[SerializeField]
	private int _enemiesToSpawn = 0;
	[SerializeField]
	private float _enemySpawnRate = 2f;
	[SerializeField]
	private float _powerupSpawnRate = 5f;
	[SerializeField]
	private int _currentWave = 0;
	[SerializeField]
	private int _enemiesInContainer;
	private UiManager _uiManager;

	private void Start()
	{
		_uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
		if (_uiManager == null)
		{
			Debug.LogError("Player::UI MANAGER IS NULL");
		}
	}
	public void startSpawn()
	{
		_currentWave++;
		
		_stopSpawning = false;
		AddWaves(_currentWave);

		_enemiesToSpawn += _currentWave * _enemySpawn;
		//Debug.Log("Wave: " + _currentWave);
		//Debug.Log("Enemy to Spawn: " + _currentWave);
		StartCoroutine(SpawnEnemyRoutine());
		StartCoroutine(SpawnPowerupRoutine());
	}

	public void OnPlayerDeath()
	{
		_stopSpawning = true;
	}
	private void AddWaves(int currentWaves)
	{
		_currentWave = currentWaves;
	}

	IEnumerator SpawnEnemyRoutine ()
	{
		yield return new WaitForSeconds(2f);
		while (_stopSpawning == false && _enemiesToSpawn > 0)
		{
			SelectEnemy(Random.Range(1, 101));
			_enemiesToSpawn--;
			yield return new WaitForSeconds(_enemySpawnRate);
		}
		while (_stopSpawning == false && _enemiesInContainer >= 1)
		{
			_enemiesInContainer = _enemyContainer.transform.childCount;
			yield return new WaitForSeconds(1);
		}
		if (_stopSpawning == false && _enemiesToSpawn <= 0 && _enemiesInContainer == 0)
		{
			_uiManager.UpdateWaves(_currentWave + 1);
			_stopSpawning = true;
			Vector3 asteroidSpawnPos = new Vector3(0, 8, 0);
			GameObject newWave = Instantiate(_asteroidPrefab, asteroidSpawnPos, Quaternion.identity);
			newWave.transform.parent = _enemyContainer.transform;
		}
	}
	private void SelectEnemy(int enemyRandomizer)
	{
		GameObject newEnemy;
		Vector3 enemySpawnPos = new Vector3(Random.Range(-9f, 9f), 7f, 0);
		switch (enemyRandomizer)
		{
			case int _EnemyRarity when (_EnemyRarity > 40):
				_enemySelected = 0;
				newEnemy = Instantiate(_enemyPrefabs[_enemySelected], enemySpawnPos, Quaternion.identity);
				newEnemy.transform.parent = _enemyContainer.transform;
				break;

			case int _EnemyRarity when (_EnemyRarity <= 40 && _EnemyRarity > 20):
				_enemySelected = 1;
				if (Random.Range(0, 2) == 0)
				{
					enemySpawnPos.x = -9;
				}
				else
				{
					enemySpawnPos.x = -9;
				}
				newEnemy = Instantiate(_enemyPrefabs[_enemySelected], enemySpawnPos, Quaternion.identity);
				newEnemy.transform.parent = _enemyContainer.transform;
				break;

			case int _EnemyRarity when (_EnemyRarity <= 20):
				_enemySelected = 2;
				enemySpawnPos = new Vector3(-10,Random.Range(-4,6),0);
				newEnemy = Instantiate(_enemyPrefabs[_enemySelected], enemySpawnPos, Quaternion.Euler(0f, 0f, 45f));
				newEnemy.transform.parent = _enemyContainer.transform;
				break;

			default:
				Debug.Log("SpawnManager::SelectEnemy ERROR");
				break;
		}
		_enemiesInContainer = _enemyContainer.transform.childCount;
	}


	IEnumerator SpawnPowerupRoutine ()
	{
		yield return new WaitForSeconds(2f);
		while (_stopSpawning == false)
		{
			SelectPowerup(Random.Range(1, 101));
			Vector3 spawnPos = new Vector3(Random.Range(-9f, 9f), 7f, 0);
			GameObject newPowerup = Instantiate(_powerupPrefabs[_powerSelected], spawnPos, Quaternion.identity);

			newPowerup.transform.parent = _powerupContainer.transform;
			yield return new WaitForSeconds(_powerupSpawnRate);
		}
	}


	private void SelectPowerup(int powerRandomizer)
	{
		switch(powerRandomizer)
		{// shield = 0, speed = 1, tripleshot = 2, ammo = 3, Life = 4, Missile = 5, Slow = 6
			case int _powerRarity when (_powerRarity <= 5):
				_powerSelected = 0;
				break;
			case int _powerRarity when (_powerRarity > 5 && _powerRarity <= 15):
				_powerSelected = 4;
				break;

			case int _powerRarity when (_powerRarity > 15 && _powerRarity <= 30):
				_powerSelected = Random.Range(5, 7);
				break;

			case int _powerRarity when (_powerRarity > 30 && _powerRarity <= 50):
				_powerSelected = Random.Range(1, 3);
				break;

			case int _powerRarity when (_powerRarity > 50):
				_powerSelected = 3;
				break;

			default:
				Debug.Log("SpawnManager::SelectPowerup ERROR");
				break;
		}

	}

}