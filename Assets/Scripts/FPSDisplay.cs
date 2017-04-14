using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour {

	public Text fpsDisplay;

	int frameCount = 0;
	float dt = 0.0f;
	int fps = 0;
	int updateRate = 2;  // 4 updates per sec.

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		frameCount++;
		dt += Time.deltaTime;
		if (dt > 1.0f/updateRate)
		{
			fps = Mathf.FloorToInt( frameCount / dt );
			fpsDisplay.text = "FPS: " + fps;
			frameCount = 0;
			dt -= 1.0f/updateRate;
		}
	}
}
