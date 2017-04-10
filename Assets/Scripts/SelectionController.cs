using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour {

	public GameObject lastSelected;
	public Canvas optionCanvas;
	public Text speedMultiTxt;
	public Slider slider;


	float speedMulti = 3f;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		speedMultiTxt.text = slider.value.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene ("Menu");
		}
	}

	/// <summary>
	/// Load scene
	/// </summary>
	/// <param name="name">Name of the scene been loaded</param>
	public void LoadScene(string sceneName) {
		SceneManager.LoadScene (sceneName);
	}

	/// <summary>
	/// Load background image and play music
	/// </summary>
	/// <param name="name">Path to the file to be loaded</param>
	public void LoadAndPlayMusic(string path) {
		StartCoroutine (LoadSongCoroutine (path));
	}

	IEnumerator LoadSongCoroutine(string path)
	{
		var audioLocation = new WWW ("file://" + path);

		yield return audioLocation;

		audioSource.clip = audioLocation.GetAudioClip (false, true);
		audioSource.Play ();

	}

	/// <summary>
	/// Open option menu
	/// </summary>
	public void Option() {
		optionCanvas.gameObject.SetActive (true);
	}

	/// <summary>
	/// Update value based on value in option menu
	/// </summary>
	public void UpdateValue() {
		speedMulti = slider.value;
		speedMulti = Mathf.Round(speedMulti * 10f) / 10f;

		speedMultiTxt.text = speedMulti.ToString();
	}

	/// <summary>
	/// Close option menu
	/// </summary>
	public void CloseOption() {
		optionCanvas.gameObject.SetActive (false);
	}
}
