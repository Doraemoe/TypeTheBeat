using System.Collections;
using System.Collections.Generic;

public static class SceneInfo {

	static Dictionary<string, string> parameters;

	public static void setParameters(Dictionary<string, string> para) {
		parameters = para;
	}

	public static string getValueForKey(string key) {
		return parameters [key];
	}
}
