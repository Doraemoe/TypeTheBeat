using System.Collections;
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
	public GameObject centerInd;
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
			noteSetup (a, "A", 0f);
		} else if (note == "S") { 
			GameObject s = (GameObject)Instantiate (noteS);
			noteSetup (s, "S", 0.8f);
		} else if (note == "D") { 
			GameObject d = (GameObject)Instantiate (noteD);
			noteSetup (d, "D", 1.6f);
		} else if (note == "F") { 
			GameObject f = (GameObject)Instantiate (noteF);
			noteSetup (f, "F", 2.4f);
		} else if (note == "J") { 
			GameObject j = (GameObject)Instantiate (noteJ);
			noteSetup (j, "J", 0.3f);
		} else if (note == "K") { 
			GameObject k = (GameObject)Instantiate (noteK);
			noteSetup (k, "K", 1.3f);
		} else if (note == "L") { 
			GameObject l = (GameObject)Instantiate (noteL);
			noteSetup (l, "L", 1.9f);
		} else if (note == "SC") { 
			GameObject sc = (GameObject)Instantiate (noteSC);
			noteSetup (sc, "SC", 2.8f);
		}
	}

	void noteSetup(GameObject note, string name, float positionY) {
		note.transform.parent = this.transform;
		note.name = name;

		GameObject center = (GameObject)Instantiate (centerInd);
		center.transform.position += Vector3.down * positionY;
		center.transform.SetParent (note.transform, false);
	}

	void noteSetup(GameObject note, string name, float positionY, float positionX) {
		note.transform.parent = this.transform;
		var tmp = note.transform.localPosition;
		tmp.x = positionX;
		note.transform.localPosition = tmp;
		note.name = name;

		GameObject center = (GameObject)Instantiate (centerInd);
		center.transform.position += Vector3.down * positionY;
		center.transform.SetParent (note.transform, false);
		//center.transform.localPosition = center.transform.position;
	}

	public void drawNoteWithPosition(string note, float localPosition) {
		if(note == "A") { 
			GameObject a = (GameObject)Instantiate (noteA);
			noteSetup (a, "A", 0f, localPosition);

		} else if (note == "S") { 
			GameObject s = (GameObject)Instantiate (noteS);
			noteSetup (s, "S", 0.8f, localPosition);
		} else if (note == "D") { 
			GameObject d = (GameObject)Instantiate (noteD);
			noteSetup (d, "D", 1.6f, localPosition);
		} else if (note == "F") { 
			GameObject f = (GameObject)Instantiate (noteF);
			noteSetup (f, "F", 2.4f, localPosition);

		} else if (note == "J") { 
			GameObject j = (GameObject)Instantiate (noteJ);
			noteSetup (j, "J", 0.3f, localPosition);

		} else if (note == "K") { 
			GameObject k = (GameObject)Instantiate (noteK);
			noteSetup (k, "K", 1.3f, localPosition);
		} else if (note == "L") { 
			GameObject l = (GameObject)Instantiate (noteL);
			noteSetup (l, "L", 1.9f, localPosition);

		} else if (note == "SC") { 
			GameObject sc = (GameObject)Instantiate (noteSC);
			noteSetup (sc, "SC", 2.8f, localPosition);
		}
	}
		
}
