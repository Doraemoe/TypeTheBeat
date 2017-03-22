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
			//Debug.Log (d);
			//Debug.LogError(d);

			XmlDocument doc = new XmlDocument();
			doc.Load (d + "/meta.xml");

			var nameNode = doc.GetElementsByTagName ("Name");
			//Debug.Log (nameNode[0].InnerText);
			var artistNode = doc.GetElementsByTagName ("Artist");
			//Debug.Log (artistNode[0].InnerText);

			generateBtn (nameNode [0].InnerText, artistNode [0].InnerText, d);

		}
	}

	void generateBtn(string name, string artist, string path) {
		GameObject insBtn = (GameObject)Instantiate (songBtnPrefab);
		insBtn.transform.SetParent (this.transform, false);
		insBtn.name = "btn" + name;

		SongButtonController btnCtrl = insBtn.GetComponent<SongButtonController> ();

		btnCtrl.setup (name, artist, path);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
