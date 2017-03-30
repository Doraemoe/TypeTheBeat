﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
	public void LoadScene(string sceneName) {
		SceneManager.LoadScene (sceneName);
	}

	public void quit() {
		Application.Quit();
	}
}
