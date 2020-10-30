using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
	[SerializeField]
	private float _speed = 8f;
  private Transform _enemyShip;
  private Transform _playerShip;
  Vector3 rightTriangle;
  private float _xValueofShip;
  private float _yValueofShip;
  private float _tangente;
  private float _radianes;
  private float _angulo;

  private void Start()
	{
    _enemyShip = GameObject.Find("EnemyContainer").transform.GetChild(0);
    if (_enemyShip == null)
    {
      Debug.LogError("Missile::_enemyShip IS NULL");
    }
    _playerShip = GameObject.Find("Player").transform;
    if (_playerShip == null)
    {
      Debug.LogError("Missile::_playerShip IS NULL");
    }
  }

	private void Update()
	{
    CalculateMovement();
  }


  private void findAngle(Vector3 ship)
  {
    rightTriangle = ship - transform.position;
    _xValueofShip = rightTriangle.x;
    _yValueofShip = rightTriangle.y;
    _tangente = (float)Math.Atan2(_yValueofShip, _xValueofShip);
    _radianes = _tangente;
    _angulo = _radianes * (180/3.1415f);
  }
  private void LockedOnEnemy()
  {
    findAngle(_enemyShip.position);
    gameObject.transform.rotation = Quaternion.Euler(0, 0, (_angulo -= 90));
  }
  private void LockedOnPlayer()
  {
    findAngle(_playerShip.position);
    gameObject.transform.rotation = Quaternion.Euler(0, 0, (_angulo -= 90));
  }


  private void CalculateMovement ()
	{
    if (gameObject.tag == "Missile")
    {
      transform.Translate(new Vector3(0, 1f, 0) * _speed * Time.deltaTime);
      LockedOnEnemy();
    }
    if (gameObject.tag == "MissileOfEnemy")
    {
      transform.Translate(new Vector3(0, 1f, 0) * _speed * Time.deltaTime);
      LockedOnPlayer();
    }

    // Boundries y
    if (transform.position.y >= 8.5f)
    {
      Destroy(this.gameObject);
    }
    else if (transform.position.y <= -6.5f)
    {
      Destroy(this.gameObject);
    }

    // Boundries X
    if (transform.position.x >= 11.5f)
    {
      Destroy(this.gameObject);
    }
    else if (transform.position.x <= -11.5f)
    {
      Destroy(this.gameObject);
    }
  }
}
