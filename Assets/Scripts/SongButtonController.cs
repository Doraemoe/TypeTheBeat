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

	/// <summary>
	/// Setup information on the button
	/// </summary>
	/// <param name="name">Name of the song</param>
	/// <param name="artist">Artist of the song</param>
	/// <param name="resolution">Resolution data for this song</param>
	public void Setup (string name, string artist, string path, string resolution) {
		songName.text = name;
		songArtist.text = artist;
		this.path = path;
		this.resolution = resolution;
	}

	/// <summary>
	/// Setup background image and play the music
	/// </summary>
	public void SetDisplayAndPlay() {
		
		if (selectionObj.GetComponent<SelectionController> ().lastSelected != null &&
			selectionObj.GetComponent<SelectionController> ().lastSelected.GetInstanceID() == gameObject.GetInstanceID()) {
			Dictionary<string, string> para = new Dictionary<string, string> ();
			para.Add ("path", path);
			para.Add ("resolution", resolution);
			para.Add ("speedMulti", selectionObj.GetComponent<SelectionController>().speedMultiTxt.text);
			SceneInfo.SetParameters (para);
			SceneManager.LoadScene ("Play");
		}
		selectionObj.GetComponent<SelectionController> ().lastSelected = gameObject;

		SetDisplay ();
		PlayMusic ();
	}

	/// <summary>
	/// Setup background image
	/// </summary>
	void SetDisplay() {


		byte[] FileData;
		string[] files = Directory.GetFiles (path, "bg.*");

		Texture2D tex = new Texture2D(2, 2);
		FileData = File.ReadAllBytes(files[0]);
		tex.LoadImage(FileData);

		RawImage backgroundImg = GameObject.FindGameObjectWithTag ("Background").GetComponent<RawImage>();
		backgroundImg.texture = tex;

	}

	/// <summary>
	/// Play the music
	/// </summary>
	void PlayMusic() {
		SelectionController ctrl = selectionObj.GetComponent<SelectionController> ();
		ctrl.LoadAndPlayMusic (path + "/song.ogg");
	}
}
