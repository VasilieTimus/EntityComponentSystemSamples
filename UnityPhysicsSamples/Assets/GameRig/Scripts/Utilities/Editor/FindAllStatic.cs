using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;

namespace GameRig.Scripts.Utilities.Editor
{
	public class FindAllStatic
	{
		private readonly List<StaticClassInfo> allStaticClassInfo = new List<StaticClassInfo>();

		public FindAllStatic()
		{
			IEnumerable<Type> staticObjs = GetAllStaticClasses();
			string[] allProjectPaths = AssetDatabase.GetAllAssetPaths();

			foreach (Type obj in staticObjs)
			{
				foreach (string path in allProjectPaths)
				{
					Match text = Regex.Match(path, obj.Name + ".cs");
					if (!string.IsNullOrEmpty(text.ToString()))
					{
						List<FieldInfo> statClassFields = GetAllStaticFieldsInClass(obj);
						allStaticClassInfo.Add(new StaticClassInfo(path, obj, statClassFields));
					}
				}
			}
		}

		public IEnumerable<StaticClassInfo> GetAllStaticClassesInfo()
		{
			return allStaticClassInfo;
		}

		public static IEnumerable<Type> GetAllStaticClasses()
		{
			return from t in Assembly.Load("Assembly-CSharp").GetTypes().Where(t => t.IsClass && t.IsSealed && t.IsAbstract) select t; // FOR CLR static classes are sealed and abstract
		}

		public static List<FieldInfo> GetAllStaticFieldsInClass(Type classToParse) // perhaps delete later
		{
			IEnumerable<FieldInfo>
				allFields = from t in classToParse.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).Where(t => t.FieldType.IsValueType)
					select t; //Finds public static and constants

			return allFields.ToList();
		}

		public class StaticClassInfo
		{
			public StaticClassInfo(string classPath, Type type, List<FieldInfo> allStatFields)
			{
				Path = classPath;
				ClassType = type;
				AllStaticFields = allStatFields;
				Folded = false;
			}

			public readonly string Path;
			public List<FieldInfo> AllStaticFields;
			public readonly Type ClassType;
			public bool Folded;
		}
	}
}