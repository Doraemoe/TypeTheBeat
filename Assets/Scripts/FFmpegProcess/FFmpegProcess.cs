using System;
using System.Diagnostics;

namespace FP
{
	public class FFmpegProcess
	{
		private static IFFmpegProcess _platformWrapper = null;

		private static IFFmpegProcess GetPlatformWrapper ()
		{
			if (_platformWrapper == null) {
				#if UNITY_STANDALONE_OSX
				_platformWrapper = new FFmpegProcessMac ();
				#elif UNITY_STANDALONE_WIN
				_platformWrapper = new FFmpegProcessWindows();
				#elif UNITY_EDITOR
				_platformWrapper = new FFmpegProcessMac();
				#endif
			}
			return _platformWrapper;
		}

		/// <summary>
		/// Converts audio to .ogg
		/// </summary>
		/// <returns>The process to convert audio to ogg</returns>
		/// <param name="source">Source file</param>
		/// <param name="destination">Destination file</param>
		public static Process ConvertToOGG (string source, string destination)
		{
			return GetPlatformWrapper ().ConvertToOGG (source, destination);
		}


	}
}

