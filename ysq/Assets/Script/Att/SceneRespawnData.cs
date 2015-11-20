using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Att
{
	public class SceneRespawnData
	{
		public List<Respawn> Respawn
		{
			get;
			set;
		}

		public double BPx
		{
			get;
			set;
		}

		public double BPy
		{
			get;
			set;
		}

		public double BPz
		{
			get;
			set;
		}

		public double BRy
		{
			get;
			set;
		}

		public static SceneRespawnData LoadFromFile(string assetFileName)
		{
			TextAsset textAsset = Res.Load<TextAsset>(assetFileName, false);
			if (textAsset == null)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("Res.Load SceneRespawnData {0} error", assetFileName)
				});
				return null;
			}
			return SceneRespawnData.LoadFromJson(textAsset.text);
		}

		public static SceneRespawnData LoadFromJson(string jsonData)
		{
			SceneRespawnData result = null;
			try
			{
				result = JsonMapper.ToObject<SceneRespawnData>(jsonData);
			}
			catch (Exception ex)
			{
				global::Debug.LogError(new object[]
				{
					string.Format("Load SceneRespawnData error, {0}", ex.Message)
				});
			}
			return result;
		}
	}
}
