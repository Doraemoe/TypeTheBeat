using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstantiateWaveform : MonoBehaviour {
	public GameObject wavePrefab;
	GameObject[] waveFormGraph;

	// Use this for initialization
	void Start () {
		var waveFormData = GameObject.Find ("EditorController").GetComponent<EditorController> ().waveForm;
		waveFormGraph = new GameObject[waveFormData.Length];

		for (int i = 0; i < waveFormGraph.Length; i++) {
			GameObject insWave = (GameObject)Instantiate (wavePrefab);
			insWave.transform.position = this.transform.position;
			insWave.transform.position += Vector3.right * i * 0.1f;
			insWave.transform.localScale = new Vector3 (1, waveFormData[i] * 300, 1);
			insWave.transform.parent = this.transform;
			insWave.name = "wave" + i;
			//var currentX = this.transform.position.x;
			//this.transform.position.x = currentX + 10;
			waveFormGraph [i] = insWave;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
}
