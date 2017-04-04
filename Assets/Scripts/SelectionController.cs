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

	public void LoadScene(string sceneName) {
		SceneManager.LoadScene (sceneName);
	}


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

	public void Option() {
		optionCanvas.gameObject.SetActive (true);
	}

	public void UpdateValue() {
		speedMulti = slider.value;
		speedMulti = Mathf.Round(speedMulti * 10f) / 10f;

		speedMultiTxt.text = speedMulti.ToString();
	}

	public void CloseOption() {
		optionCanvas.gameObject.SetActive (false);
	}
}
