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
		path = SceneInfo.getValueForKey ("path");
		setDisplay ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setDisplay() {

		byte[] FileData;
		string[] files = Directory.GetFiles (path, "bg.*");

		Texture2D tex = new Texture2D(2, 2);
		FileData = File.ReadAllBytes(files[0]);
		tex.LoadImage(FileData);

		background.texture = tex;

		perfect.text = "Pefect: " + SceneInfo.getValueForKey ("perfect");
		good.text = "Good: " + SceneInfo.getValueForKey ("good");
		bad.text = "Bad: " + SceneInfo.getValueForKey ("bad");
		miss.text = "Miss: " + SceneInfo.getValueForKey ("miss");
		score.text = "Score: " + SceneInfo.getValueForKey ("score");
		combo.text = "Combo:" + Environment.NewLine + SceneInfo.getValueForKey ("combo");
	}

	public void loadScene(string name) {
		SceneManager.LoadScene (name);
	}

	public void retry() {
		Dictionary<string, string> par = new Dictionary<string, string> () {
			{"path", path},
			{"resolution", SceneInfo.getValueForKey("resolution")},
			{"speedMulti", SceneInfo.getValueForKey("speedMulti")},
		};
		SceneInfo.setParameters (par);
		SceneManager.LoadScene ("Play");
	}
}
