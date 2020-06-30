using System;
using JetBrains.Annotations;

namespace GameRig.Scripts.Utilities.Attributes
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	[MeansImplicitUse]
	public class InitializeOnLaunch : Attribute
	{
		public Type[] Dependencies { get; }

		public InitializeOnLaunch(params Type[] dependencies)
		{
			Dependencies = dependencies;
		}
	}
}