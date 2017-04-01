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
	string resolution;

	// Use this for initialization
	void Start () {
		selectionObj = GameObject.FindGameObjectWithTag ("GameController");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setup (string name, string artist, string path, string resolution) {
		songName.text = name;
		songArtist.text = artist;
		this.path = path;
		this.resolution = resolution;
	}

	public void setDisplayAndPlay() {
		
		if (selectionObj.GetComponent<SelectionController> ().lastSelected != null &&
			selectionObj.GetComponent<SelectionController> ().lastSelected.GetInstanceID() == gameObject.GetInstanceID()) {
			Dictionary<string, string> para = new Dictionary<string, string> ();
			para.Add ("path", path);
			para.Add ("resolution", resolution);
			para.Add ("speedMulti", selectionObj.GetComponent<SelectionController>().speedMultiTxt.text);
			SceneInfo.setParameters (para);
			SceneManager.LoadScene ("Play");
		}
		selectionObj.GetComponent<SelectionController> ().lastSelected = gameObject;

		setDisplay ();
		playMusic ();
	}

	void setDisplay() {


		byte[] FileData;
		string[] files = Directory.GetFiles (path, "bg.*");

		Texture2D tex = new Texture2D(2, 2);
		FileData = File.ReadAllBytes(files[0]);
		tex.LoadImage(FileData);

		RawImage backgroundImg = GameObject.FindGameObjectWithTag ("Background").GetComponent<RawImage>();
		backgroundImg.texture = tex;

	}

	void playMusic() {
		SelectionController ctrl = selectionObj.GetComponent<SelectionController> ();
		ctrl.loadAndPlayMusic (path + "/song.ogg");
	}
}
