#if UNITY_STANDALONE_OSX

using System;
using System.Diagnostics;

using UnityEngine;

namespace FP
{
	public class FFmpegProcessMac : IFFmpegProcess
	{
		public Process ConvertToOGG(string source, string destination)
		{
			var ffmpeg = Application.streamingAssetsPath + "/Encoder/ffmpeg";
			Process ffmpegProcess = new Process();
			ffmpegProcess.StartInfo.FileName = ffmpeg;
			ffmpegProcess.StartInfo.Arguments = "-i " + source + " -vn -c:a libvorbis -q:a 5 " + destination + " -y";
			ffmpegProcess.StartInfo.CreateNoWindow = true;
			ffmpegProcess.StartInfo.UseShellExecute = false;
			ffmpegProcess.StartInfo.RedirectStandardOutput = true;
			ffmpegProcess.StartInfo.RedirectStandardError = true;

			return ffmpegProcess;
		}
	}
}

#endif