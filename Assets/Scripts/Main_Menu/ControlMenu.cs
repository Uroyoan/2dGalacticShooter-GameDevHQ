using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMenu : MonoBehaviour
{
  [SerializeField]
  private GameObject _laser;
  private RectTransform _laserVisual;
  private float _laserBoundry;
  
  private Vector3 LaserMovement;

	private void Start()
	{
    _laserVisual = _laser.GetComponent<RectTransform>();

  }


	void Update()
  {
    LaserShotMenu();
  }


  void LaserShotMenu()
  {
    Vector3 direction = new Vector3(0, 50, 0);
    _laser.transform.Translate( direction * Time.deltaTime);
    _laserBoundry = _laserVisual.localPosition.y;

    if (_laserBoundry >= 90)
    {
      _laserVisual.transform.localPosition = new Vector3 (0, 45, 0);
		}
  }
}
