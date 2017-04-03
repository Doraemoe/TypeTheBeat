using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Xml;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SFB;
//using UnityEditor;

[RequireComponent(typeof(BoxCollider2D))]

public class EditorController : MonoBehaviour {

	public GameObject instantiateWaveController;
	public float[] waveForm;
	public Canvas confirmMenu;
	public Image blurImg;
	public GameObject selectedObject;
	public Button loadNotesBtn;
	public Button saveBtn;
	public Text playPauseTxt;
	public Text songName;
	public Text songArtist;
	public InputField selectedLocation;
	public InputField nameInput;
	public InputField artistInput;

	Vector3 startPosition;
	float originalPositionX;
	float mouseX;
	AudioSource audioSource;
	int resolution;
	float[] samples;
	float waveControllerX;
	float max = 0;
	float[] notePositions;
	string[] noteName;
	string songLocation;
	Dictionary<float, List<string>> notesData;


	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();

		selectedLocation.onEndEdit.AddListener(delegate {setSelectedLocalPosition(); });

	}

	void setSelectedLocalPosition() {
		if(selectedObject == null) {
			selectedLocation.text = "";
			return;
		}
		var tmp = selectedObject.transform.localPosition;
		tmp.x = float.Parse(selectedLocation.text);
		selectedObject.transform.localPosition = tmp;

	}
	
	// Update is called once per frame
	void Update () {

		//select note
		if (Input.GetMouseButtonDown (0)) {
			Vector2 ray = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
			RaycastHit2D[] hits = Physics2D.RaycastAll (ray, Vector2.zero);


			if (hits.Length != 0) {
				if (hits.Length == 1) {
					clearSelection ();
				} else {
					foreach (var hit in hits) {
						if (hit.transform.gameObject.tag == "GameController") {
							continue;
						} else if (selectedObject != null &&
						   selectedObject.GetInstanceID () == hit.transform.gameObject.GetInstanceID ()) {
							continue;
						} else {
							clearColor ();
							selectedObject = hit.transform.gameObject;
							var color = selectedObject.GetComponent<Renderer> ().material.color;
							color.a = 0.8f;
							selectedObject.GetComponent<Renderer> ().material.color = color;
							selectedLocation.text = selectedObject.transform.localPosition.x.ToString ();
							break;
						}
					}
				}
			}
		}

		if (audioSource.isPlaying) {
			this.PlayMusic ();
		} else {
			playPauseTxt.text = "Play";
		}

		if (instantiateWaveController.activeSelf && !nameInput.isFocused && !artistInput.isFocused) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				clearSelection ();
			}
			if (Input.GetKeyDown (KeyCode.Delete)) {
				deleteSelection ();
			}
			if (Input.GetKeyDown (KeyCode.Backspace)) {
				deleteSelection ();
			}
			if (Input.GetKey (KeyCode.LeftArrow)) {
				moveSelectedLeft ();
			}
			if (Input.GetKey (KeyCode.RightArrow))  {
				moveSelectedRight ();
			}

			if (Input.GetKeyDown (KeyCode.A)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("A");
			} 
			if (Input.GetKeyDown (KeyCode.S)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("S");
			} 
			if (Input.GetKeyDown (KeyCode.D)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("D");
			} 
			if (Input.GetKeyDown (KeyCode.F)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("F");
			} 
			if (Input.GetKeyDown (KeyCode.J)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("J");
			} 
			if (Input.GetKeyDown (KeyCode.K)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("K");
			} 
			if (Input.GetKeyDown (KeyCode.L)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("L");
			} 
			if (Input.GetKeyDown (KeyCode.Semicolon)) {
				instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNote ("SC");
			}
		}
	}

	void moveSelectedRight() {
		if (selectedObject == null) {
			return;
		}
		selectedObject.transform.localPosition += Vector3.right * 0.01f;
		selectedLocation.text = selectedObject.transform.localPosition.x.ToString();
	}

	void moveSelectedLeft() {
		if (selectedObject == null) {
			return;
		}
		selectedObject.transform.localPosition += Vector3.left * 0.01f;
		selectedLocation.text = selectedObject.transform.localPosition.x.ToString();
	}

	void deleteSelection() {
		if (selectedObject == null) {
			return;
		}

		if (selectedObject.tag == "Note") {
			Destroy (selectedObject);
		}

		selectedLocation.text = "";
		selectedObject = null;
	}

	void clearSelection() {
		if (selectedObject == null) {
			return;
		}

		clearColor ();

		selectedLocation.text = "";
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

		var extensions = new [] {
			new ExtensionFilter("Music Files", "ogg"),
		};

		var path = StandaloneFileBrowser.OpenFilePanel(
			"Open File", 
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), 
			extensions, 
			false);

		if (path[0].Length != 0) {
			songLocation = new System.Uri(path[0]).AbsolutePath; 
			songLocation = WWW.UnEscapeURL (songLocation);
			StartCoroutine (LoadSongCoroutine (path[0]));
		}
	}


	IEnumerator LoadSongCoroutine(string path)
	{
		var audioLocation = new WWW (path);

		yield return audioLocation;

		audioSource.clip = audioLocation.GetAudioClip (false, false);
		generateSoundWave ();

	}

	void generateSoundWave() {
		max = 0;

		resolution = audioSource.clip.frequency / Constants.kSamplePerSecond;
		samples = new float[audioSource.clip.samples * audioSource.clip.channels];
		audioSource.clip.GetData (samples, 0);

		waveForm = new float[audioSource.clip.samples / resolution];

		for (int i = 0; i < waveForm.Length; i++) {
			waveForm[i] = 0;
			for (int ii = 0; ii < resolution * audioSource.clip.channels; ii++) {
				//waveForm[i] += samples[(i * resolution) + ii];
				waveForm [i] += Mathf.Abs (samples [(i * resolution * audioSource.clip.channels) + ii]); 
			}
			waveForm[i] /= resolution * audioSource.clip.channels;
			if (waveForm [i] > max) {
				max = waveForm [i];
			}
		}

		this.normalizeWaveForm ();

		if (instantiateWaveController.activeSelf) {
			instantiateWaveController.SetActive (false);
		}
			
		instantiateWaveController.SetActive (true);
		loadNotesBtn.gameObject.SetActive (true);
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
				playPauseTxt.text = "Play";
				audioSource.Pause ();
			}
				
			var currentPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			var offset = startPosition.x - currentPosition.x;
			var pixOffset = mouseX - Input.mousePosition.x;
			instantiateWaveController.transform.position = new Vector3 (instantiateWaveController.transform.position.x - offset, this.transform.position.y, 0f);
			startPosition = currentPosition;

			if (pixOffset > 10) {
				mouseX = Input.mousePosition.x;
				instantiateWaveController.GetComponent<InstantiateWaveform> ().redraw (Constants.kLeft);
			} else if (pixOffset < -10) {
				mouseX = Input.mousePosition.x;
				instantiateWaveController.GetComponent<InstantiateWaveform> ().redraw (Constants.kRight);
			} else {
				
			}
		}
	}

	void PlayMusic() {

		float current = audioSource.timeSamples / resolution;
		instantiateWaveController.transform.position = Vector3.left * current * 0.1f + Vector3.up * 1.2f;
		var currentPositionX = Camera.main.WorldToScreenPoint(instantiateWaveController.transform.position).x;
		var pixOffset =  waveControllerX - currentPositionX;
		if (pixOffset > 10) {
			waveControllerX = currentPositionX;
			instantiateWaveController.GetComponent<InstantiateWaveform> ().redraw (Constants.kLeft);
		}
	}

	public void PlayPause() {
		if (audioSource.isPlaying == true) {
			playPauseTxt.text = "Play";
			audioSource.Pause ();
		} else {
			waveControllerX = Camera.main.WorldToScreenPoint(instantiateWaveController.transform.position).x;
			//get location
			var currentPostionX = instantiateWaveController.transform.position.x;
			float offset = originalPositionX - currentPostionX;
			if (offset > 0) {
				var current = offset / 0.1f;
				//current /= audioSource.clip.channels;
				audioSource.timeSamples = (int)current * resolution;
				playPauseTxt.text = "Pause";
				audioSource.Play ();
			} else {
				audioSource.timeSamples = 0;
				playPauseTxt.text = "Pause";
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

	public void save() {
		

		var path = StandaloneFileBrowser.OpenFolderPanel(
			"Save to a folder", 
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
			false);


		if(path[0].Length != 0) {
			//var notes = GameObject.FindGameObjectsWithTag ("Note");


			path[0] = new System.Uri(path[0]).AbsolutePath; 
			path[0] = WWW.UnEscapeURL (path[0]);

			collectNotesData();

			var sr = File.CreateText(path[0] + "song.notemap");

			foreach(KeyValuePair<float, List<string>> data in notesData)
			{
				//sr.WriteLine (data.Value.Count);

				string names = "";
				foreach (string name in data.Value) {
					names += name + " ";
				}
				sr.WriteLine (names);
				sr.WriteLine (data.Key);
			}

			/*
			for (int i = 0; i < notes.Length; i++) {
				sr.WriteLine (notes [i].name);
				sr.WriteLine (notes[i].transform.localPosition.x);

			}
			*/
			sr.Close();

			writeMetaXML (path[0] + "meta.xml");

			File.Copy (songLocation, path [0] + "song.ogg", true);
		}
	}

	void collectNotesData() {
		GameObject[] notes = GameObject.FindGameObjectsWithTag ("Note");
		notesData = new Dictionary<float, List<string>> (notes.Length);

		foreach(GameObject note in notes) {
			string name = note.name;
			float position = note.transform.localPosition.x;
			if (notesData.ContainsKey (position)) {
				notesData [position].Add (name);
			} else {
				List<string> item = new List<string>(){
					name,
				};

				notesData.Add (position, item);
			}
		}
	}

	void writeMetaXML(string path) {
		XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
		writer.WriteStartDocument(true);
		writer.Formatting = Formatting.Indented;
		writer.Indentation = 2;
		writer.WriteStartElement("Song");

		writer.WriteStartElement("Name");
		writer.WriteString(songName.text);
		writer.WriteEndElement();
		writer.WriteStartElement("Artist");
		writer.WriteString(songArtist.text);
		writer.WriteEndElement();
		writer.WriteStartElement("Resolution");
		writer.WriteString(resolution.ToString());
		writer.WriteEndElement();

		writer.WriteEndElement();
		writer.WriteEndDocument();
		writer.Close();
	}

	public void loadNotes() {

		var extensions = new [] {
			new ExtensionFilter("Notemap File", "notemap"),
		};

		var path = StandaloneFileBrowser.OpenFilePanel(
			"Open Notemap",
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
			extensions,
			false);

		if (path[0].Length != 0) {
			
			path[0] = new System.Uri(path[0]).AbsolutePath; 
			path[0] = WWW.UnEscapeURL (path[0]);

			if (instantiateWaveController.activeSelf) {
				string tmp;
				//float position;

				//clear old notes
				var notes = GameObject.FindGameObjectsWithTag ("Note");
				foreach (GameObject note in notes) {
					Destroy (note);
				}



				var reader = new StreamReader (path[0]);

				using (reader) {
					do {
						tmp = reader.ReadLine();
						if(tmp != null) {
							//According to the document, if the separator parameter is null or contains no characters, white-space characters are assumed to be the delimiters.
							string[] names = tmp.Trim().Split(null);
							float position = float.Parse(reader.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);
							foreach(string name in names) {
								instantiateWaveController.GetComponent<InstantiateWaveform> ().drawNoteWithPosition(name, position);
							}
						}
					} while (tmp != null);
					reader.Close ();
				}
			}
		}
	}
}
