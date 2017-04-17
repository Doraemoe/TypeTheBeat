﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	public Text fps;

	public void LoadScene(string sceneName) {
		SceneManager.LoadScene (sceneName);
	}

	public void quit() {
		Application.Quit();
	}

	void Start () {
		InitFPSDisplay ();

		CDebug.Log (Application.dataPath);
			
	}

	/// <summary>
	/// Display FPS in the corner
	/// </summary>
	void InitFPSDisplay()
	{
		if (PlayerPrefs.GetInt ("Show FPS", 0) == 1) {
			fps.gameObject.SetActive (true);
		} else {
			fps.gameObject.SetActive (false);
		}
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}
}
