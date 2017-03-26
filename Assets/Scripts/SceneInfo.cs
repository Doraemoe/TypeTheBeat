using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class SceneInfo {

	static Dictionary<string, string> parameters = new Dictionary<string, string> (){
		{"path", Application.streamingAssetsPath + "/Songs/Test"},
		{"resolution", "2205"},
	};

	public static void setParameters(Dictionary<string, string> para) {
		parameters = para;
	}

	public static string getValueForKey(string key) {
		return parameters [key];
	}
}
