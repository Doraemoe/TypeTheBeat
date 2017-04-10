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

	/// <summary>
	/// Set parameters
	/// </summary>
	/// <param name="para">Parameters stored in a dictionary</param>
	public static void SetParameters(Dictionary<string, string> para) {
		parameters = para;
	}

	/// <summary>
	/// Get value in the parameter dictionary
	/// </summary>
	/// <param name="key">key</param>
	public static string GetValueForKey(string key) {
		return parameters [key];
	}
}
