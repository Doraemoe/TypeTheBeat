using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour {

	public int timestamp;
	public bool alreadyOut = false;
	public bool alreadyChecked = false;
	public List<GameObject> concurrentNotes;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setTimestamp(int t) {
		this.timestamp = t;
	}

	public void setConcurrentNotes(List<GameObject> con) {
		concurrentNotes = con;
	}

	void OnMouseDrag() { 
		//Debug.Log ("drag? " + gameObject.name);
	}
}
