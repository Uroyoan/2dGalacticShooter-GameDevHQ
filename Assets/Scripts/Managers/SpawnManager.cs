﻿using System.Collections;
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
	private bool _stopSpawning = false;

	public void startSpawn()
	{
		Debug.Log("Starting Spawn Routine...");
		StartCoroutine(SpawnEnemyRoutine());
		StartCoroutine(SpawnPowerupRoutine());
	}
	public void OnPlayerDeath()
	{
		_stopSpawning = true;
	}


	IEnumerator SpawnEnemyRoutine ()
	{
		yield return new WaitForSeconds(2f);

		while (_stopSpawning == false)
		{
			Vector3 spawnPos = new Vector3(Random.Range(-9f, 9f), 7f, 0);
			GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);

			newEnemy.transform.parent = _enemyContainer.transform;
			yield return new WaitForSeconds(_enemySpawnRate);
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