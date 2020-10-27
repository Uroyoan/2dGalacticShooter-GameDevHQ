using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	private Vector3 _startPos;
	private float _timePassed = 0f;
	private float _xOffSet;
	private float _yOffSet;
	private float _duration = .5f;
	private float _magnitude = .1f;

	public IEnumerator Shake()
	{
		_startPos = transform.localPosition;
		while (_timePassed < _duration)
		{
			_xOffSet = Random.Range(-1f, 1f) * _magnitude;
			_yOffSet = Random.Range(-1f, 1f) * _magnitude;

			transform.localPosition = new Vector3(_xOffSet, _yOffSet, _startPos.z);
			_timePassed += Time.deltaTime;
			//yield return null;
			yield return new WaitForEndOfFrame();
		}
		transform.localPosition = _startPos;
		_timePassed = 0;
	}
}
