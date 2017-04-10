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

	/// <summary>
	/// Set timestap for the note
	/// </summary>
	public void SetTimestamp(int t) {
		this.timestamp = t;
	}

	/// <summary>
	/// Set all othernotes appear at the same time with this note
	/// </summary>
	public void SetConcurrentNotes(List<GameObject> con) {
		concurrentNotes = con;
	}

	void OnMouseDrag() { 
		//Debug.Log ("drag? " + gameObject.name);
	}
}
