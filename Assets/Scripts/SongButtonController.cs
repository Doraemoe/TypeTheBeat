using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SongButtonController : MonoBehaviour {

	public Button songBtn;
	public Text songName;
	public Text songArtist;
	public GameObject selectionObj;

	string path;

	// Use this for initialization
	void Start () {
		selectionObj = GameObject.FindGameObjectWithTag ("GameController");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setup (string name, string artist, string path) {
		songName.text = name;
		songArtist.text = artist;
		this.path = path;
	}

	public void setDisplayAndPlay() {
		setDisplay ();
		playMusic ();
	}

	void setDisplay() {
		if (selectionObj.GetComponent<SelectionController> ().lastSelected != null &&
			selectionObj.GetComponent<SelectionController> ().lastSelected.GetInstanceID() == gameObject.GetInstanceID()) {
			Dictionary<string, string> para = new Dictionary<string, string> ();
			para.Add ("path", path);
			SceneInfo.setParameters (para);
			SceneManager.LoadScene ("Play");
		}
		selectionObj.GetComponent<SelectionController> ().lastSelected = gameObject;

		byte[] FileData;
		string[] files = Directory.GetFiles (path, "bg.*");

		Texture2D tex = new Texture2D(2, 2);
		FileData = File.ReadAllBytes(files[0]);
		tex.LoadImage(FileData);

		GameObject bg = GameObject.FindGameObjectWithTag ("Background");
		bg.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0,0, tex.width, tex.height), new Vector2(0.5f,0.5f));

		if(Screen.height > tex.height || Screen.width > tex.width) {
			float sc;
			if ((float)Screen.height / tex.height > (float)Screen.width / tex.width) {
				sc = (float)Screen.height / tex.height;
			} else {
				sc = (float)Screen.width / tex.width;
			}

			bg.transform.localScale = new Vector3 (sc, sc, 1f);
		}
	}

	void playMusic() {
		SelectionController ctrl = selectionObj.GetComponent<SelectionController> ();
		ctrl.loadAndPlayMusic (path + "/song.ogg");
	}
}
