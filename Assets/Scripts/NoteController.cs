using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour {

	public int timestamp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setTimestamp(int t) {
		this.timestamp = t;
	}

	void OnMouseDrag() { 
		//Debug.Log ("drag? " + gameObject.name);
	}
}
