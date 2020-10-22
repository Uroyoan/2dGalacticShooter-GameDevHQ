using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject _controlsMenu;
	private bool _controlMenuBool;
	[SerializeField]
	private GameObject _creditsMenu;
	private bool _creditsMenuBool;
	[SerializeField]
	private GameObject _mainMenu;
	private bool _mainMenuBool;
	private bool _turnedOff = false;
	private bool _turnedON = true;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
	public void LoadGame()
	{
		SceneManager.LoadScene(1);
	}

	public void GotoMainMenu()
	{
		_controlsMenu.SetActive(false);
		_creditsMenu.SetActive(false);
		_mainMenu.SetActive(true);
	}

	public void GotoControlMenu()
	{
		_mainMenu.SetActive(false);
		_controlsMenu.SetActive(true);
	}
	public void GotoCreditMenu()
	{
		_mainMenu.SetActive(false);
		_creditsMenu.SetActive(true);
	}




}
