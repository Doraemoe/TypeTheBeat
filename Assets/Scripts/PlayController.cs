using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayController : MonoBehaviour {

	public Canvas confirmCanvas;
	public Button yesBtn;
	public Button noBtn;
	public Image darkImg;

	public GameObject noteA;
	public GameObject noteS;
	public GameObject noteD;
	public GameObject noteF;
	public GameObject noteJ;
	public GameObject noteK;
	public GameObject noteL;
	public GameObject noteSC;

	string path;
	Dictionary<string, float> notemap;
	AudioSource audioSource;
	// Use this for initialization

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		//Debug.Log( SceneInfo.getValueForKey ("path"));
		path = SceneInfo.getValueForKey ("path");
		notemap = new Dictionary<string, float>();
		setDisplay ();
		loadNotemap ();
		StartCoroutine(LoadSongCoroutine ());
		prepareNote ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			pauseAndDisplayMenu ();
		}
	}

	public void goBack () {
		SceneManager.LoadScene ("Selection");
	}

	void setDisplay() {

		byte[] FileData;
		string[] files = Directory.GetFiles (path, "bg.*");

		Texture2D tex = new Texture2D(2, 2);
		FileData = File.ReadAllBytes(files[0]);
		tex.LoadImage(FileData);

		RawImage backgroundImg = GameObject.FindGameObjectWithTag ("Background").GetComponent<RawImage> ();;

		backgroundImg.texture = tex;
	}

	void loadNotemap() {
		string name;
		float position;

		var reader = new StreamReader (path + "/song.notemap");

		using (reader) {
			do {
				name = reader.ReadLine();
				if(name != null) {
					position = float.Parse(reader.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);
					notemap.Add(name, position);
				}
			} while (name != null);
			reader.Close ();
		}

	}

	IEnumerator LoadSongCoroutine() {
		var audioLocation = new WWW ("file://" + path + "/song.ogg");

		yield return audioLocation;

		audioSource.clip = audioLocation.GetAudioClip (false, false);

		//audioSource.Play ();
	}

	void prepareNote() {
		foreach (KeyValuePair<string, float> entry in notemap) {
			if(entry.Key == "A") { 
				GameObject a = (GameObject)Instantiate (noteA);
				a.transform.parent = this.transform;
				var tmp = a.transform.position;
				tmp.x += entry.Value; 
				a.transform.position = tmp;
				a.name = "A";
			} else if (entry.Key == "S") { 
				GameObject s = (GameObject)Instantiate (noteS);
				s.transform.parent = this.transform;
				var tmp = s.transform.position;
				tmp.x += entry.Value; 
				s.transform.position = tmp;
				s.name = "S";
			} else if (entry.Key == "D") { 
				GameObject d = (GameObject)Instantiate (noteD);
				d.transform.parent = this.transform;
				var tmp = d.transform.position;
				tmp.x += entry.Value; 
				d.transform.position = tmp;
				d.name = "D";
			} else if (entry.Key == "F") { 
				GameObject f = (GameObject)Instantiate (noteF);
				f.transform.parent = this.transform;
				var tmp = f.transform.position;
				tmp.x += entry.Value; 
				f.transform.position = tmp;
				f.name = "F";
			} else if (entry.Key == "J") { 
				GameObject j = (GameObject)Instantiate (noteJ);
				j.transform.parent = this.transform;
				var tmp = j.transform.position;
				tmp.x += entry.Value; 
				j.transform.position = tmp;
				j.name = "J";
			} else if (entry.Key == "K") { 
				GameObject k = (GameObject)Instantiate (noteK);
				k.transform.parent = this.transform;
				var tmp = k.transform.position;
				tmp.x += entry.Value; 
				k.transform.position = tmp;
				k.name = "K";
			} else if (entry.Key == "L") { 
				GameObject l = (GameObject)Instantiate (noteL);
				l.transform.parent = this.transform;
				var tmp = l.transform.position;
				tmp.x += entry.Value; 
				l.transform.position = tmp;
				l.name = "L";
			} else if (entry.Key == "SC") { 
				GameObject sc = (GameObject)Instantiate (noteSC);
				sc.transform.parent = this.transform;
				var tmp = sc.transform.position;
				tmp.x += entry.Value; 
				sc.transform.position = tmp;
				sc.name = "SC";
			}
		}
	}

	void pauseAndDisplayMenu() {
		darkImg.gameObject.SetActive (true);
		confirmCanvas.gameObject.SetActive (true);
	}

	public void clickedYes() {
		SceneManager.LoadScene ("Selection");
	}

	public void clickedNo() {
		darkImg.gameObject.SetActive (false);
		confirmCanvas.gameObject.SetActive (false);
	}
}
