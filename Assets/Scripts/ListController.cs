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

			generateBtn (nameNode [0].InnerText, artistNode [0].InnerText, d, resolutionNode[0].InnerText);

		}
	}

	void generateBtn(string name, string artist, string path, string resolution) {
		GameObject insBtn = (GameObject)Instantiate (songBtnPrefab);
		insBtn.transform.SetParent (this.transform, false);
		insBtn.name = "btn" + name;

		SongButtonController btnCtrl = insBtn.GetComponent<SongButtonController> ();

		btnCtrl.setup (name, artist, path, resolution);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
