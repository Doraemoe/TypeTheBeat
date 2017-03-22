using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Debug.Log( SceneInfo.getValueForKey ("path"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void goBack () {
		SceneManager.LoadScene ("Selection");
	}
}
