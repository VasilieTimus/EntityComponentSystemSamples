using System;
using SRDebugger;
using UnityEditor;

namespace GameRig.Scripts.Utilities.Editor
{
	// ReSharper disable once UnusedType.Global
	public static class JenkinsBuildProcess
	{
		public static void PerformBuild()
		{
			string[] args = Environment.GetCommandLineArgs();

			string buildPath = args[7];

			Enum.TryParse(args[8], true, out BuildTarget buildTarget);

			PlayerSettings.SplashScreen.show = false;

			if (args.Length > 10)
			{
				string bundleVersion = args[9];
				string buildNumber = args[10];

				PlayerSettings.bundleVersion = bundleVersion;
				PlayerSettings.iOS.buildNumber = buildNumber;

				Settings.Instance.IsEnabled = false;
			}

			BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath, buildTarget, BuildOptions.None);
		}
	}
}