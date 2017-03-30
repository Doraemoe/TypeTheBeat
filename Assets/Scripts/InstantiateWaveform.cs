﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InstantiateWaveform : MonoBehaviour {
	public GameObject wavePrefab;
	public GameObject noteA;
	public GameObject noteS;
	public GameObject noteD;
	public GameObject noteF;
	public GameObject noteJ;
	public GameObject noteK;
	public GameObject noteL;
	public GameObject noteSC;
	public EditorController editorCtrl;

	GameObject[] waveFormGraph;
	int leftMost = 0;
	int rightMost;

	// Use this for initialization
	void Start () {
		
	}

	void OnEnable() {
		int i;
		//var waveFormData = editorCtrl.waveForm;
		waveFormGraph = new GameObject[editorCtrl.waveForm.Length];
		this.transform.position = new Vector3 (0f, 1.2f, 0f);

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

	void OnDisable() {
		foreach (Transform child in this.transform) {
			Destroy (child.gameObject);
		}

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
					Destroy (wavebar);
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
					Destroy (wavebar);//.SetActive (false);
				} else {
					break;
				}
			}
			rightMost = i + 1;
		}
	}

	public void drawNote(string note) {
		if(note == "A") { 
			GameObject a = (GameObject)Instantiate (noteA);
			a.transform.parent = this.transform;
			a.name = "A";
			//Debug.Log (this.transform.position.x);
		} else if (note == "S") { 
			GameObject s = (GameObject)Instantiate (noteS);
			s.transform.parent = this.transform;
			s.name = "S";
		} else if (note == "D") { 
			GameObject d = (GameObject)Instantiate (noteD);
			d.transform.parent = this.transform;
			d.name = "D";
		} else if (note == "F") { 
			GameObject f = (GameObject)Instantiate (noteF);
			f.transform.parent = this.transform;
			f.name = "F";
		} else if (note == "J") { 
			GameObject j = (GameObject)Instantiate (noteJ);
			j.transform.parent = this.transform;
			j.name = "J";
		} else if (note == "K") { 
			GameObject k = (GameObject)Instantiate (noteK);
			k.transform.parent = this.transform;
			k.name = "K";
		} else if (note == "L") { 
			GameObject l = (GameObject)Instantiate (noteL);
			l.transform.parent = this.transform;
			l.name = "L";
		} else if (note == "SC") { 
			GameObject sc = (GameObject)Instantiate (noteSC);
			sc.transform.parent = this.transform;
			sc.name = "SC";
		}
	}

	public void drawNoteWithPosition(string note, float localPosition) {
		if(note == "A") { 
			GameObject a = (GameObject)Instantiate (noteA);
			a.transform.parent = this.transform;
			var tmp = a.transform.localPosition;
			tmp.x = localPosition;
			//tmp.y = a.transform.position.y;
			a.transform.localPosition = tmp;
			a.name = "A";
			//Debug.Log (this.transform.position.x);
		} else if (note == "S") { 
			GameObject s = (GameObject)Instantiate (noteS);
			s.transform.parent = this.transform;
			var tmp = s.transform.localPosition;
			tmp.x = localPosition; 
			//tmp.y = s.transform.position.y;
			s.transform.localPosition = tmp;
			s.name = "S";
		} else if (note == "D") { 
			GameObject d = (GameObject)Instantiate (noteD);
			d.transform.parent = this.transform;
			var tmp = d.transform.localPosition;
			tmp.x = localPosition; 
			//tmp.y = d.transform.position.y;
			d.transform.localPosition = tmp;
			d.name = "D";
		} else if (note == "F") { 
			GameObject f = (GameObject)Instantiate (noteF);
			f.transform.parent = this.transform;
			var tmp = f.transform.localPosition;
			tmp.x = localPosition; 
			//tmp.y = f.transform.position.y;
			f.transform.localPosition = tmp;
			f.name = "F";
		} else if (note == "J") { 
			GameObject j = (GameObject)Instantiate (noteJ);
			j.transform.parent = this.transform;
			var tmp = j.transform.localPosition;
			tmp.x = localPosition; 
			//tmp.y = j.transform.position.y;
			j.transform.localPosition = tmp;
			j.name = "J";
		} else if (note == "K") { 
			GameObject k = (GameObject)Instantiate (noteK);
			k.transform.parent = this.transform;
			var tmp = k.transform.localPosition;
			tmp.x = localPosition; 
			//tmp.y = k.transform.position.y;
			k.transform.localPosition = tmp;
			k.name = "K";
		} else if (note == "L") { 
			GameObject l = (GameObject)Instantiate (noteL);
			l.transform.parent = this.transform;
			var tmp = l.transform.localPosition;
			tmp.x = localPosition; 
			//tmp.y = l.transform.position.y;
			l.transform.localPosition = tmp;
			l.name = "L";
		} else if (note == "SC") { 
			GameObject sc = (GameObject)Instantiate (noteSC);
			sc.transform.parent = this.transform;
			var tmp = sc.transform.position;
			tmp.x = localPosition; 
			//tmp.y = sc.transform.position.y;
			sc.transform.localPosition = tmp;
			sc.name = "SC";
		}
	}
		
}
