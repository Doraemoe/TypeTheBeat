using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour {

	public PlayController playCtrl;

	Queue<GameObject> currentNotes;


	float size;

	// Use this for initialization
	void Start () {
		currentNotes = new Queue<GameObject> (3);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			verifyInput ("A");
		} 
		if (Input.GetKeyDown (KeyCode.S)) {
			verifyInput ("S");
		} 
		if (Input.GetKeyDown (KeyCode.D)) {
			verifyInput ("D");
		} 
		if (Input.GetKeyDown (KeyCode.F)) {
			verifyInput ("F");
		} 
		if (Input.GetKeyDown (KeyCode.J)) {
			verifyInput ("J");
		} 
		if (Input.GetKeyDown (KeyCode.K)) {
			verifyInput ("K");
		} 
		if (Input.GetKeyDown (KeyCode.L)) {
			verifyInput ("L");
		} 
		if (Input.GetKeyDown (KeyCode.Semicolon)) {
			verifyInput ("SC");
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if(col.gameObject.tag == "Note")
		{
			//size = col.gameObject.transform.position.x;

			currentNotes.Enqueue (col.gameObject);

			/*
			Debug.Log("in " + col.gameObject.name);
			Debug.Log("pos " + col.gameObject.transform.position);
			Debug.Log("mpos " + this.transform.position);
			BoxCollider2D col2d = GetComponent<BoxCollider2D> ();
			Debug.Log (col2d.bounds.max);
			Debug.Log (col2d.bounds.min);
			Debug.Log(Camera.main.WorldToScreenPoint (col2d.bounds.max));
			Debug.Log(Camera.main.WorldToScreenPoint (col2d.bounds.min));
			*/

		}
	}

	void OnCollisionExit2D(Collision2D col) {
		if(col.gameObject.tag == "Note")
		{
			//size -= col.gameObject.transform.position.x;
			//Debug.Log (size);
				
			GameObject note = currentNotes.Dequeue ();
			playCtrl.increaseMiss ();

			changeNoteColor (note);


			/*
			Debug.Log("out " + col.gameObject.name);
			Debug.Log("pos " + col.gameObject.transform.position);
			Debug.Log("mpos " + this.transform.position);
			*/

		}
	}

	void changeNoteColor(GameObject note) {
		var color = note.GetComponent<Renderer> ().material.color;
		color.a = 0.5f;
		note.GetComponent<Renderer> ().material.color = color;
	}

	void verifyInput(string input) {
		//Debug.Log (input);
		if (currentNotes.Count != 0) {
			GameObject note = currentNotes.Dequeue ();
			if (note.name == input) {
				if (note.transform.position.x >= this.transform.position.x - 0.15
				    && note.transform.position.x <= this.transform.position.x + 0.15) {

					Destroy (note);
					playCtrl.increasePerfect ();

				} else if (note.transform.position.x >= this.transform.position.x - 0.35
				           && note.transform.position.x <= this.transform.position.x + 0.35) {
					Destroy (note);
					playCtrl.increaseGood ();

				} else {
					Destroy (note);
					playCtrl.increaseBad ();

				}
			} else {
				note.GetComponent<BoxCollider2D> ().enabled = false;
				changeNoteColor (note);
				playCtrl.increaseMiss ();
			}

		}
	}
}
