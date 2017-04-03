using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;


public static class CDebug {

	//same condition as UNITY_EDITOR as editor is always in DEBUG by design
	[Conditional("DEBUG")]
	public static void Log(object obj) {
		UnityEngine.Debug.Log (obj);
	}
}
