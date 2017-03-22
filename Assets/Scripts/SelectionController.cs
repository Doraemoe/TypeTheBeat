using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionController : MonoBehaviour {

	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadScene(string sceneName) {
		SceneManager.LoadScene (sceneName);
	}


	public void loadAndPlayMusic(string path) {
		StartCoroutine (LoadSongCoroutine (path));
	}

	IEnumerator LoadSongCoroutine(string path)
	{
		var audioLocation = new WWW ("file://" + path);

		yield return audioLocation;

		audioSource.clip = audioLocation.GetAudioClip (false, false);
		audioSource.Play ();

	}
}
