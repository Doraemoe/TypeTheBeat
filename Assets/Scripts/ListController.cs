using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class ListController : MonoBehaviour {

	public GameObject songBtnPrefab;
	// Use this for initialization
	void Start () {
		var path = Application.streamingAssetsPath + "/Songs";
		var directories = Directory.GetDirectories(path);

		foreach (var d in directories) {

			XmlDocument doc = new XmlDocument();
			doc.Load (d + "/meta.xml");

			var nameNode = doc.GetElementsByTagName ("Name");
			var artistNode = doc.GetElementsByTagName ("Artist");
			var resolutionNode = doc.GetElementsByTagName("Resolution");

			GenerateBtn (nameNode [0].InnerText, artistNode [0].InnerText, d, resolutionNode[0].InnerText);

		}
	}

	/// <summary>
	/// Generate button on screen
	/// </summary>
	/// <param name="name">Name of the button</param>
	/// <param name="artist">Artist display on the button</param>
	/// <param name="path">Path to the song file</param>
	/// <param name="resolution">Resolution data for the song file</param>
	void GenerateBtn(string name, string artist, string path, string resolution) {
		GameObject insBtn = (GameObject)Instantiate (songBtnPrefab);
		insBtn.transform.SetParent (this.transform, false);
		insBtn.name = "btn" + name;

		SongButtonController btnCtrl = insBtn.GetComponent<SongButtonController> ();

		btnCtrl.Setup (name, artist, path, resolution);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
