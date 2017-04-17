using System;
using System.Diagnostics;

namespace FP
{
	public interface IFFmpegProcess
	{
		Process ConvertToOGG(string source, string destination);
	}
}

