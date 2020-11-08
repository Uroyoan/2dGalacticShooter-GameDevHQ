using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField]
	private float _speed = 12f;


  private void Update()
	{
    CalculateLaserMovement();
	}


	void CalculateLaserMovement()
  {
    if (gameObject.tag == "Laser")
    {
      transform.Translate(new Vector3( 0, 1f, 0) * _speed * Time.deltaTime);
    }
    else if (gameObject.tag == "LaserOfEnemy")
    {
      transform.Translate(new Vector3( 0, -1f, 0) *_speed * Time.deltaTime);
    }
    else if (gameObject.tag == "LaserOfEnemyBack")
    {
      transform.Translate(new Vector3(0, 1f, 0) * _speed * Time.deltaTime);
    }

    // Boundries y
    if (transform.position.y >= 8f)
    {
      Destroy(this.gameObject);
      if (transform.parent != null)
      {
        Destroy(transform.parent.gameObject);
			}
    }
    else if (transform.position.y <= -6f)
    {
      Destroy(this.gameObject);
      if (transform.parent != null)
      {
        Destroy(transform.parent.gameObject);
      }
    }

    // Boundries X
    if (transform.position.x >= 11f)
    {
      Destroy(this.gameObject);
      if (transform.parent != null)
      {
        Destroy(transform.parent.gameObject);
      }
    }
    else if (transform.position.x <= -11f)
    {
      Destroy(this.gameObject);
      if (transform.parent != null)
      {
        Destroy(transform.parent.gameObject);
      }
    }

  }



}
