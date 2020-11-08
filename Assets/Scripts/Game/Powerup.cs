using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
	private float _speed = 3f;
	[SerializeField] // shield = 0, speed = 1, tripleshot = 2, ammo = 3, Life = 4, Missile = 5, Slow = 6
	private int _powerUpID;

	private Transform _playerLocation;
	Vector3 rightTriangle;
	private Vector3 _direction = new Vector3(0, -1, 0);
	private Player _player;


	private void Start()
	{
		_playerLocation = GameObject.Find("Player").transform;
		if (_playerLocation == null)
		{
			Debug.LogError("Powerup::The PlayerLocation is NULL");
		}
	}
	private void Update()
	{
		CalculateMovement();

		if (Input.GetKeyDown(KeyCode.C))
		{
			GoToPlayer();
		}

	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			Player player = other.GetComponent<Player>();
			switch (_powerUpID)
			{
				case 0:
					player.ShieldActive();
					break;

				case 1:
					player.AddFuel();
					break;

				case 2:
					player.TripleShotActive();
					break;

				case 3:
					player.AddAmmo();
					break;

				case 4:
					player.AddHealth();
					break;

				case 5:
					player.MissileActive();
					break;

				case 6:
					player.SlowDownActive();
					break;

				default:
					Debug.Log("Powerup::OnTrigger Unidentified SwitchCase");
					break;

			}
			Destroy(this.gameObject);
		}
		if (other.tag == "LaserOfEnemy")
		{
			Destroy(other.gameObject);
			Destroy(this.gameObject);
		}
	}

	private void GoToPlayer()
	{
		rightTriangle = _playerLocation.position - transform.position;
		if (rightTriangle.x > 1)
		{
			_direction.x = 2;
		}
		else if (rightTriangle.x >= 0f && rightTriangle.x <= 1f)
		{
			_direction.x = 1;
		}
		else if (rightTriangle.x < -1)
		{
			_direction.x = -2;
		}
		else if (rightTriangle.x <= 0f && rightTriangle.x >= -1f)
		{
			_direction.x = -1f;
		}
		if (rightTriangle.y > 1)
		{
			_direction.y = 2;
		}
		else if (rightTriangle.y >= 0f && rightTriangle.y <= 1f)
		{
			_direction.y = 1f;
		}
		else if (rightTriangle.y < -1)
		{
			_direction.y = -2;
		}
		else if (rightTriangle.y <= 0f && rightTriangle.y >= -1f)
		{
			_direction.y = -1f;
		}
	}
	private void CalculateMovement()
	{
		transform.Translate(_direction * _speed * Time.deltaTime);

		// Boundries X
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -11.1f, 10.3f), transform.position.y, 0);
		// Boundries Y
		if (transform.position.y <= -5.5f)
		{
			Destroy(this.gameObject);
		}
	}
}
