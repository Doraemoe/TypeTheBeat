using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class SelectionController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var path = Application.dataPath + "/StreamingAssets/Songs";
		var directories = Directory.GetDirectories(path);

		foreach (var d in directories) {
			Debug.Log (d);
		}

		XmlTextWriter writer = new XmlTextWriter(directories[0] + "/meta.xml", System.Text.Encoding.UTF8);
		writer.WriteStartDocument(true);
		writer.Formatting = Formatting.Indented;
		writer.Indentation = 2;
		writer.WriteStartElement("Song");

		writer.WriteStartElement("Name");
		writer.WriteString("test song");
		writer.WriteEndElement();
		writer.WriteStartElement("Artist");
		writer.WriteString("test artist");
		writer.WriteEndElement();

		writer.WriteEndElement();
		writer.WriteEndDocument();
		writer.Close();

		XmlDocument doc = new XmlDocument();
		doc.Load (directories[0] + "/meta.xml");

		var xmlnode = doc.GetElementsByTagName ("Name");
		Debug.Log (xmlnode[0].InnerText);

		xmlnode = doc.GetElementsByTagName ("Artist");
		Debug.Log (xmlnode[0].InnerText);


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
