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
	int maxCombo = 0;
	float distance;
	float timeDelay;
	float speed;
	float speedMulti = 3f;
	float lastPosX = 0f;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {

		var tmp = positionImg.transform.position;
		tmp.x = Camera.main.ScreenToWorldPoint (Vector3.zero).x + 2;
		positionImg.transform.position = tmp;
		speedMulti = float.Parse(SceneInfo.getValueForKey ("speedMulti"));

		audioSource = GetComponent<AudioSource> ();
		path = SceneInfo.getValueForKey ("path");
		resolution = int.Parse(SceneInfo.getValueForKey ("resolution"));
		setDisplay ();
		loadNotemap ();

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
			renderNotes ();
		} else {
			if (!paused && played) {
				if (combo >= maxCombo) {
					maxCombo = combo;
				}
				Dictionary<string, string> arg = new Dictionary<string, string> () {
					{"path", path},
					{"perfect", perfect.ToString()},
					{"good", good.ToString()},
					{"bad", bad.ToString()},
					{"miss", miss.ToString()},
					{"score", score.ToString()},
					{"combo", maxCombo.ToString()},
					{"resolution", resolution.ToString()},
					{"speedMulti", speedMulti.ToString()},
				};
				SceneInfo.setParameters (arg);
				SceneManager.LoadScene ("Score");
			}
		}
	}

	void renderNotes() {
		if (audioSource.timeSamples != 0) {

			float current = (float)audioSource.timeSamples / resolution;
			var pos = this.transform.position;
			pos.x = positionImg.transform.position.x - current * 0.1f * speedMulti;

			if(lastPosX != pos.x) {
				this.transform.position = pos;
			} else {
				pos.x -= (Time.deltaTime) * speed;
				this.transform.position = pos;

			}

			lastPosX = this.transform.position.x;

		} else {
			var pos = this.transform.position;
			pos.x -= (Time.deltaTime) * speed;
			this.transform.position = pos;
			lastPosX = pos.x;
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

		timeDelay = distance * 10f * resolution / audioSource.clip.frequency / speedMulti;
		speed = distance / timeDelay;
		audioSource.PlayDelayed (timeDelay);
		played = true;
	}

	void setupNote(GameObject obj, string name, float valueX) {
		obj.layer = LayerMask.NameToLayer ("Background Image");
		obj.transform.parent = this.transform;
		var tmp = obj.transform.localPosition;
		tmp.x = valueX; 
		tmp.y = obj.transform.position.y;
		tmp.z = 0;
		obj.transform.localPosition = tmp;
		obj.name = name;
	}

	void prepareNote(string name, float location) {

		float value = location * speedMulti;

		if(name == "A") { 
			GameObject a = (GameObject)Instantiate (noteA);
			setupNote (a, "A", value);
		} else if (name == "S") { 
			GameObject s = (GameObject)Instantiate (noteS);
			setupNote (s, "S", value);
		} else if (name == "D") { 
			GameObject d = (GameObject)Instantiate (noteD);
			setupNote (d, "D", value);
		} else if (name == "F") { 
			GameObject f = (GameObject)Instantiate (noteF);
			setupNote (f, "F", value);
		} else if (name == "J") { 
			GameObject j = (GameObject)Instantiate (noteJ);
			setupNote (j, "J", value);
		} else if (name == "K") { 
			GameObject k = (GameObject)Instantiate (noteK);
			setupNote (k, "K", value);
		} else if (name == "L") { 
			GameObject l = (GameObject)Instantiate (noteL);
			setupNote (l, "L", value);
		} else if (name == "SC") { 
			GameObject sc = (GameObject)Instantiate (noteSC);
			setupNote (sc, "SC", value);
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
		if (combo >= maxCombo) {
			maxCombo = combo;
		}
		this.combo = 0;
		score += 30;
		scoreTxt.text = score.ToString();
	}

	public void increaseMiss() {
		this.miss++;
		if (combo >= maxCombo) {
			maxCombo = combo;
		}
		this.combo = 0;
	}
}
