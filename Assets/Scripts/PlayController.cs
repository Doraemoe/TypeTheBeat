using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayController : MonoBehaviour {

	public Canvas confirmCanvas;
	public Button yesBtn;
	public Button noBtn;
	public Image darkImg;

	string path;
	// Use this for initialization

	void Start () {
		Debug.Log( SceneInfo.getValueForKey ("path"));
		path = SceneInfo.getValueForKey ("path");
		setDisplay ();
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
