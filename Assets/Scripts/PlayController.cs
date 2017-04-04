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
	int samplesDelay;
	float distance;
	float timeDelay;
	float speed;
	float speedMulti = 3f;
	//float lastPosX = 0f;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {

		var tmp = positionImg.transform.position;
		tmp.x = Camera.main.ScreenToWorldPoint (Vector3.zero).x + 2;
		positionImg.transform.position = tmp;
		speedMulti = float.Parse(SceneInfo.GetValueForKey ("speedMulti"));

		audioSource = GetComponent<AudioSource> ();
		path = SceneInfo.GetValueForKey ("path");
		resolution = int.Parse(SceneInfo.GetValueForKey ("resolution"));
		SetDisplay ();
		LoadNotemap ();

		StartCoroutine(LoadSongCoroutine ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (audioSource.timeSamples == 0) {
				SceneManager.LoadScene ("Selection");
			} else {
				PauseAndDisplayMenu ();
			}
		}

		if (audioSource.isPlaying) {
			RenderNotes();
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
				SceneInfo.SetParameters (arg);
				SceneManager.LoadScene ("Score");
			}
		}
	}

	/*
	void renderNotes() {
		if(audioSource.timeSamples >= samplesDelay) {
			CDebug.Log("started play");
		}

		if (audioSource.timeSamples != 0) {

			float current = (float)audioSource.timeSamples / resolution;
			var pos = this.transform.position;
			pos.x = positionImg.transform.position.x - current * 0.1f * speedMulti;

			if(lastPosX != pos.x) {
				this.transform.position = pos;
				//this.transform.Translate(Vector3.left);
			} else {
				pos.x -= (Time.deltaTime) * speed;
				this.transform.position = pos;

			}

			lastPosX = this.transform.position.x;

		} else {

			this.transform.Translate (Vector3.left * speed * Time.deltaTime);
			//var pos = this.transform.position;
			//pos.x -= (Time.deltaTime) * speed;
			//this.transform.position = pos;
			lastPosX = this.transform.position.x;
		}
	}
	*/

	void RenderNotes() {
		
		int delta = audioSource.timeSamples - samplesDelay;

		float current = (float)delta / resolution;
		var pos = this.transform.position;
		pos.x = positionImg.transform.position.x - current * 0.1f * speedMulti;
		this.transform.position = pos;
	}

	public void GoBack () {
		SceneManager.LoadScene ("Selection");
	}

	void SetDisplay() {

		byte[] FileData;
		string[] files = Directory.GetFiles (path, "bg.*");

		Texture2D tex = new Texture2D(2, 2);
		FileData = File.ReadAllBytes(files[0]);
		tex.LoadImage(FileData);

		RawImage backgroundImg = GameObject.FindGameObjectWithTag ("Background").GetComponent<RawImage> ();;

		backgroundImg.texture = tex;
	}

	void LoadNotemap() {
		string tmp;
		List<GameObject> concurrentNotes;
		string[] names;
		float position;

		var reader = new StreamReader (path + "/song.notemap");

		using (reader) {
			do {
				tmp = reader.ReadLine();
				if(tmp != null) {
					//concurrent = int.Parse(tmp);
					//According to the document, if the separator parameter is null or contains no characters, white-space characters are assumed to be the delimiters.
					names = tmp.Trim().Split(null);
					position = float.Parse(reader.ReadLine(), CultureInfo.InvariantCulture.NumberFormat);

					concurrentNotes = new List<GameObject>(names.Length);

					foreach(string name in names) {
						concurrentNotes.Add(PrepareNote(name, position));
					}

					foreach (GameObject note in concurrentNotes) {
						note.GetComponent<NoteController>().SetConcurrentNotes(concurrentNotes);
					}


				}
			} while (tmp != null);
			reader.Close ();
		}

		float rightMostX = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width, Screen.height)).x;
		var pos = this.transform.position;
		pos.x = rightMostX + 2;
		this.transform.position = pos;
		distance = rightMostX - positionImg.transform.position.x;



	}
	/*
	IEnumerator LoadSongCoroutine() {
		var audioLocation = new WWW ("file://" + path + "/song.ogg");

		yield return audioLocation;

		audioSource.clip = audioLocation.GetAudioClip (false, false);

		timeDelay = distance * 10f * resolution / audioSource.clip.frequency / speedMulti;
		speed = distance / timeDelay;
		audioSource.PlayDelayed (timeDelay);
		played = true;
	}
	*/

	IEnumerator LoadSongCoroutine() {
		var audioLocation = new WWW ("file://" + path + "/song.ogg");

		yield return audioLocation;

		AudioClip oldClip = audioLocation.GetAudioClip (false, false);
		float[] oldSamples = new float[oldClip.samples * oldClip.channels];
		oldClip.GetData(oldSamples, 0);


		samplesDelay = Mathf.CeilToInt(distance) * 10 * resolution / oldClip.frequency / Mathf.CeilToInt(speedMulti) * oldClip.frequency * oldClip.channels;
		AudioClip newClip = AudioClip.Create ("song", oldClip.samples + samplesDelay, oldClip.channels, oldClip.frequency, false);

		newClip.SetData(oldSamples, samplesDelay);

		audioSource.clip = newClip;

		audioSource.Play ();
		played = true;
	}


	void SetupNote(GameObject obj, string name, float valueX) {
		obj.layer = LayerMask.NameToLayer ("Background Image");
		obj.transform.parent = this.transform;
		var tmp = obj.transform.localPosition;
		tmp.x = valueX; 
		tmp.y = obj.transform.position.y;
		tmp.z = 0;
		obj.transform.localPosition = tmp;
		obj.name = name;
	}

	GameObject PrepareNote(string name, float location) {

		float value = location * speedMulti;

		if(name == "A") { 
			GameObject a = (GameObject)Instantiate (noteA);
			SetupNote (a, "A", value);
			return a;
		} else if (name == "S") { 
			GameObject s = (GameObject)Instantiate (noteS);
			SetupNote (s, "S", value);
			return s;
		} else if (name == "D") { 
			GameObject d = (GameObject)Instantiate (noteD);
			SetupNote (d, "D", value);
			return d;
		} else if (name == "F") { 
			GameObject f = (GameObject)Instantiate (noteF);
			SetupNote (f, "F", value);
			return f;
		} else if (name == "J") { 
			GameObject j = (GameObject)Instantiate (noteJ);
			SetupNote (j, "J", value);
			return j;
		} else if (name == "K") { 
			GameObject k = (GameObject)Instantiate (noteK);
			SetupNote (k, "K", value);
			return k;
		} else if (name == "L") { 
			GameObject l = (GameObject)Instantiate (noteL);
			SetupNote (l, "L", value);
			return l;
		} else if (name == "SC") { 
			GameObject sc = (GameObject)Instantiate (noteSC);
			SetupNote (sc, "SC", value);
			return sc;
		}

		return null;

	}

	void PauseAndDisplayMenu() {
		
		paused = true;
		audioSource.Pause ();


		darkImg.gameObject.SetActive (true);
		confirmCanvas.gameObject.SetActive (true);

	}

	public void ClickedYes() {
		SceneManager.LoadScene ("Selection");
	}

	public void ClickedNo() {
		
		audioSource.Play ();
		paused = false;

		darkImg.gameObject.SetActive (false);
		confirmCanvas.gameObject.SetActive (false);
	}

	public void IncreasePerfect() {
		this.perfect++;
		this.combo++;
		score += Constants.kPerfectScore * this.combo;
		scoreTxt.text = score.ToString();
	}

	public void IncreaseGood() {
		this.good++;
		this.combo++;
		score += Constants.kGoodScore * this.combo;
		scoreTxt.text = score.ToString();
	}

	public void IncreaseBad() {
		this.bad++;
		if (combo >= maxCombo) {
			maxCombo = combo;
		}
		this.combo = 0;
		score += Constants.kBadScore;
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
