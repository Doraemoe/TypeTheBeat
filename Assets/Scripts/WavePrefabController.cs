using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavePrefabController : MonoBehaviour {

	SpriteRenderer waveSprite;
	// Use this for initialization
	void Start () {
		waveSprite = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		var xPoint = Camera.main.WorldToScreenPoint (this.transform.position).x;
		if (xPoint < 0 || xPoint > Camera.main.pixelWidth) {
			//Debug.Log (this.name);
			//this.enabled = false;
			//waveSprite.enabled = false;
		} else {
			//Debug.Log (this.name);
			//this.enabled = true;
			//this.gameObject.SetActive (true);
			//waveSprite.enabled = true;
		}
	}
}
