using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDetection : MonoBehaviour
{
  private Transform _parentLocation;
  private float _detection;
  public float _directiontoMove;


  public void Start()
	{
    _parentLocation = gameObject.GetComponentInParent<Transform>();
    if (_parentLocation == null)
    {
      Debug.LogError("SideDetection::The ParentLocation is NULL");
    }
  }


	public void OnTriggerStay2D (Collider2D other)
	{
    if (other.tag == "Player")
    {
      _detection = other.transform.position.x - _parentLocation.position.x;
      if (_detection > 0)
      {
        _directiontoMove = 1;
      }
      else if (_detection < 0)
      {
        _directiontoMove = -1;
      }
    }
    Debug.Log(other.tag);
  }
}
