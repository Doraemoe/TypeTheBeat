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
	public GameObject positionImg;
	public Text scoreTxt;

	int perfect = 0;
	int good = 0;
	int bad = 0;
	int miss = 0 ;

	public GameObject noteA;
	public GameObject noteS;
	public GameObject noteD;
	public GameObject noteF;
	public GameObject noteJ;
	public GameObject noteK;
	public GameObject noteL;
	public GameObject noteSC;

	bool paused = false;
	bool played = false;
	string path;
	int resolution;
	int score = 0;
	int combo = 0;
	float distance;
	float timeDelay;
	float speed;
	float speedMulti = 1f;
	//float positionX;
	//List<Tuple<string, int>> notemap;
	AudioSource audioSource;
	// Use this for initialization

	void Start () {

		var tmp = positionImg.transform.position;
		tmp.x = Camera.main.ScreenToWorldPoint (Vector3.zero).x + 2;
		positionImg.transform.position = tmp;
		speedMulti = float.Parse(SceneInfo.getValueForKey ("speedMulti"));
		//positionX = tmp.x;

		audioSource = GetComponent<AudioSource> ();
		//Debug.Log( SceneInfo.getValueForKey ("path"));
		path = SceneInfo.getValueForKey ("path");
		resolution = int.Parse(SceneInfo.getValueForKey ("resolution"));
		//notemap = new Dictionary<string, float>();
		setDisplay ();
		loadNotemap ();
		//prepareNote ();

		StartCoroutine(LoadSongCoroutine ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (audioSource.timeSamples == 0) {
				SceneManager.LoadScene ("Selection");
			} else {
				pauseAndDisplayMenu ();
			}
		}

		if (audioSource.isPlaying) {
			//Debug.Log(audioSource.timeSamples);
			renderNotes ();
		} else {
			if (!paused && played) {
				Dictionary<string, string> arg = new Dictionary<string, string> () {
					{"path", path},
					{"perfect", perfect.ToString()},
					{"good", good.ToString()},
					{"bad", bad.ToString()},
					{"miss", miss.ToString()},
					{"score", score.ToString()},
					{"combo", combo.ToString()},
					{"resolution", resolution.ToString()},
				};
				SceneInfo.setParameters (arg);
				SceneManager.LoadScene ("Score");
			}
		}
	}

	void renderNotes() {
		//Debug.Log (speed);
		if (audioSource.timeSamples != 0) {

			var pos1 = this.transform.position;
			pos1.x -= (Time.deltaTime) * speed;
			//Debug.Log (Time.deltaTime * speed);
			//this.transform.position = pos1;


			int current = audioSource.timeSamples / resolution;
			var pos = this.transform.position;
			pos.x = positionImg.transform.position.x - current * 0.1f * speedMulti;
			this.transform.position = pos;
			//Debug.Log("error: " + (pos1.x - pos.x));

		} else {
			var pos = this.transform.position;
			pos.x -= (Time.deltaTime) * speed;
			this.transform.position = pos;

		}

		//var pos = this.transform.position;
		//pos.x -= Time.deltaTime * speed;
		//this.transform.position = pos;
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
					prepareNote(name, position);
				}
			} while (name != null);
			reader.Close ();
		}

		float rightMostX = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width, Screen.height)).x;
		var pos = this.transform.position;
		pos.x = rightMostX;
		this.transform.position = pos;
		distance = rightMostX - positionImg.transform.position.x;

	}

	IEnumerator LoadSongCoroutine() {
		var audioLocation = new WWW ("file://" + path + "/song.ogg");

		yield return audioLocation;

		audioSource.clip = audioLocation.GetAudioClip (false, false);

		//Debug.Log (audioSource.clip.frequency);

		//float t = 0f;

		//Debug.Log (audioSource.clip.frequency);

		timeDelay = distance * 10f * resolution / audioSource.clip.frequency / speedMulti;
		//Debug.Log ("delay" + timeDelay);
		//Debug.Log ("dis" + distance);
		speed = distance / timeDelay;
		//Debug.Log ("speedc " + ((float)audioSource.clip.frequency / (10f * resolution * speedMulti)));
		audioSource.PlayDelayed (timeDelay);
		played = true;
		//audioSource.PlayDelayed (5);
	}

	void prepareNote(string name, float location) {

		float value = location * speedMulti;

		if(name == "A") { 
			GameObject a = (GameObject)Instantiate (noteA);
			a.layer = LayerMask.NameToLayer ("Background Image");
			a.transform.parent = this.transform;
			var tmp = a.transform.localPosition;
			//tmp.x += entry.Value; 
			tmp.x = value; 
			tmp.y = a.transform.position.y;
			tmp.z = 0;
			a.transform.localPosition = tmp;
			a.name = "A";
		} else if (name == "S") { 
			GameObject s = (GameObject)Instantiate (noteS);
			s.layer = LayerMask.NameToLayer ("Background Image");
			s.transform.parent = this.transform;
			var tmp = s.transform.localPosition;
			//tmp.x += entry.Value; 
			tmp.x = value;
			tmp.y = s.transform.position.y;
			tmp.z = 0;
			s.transform.localPosition = tmp;
			s.name = "S";
		} else if (name == "D") { 
			GameObject d = (GameObject)Instantiate (noteD);
			d.layer = LayerMask.NameToLayer ("Background Image");
			d.transform.parent = this.transform;
			var tmp = d.transform.localPosition;
			//tmp.x += entry.Value; 
			tmp.x = value;
			tmp.y = d.transform.position.y;
			tmp.z = 0;
			d.transform.localPosition = tmp;
			d.name = "D";
		} else if (name == "F") { 
			GameObject f = (GameObject)Instantiate (noteF);
			f.layer = LayerMask.NameToLayer ("Background Image");
			f.transform.parent = this.transform;
			var tmp = f.transform.localPosition;
			//tmp.x += entry.Value; 
			tmp.x = value;
			tmp.y = f.transform.position.y;
			tmp.z = 0;
			f.transform.localPosition = tmp;
			f.name = "F";
		} else if (name == "J") { 
			GameObject j = (GameObject)Instantiate (noteJ);
			j.layer = LayerMask.NameToLayer ("Background Image");
			j.transform.parent = this.transform;
			var tmp = j.transform.localPosition;
			//tmp.x += entry.Value; 
			tmp.x = value;
			tmp.y = j.transform.position.y;
			tmp.z = 0;
			j.transform.localPosition = tmp;
			j.name = "J";
		} else if (name == "K") { 
			GameObject k = (GameObject)Instantiate (noteK);
			k.layer = LayerMask.NameToLayer ("Background Image");
			k.transform.parent = this.transform;
			var tmp = k.transform.localPosition;
			//tmp.x += entry.Value; 
			tmp.x = value;
			tmp.y = k.transform.position.y;
			tmp.z = 0;
			k.transform.localPosition = tmp;
			k.name = "K";
		} else if (name == "L") { 
			GameObject l = (GameObject)Instantiate (noteL);
			l.layer = LayerMask.NameToLayer ("Background Image");
			l.transform.parent = this.transform;
			var tmp = l.transform.localPosition;
			//tmp.x += entry.Value; 
			tmp.x = value;
			tmp.y = l.transform.position.y;
			tmp.z = 0;
			l.transform.localPosition = tmp;
			l.name = "L";
		} else if (name == "SC") { 
			GameObject sc = (GameObject)Instantiate (noteSC);
			sc.layer = LayerMask.NameToLayer ("Background Image");
			sc.transform.parent = this.transform;
			var tmp = sc.transform.localPosition;
			//tmp.x += entry.Value; 
			tmp.x = value;
			tmp.y = sc.transform.position.y;
			tmp.z = 0;
			sc.transform.localPosition = tmp;
			sc.name = "SC";
		}

	}

	void pauseAndDisplayMenu() {
		
		paused = true;
		audioSource.Pause ();


		darkImg.gameObject.SetActive (true);
		confirmCanvas.gameObject.SetActive (true);

	}

	public void clickedYes() {
		SceneManager.LoadScene ("Selection");
	}

	public void clickedNo() {
		
		audioSource.Play ();
		paused = false;

		darkImg.gameObject.SetActive (false);
		confirmCanvas.gameObject.SetActive (false);
	}

	public void increasePerfect() {
		this.perfect++;
		this.combo++;
		score += 100 * this.combo;
		scoreTxt.text = score.ToString();
	}

	public void increaseGood() {
		this.good++;
		this.combo++;
		score += 50 * this.combo;
		scoreTxt.text = score.ToString();
	}

	public void increaseBad() {
		this.bad++;
		this.combo = 0;
		score += 30;
		scoreTxt.text = score.ToString();
	}

	public void increaseMiss() {
		this.miss++;
		this.combo = 0;
	}
}
