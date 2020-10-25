using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
	private float _speed = 3f;
	[SerializeField] // shield = 1, speed = 2, tripleshot = 3, ammo = 4,
	private int _powerUpID;

	private void Update()
	{
		CalculateMovement();
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			Player player = other.GetComponent<Player>();
			switch (_powerUpID + 1)
			{
				case 1:
					player.ShieldActive();
					//Debug.Log("Shield collected by:" + other.tag);
					break;

				case 2:
					player.SpeedActive();
					//Debug.Log("Speed collected by:" + other.tag);
					break;

				case 3:
					player.TripleShotActive();
					//Debug.Log("TripleShot collected by:" + other.tag);
					break;

				case 4:
					player.AddAmmo();
					//Debug.Log("Ammo collected by:" + other.tag);
					break;

				default:
					Debug.Log("Powerup::OnTrigger Unidentified SwitchCase");
					break;

			}
			Destroy(this.gameObject);
		}
	}
	private void CalculateMovement()
	{
		Vector3 direction = new Vector3 (0,-1,0);
		transform.Translate(direction * _speed * Time.deltaTime);
		// Boundries X
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, -11.1f, 10.3f), transform.position.y, 0);
		// Boundries Y
		if (transform.position.y <= -5.5f)
		{
			Destroy(this.gameObject);
		}
	}
}
