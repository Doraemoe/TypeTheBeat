using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstantiateWaveform : MonoBehaviour {
	public GameObject wavePrefab;
	public EditorController editorCtrl;

	GameObject[] waveFormGraph;
	int leftMost = 0;
	int rightMost;

	// Use this for initialization
	void Start () {
		int i;
		//var waveFormData = editorCtrl.waveForm;
		waveFormGraph = new GameObject[editorCtrl.waveForm.Length];

		for (i = 0; i < waveFormGraph.Length; i++) {
			var position = generateWaveBar (i);
			if (position > Camera.main.pixelWidth) {
				i += 1;
				break;
			}
		}
		rightMost = i;
		//Debug.Log ("rightmost" + i);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	float generateWaveBar(int i) {
		GameObject insWave = (GameObject)Instantiate (wavePrefab);
		insWave.transform.position = this.transform.position;
		insWave.transform.position += Vector3.right * i * 0.1f;
		insWave.transform.localScale = new Vector3 (1, editorCtrl.waveForm[i] * 30, 1);
		insWave.transform.parent = this.transform;
		insWave.name = "wave" + i;
		//var currentX = this.transform.position.x;
		//this.transform.position.x = currentX + 10;
		waveFormGraph [i] = insWave;
		return Camera.main.WorldToScreenPoint (insWave.transform.position).x;

	}

	public void redraw(int direction) {
		//Debug.Log (number);
		if (direction == Constants.kLeft) { //left
			//generate right
			int i;
			for (i = rightMost; i < waveFormGraph.Length; i++) {
				var position = generateWaveBar (i);
				if (position > Camera.main.pixelWidth) {
					i += 1;
					break;
				}
			}
			rightMost = i;

			//disappear left
			for (i = leftMost; i < waveFormGraph.Length; i++) {
				var wavebar = GameObject.Find ("wave" + i);
				var xPoint = Camera.main.WorldToScreenPoint (wavebar.transform.position).x;
				if (xPoint < 0) {
					wavebar.SetActive (false);
				} else {
					break;
				}
			}
			leftMost = i;
			//Debug.Log ("leftmost " + leftMost);
		}

		if (direction == Constants.kRight) { //right
			//generate left
			int i;
			for (i = leftMost - 1; i >= 0; i--) {
				var position = generateWaveBar (i);
				if (position < 0) {
					i -= 1;
					break;
				}
			}
			leftMost = i + 1;

			//disappear right
			for (i = rightMost - 1; i >= 0; i--) {
				var wavebar = GameObject.Find ("wave" + i);
				var xPoint = Camera.main.WorldToScreenPoint (wavebar.transform.position).x;
				if (xPoint > Camera.main.pixelWidth) {
					wavebar.SetActive (false);
				} else {
					break;
				}
			}
			rightMost = i + 1;
		}
	}
		
}
