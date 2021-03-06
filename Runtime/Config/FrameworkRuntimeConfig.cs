﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework
{
	[ShowOdinSerializedPropertiesInInspector]
	public class FrameworkRuntimeConfig : ConfigBase
	{
		[LabelText("资源加载类型")]
		public ResType LoadType = ResType.Addressable;

		[FoldoutGroup("热更设置", expanded: true)]
		[HideLabel]
		[InlinePropertyAttribute]
		public ILRConfig ILRConfig = new ILRConfig();

		public enum ResType
		{
			Resources,
			Addressable,
		}
	}

	[Serializable]
	public class ILRConfig
	{
		[LabelText("是否使用热更dll")]
		public bool UseHotFix = false;

		[LabelText("是否使用pbd")]
		public bool UsePbd = true;

		[LabelText("热更dll名字")]
		public string DllName = "Game@hotfix";
	}
}