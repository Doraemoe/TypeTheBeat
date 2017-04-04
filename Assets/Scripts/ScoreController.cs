using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour {

	public RawImage background;
	public Text perfect;
	public Text good;
	public Text bad;
	public Text miss;
	public Text score;
	public Text combo;

	string path;
	// Use this for initialization
	void Start () {
		path = SceneInfo.GetValueForKey ("path");
		SetDisplay ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetDisplay() {

		byte[] FileData;
		string[] files = Directory.GetFiles (path, "bg.*");

		Texture2D tex = new Texture2D(2, 2);
		FileData = File.ReadAllBytes(files[0]);
		tex.LoadImage(FileData);

		background.texture = tex;

		perfect.text = "Pefect: " + SceneInfo.GetValueForKey ("perfect");
		good.text = "Good: " + SceneInfo.GetValueForKey ("good");
		bad.text = "Bad: " + SceneInfo.GetValueForKey ("bad");
		miss.text = "Miss: " + SceneInfo.GetValueForKey ("miss");
		score.text = "Score: " + SceneInfo.GetValueForKey ("score");
		combo.text = "Combo:" + Environment.NewLine + SceneInfo.GetValueForKey ("combo");
	}

	public void LoadScene(string name) {
		SceneManager.LoadScene (name);
	}

	public void Retry() {
		Dictionary<string, string> par = new Dictionary<string, string> () {
			{"path", path},
			{"resolution", SceneInfo.GetValueForKey("resolution")},
			{"speedMulti", SceneInfo.GetValueForKey("speedMulti")},
		};
		SceneInfo.SetParameters (par);
		SceneManager.LoadScene ("Play");
	}
}
