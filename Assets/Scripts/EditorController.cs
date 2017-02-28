using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[RequireComponent(typeof(BoxCollider2D))]

public class EditorController : MonoBehaviour {

	public GameObject instantiateWaveController;
	//public Scrollbar scrollBar;
	//public ScrollRect scrollView;
	public float[] waveForm;

	Vector3 startPosition;
	AudioSource audioSource;
	int resolution = 60;

	float[] samples;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		//scrollView.SetActive (false);
		//scrollView.enabled = false;
		//scrollView.gameObject.SetActive (false);
		//Debug.Log (scrollBar);

	}
	
	// Update is called once per frame
	void Update () {

		if (audioSource.isPlaying == true) {
			for (int i = 0; i < waveForm.Length; i++) {
				Vector3 sv = new Vector3 (i * 0.04f, waveForm [i] * 10, 0);
				Vector3 ev = new Vector3 (i * 0.04f, - waveForm [i] * 10, 0);

				Debug.DrawLine (sv, ev, Color.yellow);
			}

			int current = audioSource.timeSamples / resolution;
			current *= audioSource.clip.channels;

			Vector3 c = new Vector3 (current * 0.04f, 0, 0);

			Debug.DrawLine (c, c + Vector3.up * 10, Color.white);
		}
	}

	public void OpenFile () {

		var path = EditorUtility.OpenFilePanel(
			"Open Music",
			"",
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
				waveForm[i] += samples[(i * resolution) + ii];

				//Mathf.abs <-- another option
			}
			waveForm[i] /= resolution;
		}
		audioSource.Play ();
		instantiateWaveController.SetActive (true);
		//scrollView.SetActive (true);
	}

	void OnMouseDown() {
		startPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
	}

	void OnMouseDrag() {
		var currentPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		var offset = startPosition.x - currentPosition.x;
		instantiateWaveController.transform.position = new Vector3 (instantiateWaveController.transform.position.x - offset, this.transform.position.y, 0f);
		startPosition = currentPosition;
	}
}
