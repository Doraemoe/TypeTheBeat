using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[RequireComponent(typeof(BoxCollider2D))]

public class EditorController : MonoBehaviour {

	public GameObject instantiateWaveController;
	public float[] waveForm;

	Vector3 startPosition;
	float mouseX;
	AudioSource audioSource;
	int resolution = 10;
	float[] samples;
	float waveControllerX;
	float max = 0;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (audioSource.isPlaying == true) {
			/*
			for (int i = 0; i < waveForm.Length; i++) {
				Vector3 sv = new Vector3 (i * 0.04f, waveForm [i] * 50, 0);
				Vector3 ev = new Vector3 (i * 0.04f, - waveForm [i] * 50, 0);

				Debug.DrawLine (sv, ev, Color.yellow);
			}

			int current = audioSource.timeSamples / resolution;
			current *= audioSource.clip.channels;

			Vector3 c = new Vector3 (current * 0.04f, 0, 0);

			Debug.DrawLine (c, c + Vector3.up * 50, Color.white);
*/
			this.PlayMusic ();
		}
	}

	public void OpenFile () {

		var path = EditorUtility.OpenFilePanel(
			"Open Music",
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
			"ogg");
		
		if (path.Length != 0) {
			StartCoroutine (LoadSongCoroutine (path));
		}
	}


	IEnumerator LoadSongCoroutine(string path)
	{
		var audioLocation = new WWW ("file://" + path);

		yield return audioLocation;

		audioSource.clip = audioLocation.GetAudioClip (false, false);
		generateSoundWave ();

	}

	void generateSoundWave() {
		resolution = audioSource.clip.frequency / resolution;
		samples = new float[audioSource.clip.samples * audioSource.clip.channels];
		audioSource.clip.GetData (samples, 0);

		waveForm = new float[(samples.Length/resolution)];

		for (int i = 0; i < waveForm.Length; i++) {
			waveForm[i] = 0;
			for (int ii = 0; ii < resolution; ii++) {
				//waveForm[i] += samples[(i * resolution) + ii];
				waveForm [i] += Mathf.Abs (samples [(i * resolution) + ii]); //<-- another option
			}
			waveForm[i] /= resolution;
			if (waveForm [i] > max) {
				max = waveForm [i];
			}
		}

		this.normalizeWaveForm ();
			
		instantiateWaveController.SetActive (true);
		waveControllerX = Camera.main.WorldToScreenPoint(instantiateWaveController.transform.position).x;
	}

	void normalizeWaveForm() {
		for (int i = 0; i < waveForm.Length; i++) {
			waveForm [i] /= max;
		}
	}

	void OnMouseDown() {
		mouseX = Input.mousePosition.x;
		startPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
	}

	void OnMouseDrag() {
		if (instantiateWaveController.activeSelf) {
			var currentPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			var offset = startPosition.x - currentPosition.x;
			var pixOffset = mouseX - Input.mousePosition.x;
			//Debug.Log (offset);
			instantiateWaveController.transform.position = new Vector3 (instantiateWaveController.transform.position.x - offset, this.transform.position.y, 0f);
			startPosition = currentPosition;

			if (pixOffset > 10) {
				//how many?
				int number = (int)pixOffset / 10 + 1;
				mouseX = Input.mousePosition.x;

				instantiateWaveController.GetComponent<InstantiateWaveform> ().redraw (number);

			} else if (pixOffset < -10) {
				int number = (int)pixOffset / 10;
				mouseX = Input.mousePosition.x;
				instantiateWaveController.GetComponent<InstantiateWaveform> ().redraw (number);
			} else {
				//Debug.Log ("no");
			}
		}
	}

	void PlayMusic() {
		int current = audioSource.timeSamples / resolution;
		current *= audioSource.clip.channels;
		instantiateWaveController.transform.position = Vector3.left * current * 0.1f + Vector3.up * 1.2f;
		var currentPositionX = Camera.main.WorldToScreenPoint(instantiateWaveController.transform.position).x;
		var pixOffset =  waveControllerX - currentPositionX;
		if (pixOffset > 10) {
			int number = (int)pixOffset / 10;
			waveControllerX = currentPositionX;
			instantiateWaveController.GetComponent<InstantiateWaveform> ().redraw (number);
		}
	}

	public void PlayPause() {
		if (audioSource.isPlaying == true) {
			audioSource.Pause ();
		} else {
			audioSource.Play ();
		}
	}
}
