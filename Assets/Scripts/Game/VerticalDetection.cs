using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalDetection : MonoBehaviour
{

  private Transform _parentLocation;
  public float _directiontoShoot = 0;
  public float _directiontoDestroy = 0;
  private float _resetShoot = -1;

  public void Start()
  {
    _parentLocation = gameObject.GetComponentInParent<Transform>();
    if (_parentLocation == null)
    {
      Debug.LogError("SideDetection::The ParentLocation is NULL");
    }
  }
	public void Update()
	{
    if (Time.time > _resetShoot)
    {
      _directiontoShoot = 0;
      _directiontoDestroy = 0;
      _resetShoot = Time.time + 2;
    }
	}

	public void OnTriggerStay2D(Collider2D other)
  {
    if (other.tag == "Player")
    {
      if (other.transform.position.y > _parentLocation.position.y)
      {
        _directiontoShoot = -1;
      }
      else if (other.transform.position.y < _parentLocation.position.y)
      {
        _directiontoShoot = 1;
      }
    }
    if (other.tag == "Powerup")
    {
      if (other.transform.position.y < _parentLocation.position.y)
      {
        _directiontoShoot = 1;
      }
    }
  }
}
