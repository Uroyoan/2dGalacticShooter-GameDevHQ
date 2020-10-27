using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private bool _gameover;

	private void Update()
	{
		CheckKeycodes();
	}
	public void Gameover()
	{
		_gameover = true;
	}

	public void CheckKeycodes()
	{
		if (Input.GetKeyDown(KeyCode.F) && _gameover == true)
		{
			SceneManager.LoadScene(1); // Current Game Scene
		}
		if (Input.GetKeyDown(KeyCode.F) && gameObject.scene.buildIndex == 0)
		{
			SceneManager.LoadScene(1); // Current Game Scene
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneManager.LoadScene(0);
		}
	}
}
