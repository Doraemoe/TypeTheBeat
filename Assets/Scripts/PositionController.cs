using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour {

	public PlayController playCtrl;

	public Queue<GameObject> currentNotes;


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

			currentNotes.Enqueue (col.gameObject);

		}
	}

	void OnCollisionExit2D(Collision2D col) {
		if(col.gameObject.tag == "Note")
		{
			if(col.gameObject.GetComponent<NoteController> ().alreadyOut) {
				if (col.gameObject.GetComponent<NoteController> ().alreadyChecked) {
					return;
				} else {
					missNote (col.gameObject);
				}
			}

			if (currentNotes.Count != 0) {
				GameObject note = currentNotes.Dequeue ();
				if (note.GetComponent<NoteController> ().alreadyChecked) {
					return;
				} else {
					missNote (note);
				}
			}
		}
	}

	void changeNoteColor(GameObject note) {
		var color = note.GetComponent<Renderer> ().material.color;
		color.a = 0.5f;
		note.GetComponent<SpriteRenderer> ().material.color = color;
	}

	void verifyInput(string input) {
		if (currentNotes.Count != 0) {
			
			GameObject note = currentNotes.Dequeue ();
			note.GetComponent<NoteController> ().alreadyOut = true;

			if (note.name == input) {
				detectAcc (note);
			} else {
				List<GameObject> concurrentNotes = note.GetComponent<NoteController> ().concurrentNotes;
				if (concurrentNotes.Count > 1) {
					foreach(GameObject otherNote in concurrentNotes) {
						checkConcurrent (otherNote, input);
					}
				} else {
					note.GetComponent<NoteController> ().alreadyChecked = true;
					missNote (note);
				}
			}

		}
	}


	void checkConcurrent(GameObject note, string input) {
		if (note.GetComponent<NoteController> ().alreadyChecked) {
			return;
		} 

		if (note.name == input) {
			detectAcc (note);
		}
	}

	void detectAcc(GameObject note) {
		if (note.transform.position.x >= this.transform.position.x - 0.15
			&& note.transform.position.x <= this.transform.position.x + 0.15) {

			removeNote (note);
			playCtrl.increasePerfect ();

		} else if (note.transform.position.x >= this.transform.position.x - 0.35
			&& note.transform.position.x <= this.transform.position.x + 0.35) {
			removeNote (note);
			playCtrl.increaseGood ();

		} else {
			removeNote (note);
			playCtrl.increaseBad ();

		}
	}

	void missNote(GameObject note) {
		changeNoteColor (note);
		playCtrl.increaseMiss ();
	}

	void removeNote(GameObject note) {
		note.GetComponent<NoteController> ().alreadyChecked = true;
		note.GetComponent<SpriteRenderer> ().enabled = false;
	}
}
