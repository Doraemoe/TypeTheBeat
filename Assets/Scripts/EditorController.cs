using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

[RequireComponent(typeof(BoxCollider2D))]

public class EditorController : MonoBehaviour {

	public GameObject instantiateWaveController;
	public float[] waveForm;
	public Canvas confirmMenu;
	public Image blurImg;
	public GameObject selectedObject;


	Vector3 startPosition;
	float originalPositionX;
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

		//select note
		if (Input.GetMouseButtonDown (0)) {
			Vector2 ray = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);

			RaycastHit2D hit = Physics2D.Raycast (ray, Vector2.zero, 0f);
			if (hit) {
				clearColor ();
				if (hit.transform.gameObject.tag == "Note") {
					selectedObject = hit.transform.gameObject;
					var color = selectedObject.GetComponent<Renderer> ().material.color;
					color.a = 0.8f;
					selectedObject.GetComponent<Renderer> ().material.color = color;
				} else {
					clearSelection ();
				}
			}
		}

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

		if (instantiateWaveController.activeSelf) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				clearSelection ();
			}
			if (Input.GetKeyDown (KeyCode.Delete)) {
				deleteSelection ();
			}
			if (Input.GetKeyDown (KeyCode.Backspace)) {
				deleteSelection ();
			}

			if (Input.GetKeyDown (KeyCode.A)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("A");
			} else if (Input.GetKeyDown (KeyCode.S)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("S");
			} else if (Input.GetKeyDown (KeyCode.D)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("D");
			} else if (Input.GetKeyDown (KeyCode.F)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("F");
			} else if (Input.GetKeyDown (KeyCode.J)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("J");
			} else if (Input.GetKeyDown (KeyCode.K)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("K");
			} else if (Input.GetKeyDown (KeyCode.L)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("L");
			} else if (Input.GetKeyDown (KeyCode.Semicolon)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("SC");
			}
		}
	}

	void deleteSelection() {
		if (selectedObject == null) {
			return;
		}

		if (selectedObject.tag == "Note") {
			Destroy (selectedObject);
		}

		selectedObject = null;
	}

	void clearSelection() {
		if (selectedObject == null) {
			return;
		}

		clearColor ();

		selectedObject = null;

	}

	void clearColor() {
		if (selectedObject == null) {
			return;
		}

		if (selectedObject.tag == "Note") {
			var color = selectedObject.GetComponent<Renderer> ().material.color;
			color.a = 1f;
			selectedObject.GetComponent<Renderer> ().material.color = color;
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
		max = 0;
		resolution = 10;

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

		if (instantiateWaveController.activeSelf) {
			instantiateWaveController.SetActive (false);
		}
			
		instantiateWaveController.SetActive (true);
		originalPositionX = instantiateWaveController.transform.position.x;
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

			if (audioSource.isPlaying == true) {
				audioSource.Pause ();
			}


			var currentPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			var offset = startPosition.x - currentPosition.x;
			var pixOffset = mouseX - Input.mousePosition.x;
			//Debug.Log (offset);
			instantiateWaveController.transform.position = new Vector3 (instantiateWaveController.transform.position.x - offset, this.transform.position.y, 0f);
			startPosition = currentPosition;

			if (pixOffset > 10) {
				//how many?
				//int number = (int)pixOffset / 10 + 1;
				mouseX = Input.mousePosition.x;

				instantiateWaveController.GetComponent<InstantiateWaveform> ().redraw (Constants.kLeft);

			} else if (pixOffset < -10) {
				//int number = (int)pixOffset / 10;
				mouseX = Input.mousePosition.x;
				instantiateWaveController.GetComponent<InstantiateWaveform> ().redraw (Constants.kRight);
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
			//int number = (int)pixOffset / 10;
			waveControllerX = currentPositionX;
			instantiateWaveController.GetComponent<InstantiateWaveform> ().redraw (Constants.kLeft);
		}
	}

	public void PlayPause() {
		if (audioSource.isPlaying == true) {
			audioSource.Pause ();
		} else {
			waveControllerX = Camera.main.WorldToScreenPoint(instantiateWaveController.transform.position).x;
			//get location
			var currentPostionX = instantiateWaveController.transform.position.x;
			float offset = originalPositionX - currentPostionX;
			if (offset > 0) {
				var current = offset / 0.1f;
				current /= audioSource.clip.channels;
				audioSource.timeSamples = (int)current * resolution;
				audioSource.Play ();
			} else {
				audioSource.timeSamples = 0;
				audioSource.Play ();
			}


		}
	}

	public void backHome() {
		blurImg.gameObject.SetActive (true);
		confirmMenu.gameObject.SetActive (true);
	}

	public void cancel() {
		confirmMenu.gameObject.SetActive (false);
		blurImg.gameObject.SetActive (false);
	}

	public void LoadScene(string sceneName) {
		SceneManager.LoadScene (sceneName);
	}
}
