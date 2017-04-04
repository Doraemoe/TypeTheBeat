using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class SceneInfo {

	//static Dictionary<string, string> parameters = new Dictionary<string, string> ();

	static Dictionary<string, string> parameters = new Dictionary<string, string> (){
		{"path", Application.streamingAssetsPath + "/Songs/Test"},
		{"resolution", "2205"},
		{"speedMulti", "3"},
	};

	public static void SetParameters(Dictionary<string, string> para) {
		parameters = para;
	}

	public static string GetValueForKey(string key) {
		return parameters [key];
	}
}
