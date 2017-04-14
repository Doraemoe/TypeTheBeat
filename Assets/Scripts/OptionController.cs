using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionController : MonoBehaviour {

	public Toggle showFPS;

	public Text fps;

	// Use this for initialization
	void Start () {
		InitFPSDisplay ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Display FPS in the corner
	/// </summary>
	void InitFPSDisplay()
	{
		if (PlayerPrefs.GetInt ("Show FPS", 0) == 1) {
			fps.gameObject.SetActive (true);
			showFPS.isOn = true;
		} else {
			fps.gameObject.SetActive (false);
			showFPS.isOn = false;
		}
	}

	/// <summary>
	/// Toggle FPS display in the corner
	/// </summary>
	public void ToggleFPS() {
		CDebug.Log (showFPS.isOn);
		if (showFPS.isOn) 
		{
			fps.gameObject.SetActive (true);
			PlayerPrefs.SetInt("Show FPS", 1);
		} 
		else 
		{
			fps.gameObject.SetActive (false);
			PlayerPrefs.SetInt("Show FPS", 0);
		}
	}

	public void LoadScene(string sceneName) {
		SceneManager.LoadScene (sceneName);
	}
}
