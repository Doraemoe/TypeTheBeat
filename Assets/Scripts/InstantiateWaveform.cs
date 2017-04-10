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
		waveFormGraph = new GameObject[editorCtrl.waveForm.Length];
		this.transform.position = new Vector3 (0f, 1.2f, 0f);

		for (i = 0; i < waveFormGraph.Length; i++) {
			var position = GenerateWaveBar (i);
			if (position > Camera.main.pixelWidth) {
				i += 1;
				break;
			}
		}
		rightMost = i;
	}

	void OnDisable() {
		foreach (Transform child in this.transform) {
			Destroy (child.gameObject);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Generate wave bar based on song data
	/// </summary>
	/// <returns>Screen position of generated wave bar</returns>
	float GenerateWaveBar(int i) {
		GameObject insWave = (GameObject)Instantiate (wavePrefab);
		insWave.transform.position = this.transform.position;
		insWave.transform.position += Vector3.right * i * 0.1f;
		insWave.transform.localScale = new Vector3 (1, editorCtrl.waveForm[i] * 30, 1);
		insWave.transform.parent = this.transform;
		insWave.name = "wave" + i;
		waveFormGraph [i] = insWave;
		return Camera.main.WorldToScreenPoint (insWave.transform.position).x;

	}

	/// <summary>
	/// Redraw wave bar when user move left or right
	/// </summary>
	/// <param name="direction">Direction user moved</param>
	public void Redraw(int direction) {
		if (direction == Constants.kLeft) { //left
			//generate right
			int i;
			for (i = rightMost; i < waveFormGraph.Length; i++) {
				var position = GenerateWaveBar (i);
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
		}

		if (direction == Constants.kRight) { //right
			//generate left
			int i;
			for (i = leftMost - 1; i >= 0; i--) {
				var position = GenerateWaveBar (i);
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


	/// <summary>
	/// Draw note on screen based on user input
	/// </summary>
	public void DrawNote(string note) {
		if(note == "A") { 
			GameObject a = (GameObject)Instantiate (noteA);
			NoteSetup (a, "A", 0f);
		} else if (note == "S") { 
			GameObject s = (GameObject)Instantiate (noteS);
			NoteSetup (s, "S", 0.8f);
		} else if (note == "D") { 
			GameObject d = (GameObject)Instantiate (noteD);
			NoteSetup (d, "D", 1.6f);
		} else if (note == "F") { 
			GameObject f = (GameObject)Instantiate (noteF);
			NoteSetup (f, "F", 2.4f);
		} else if (note == "J") { 
			GameObject j = (GameObject)Instantiate (noteJ);
			NoteSetup (j, "J", 0.3f);
		} else if (note == "K") { 
			GameObject k = (GameObject)Instantiate (noteK);
			NoteSetup (k, "K", 1.3f);
		} else if (note == "L") { 
			GameObject l = (GameObject)Instantiate (noteL);
			NoteSetup (l, "L", 1.9f);
		} else if (note == "SC") { 
			GameObject sc = (GameObject)Instantiate (noteSC);
			NoteSetup (sc, "SC", 2.8f);
		}
	}

	/// <summary>
	/// Setup note
	/// </summary>
	/// <param name="note">The note need to setup</param>
	/// <param name="name">Name of the note</param>
	/// <param name="positionY">Y position on screen</param>
	void NoteSetup(GameObject note, string name, float positionY) {
		note.transform.parent = this.transform;
		note.name = name;

		GameObject center = (GameObject)Instantiate (centerInd);
		center.transform.position += Vector3.down * positionY;
		center.transform.SetParent (note.transform, false);
	}

	/// <summary>
	/// Open a home loan account
	/// </summary>
	/// <param name="note">The note need to setup</param>
	/// <param name="name">Name of the note</param>
	/// <param name="positionY">Y position on screen</param>
	/// <param name="positionX">X position on screen</param>
	void NoteSetup(GameObject note, string name, float positionY, float positionX) {
		note.transform.parent = this.transform;
		var tmp = note.transform.localPosition;
		tmp.x = positionX;
		note.transform.localPosition = tmp;
		note.name = name;

		GameObject center = (GameObject)Instantiate (centerInd);
		center.transform.position += Vector3.down * positionY;
		center.transform.SetParent (note.transform, false);
	}

	/// <summary>
	/// Draw note at given position
	/// </summary>
	/// <param name="note">The note need to draw</param>
	/// <param name="localPosition">Local position of the note</param>
	public void DrawNoteWithPosition(string note, float localPosition) {
		if(note == "A") { 
			GameObject a = (GameObject)Instantiate (noteA);
			NoteSetup (a, "A", 0f, localPosition);

		} else if (note == "S") { 
			GameObject s = (GameObject)Instantiate (noteS);
			NoteSetup (s, "S", 0.8f, localPosition);
		} else if (note == "D") { 
			GameObject d = (GameObject)Instantiate (noteD);
			NoteSetup (d, "D", 1.6f, localPosition);
		} else if (note == "F") { 
			GameObject f = (GameObject)Instantiate (noteF);
			NoteSetup (f, "F", 2.4f, localPosition);

		} else if (note == "J") { 
			GameObject j = (GameObject)Instantiate (noteJ);
			NoteSetup (j, "J", 0.3f, localPosition);

		} else if (note == "K") { 
			GameObject k = (GameObject)Instantiate (noteK);
			NoteSetup (k, "K", 1.3f, localPosition);
		} else if (note == "L") { 
			GameObject l = (GameObject)Instantiate (noteL);
			NoteSetup (l, "L", 1.9f, localPosition);

		} else if (note == "SC") { 
			GameObject sc = (GameObject)Instantiate (noteSC);
			NoteSetup (sc, "SC", 2.8f, localPosition);
		}
	}
		
}
