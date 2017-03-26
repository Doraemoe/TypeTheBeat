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
	public Image positionImg;

	public GameObject noteA;
	public GameObject noteS;
	public GameObject noteD;
	public GameObject noteF;
	public GameObject noteJ;
	public GameObject noteK;
	public GameObject noteL;
	public GameObject noteSC;

	string path;
	int resolution;
	float distance;
	float timeDelay;
	float speed;
	float speedMulti = 1f;
	Dictionary<string, float> notemap;
	AudioSource audioSource;
	bool finishedPrepare = false;
	// Use this for initialization

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		//Debug.Log( SceneInfo.getValueForKey ("path"));
		path = SceneInfo.getValueForKey ("path");
		resolution = int.Parse(SceneInfo.getValueForKey ("resolution"));
		notemap = new Dictionary<string, float>();
		setDisplay ();
		loadNotemap ();
		prepareNote ();

		StartCoroutine(LoadSongCoroutine ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			pauseAndDisplayMenu ();
		}

		if(audioSource.isPlaying) {
			//Debug.Log(audioSource.timeSamples);
			renderNotes ();
		}
	}

	void renderNotes() {
		var pos = this.transform.position;
		pos.x -= Time.deltaTime * speed;
		this.transform.position = pos;
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

		Debug.Log (audioSource.clip.frequency);

		//float t = 0f;

		//Debug.Log (audioSource.clip.frequency);

		timeDelay = distance / 0.1f * resolution / audioSource.clip.frequency / speedMulti;
		//Debug.Log (timeDelay);

		speed = distance / timeDelay;
		audioSource.PlayDelayed (timeDelay);
		//audioSource.PlayDelayed (5);
	}

	void prepareNote() {

		float p = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width, Screen.height)).x;
		//Debug.Log (p);
		var pos = this.transform.position;
		pos.x = p;
		this.transform.position = pos;

		//Debug.Log (positionImg.transform.position);

		distance = p - positionImg.transform.position.x;

		foreach (KeyValuePair<string, float> entry in notemap) {
			float value = entry.Value * speedMulti;
			if(entry.Key == "A") { 
				GameObject a = (GameObject)Instantiate (noteA);
				a.layer = LayerMask.NameToLayer ("Background Image");
				a.transform.parent = this.transform;
				var tmp = this.transform.position;
				//tmp.x += entry.Value; 
				tmp.x += value; 
				tmp.y += a.transform.position.y;
				a.transform.position = tmp;
				a.name = "A";
			} else if (entry.Key == "S") { 
				GameObject s = (GameObject)Instantiate (noteS);
				s.layer = LayerMask.NameToLayer ("Background Image");
				s.transform.parent = this.transform;
				var tmp = this.transform.position;
				//tmp.x += entry.Value; 
				tmp.x += value;
				tmp.y += s.transform.position.y;
				s.transform.position = tmp;
				s.name = "S";
			} else if (entry.Key == "D") { 
				GameObject d = (GameObject)Instantiate (noteD);
				d.layer = LayerMask.NameToLayer ("Background Image");
				d.transform.parent = this.transform;
				var tmp = this.transform.position;
				//tmp.x += entry.Value; 
				tmp.x += value;
				tmp.y += d.transform.position.y;
				d.transform.position = tmp;
				d.name = "D";
			} else if (entry.Key == "F") { 
				GameObject f = (GameObject)Instantiate (noteF);
				f.layer = LayerMask.NameToLayer ("Background Image");
				f.transform.parent = this.transform;
				var tmp = this.transform.position;
				//tmp.x += entry.Value; 
				tmp.x += value;
				tmp.y += f.transform.position.y;
				f.transform.position = tmp;
				f.name = "F";
			} else if (entry.Key == "J") { 
				GameObject j = (GameObject)Instantiate (noteJ);
				j.layer = LayerMask.NameToLayer ("Background Image");
				j.transform.parent = this.transform;
				var tmp = this.transform.position;
				//tmp.x += entry.Value; 
				tmp.x += value;
				tmp.y += j.transform.position.y;
				j.transform.position = tmp;
				j.name = "J";
			} else if (entry.Key == "K") { 
				GameObject k = (GameObject)Instantiate (noteK);
				k.layer = LayerMask.NameToLayer ("Background Image");
				k.transform.parent = this.transform;
				var tmp = this.transform.position;
				//tmp.x += entry.Value; 
				tmp.x += value;
				tmp.y += k.transform.position.y;
				k.transform.position = tmp;
				k.name = "K";
			} else if (entry.Key == "L") { 
				GameObject l = (GameObject)Instantiate (noteL);
				l.layer = LayerMask.NameToLayer ("Background Image");
				l.transform.parent = this.transform;
				var tmp = this.transform.position;
				//tmp.x += entry.Value; 
				tmp.x += value;
				tmp.y += l.transform.position.y;
				l.transform.position = tmp;
				l.name = "L";
			} else if (entry.Key == "SC") { 
				GameObject sc = (GameObject)Instantiate (noteSC);
				sc.layer = LayerMask.NameToLayer ("Background Image");
				sc.transform.parent = this.transform;
				var tmp = this.transform.position;
				//tmp.x += entry.Value; 
				tmp.x += value;
				tmp.y += sc.transform.position.y;
				sc.transform.position = tmp;
				sc.name = "SC";
			}
		}
	}

	void pauseAndDisplayMenu() {
		audioSource.Pause ();


		darkImg.gameObject.SetActive (true);
		confirmCanvas.gameObject.SetActive (true);
	}

	public void clickedYes() {
		SceneManager.LoadScene ("Selection");
	}

	public void clickedNo() {
		audioSource.Play ();

		darkImg.gameObject.SetActive (false);
		confirmCanvas.gameObject.SetActive (false);
	}
}
