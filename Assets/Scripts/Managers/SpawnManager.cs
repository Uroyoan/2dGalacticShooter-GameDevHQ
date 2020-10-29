using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	[SerializeField]
	private GameObject _enemyPrefab;
	[SerializeField]
	private float _enemySpawnRate = 5f;
	[SerializeField]
	private GameObject _enemyContainer;
	[SerializeField]
	private GameObject[] _powerupPrefabs;
	[SerializeField]
	private float _powerupSpawnRate = 10f;
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
			Vector3 enemySpawnPos = new Vector3(Random.Range(-9f, 9f), 7f, 0);
			GameObject newEnemy = Instantiate(_enemyPrefab, enemySpawnPos, Quaternion.identity);

			newEnemy.transform.parent = _enemyContainer.transform;
			_enemiesInContainer = _enemyContainer.transform.childCount;
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

	IEnumerator SpawnPowerupRoutine ()
	{
		yield return new WaitForSeconds(4f);

		while (_stopSpawning == false)
		{
			SelectPowerup(Random.Range(1, 101));
			Vector3 spawnPos = new Vector3(Random.Range(-9f, 9f), 7f, 0);
			GameObject newPowerup = Instantiate(_powerupPrefabs[_powerSelected], spawnPos, Quaternion.identity);

			newPowerup.transform.parent = _powerupContainer.transform;
			yield return new WaitForSeconds(Random.Range(_powerupSpawnRate - 3, _powerupSpawnRate));
		}
	}


	private void SelectPowerup(int powerRandomizer)
	{
		switch(powerRandomizer)
		{
			case int _powerRarity when (_powerRarity > 10):
				_powerSelected = Random.Range(0, 5);
				break;
			case int _powerRarity when (_powerRarity <= 10):
				_powerSelected = 5;
				break;
			default:
				Debug.Log("SpawnManager::SelectPowerup ERROR");
				break;
		}

	}

}