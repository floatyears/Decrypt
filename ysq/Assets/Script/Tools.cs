using Att;
using PigeonCoopToolkit.Effects.Trails;
using Proto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;

public static class Tools
{
	public enum CustomQualityLevel
	{
		Default,
		Fantastic,
		Simple,
		Fastest,
		MAX
	}

	private static int SortSameSocketIndex;

	public static float WrapAngle(float a)
	{
		while (a < -180f)
		{
			a += 360f;
		}
		while (a > 180f)
		{
			a -= 360f;
		}
		return a;
	}

	public static Bounds CalculateWorldBounds(Transform transform)
	{
		Collider collider = transform.collider;
		Bounds result = (!(collider != null)) ? new Bounds(transform.position, Vector3.zero) : collider.bounds;
		Collider[] componentsInChildren = transform.GetComponentsInChildren<Collider>();
		Collider[] array = componentsInChildren;
		for (int i = 0; i < array.Length; i++)
		{
			Collider collider2 = array[i];
			result.Encapsulate(collider2.bounds);
		}
		return result;
	}

	public static Bounds CalculateLocalBounds(Transform transform)
	{
		Bounds bounds = Tools.CalculateWorldBounds(transform);
		return new Bounds(bounds.min - transform.position, bounds.max - transform.position);
	}

	public static float DistanceToPlane(Vector3 planeOrigin, Vector3 planeNormal, Vector3 rayOrigin, Vector3 rayNormal)
	{
		float num = Vector3.Dot(planeOrigin, planeNormal);
		float num2 = Vector3.Dot(planeNormal, rayOrigin);
		float num3 = Vector3.Dot(planeNormal, rayNormal);
		return -((num2 + num) / num3);
	}

	public static int EncodeFloatToInt(float val)
	{
		int num = Mathf.RoundToInt(val);
		if (num < 0)
		{
			num = -num;
			num <<= 1;
			num |= 1;
		}
		else
		{
			num <<= 1;
		}
		return num;
	}

	public static float Ramp(float val, float factor)
	{
		float num = Mathf.Sign(val);
		val *= num;
		return num * Mathf.Lerp(val, val * val, factor);
	}

	public static bool Deviates(float a, float b, float units)
	{
		return Mathf.Abs(a - b) > units;
	}

	public static bool Deviates(Vector3 a, Vector3 b, float units)
	{
		Vector3 vector = a - b;
		return Mathf.Abs(vector.x) > units || Mathf.Abs(vector.y) > units || Mathf.Abs(vector.z) > units;
	}

	public static int GetPositionID(Vector3 pos)
	{
		return Tools.EncodeFloatToInt(pos.x) << 16 | Tools.EncodeFloatToInt(pos.z);
	}

	public static bool IsChild(Transform parent, Transform child)
	{
		if (parent == null || child == null)
		{
			return false;
		}
		while (child != null)
		{
			if (child == parent)
			{
				return true;
			}
			child = child.parent;
		}
		return false;
	}

	public static GameObject AddChild(GameObject parent, GameObject prefab)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab) as GameObject;
		if (gameObject != null && parent != null)
		{
			Transform transform = gameObject.transform;
			transform.parent = parent.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
			gameObject.layer = parent.layer;
		}
		return gameObject;
	}

	public static string GetHierarchy(GameObject obj)
	{
		string text = "/" + obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			text = "/" + obj.name + text;
		}
		return text;
	}

	public static T GetSafeComponent<T>(GameObject go) where T : Component
	{
		T t = go.GetComponent<T>();
		if (t == null)
		{
			t = go.AddComponent<T>();
		}
		return t;
	}

	public static T GetSafeComponentInChildren<T>(GameObject go) where T : Component
	{
		T t = go.GetComponentInChildren<T>();
		if (t == null)
		{
			t = go.AddComponent<T>();
		}
		return t;
	}

	public static Rigidbody GetRigidbody(Transform trans)
	{
		Rigidbody rigidbody = null;
		while (rigidbody == null && trans != null)
		{
			rigidbody = trans.rigidbody;
			trans = trans.parent;
		}
		return rigidbody;
	}

	public static List<Rigidbody> GetRigidbodies(Collider[] cols)
	{
		List<Rigidbody> list = new List<Rigidbody>();
		for (int i = 0; i < cols.Length; i++)
		{
			Collider collider = cols[i];
			if (collider != null)
			{
				Rigidbody rigidbody = Tools.GetRigidbody(collider.transform);
				if (rigidbody != null && !list.Contains(rigidbody))
				{
					list.Add(rigidbody);
				}
			}
		}
		return list;
	}

	public static void Broadcast(string funcName)
	{
		GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		GameObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			GameObject gameObject = array2[i];
			gameObject.SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
		}
	}

	public static void SetRenderEnable(GameObject go, bool flag)
	{
		MeshRenderer x = go.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
		if (x != null)
		{
			go.renderer.enabled = flag;
		}
		for (int i = 0; i < go.transform.childCount; i++)
		{
			GameObject gameObject = go.transform.GetChild(i).gameObject;
			Tools.SetRenderEnable(gameObject, flag);
		}
	}

	public static bool IsWebAddress(string s)
	{
		return s.StartsWith("http:", StringComparison.OrdinalIgnoreCase) || s.StartsWith("file:", StringComparison.OrdinalIgnoreCase);
	}

	public static string GetFullPath(string s)
	{
		string text = Application.dataPath;
		if (!text.EndsWith("/"))
		{
			text += "/";
		}
		text += s;
		text = text.Replace("\\", "/");
		while (true)
		{
			int num = text.IndexOf("/../");
			if (num == -1)
			{
				break;
			}
			string text2 = text.Remove(num);
			string str = text.Substring(num + 4);
			num = text2.LastIndexOf('/');
			if (num == -1)
			{
				break;
			}
			text2 = text2.Remove(num);
			text = text2 + "/" + str;
		}
		return text;
	}

	public static int GetLength(string str)
	{
		if (str == null || str.Length == 0)
		{
			return 0;
		}
		int length = str.Length;
		int num = length;
		for (int i = 0; i < length; i++)
		{
			if (str[i] > '\u0080')
			{
				num++;
			}
		}
		return num;
	}

	public static int GetLength_Obsoleted(string str)
	{
		if (str == null || str.Length == 0)
		{
			return 0;
		}
		byte[] bytes = Encoding.Default.GetBytes(str);
		return bytes.Length;
	}

	public static string GetCallStack()
	{
		StackTrace stackTrace = new StackTrace(1, true);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < stackTrace.FrameCount; i++)
		{
			StackFrame frame = stackTrace.GetFrame(i);
			if (i != 0)
			{
				stringBuilder.Append("  ");
			}
			stringBuilder.Append(frame.GetMethod());
			string fileName = frame.GetFileName();
			if (!string.IsNullOrEmpty(fileName))
			{
				stringBuilder.Append(" (").Append(fileName).Append(": ").Append(frame.GetFileLineNumber()).Append(")");
			}
			stringBuilder.AppendLine();
		}
		return stringBuilder.ToString();
	}

	public static long CurrTimeStamp()
	{
		return (DateTime.UtcNow.Ticks - 621355968000000000L) / 10000L;
	}

	public static string TimeStampFormat(int timeStamp)
	{
		long ticks = (long)timeStamp * 10000000L + 621355968000000000L;
		return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(ticks)).ToString("yyyy-MM-dd HH:mm:ss");
	}

	public static DateTime ServerDateTime(int serverTimeStamp)
	{
		long ticks = (long)serverTimeStamp * 10000000L + 630822816000000000L;
		return new DateTime(ticks);
	}

	public static string ServerDateTimeFormat1(int serverTimeStamp)
	{
		return Tools.ServerDateTime(serverTimeStamp).ToString("HH:mm:ss");
	}

	public static string ServerDateTimeFormat2(int serverTimeStamp)
	{
		return Tools.ServerDateTime(serverTimeStamp).ToString("yyyy-MM-dd");
	}

	public static string ServerDateTimeFormat3(int serverTimeStamp)
	{
		return Tools.ServerDateTime(serverTimeStamp).ToString(Singleton<StringManager>.Instance.GetString("timeTxt4", new object[]
		{
			"MM",
			"dd"
		}));
	}

	public static string ServerDateTimeFormat4(int serverTimeStamp)
	{
		return Tools.ServerDateTime(serverTimeStamp).ToString("HH:mm");
	}

	public static string ServerDateTimeFormat5(int serverTimeStamp)
	{
		return Tools.ServerDateTime(serverTimeStamp).ToString("MM-dd");
	}

	public static string ServerDateTimeFormat6(int serverTimeStamp)
	{
		return Tools.ServerDateTime(serverTimeStamp).ToString(Singleton<StringManager>.Instance.GetString("timeTxt5", new object[]
		{
			"MM",
			"dd"
		}));
	}

	public static string FormatTimeStr(int timecount, bool isBefore = false, bool isAfter = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = timecount / 86400;
		if (num != 0)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt3", new object[]
			{
				num
			}));
		}
		int num2 = timecount % 86400 / 3600;
		if (num2 != 0)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt2", new object[]
			{
				num2
			}));
		}
		int num3 = timecount % 86400 % 3600 / 60;
		if (num3 != 0)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt1", new object[]
			{
				num3
			}));
		}
		int num4 = timecount % 86400 % 3600 % 60;
		if (num4 != 0)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt0", new object[]
			{
				num4
			}));
		}
		if (isBefore)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt10"));
		}
		if (isAfter)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt11"));
		}
		return stringBuilder.ToString();
	}

	public static string FormatTimeStr2(int timecount, bool isBefore = false, bool isAfter = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = timecount / 86400;
		if (num != 0)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt3", new object[]
			{
				num
			}));
		}
		int num2 = timecount % 86400 / 3600;
		if (num2 != 0 || num != 0)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt6", new object[]
			{
				string.Format("{0:D2}", num2)
			}));
		}
		int num3 = timecount % 86400 % 3600 / 60;
		if (num3 != 0 || num2 != 0 || num != 0)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt1", new object[]
			{
				string.Format("{0:D2}", num3)
			}));
		}
		int num4 = timecount % 86400 % 3600 % 60;
		if (num4 != 0 || num3 != 0 || num2 != 0 || num != 0)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt0", new object[]
			{
				string.Format("{0:D2}", num4)
			}));
		}
		if (isBefore)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt10"));
		}
		if (isAfter)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt11"));
		}
		return stringBuilder.ToString();
	}

	public static string FormatTimeStr3(int timecount, bool isBefore = false, bool isAfter = false)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = timecount / 86400;
		if (num != 0)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt3", new object[]
			{
				num
			}));
		}
		else
		{
			int num2 = timecount % 86400 / 3600;
			if (num2 != 0)
			{
				stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt6", new object[]
				{
					string.Format("{0}", num2)
				}));
			}
			else
			{
				int num3 = timecount % 86400 % 3600 / 60;
				if (num3 != 0)
				{
					stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt1", new object[]
					{
						string.Format("{0}", num3)
					}));
				}
				else
				{
					int num4 = timecount % 86400 % 3600 % 60;
					if (num4 != 0)
					{
						stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt0", new object[]
						{
							string.Format("{0}", num4)
						}));
					}
				}
			}
		}
		if (isBefore)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt10"));
		}
		if (isAfter)
		{
			stringBuilder.Append(Singleton<StringManager>.Instance.GetString("timeTxt11"));
		}
		return stringBuilder.ToString();
	}

	public static string GetGuildApplyAutoTimeStr()
	{
		DateTime dateTime = Tools.ServerDateTime(Globals.Instance.Player.GetTimeStamp());
		int timecount;
		if (dateTime.Hour < 10)
		{
			timecount = 39600 - (dateTime.Hour * 60 * 60 + dateTime.Minute * 60);
		}
		else
		{
			timecount = (60 - dateTime.Minute) * 60;
		}
		return Tools.FormatTimeStr(timecount, false, false);
	}

	public static int GetRemainAARewardTime(int AATimeStamp)
	{
		if (AATimeStamp == 0)
		{
			return 0;
		}
		int num = AATimeStamp - Globals.Instance.Player.GetTimeStamp();
		if (num <= 0)
		{
			return 0;
		}
		return num;
	}

	public static int GetRemainTakeSevenDayRewardTime()
	{
		LocalPlayer player = Globals.Instance.Player;
		if (player.ActivitySystem.WorldOpenTimeStamp == 0)
		{
			return 0;
		}
		int num = player.ActivitySystem.WorldOpenTimeStamp % 86400;
		int num2 = player.ActivitySystem.WorldOpenTimeStamp + 864000 - num - player.GetTimeStamp();
		if (num2 <= 0)
		{
			return 0;
		}
		return num2;
	}

	public static int GetTakeSevenDayRewardTime()
	{
		LocalPlayer player = Globals.Instance.Player;
		if (player.ActivitySystem.WorldOpenTimeStamp == 0)
		{
			return 0;
		}
		int num = player.ActivitySystem.WorldOpenTimeStamp % 86400;
		int num2 = player.ActivitySystem.WorldOpenTimeStamp + 604800 - num - player.GetTimeStamp();
		if (num2 <= 0)
		{
			return 0;
		}
		return num2;
	}

	public static string FormatTime(int timecount)
	{
		int num = timecount / 3600;
		int num2 = timecount / 60 % 60;
		int num3 = timecount % 60;
		return string.Format("{0:D2}:{1:D2}:{2:D2}", num, num2, num3);
	}

	public static string FormatTime2(int timecount)
	{
		int num = timecount / 60 % 60;
		int num2 = timecount % 60;
		return string.Format("{0:D2}:{1:D2}", num, num2);
	}

	public static void SetQualityLevel(Tools.CustomQualityLevel level)
	{
		GameSetting.Data.GraphQuality = (int)level;
		switch (level)
		{
		case Tools.CustomQualityLevel.Fantastic:
			QualitySettings.SetQualityLevel(3, false);
			Application.targetFrameRate = 30;
			break;
		case Tools.CustomQualityLevel.Simple:
			QualitySettings.SetQualityLevel(2, false);
			Application.targetFrameRate = 30;
			break;
		case Tools.CustomQualityLevel.Fastest:
			QualitySettings.SetQualityLevel(0, false);
			Application.targetFrameRate = 30;
			break;
		default:
			GameSetting.Data.GraphQuality = 1;
			if (SystemInfo.systemMemorySize <= 512)
			{
				GameSetting.Data.GraphQuality = 3;
			}
			else if (SystemInfo.systemMemorySize < 1024)
			{
				GameSetting.Data.GraphQuality = 2;
			}
			else if (SystemInfo.graphicsShaderLevel <= 20 && SystemInfo.graphicsMemorySize <= 28)
			{
				GameSetting.Data.GraphQuality = 3;
			}
			else if (SystemInfo.graphicsShaderLevel <= 20 && SystemInfo.graphicsMemorySize <= 32)
			{
				GameSetting.Data.GraphQuality = 2;
			}
			Tools.SetQualityLevel((Tools.CustomQualityLevel)GameSetting.Data.GraphQuality);
			break;
		}
	}

	public static void Assert(object obj, string strErrorMessage)
	{
		if (obj == null)
		{
			UnityEngine.Debug.Break();
		}
	}

	public static void SetParticleRenderQueue(GameObject go, int value, float scale = 1f)
	{
		Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].material.shader.name.Contains("Transparent"))
			{
				componentsInChildren[i].material.renderQueue = value + 1;
			}
			else
			{
				componentsInChildren[i].material.renderQueue = value;
			}
		}
		TrailRenderer_Base[] componentsInChildren2 = go.gameObject.GetComponentsInChildren<TrailRenderer_Base>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			if (!(componentsInChildren2[j] == null))
			{
				componentsInChildren2[j].TrailData.TrailMaterial.renderQueue = value + 1;
			}
		}
		if (scale != 1f)
		{
			go.transform.localScale = new Vector3(scale, scale, 1f);
			Tools.ScaleParticleSize(go.GetComponentsInChildren<ParticleSystem>(true), scale);
		}
	}

	public static void SetParticleRenderQueue2(GameObject go, int value)
	{
		Tools.SetParticleRenderQueue(go, value, 1f);
		ParticleScaler[] componentsInChildren = go.GetComponentsInChildren<ParticleScaler>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].renderQueue = value;
		}
	}

	public static void SetParticleRQWithUIScale(GameObject go, int value)
	{
		if (go.GetComponent<ParticleScaler>() == null)
		{
			go.AddComponent<ParticleScaler>();
		}
		Tools.SetParticleRenderQueue(go, value, 1f);
	}

	public static void SetParticleRenderQueue(ParticleSystem[] ps, int value)
	{
		for (int i = 0; i < ps.Length; i++)
		{
			ps[i].renderer.material.renderQueue = value;
		}
	}

	public static void ScaleParticleSize(ParticleSystem[] ps, float effectScale)
	{
		for (int i = 0; i < ps.Length; i++)
		{
			ps[i].startSize *= effectScale;
			ps[i].startSpeed *= effectScale;
		}
	}

	public static void SetMeshRenderQueue(GameObject go, int value)
	{
		Tools.SetParticleRenderQueue2(go, value);
	}

	public static void AddUIWidgetDepth(GameObject go, int value)
	{
		UIWidget[] componentsInChildren = go.GetComponentsInChildren<UIWidget>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].depth += value;
		}
	}

	public static bool IsRebot(ulong GUID)
	{
		return GUID < 16777216uL;
	}

	public static string GetPropertyBg(PetInfo petInfo)
	{
		return Tools.GetPropertyBg((EElementType)petInfo.ElementType);
	}

	public static string GetPropertyBg(MonsterInfo monsetInfo)
	{
		return Tools.GetPropertyBg((EElementType)monsetInfo.ElementType);
	}

	public static string GetPropertyBg(EElementType elementType)
	{
		string result = string.Empty;
		switch (elementType)
		{
		case EElementType.EET_Fire:
			result = "summonIconBg0";
			break;
		case EElementType.EET_Wood:
			result = "summonIconBg3";
			break;
		case EElementType.EET_Water:
			result = "summonIconBg2";
			break;
		case EElementType.EET_Light:
			result = "summonIconBg1";
			break;
		case EElementType.EET_Dark:
			result = "summonIconBg4";
			break;
		}
		return result;
	}

	public static string GetInGamePropertyBg(PetInfo petInfo)
	{
		string result = string.Empty;
		switch (petInfo.ElementType)
		{
		case 1:
			result = "pet-bg01";
			break;
		case 2:
			result = "pet-bg04";
			break;
		case 3:
			result = "pet-bg03";
			break;
		case 4:
			result = "pet-bg02";
			break;
		case 5:
			result = "pet-bg05";
			break;
		}
		return result;
	}

	public static string GetPropertyIcon(PetInfo petInfo)
	{
		return Tools.GetPropertyIcon((EElementType)petInfo.ElementType);
	}

	public static string GetPropertyIcon(MonsterInfo monsetInfo)
	{
		return Tools.GetPropertyIcon((EElementType)monsetInfo.ElementType);
	}

	public static string GetPropertyIcon(EElementType elementType)
	{
		string result = string.Empty;
		switch (elementType)
		{
		case EElementType.EET_Fire:
			result = "property0";
			break;
		case EElementType.EET_Wood:
			result = "property3";
			break;
		case EElementType.EET_Water:
			result = "property2";
			break;
		case EElementType.EET_Light:
			result = "property1";
			break;
		case EElementType.EET_Dark:
			result = "property4";
			break;
		}
		return result;
	}

	public static string GetPropertyIconWithBorder(EElementType elementType)
	{
		string result = string.Empty;
		switch (elementType)
		{
		case EElementType.EET_Fire:
			result = "property00";
			break;
		case EElementType.EET_Wood:
			result = "property01";
			break;
		case EElementType.EET_Water:
			result = "property02";
			break;
		case EElementType.EET_Light:
			result = "property03";
			break;
		case EElementType.EET_Dark:
			result = "property04";
			break;
		}
		return result;
	}

	public static string GetPropertyIconNoBorder(EElementType elementType)
	{
		string result = string.Empty;
		switch (elementType)
		{
		case EElementType.EET_Fire:
			result = "property10";
			break;
		case EElementType.EET_Wood:
			result = "property11";
			break;
		case EElementType.EET_Water:
			result = "property12";
			break;
		case EElementType.EET_Light:
			result = "property13";
			break;
		case EElementType.EET_Dark:
			result = "property14";
			break;
		}
		return result;
	}

	public static string GetPropertyIconMask(EElementType elementType)
	{
		string arg_0E_0 = "slice";
		int num = (int)elementType;
		return arg_0E_0 + num.ToString();
	}

	public static string GetSummonCardBg(PetInfo petInfo)
	{
		return Tools.GetSummonCardBg((EElementType)petInfo.ElementType);
	}

	public static string GetSummonCardBg(EElementType elementType)
	{
		string result = string.Empty;
		switch (elementType)
		{
		case EElementType.EET_Fire:
			result = "card0";
			break;
		case EElementType.EET_Wood:
			result = "card1";
			break;
		case EElementType.EET_Water:
			result = "card2";
			break;
		case EElementType.EET_Light:
			result = "card3";
			break;
		case EElementType.EET_Dark:
			result = "card4";
			break;
		}
		return result;
	}

	public static string GetSummonCardSkillBg(PetInfo petInfo)
	{
		return Tools.GetSummonCardSkillBg((EElementType)petInfo.ElementType);
	}

	public static string GetSummonCardSkillBg(EElementType elementType)
	{
		string result = string.Empty;
		switch (elementType)
		{
		case EElementType.EET_Fire:
			result = "skillbg0";
			break;
		case EElementType.EET_Wood:
			result = "skillbg1";
			break;
		case EElementType.EET_Water:
			result = "skillbg2";
			break;
		case EElementType.EET_Light:
			result = "skillbg3";
			break;
		case EElementType.EET_Dark:
			result = "skillbg4";
			break;
		}
		return result;
	}

	public static string GetSummonPropertyStr(EElementType summonType)
	{
		StringManager arg_18_0 = Singleton<StringManager>.Instance;
		string arg_13_0 = "EET_";
		int num = (int)summonType;
		return arg_18_0.GetString(arg_13_0 + num.ToString());
	}

	public static EElementType GetSummonRestrainType(EElementType summonType)
	{
		EElementType result = EElementType.EET_Null;
		switch (summonType)
		{
		case EElementType.EET_Fire:
			result = EElementType.EET_Wood;
			break;
		case EElementType.EET_Wood:
			result = EElementType.EET_Water;
			break;
		case EElementType.EET_Water:
			result = EElementType.EET_Fire;
			break;
		case EElementType.EET_Light:
			result = EElementType.EET_Dark;
			break;
		case EElementType.EET_Dark:
			result = EElementType.EET_Light;
			break;
		}
		return result;
	}

	public static EElementType GetSummonDerestrainType(EElementType summonType)
	{
		EElementType result = EElementType.EET_Null;
		switch (summonType)
		{
		case EElementType.EET_Fire:
			result = EElementType.EET_Water;
			break;
		case EElementType.EET_Wood:
			result = EElementType.EET_Fire;
			break;
		case EElementType.EET_Water:
			result = EElementType.EET_Wood;
			break;
		case EElementType.EET_Light:
			result = EElementType.EET_Dark;
			break;
		case EElementType.EET_Dark:
			result = EElementType.EET_Light;
			break;
		}
		return result;
	}

	public static string GetSummonRecommendElementDesc(EElementType recommendType)
	{
		return Singleton<StringManager>.Instance.GetString("pveRestrainLb", new object[]
		{
			Singleton<StringManager>.Instance.GetString("textColor", new object[]
			{
				Tools.GetSummonElementTypeColor2(recommendType),
				Tools.GetSummonPropertyStr(recommendType)
			}),
			Singleton<StringManager>.Instance.GetString("textColor", new object[]
			{
				Tools.GetSummonElementTypeColor2(Tools.GetSummonRestrainType(recommendType)),
				Tools.GetSummonPropertyStr(Tools.GetSummonRestrainType(recommendType))
			})
		});
	}

	public static string GetSummonRecommendElementDescWithEnemyElement(EElementType enemyType)
	{
		EElementType summonDerestrainType = Tools.GetSummonDerestrainType(enemyType);
		return Tools.GetSummonRecommendElementDesc(summonDerestrainType);
	}

	public static Color GetSummonElementTypeColor(EElementType summonType)
	{
		switch (summonType)
		{
		case EElementType.EET_Fire:
			return new Color32(247, 43, 8, 255);
		case EElementType.EET_Wood:
			return new Color32(110, 214, 22, 255);
		case EElementType.EET_Water:
			return new Color32(88, 231, 239, 255);
		case EElementType.EET_Light:
			return new Color32(254, 195, 53, 255);
		case EElementType.EET_Dark:
			return new Color32(152, 63, 246, 255);
		default:
			return Color.white;
		}
	}

	public static string GetSummonElementTypeColor2(EElementType summonType)
	{
		switch (summonType)
		{
		case EElementType.EET_Fire:
			return "F72B08";
		case EElementType.EET_Wood:
			return "6ED616";
		case EElementType.EET_Water:
			return "58E7EF";
		case EElementType.EET_Light:
			return "FEC335";
		case EElementType.EET_Dark:
			return "983FF6";
		default:
			return "FFFFFF";
		}
	}

	public static string GetSummonElementTypeAndColor(EElementType summonType)
	{
		return Singleton<StringManager>.Instance.GetString("textColor", new object[]
		{
			Tools.GetSummonElementTypeColor2(summonType),
			Tools.GetSummonPropertyStr(summonType)
		});
	}

	public static string GetSummonElementTypeColorWithEnemyElement(EElementType enemyType)
	{
		return Tools.GetSummonElementTypeAndColor(Tools.GetSummonDerestrainType(enemyType));
	}

	public static string GetSummonCardStarBg(EElementType summonType)
	{
		string result = string.Empty;
		switch (summonType)
		{
		case EElementType.EET_Fire:
			result = "star0";
			break;
		case EElementType.EET_Wood:
			result = "star1";
			break;
		case EElementType.EET_Water:
			result = "star2";
			break;
		case EElementType.EET_Light:
			result = "star3";
			break;
		case EElementType.EET_Dark:
			result = "star4";
			break;
		}
		return result;
	}

	public static string GetCollectionSummonTitleBg(EElementType summonType)
	{
		string result = string.Empty;
		switch (summonType)
		{
		case EElementType.EET_Fire:
			result = "property00_bg";
			break;
		case EElementType.EET_Wood:
			result = "property01_bg";
			break;
		case EElementType.EET_Water:
			result = "property02_bg";
			break;
		case EElementType.EET_Light:
			result = "property03_bg";
			break;
		case EElementType.EET_Dark:
			result = "property04_bg";
			break;
		}
		return result;
	}

	public static string GetItemQualityIcon(int Quality)
	{
		switch (Quality)
		{
		case 0:
			return "Quality_G";
		case 1:
			return "Quality_B";
		case 2:
			return "Quality_V";
		case 3:
			return "Quality_O";
		case 4:
			return "Quality_Gold";
		default:
			return string.Empty;
		}
	}

	public static string GetItemQualityTagIcon(int Quality)
	{
		switch (Quality)
		{
		case 0:
			return "qTag1";
		case 1:
			return "qTag2";
		case 2:
			return "qTag3";
		case 3:
			return "qTag4";
		case 4:
			return "qTag5";
		default:
			return string.Empty;
		}
	}

	public static Color GetItemQualityColor(int Quality)
	{
		switch (Quality)
		{
		case 0:
			return new Color32(150, 230, 0, 255);
		case 1:
			return new Color32(21, 202, 255, 255);
		case 2:
			return new Color32(205, 111, 255, 255);
		case 3:
			return new Color32(255, 153, 9, 255);
		case 4:
			return new Color32(255, 255, 0, 255);
		default:
			return Color.white;
		}
	}

	public static string GetItemQualityColorHex(int Quality)
	{
		switch (Quality)
		{
		case 0:
			return "[96e600]";
		case 1:
			return "[15caff]";
		case 2:
			return "[cd6fff]";
		case 3:
			return "[ff9909]";
		case 4:
			return "[ffff00]";
		default:
			return "[ffffff]";
		}
	}

	public static string GetItemQualityColorHex(int Quality, string str)
	{
		string format = "[ffffff]{0}[-]";
		switch (Quality)
		{
		case 0:
			format = "[96e600]{0}[-]";
			break;
		case 1:
			format = "[15caff]{0}[-]";
			break;
		case 2:
			format = "[cd6fff]{0}[-]";
			break;
		case 3:
			format = "[ff9909]{0}[-]";
			break;
		case 4:
			format = "[ffff00]{0}[-]";
			break;
		}
		return string.Format(format, str);
	}

	public static string GetDefaultColorHex(string str)
	{
		return string.Format("[e6d67b]{0}[-]", str);
	}

	public static string GetColorHex(Color col, string str)
	{
		string format = "[ffffff]{0}[-]";
		if (col == Color.red)
		{
			format = "[ff0000]{0}[-]";
		}
		else if (col == Color.yellow)
		{
			format = "[ffff00]{0}[-]";
		}
		else if (col == Color.green)
		{
			format = "[00ff00]{0}[-]";
		}
		else if (col == Color.gray)
		{
			format = "[808080]{0}[-]";
		}
		return string.Format(format, str);
	}

	public static string GetUrl(string url, string content)
	{
		return string.Format("[url={0}]{1}[/url]", url, content);
	}

	public static bool IsCanComposite(RecipeInfo Info)
	{
		ItemSubSystem itemSystem = Globals.Instance.Player.ItemSystem;
		for (int i = 0; i < Info.ItemID.Count; i++)
		{
			if (Info.ItemID[i] != 0)
			{
				if (itemSystem.GetItemByInfoID(Info.ItemID[i]) == null)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool IsAnyRecipeComposite()
	{
		foreach (RecipeInfo current in Globals.Instance.AttDB.RecipeDict.Values)
		{
			if (Tools.IsCanComposite(current))
			{
				return true;
			}
		}
		return false;
	}

	public static Color GetDefaultTextColor()
	{
		return new Color32(230, 214, 123, 255);
	}

	public static Color GetUnactivatedLabelColor()
	{
		return new Color32(153, 133, 87, 255);
	}

	public static Color GetActivatedLabelColor()
	{
		return new Color32(253, 237, 186, 255);
	}

	public static Color GetBillboardSelfBgColor()
	{
		return new Color32(144, 144, 144, 255);
	}

	public static Color GetDisabledTextColor(byte value = 96)
	{
		return new Color32(value, value, value, 255);
	}

	public static void PlaySceneBGM(SceneInfo sceneInfo)
	{
		if (sceneInfo != null)
		{
			Globals.Instance.BackgroundMusicMgr.ClearLobbySound();
			Globals.Instance.BackgroundMusicMgr.PlayGameBGM(sceneInfo.Sound);
			Globals.Instance.BackgroundMusicMgr.PlayGameBGM_ETC(sceneInfo.SoundETC);
		}
	}

	public static int GetAchievementScore(AchievementInfo info)
	{
		if (!info.Daily)
		{
			return 0;
		}
		LocalPlayer player = Globals.Instance.Player;
		if (info.Score2 != 0 && (ulong)player.Data.DayLevel >= (ulong)((long)info.Level))
		{
			return info.Score2;
		}
		return info.Score;
	}

	public static int GetShopBuyTimes(ShopInfo info)
	{
		if (info.Times >= 0)
		{
			return info.Times;
		}
		VipLevelInfo vipLevelInfo = Globals.Instance.Player.GetVipLevelInfo();
		int num = -1 - info.Times;
		if (num < 0 || num >= vipLevelInfo.BuyCount.Count)
		{
			global::Debug.LogErrorFormat("Shop buy time error Info:{0} Times:{1}", new object[]
			{
				info.ID,
				info.Count
			});
			return info.Times;
		}
		return vipLevelInfo.BuyCount[num];
	}

	public static int GetItemBuyConst(ItemInfo info, int count, ShopInfo shopInfo)
	{
		if (info == null)
		{
			return 0;
		}
		if (shopInfo.ID > 1000)
		{
			return count / ((info.Value4 != 0) ? info.Value4 : 1) * info.Value5 + shopInfo.Price * shopInfo.Count;
		}
		return (GameConst.GetInt32(189) + GameConst.GetInt32(190) * count) * shopInfo.Price * shopInfo.Count / 100;
	}

	public static int GetShopRefreshTimes(EShopType type)
	{
		if (type == EShopType.EShop_Common2)
		{
			VipLevelInfo vipLevelInfo = Globals.Instance.Player.GetVipLevelInfo();
			return vipLevelInfo.ShopCommon2Count;
		}
		if (type == EShopType.EShop_Awaken)
		{
			VipLevelInfo vipLevelInfo2 = Globals.Instance.Player.GetVipLevelInfo();
			return vipLevelInfo2.ShopAwakenCount;
		}
		if (type == EShopType.EShop_Lopet)
		{
			VipLevelInfo vipLevelInfo3 = Globals.Instance.Player.GetVipLevelInfo();
			return vipLevelInfo3.ShopLopetCount;
		}
		return -1;
	}

	public static string GetShopCurrencyIcon(EShopType type)
	{
		switch (type)
		{
		case EShopType.EShop_Common2:
			return Tools.GetCurrencyIcon(ECurrencyType.ECurrencyT_MagicSoul);
		case EShopType.EShop_Awaken:
			return Tools.GetCurrencyIcon(ECurrencyType.ECurrencyT_StarSoul);
		case EShopType.EShop_Guild:
			return Tools.GetCurrencyIcon(ECurrencyType.ECurrencyT_Reputation);
		case EShopType.EShop_Trial:
			return Tools.GetCurrencyIcon(ECurrencyType.ECurrencyT_MagicCrystal);
		case EShopType.EShop_KR:
			return Tools.GetCurrencyIcon(ECurrencyType.ECurrencyT_KingMedal);
		case EShopType.EShop_Pvp:
			return Tools.GetCurrencyIcon(ECurrencyType.ECurrencyT_Honor);
		case EShopType.EShop_Lopet:
			return Tools.GetCurrencyIcon(ECurrencyType.ECurrencyT_LopetSoul);
		}
		return string.Empty;
	}

	public static string GetShopCurrencyName(EShopType type)
	{
		switch (type)
		{
		case EShopType.EShop_Common2:
			return "magicSoul";
		case EShopType.EShop_Awaken:
			return "starSoul";
		case EShopType.EShop_Guild:
			return "guildValue";
		case EShopType.EShop_Trial:
			return "magicCrystal";
		case EShopType.EShop_KR:
			return "kingRewardValue";
		case EShopType.EShop_Pvp:
			return "pvpValue";
		}
		return string.Empty;
	}

	public static int GetShopCurrencyValue(EShopType type)
	{
		LocalPlayer player = Globals.Instance.Player;
		switch (type)
		{
		case EShopType.EShop_Common2:
			return player.Data.MagicSoul;
		case EShopType.EShop_Awaken:
			return player.Data.StarSoul;
		case EShopType.EShop_Guild:
			return player.Data.Reputation;
		case EShopType.EShop_Trial:
			return player.Data.MagicCrystal;
		case EShopType.EShop_KR:
			return player.Data.KingMedal;
		case EShopType.EShop_Pvp:
			return player.Data.Honor;
		case EShopType.EShop_Lopet:
			return player.Data.LopetSoul;
		}
		return 0;
	}

	public static string FormatCurrency(int num)
	{
		int num2 = Mathf.Abs(num);
		if (num2 < 1000000)
		{
			return string.Format("{0}", num);
		}
		if (num2 < 100000000)
		{
			return string.Format("{0}{1}", num / 10000, Singleton<StringManager>.Instance.GetString("wan"));
		}
		return string.Format("{0}{1}", num / 100000000, Singleton<StringManager>.Instance.GetString("yi"));
	}

	public static string FormatValue(int num)
	{
		int num2 = Mathf.Abs(num);
		if (num2 < 10000)
		{
			return string.Format("{0}", num);
		}
		if (num2 < 1000000)
		{
			return string.Format("{0}{1}", (num % 10000 < 1000) ? (num / 10000).ToString() : ((float)num / 10000f).ToString("0.0"), Singleton<StringManager>.Instance.GetString("wan"));
		}
		if (num2 < 100000000)
		{
			return string.Format("{0}{1}", num / 10000, Singleton<StringManager>.Instance.GetString("wan"));
		}
		return string.Format("{0}{1}", num / 100000000, Singleton<StringManager>.Instance.GetString("yi"));
	}

	public static string FormatValue(long num)
	{
		long num2 = (num >= 0L) ? num : (-num);
		if (num2 < 10000L)
		{
			return string.Format("{0}", num);
		}
		if (num2 < 1000000L)
		{
			return string.Format("{0}{1}", (num % 10000L < 1000L) ? (num / 10000L).ToString() : ((float)num / 10000f).ToString("0.0"), Singleton<StringManager>.Instance.GetString("wan"));
		}
		if (num2 < 100000000L)
		{
			return string.Format("{0}{1}", num / 10000L, Singleton<StringManager>.Instance.GetString("wan"));
		}
		return string.Format("{0}{1}", num / 100000000L, Singleton<StringManager>.Instance.GetString("yi"));
	}

	public static string FormatOffPrice(int value)
	{
		return Singleton<StringManager>.Instance.GetString("ShopCommon1", new object[]
		{
			(float)value / 10f
		});
	}

	public static string GetValueText(EAchievementConditionType type, int value)
	{
		if (type == EAchievementConditionType.EACT_CombatValue)
		{
			return Tools.FormatValue(value);
		}
		if (type != EAchievementConditionType.EACT_PetEquipQuality)
		{
			return value.ToString();
		}
		return Singleton<StringManager>.Instance.GetString(string.Format("Quality{0}", value));
	}

	public static int GetVipLevel(VipLevelInfo vipLevelInfo)
	{
		return (vipLevelInfo.ID != 16) ? vipLevelInfo.ID : 0;
	}

	public static VipLevelInfo GetVipLevelInfo(int vipLevel)
	{
		return Globals.Instance.AttDB.VipLevelDict.GetInfo((vipLevel != 0) ? vipLevel : 16);
	}

	public static string GetVIPPayRewardTitle(VipLevelInfo vipLevelInfo)
	{
		if (vipLevelInfo == null)
		{
			return string.Empty;
		}
		return string.Format(Singleton<StringManager>.Instance.GetString("VIPTitle3"), Tools.GetVipLevel(vipLevelInfo));
	}

	public static string GetVIPWeekRewardTitle(VipLevelInfo vipLevelInfo)
	{
		if (vipLevelInfo == null)
		{
			return string.Empty;
		}
		return string.Format(Singleton<StringManager>.Instance.GetString("VIPTitle6"), Tools.GetVipLevel(vipLevelInfo));
	}

	public static string GetRewardTypeIcon(ERewardType ct)
	{
		switch (ct)
		{
		case ERewardType.EReward_Money:
			return "Gold_1";
		case ERewardType.EReward_Diamond:
			return "redGem_1";
		case ERewardType.EReward_Energy:
			return "key_1";
		case ERewardType.EReward_Exp:
			return "exp_1";
		case ERewardType.EReward_GuildRepution:
			return "Guild_1";
		case ERewardType.EReward_MagicCrystal:
			return "magicCrystal";
		case ERewardType.EReward_MagicSoul:
			return "magicSoul";
		case ERewardType.EReward_FireDragonScale:
			return "FireDragon";
		case ERewardType.EReward_StarSoul:
			return "starSoul";
		case ERewardType.EReward_Honor:
			return "honor";
		case ERewardType.EReward_Emblem:
			return "emblem";
		case ERewardType.EReward_LopetSoul:
			return "lopetSoul";
		case ERewardType.EReward_FestivalVoucher:
			return "festivalVoucher";
		case ERewardType.EReward_VipExp:
			return string.Empty;
		}
		global::Debug.LogErrorFormat("param Error!", new object[0]);
		return string.Empty;
	}

	public static string GetRewardTypeName(ERewardType ct, int RewardValue1)
	{
		switch (ct)
		{
		case ERewardType.EReward_Money:
			return Singleton<StringManager>.Instance.GetString("money");
		case ERewardType.EReward_Diamond:
			return Singleton<StringManager>.Instance.GetString("diamond");
		case ERewardType.EReward_Item:
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(RewardValue1);
			return (info != null) ? info.Name : string.Empty;
		}
		case ERewardType.EReward_Pet:
		{
			PetInfo info2 = Globals.Instance.AttDB.PetDict.GetInfo(RewardValue1);
			return (info2 != null) ? Tools.GetPetName(info2) : string.Empty;
		}
		case ERewardType.EReward_Energy:
			return Singleton<StringManager>.Instance.GetString("energy");
		case ERewardType.EReward_Exp:
			return Singleton<StringManager>.Instance.GetString("exp");
		case ERewardType.EReward_GuildRepution:
			return Singleton<StringManager>.Instance.GetString("guildValue");
		case ERewardType.EReward_MagicCrystal:
			return Singleton<StringManager>.Instance.GetString("magicCrystal");
		case ERewardType.EReward_MagicSoul:
			return Singleton<StringManager>.Instance.GetString("magicSoul");
		case ERewardType.EReward_FireDragonScale:
			return Singleton<StringManager>.Instance.GetString("fireDragonScale");
		case ERewardType.EReward_KingMedal:
			return Singleton<StringManager>.Instance.GetString("kingRewardValue");
		case ERewardType.EReward_Fashion:
		{
			FashionInfo info3 = Globals.Instance.AttDB.FashionDict.GetInfo(RewardValue1);
			return (info3 != null) ? info3.Name : string.Empty;
		}
		case ERewardType.EReward_StarSoul:
			return Singleton<StringManager>.Instance.GetString("starSoul");
		case ERewardType.EReward_Honor:
			return Singleton<StringManager>.Instance.GetString("pvpValue");
		case ERewardType.EReward_Emblem:
			return Singleton<StringManager>.Instance.GetString("emblem");
		case ERewardType.EReward_Lopet:
		{
			LopetInfo info4 = Globals.Instance.AttDB.LopetDict.GetInfo(RewardValue1);
			return (info4 != null) ? info4.Name : string.Empty;
		}
		case ERewardType.EReward_LopetSoul:
			return Singleton<StringManager>.Instance.GetString("lopetSoul");
		case ERewardType.EReward_FestivalVoucher:
			return Singleton<StringManager>.Instance.GetString("festivalVoucher");
		case ERewardType.EReward_VipExp:
			return Singleton<StringManager>.Instance.GetString("vipExp");
		default:
			global::Debug.LogErrorFormat("param Error!", new object[0]);
			return string.Empty;
		}
	}

	public static string GetCurrencyIcon(ECurrencyType ct)
	{
		switch (ct)
		{
		case ECurrencyType.ECurrencyT_Money:
			return "Gold_1";
		case ECurrencyType.ECurrencyT_Diamond:
			return "redGem_1";
		case ECurrencyType.ECurrencyT_Honor:
			return "Honor_1";
		case ECurrencyType.ECurrencyT_Reputation:
			return "Guild_1";
		case ECurrencyType.ECurrencyT_MagicCrystal:
			return "magicCrystal";
		case ECurrencyType.ECurrencyT_KingMedal:
			return "KingMedal_1";
		case ECurrencyType.ECurrencyT_MagicSoul:
			return "magicSoul";
		case ECurrencyType.ECurrencyT_LuckyDraw:
			return "luck_1";
		case ECurrencyType.ECurrencyT_StarSoul:
			return "starSoul";
		case ECurrencyType.ECurrencyT_Emblem:
			return "emblem";
		case ECurrencyType.ECurrencyT_LopetSoul:
			return "lopetSoul";
		case ECurrencyType.ECurrencyT_FestivalVoucher:
			return "festivalVoucher";
		}
		global::Debug.LogErrorFormat("Unknow Currency Type {0}", new object[]
		{
			ct
		});
		return string.Empty;
	}

	public static int GetCurrencyMoney(ECurrencyType ct, int itemID = 0)
	{
		ObscuredStats data = Globals.Instance.Player.Data;
		switch (ct)
		{
		case ECurrencyType.ECurrencyT_Money:
			return data.Money;
		case ECurrencyType.ECurrencyT_Diamond:
			return data.Diamond;
		case ECurrencyType.ECurrencyT_Honor:
			return data.Honor;
		case ECurrencyType.ECurrencyT_Reputation:
			return data.Reputation;
		case ECurrencyType.ECurrencyT_MagicCrystal:
			return data.MagicCrystal;
		case ECurrencyType.ECurrencyT_KingMedal:
			return data.KingMedal;
		case ECurrencyType.ECurrencyT_MagicSoul:
			return data.MagicSoul;
		case ECurrencyType.ECurrencyT_LuckyDraw:
			return Globals.Instance.Player.ActivitySystem.LuckyDrawScore;
		case ECurrencyType.ECurrencyT_Item:
			return Globals.Instance.Player.ItemSystem.GetItemCount(itemID);
		case ECurrencyType.ECurrencyT_StarSoul:
			return data.StarSoul;
		case ECurrencyType.ECurrencyT_Emblem:
			return data.Emblem;
		case ECurrencyType.ECurrencyT_LopetSoul:
			return data.LopetSoul;
		case ECurrencyType.ECurrencyT_FestivalVoucher:
			return data.FestivalVoucher;
		default:
			global::Debug.LogErrorFormat("Unknow Currency Type {0}", new object[]
			{
				ct
			});
			return 0;
		}
	}

	public static bool MoneyNotEnough(ECurrencyType ct, int price, int itemID = 0)
	{
		int currencyMoney = Tools.GetCurrencyMoney(ct, itemID);
		if (price > currencyMoney)
		{
			switch (ct)
			{
			case ECurrencyType.ECurrencyT_Money:
				GameMessageBox.ShowMoneyLackMessageBox();
				break;
			case ECurrencyType.ECurrencyT_Diamond:
				GameMessageBox.ShowRechargeMessageBox();
				break;
			case ECurrencyType.ECurrencyT_Honor:
				GameUIManager.mInstance.ShowMessageTip("ItemR", 13);
				break;
			case ECurrencyType.ECurrencyT_Reputation:
				GameUIManager.mInstance.ShowMessageTip("ItemR", 14);
				break;
			case ECurrencyType.ECurrencyT_MagicCrystal:
				GameUIManager.mInstance.ShowMessageTip("ItemR", 37);
				break;
			case ECurrencyType.ECurrencyT_KingMedal:
				GameUIManager.mInstance.ShowMessageTip("ItemR", 27);
				break;
			case ECurrencyType.ECurrencyT_MagicSoul:
				GameUIManager.mInstance.ShowMessageTip("ItemR", 38);
				break;
			case ECurrencyType.ECurrencyT_LuckyDraw:
				GameUIManager.mInstance.ShowMessageTipByKey("activityLuckyDrawMScore2", 0f, 0f);
				break;
			case ECurrencyType.ECurrencyT_Item:
				GameUIManager.mInstance.ShowMessageTipByKey("PlayerR_6", 0f, 0f);
				break;
			case ECurrencyType.ECurrencyT_StarSoul:
				GameUIManager.mInstance.ShowMessageTipByKey("ShopT8", 0f, 0f);
				break;
			case ECurrencyType.ECurrencyT_Emblem:
				GameUIManager.mInstance.ShowMessageTipByKey("ShopT9", 0f, 0f);
				break;
			case ECurrencyType.ECurrencyT_LopetSoul:
				GameUIManager.mInstance.ShowMessageTipByKey("ShopT10", 0f, 0f);
				break;
			case ECurrencyType.ECurrencyT_FestivalVoucher:
				GameUIManager.mInstance.ShowMessageTipByKey("ShopT11", 0f, 0f);
				break;
			default:
				global::Debug.LogErrorFormat("Unknow Currency Type {0}", new object[]
				{
					ct
				});
				break;
			}
			return true;
		}
		return false;
	}

	public static GameObject InstantiateGUIPrefab(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(prefab, position, rotation) as GameObject;
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Instantiate error, Name = {0}", prefab.name)
			});
			return null;
		}
		return gameObject;
	}

	public static GameObject InstantiateGUIPrefab(string prefabName, Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = Res.LoadGUI(prefabName);
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Res.Load error, Name = {0}", prefabName)
			});
			return null;
		}
		return Tools.InstantiateGUIPrefab(gameObject, position, rotation);
	}

	public static GameObject InstantiateGUIPrefab(GameObject prefab)
	{
		return Tools.InstantiateGUIPrefab(prefab, Vector3.zero, Quaternion.identity);
	}

	public static GameObject InstantiateGUIPrefab(string prefabName)
	{
		return Tools.InstantiateGUIPrefab(prefabName, Vector3.zero, Quaternion.identity);
	}

	public static bool IsUnlockPveCombatSpeedupX2()
	{
		return Globals.Instance.Player.Data.VipLevel >= 1u || (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(17));
	}

	public static bool IsUnlockPveCombatSpeedupX3()
	{
		return Globals.Instance.Player.Data.VipLevel >= 5u;
	}

	public static SceneInfo GetNextSceneInfo(int difficulty)
	{
		SceneInfo result = null;
		foreach (SceneInfo current in Globals.Instance.AttDB.SceneDict.Values)
		{
			if (current.Difficulty == difficulty && current.PreID2 != 0)
			{
				if (current.MapID % 100 > 18)
				{
					break;
				}
				if (Globals.Instance.Player.GetSceneScore(current.ID) <= 0)
				{
					return current;
				}
				result = current;
			}
		}
		return result;
	}

	public static int GetPVEStars(int mapID = 0)
	{
		int num = 0;
		foreach (SceneInfo current in Globals.Instance.AttDB.SceneDict.Values)
		{
			if (mapID == 0 || current.MapID == mapID)
			{
				int sceneScore = Globals.Instance.Player.GetSceneScore(current.ID);
				if (sceneScore > 0)
				{
					num += sceneScore;
				}
			}
		}
		return num;
	}

	public static SceneInfo GetNextMapSceneInfo(int difficulty, uint level)
	{
		SceneInfo result = null;
		foreach (SceneInfo current in Globals.Instance.AttDB.SceneDict.Values)
		{
			if (current.Difficulty == difficulty && current.PreID2 != 0)
			{
				if (current.MapID % 100 > 18)
				{
					break;
				}
				if ((ulong)level < (ulong)((long)current.MinLevel))
				{
					return current;
				}
				result = current;
			}
		}
		return result;
	}

	public static SceneInfo GetNextMapSceneInfo(SceneInfo sceneInfo)
	{
		if (sceneInfo.MapID <= 0 || sceneInfo.MapID % 100 > 18)
		{
			return null;
		}
		SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(sceneInfo.NextID);
		if (info == null || (ulong)Globals.Instance.Player.Data.Level < (ulong)((long)info.MinLevel))
		{
			return null;
		}
		return info;
	}

	public static SceneInfo GetNextAwakeMapSceneInfo(SceneInfo sceneInfo)
	{
		if (sceneInfo.MapID <= 0)
		{
			return null;
		}
		SceneInfo info = Globals.Instance.AttDB.SceneDict.GetInfo(sceneInfo.NextID);
		if (info == null || (ulong)Globals.Instance.Player.Data.Level < (ulong)((long)info.MinLevel))
		{
			return null;
		}
		return info;
	}

	public static bool IsMapAllPassed(MapInfo mapInfo)
	{
		if (mapInfo == null)
		{
			return false;
		}
		int num = mapInfo.ID / 1000;
		int num2 = mapInfo.ID % 1000;
		for (int i = 0; i < 10; i++)
		{
			int num3 = num * 100000 + num2 * 1000 + i + 1;
			if (Globals.Instance.AttDB.SceneDict.GetInfo(num3) != null)
			{
				if (Globals.Instance.Player.GetSceneScore(num3) == 0)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static bool IsMapAnyPassed(MapInfo mapInfo)
	{
		if (mapInfo == null)
		{
			return false;
		}
		int num = mapInfo.ID / 1000;
		int num2 = mapInfo.ID % 1000;
		for (int i = 0; i < 10; i++)
		{
			int num3 = num * 100000 + num2 * 1000 + i + 1;
			if (Globals.Instance.AttDB.SceneDict.GetInfo(num3) != null)
			{
				if (Globals.Instance.Player.GetSceneScore(num3) > 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static Vector3 GetRelativePos(Transform from, Transform to, Vector3 initPos)
	{
		Vector3 vector = initPos;
		while (from != to)
		{
			vector += from.localPosition;
			from = from.parent;
		}
		return vector;
	}

	public static Transform GetCameraRootParent(Transform from)
	{
		Transform transform = GameUIManager.mInstance.uiCamera.transform;
		while (from != null && from.parent != transform)
		{
			from = from.parent;
		}
		return from;
	}

	public static string GetPetName(PetInfo info)
	{
		if (info == null)
		{
			return string.Empty;
		}
		if (string.IsNullOrEmpty(info.FirstName))
		{
			return info.Name;
		}
		return string.Format("{0}.{1}", info.FirstName, info.Name);
	}

	public static string UpdateTimeText(int time)
	{
		if (time < 60)
		{
			return string.Format("{0}{1}", time, Singleton<StringManager>.Instance.GetString("Second"));
		}
		int num = time / 60;
		int num2 = time % 60;
		return string.Format("{0}{1}{2,2:00}{3}", new object[]
		{
			num,
			Singleton<StringManager>.Instance.GetString("Minute"),
			num2,
			Singleton<StringManager>.Instance.GetString("Second")
		});
	}

	public static int GetPVP4BuyCountCost()
	{
		return 0;
	}

	public static int GetPVP6BuyCountCost()
	{
		return 0;
	}

	public static string GetAFStr(EAttID ef)
	{
		return Singleton<StringManager>.Instance.GetString(string.Format("EAF_{0}", (int)ef));
	}

	public static string GetPetTypeIcon(int pt)
	{
		string result = string.Empty;
		switch (pt)
		{
		case 1:
			result = "gongji";
			break;
		case 2:
			result = "fangyu";
			break;
		case 3:
			result = "fagong";
			break;
		case 4:
			result = "fuzhu";
			break;
		}
		return result;
	}

	public static string GetPetTypeStrDesc(int pt)
	{
		return Singleton<StringManager>.Instance.GetString(string.Format("petType{0}", pt));
	}

	public static string GetIconFrame(bool isVIP)
	{
		return (!isVIP) ? "Frame0" : "Frame1";
	}

	public static string GetPlayerIcon(int fashionID)
	{
		if (fashionID != 0)
		{
			FashionInfo info = Globals.Instance.AttDB.FashionDict.GetInfo(fashionID);
			if (info != null)
			{
				return info.Icon;
			}
		}
		return string.Empty;
	}

	public static string GetGuildMemberJobDesc(int guildRank)
	{
		return Singleton<StringManager>.Instance.GetString(string.Format("guildRk{0}", guildRank));
	}

	public static string GetMasterName()
	{
		string result = string.Empty;
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			for (int i = 0; i < Globals.Instance.Player.GuildSystem.Members.Count; i++)
			{
				GuildMember guildMember = Globals.Instance.Player.GuildSystem.Members[i];
				if (guildMember.Rank == 1)
				{
					result = guildMember.Name;
					break;
				}
			}
		}
		return result;
	}

	public static int GetSelfGuildJob()
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild() && Globals.Instance.Player.GuildSystem.Members != null)
		{
			for (int i = 0; i < Globals.Instance.Player.GuildSystem.Members.Count; i++)
			{
				GuildMember guildMember = Globals.Instance.Player.GuildSystem.Members[i];
				if (guildMember.ID == Globals.Instance.Player.Data.ID)
				{
					return guildMember.Rank;
				}
			}
		}
		return 0;
	}

	public static bool GetSelfCanImpectMaster()
	{
		if (Globals.Instance.Player.GuildSystem.HasGuild())
		{
			for (int i = 0; i < Globals.Instance.Player.GuildSystem.Members.Count; i++)
			{
				GuildMember guildMember = Globals.Instance.Player.GuildSystem.Members[i];
				if (guildMember.Rank == 1 && Globals.Instance.Player.GetTimeStamp() - guildMember.LastOnlineTime <= 604800)
				{
					return false;
				}
				if (guildMember.ID == Globals.Instance.Player.Data.ID && guildMember.TotalReputation < 6000)
				{
					return false;
				}
				if (Globals.Instance.Player.GuildSystem.Guild.ImpeachID != 0uL)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static int ComparePetSlot(PetDataEx aItem, PetDataEx bItem)
	{
		int socketSlot = aItem.GetSocketSlot();
		int socketSlot2 = bItem.GetSocketSlot();
		if (socketSlot >= 0)
		{
			if (socketSlot2 < 0)
			{
				return -1;
			}
			if (socketSlot < socketSlot2)
			{
				return -1;
			}
			return 1;
		}
		else
		{
			if (socketSlot2 >= 0)
			{
				return 1;
			}
			return 0;
		}
	}

	public static string GetMacAddress()
	{
		string result;
		try
		{
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			if (allNetworkInterfaces.Length > 0)
			{
				byte[] addressBytes = allNetworkInterfaces[0].GetPhysicalAddress().GetAddressBytes();
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < addressBytes.Length; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(":");
					}
					int num = addressBytes[i] >> 4 & 15;
					for (int j = 0; j < 2; j++)
					{
						if (num < 10)
						{
							stringBuilder.Append((char)(num + 48));
						}
						else
						{
							stringBuilder.Append((char)(num + 55));
						}
						num = (int)(addressBytes[i] & 15);
					}
				}
				result = stringBuilder.ToString();
			}
			else
			{
				result = "00:00:00:00";
			}
		}
		catch (Exception)
		{
			result = "00:00:00:00";
		}
		return result;
	}

	public static string GetOSName()
	{
		return "Android";
	}

	public static bool CanShowWebView()
	{
		bool result = false;
		DateTime t = new DateTime(GameSetting.Data.WebViewDontShowTimeStamp);
		DateTime now = DateTime.Now;
		if (!GameSetting.Data.WebViewDontShow || (now > t && (now.Day > t.Day || now.Month > t.Month || now.Year > t.Year)))
		{
			result = true;
			GameSetting.Data.WebViewDontShow = false;
			GameSetting.UpdateNow = true;
		}
		return result;
	}

	public static bool CanShowKoreaBuyWnd()
	{
		bool result = false;
		DateTime t = new DateTime(GameSetting.Data.KoreaBuyWndDontShowTimeStamp);
		DateTime now = DateTime.Now;
		if (!GameSetting.Data.KoreaBuyWndDontShow || (now > t && (now.Day > t.Day || now.Month > t.Month || now.Year > t.Year)))
		{
			result = true;
			GameSetting.Data.KoreaBuyWndDontShow = false;
			GameSetting.UpdateNow = true;
		}
		return result;
	}

	public static string GetEquipTypeName(ItemInfo info)
	{
		if (info.Type == 0 || info.Type == 1)
		{
			return Singleton<StringManager>.Instance.GetString(string.Format("equipType_{0}{1}", info.Type, info.SubType));
		}
		if (info.Type == 3 && (info.SubType == 1 || info.SubType == 2))
		{
			return Singleton<StringManager>.Instance.GetString(string.Format("equipType_{0}{1}", info.Type, info.SubType));
		}
		return string.Empty;
	}

	public static string GetEquipTypeName(int slot)
	{
		switch (slot)
		{
		case 0:
			return Singleton<StringManager>.Instance.GetString("equipType_00");
		case 1:
			return Singleton<StringManager>.Instance.GetString("equipType_01");
		case 2:
			return Singleton<StringManager>.Instance.GetString("equipType_02");
		case 3:
			return Singleton<StringManager>.Instance.GetString("equipType_03");
		case 4:
			return Singleton<StringManager>.Instance.GetString("equipType_10");
		case 5:
			return Singleton<StringManager>.Instance.GetString("equipType_11");
		default:
			return string.Empty;
		}
	}

	public static void GetEquipEnhanceAttTxt(ItemDataEx mData, out string attNames, out string attValues, bool isLocal = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		if (mData.Info.Type == 0)
		{
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove11"));
			stringBuilder2.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				mData.GetEquipEnhanceLevel(),
				Globals.Instance.Player.ItemSystem.GetMaxEquipEnhanceLevel(isLocal)
			}));
			stringBuilder.Append(Tools.GetEquipAEStr((ESubTypeEquip)mData.Info.SubType));
			stringBuilder2.Append(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
			{
				mData.GetEquipEnhanceAttValue()
			}));
		}
		else if (mData.Info.Type == 1)
		{
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove11"));
			stringBuilder2.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				mData.GetTrinketEnhanceLevel(),
				Globals.Instance.Player.ItemSystem.GetMaxTrinketEnhanceLevel()
			}));
			stringBuilder.AppendLine(Tools.GetTrinketAEStr(mData, 0));
			stringBuilder2.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
			{
				mData.GetTrinketEnhanceAttValue0()
			}));
			stringBuilder.Append(Tools.GetTrinketAEStr(mData, 1));
			stringBuilder2.Append(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
			{
				Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
				{
					mData.GetTrinketEnhanceAttValue1().ToString("0.0")
				})
			}));
		}
		else if (mData.Info.Type == 3)
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(mData.Info.Value2);
			if (info.Type == 0)
			{
				stringBuilder.Append(Tools.GetEquipAEStr((ESubTypeEquip)info.SubType));
				stringBuilder2.Append(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
				{
					info.Value1
				}));
			}
			else if (info.Type == 1)
			{
				stringBuilder.AppendLine(Tools.GetTrinketAEStr(new ItemDataEx(new ItemData(), info), 0));
				stringBuilder2.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
				{
					info.Value1
				}));
				stringBuilder.Append(Tools.GetTrinketAEStr(new ItemDataEx(new ItemData(), info), 1));
				stringBuilder2.Append(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
				{
					((float)info.Value2 / 100f).ToString("0.0")
				}));
			}
		}
		attNames = stringBuilder.ToString();
		attValues = stringBuilder2.ToString();
	}

	public static void GetEquipRefineAttTxt(ItemDataEx mData, out string attNames, out string attValues, bool isLocal = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		if (mData.Info.Type == 0)
		{
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove12"));
			stringBuilder2.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				mData.GetEquipRefineLevel(),
				Globals.Instance.Player.ItemSystem.GetMaxEquipRefineLevel()
			}));
			stringBuilder.AppendLine(Tools.GetEquipAEStr((ESubTypeEquip)mData.Info.SubType));
			stringBuilder2.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
			{
				mData.GetEquipRefineAttValue0().ToString()
			}));
			stringBuilder.Append(Tools.GetEquipARStr((ESubTypeEquip)mData.Info.SubType));
			stringBuilder2.Append(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
			{
				Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
				{
					mData.GetEquipRefineAttValue1().ToString("0.0")
				})
			}));
		}
		else if (mData.Info.Type == 1)
		{
			stringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove12"));
			stringBuilder2.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove6", new object[]
			{
				mData.GetTrinketRefineLevel(),
				Globals.Instance.Player.ItemSystem.GetMaxTrinketRefineLevel()
			}));
			stringBuilder.AppendLine(Tools.GetTrinketARStr(mData, 0));
			stringBuilder2.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
			{
				Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
				{
					mData.GetTrinketRefineAttValue0().ToString("0.0")
				})
			}));
			stringBuilder.Append(Tools.GetTrinketARStr(mData, 1));
			stringBuilder2.Append(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
			{
				Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
				{
					mData.GetTrinketRefineAttValue1().ToString("0.0")
				})
			}));
		}
		else if (mData.Info.Type == 3)
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(mData.Info.Value2);
			if (info.Type == 0)
			{
				stringBuilder.Append(Tools.GetEquipAEStr((ESubTypeEquip)info.SubType));
				stringBuilder2.Append(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
				{
					info.Value1
				}));
			}
			else if (info.Type == 1)
			{
				stringBuilder.AppendLine(Tools.GetTrinketAEStr(new ItemDataEx(new ItemData(), info), 0));
				stringBuilder2.AppendLine(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
				{
					info.Value1
				}));
				stringBuilder.Append(Tools.GetTrinketAEStr(new ItemDataEx(new ItemData(), info), 1));
				stringBuilder2.Append(Singleton<StringManager>.Instance.GetString("equipImprove23", new object[]
				{
					((float)info.Value2 / 100f).ToString("0.0")
				}));
			}
		}
		attNames = stringBuilder.ToString();
		attValues = stringBuilder2.ToString();
	}

	public static string GetEAttIDName(int id)
	{
		return Singleton<StringManager>.Instance.GetString(string.Format("EAID_{0}", id));
	}

	public static string GetEAttIDValue(int attID, int value)
	{
		if (value <= 0)
		{
			return string.Empty;
		}
		if (attID < 5 && attID > 0)
		{
			return value.ToString();
		}
		if (attID > 4 && attID < 11)
		{
			return ((float)value / 100f).ToString("0.0");
		}
		if (attID == 20)
		{
			return value.ToString();
		}
		return string.Empty;
	}

	public static string GetETAttStr(int attID, int value)
	{
		if (value <= 0)
		{
			return string.Empty;
		}
		if (attID < 5 && attID > 0)
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
			{
				Tools.GetEAttIDName(attID),
				value
			});
		}
		if (attID > 4 && attID < 11)
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
			{
				Tools.GetEAttIDName(attID),
				Singleton<StringManager>.Instance.GetString("equipImprove18", new object[]
				{
					((float)value / 100f).ToString("0.0")
				})
			});
		}
		if (attID == 20)
		{
			return Singleton<StringManager>.Instance.GetString("equipImprove14", new object[]
			{
				Tools.GetEAttIDName(attID),
				value
			});
		}
		return string.Empty;
	}

	public static string GetEquipAEStr(ESubTypeEquip type)
	{
		return Singleton<StringManager>.Instance.GetString(string.Format("EEAF_{0}", (int)type));
	}

	public static string GetEquipARStr(ESubTypeEquip type)
	{
		return Singleton<StringManager>.Instance.GetString(string.Format("EEAR_{0}", (int)type));
	}

	public static string GetTrinketAEStr(ItemDataEx data, int num)
	{
		if (data.Info.Value5 == 0)
		{
			return Singleton<StringManager>.Instance.GetString(string.Format("ETAF{0}_{1}", num, data.Info.SubType));
		}
		return Singleton<StringManager>.Instance.GetString(string.Format("ETAR{0}_{1}", num, data.Info.SubType));
	}

	public static string GetTrinketARStr(ItemDataEx data, int num)
	{
		if (data.Info.Value5 == 0)
		{
			return Singleton<StringManager>.Instance.GetString(string.Format("ETAR{0}_{1}", num, data.Info.SubType));
		}
		return Singleton<StringManager>.Instance.GetString(string.Format("ETAF{0}_{1}", num, data.Info.SubType));
	}

	public static void SetButtonState(GameObject btn, bool isEnable)
	{
		UIButton[] components = btn.GetComponents<UIButton>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].SetState((!isEnable) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, true);
		}
	}

	public static void SetButtonSprite(UIButton btn, string spName)
	{
		btn.normalSprite = spName;
		btn.hoverSprite = spName;
		btn.pressedSprite = spName;
		btn.disabledSprite = spName;
	}

	public static bool CanPlay(int level, bool local = true)
	{
		if (local)
		{
			return (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)level);
		}
		return Globals.Instance.Player.TeamSystem.GetRemoteLevel() >= level;
	}

	public static bool CanBuy(ECurrencyType type, int cost)
	{
		return Tools.GetCurrencyMoney(type, 0) >= cost;
	}

	public static string GetRewardIcon(ERewardType type)
	{
		switch (type)
		{
		case ERewardType.EReward_Money:
			return "M101";
		case ERewardType.EReward_Diamond:
			return "M102";
		case ERewardType.EReward_Exp:
			return "exp";
		case ERewardType.EReward_GuildRepution:
			return "M103";
		case ERewardType.EReward_MagicCrystal:
			return "M107";
		case ERewardType.EReward_MagicSoul:
			return "M105";
		case ERewardType.EReward_FireDragonScale:
			return "M109";
		case ERewardType.EReward_KingMedal:
			return "M108";
		case ERewardType.EReward_StarSoul:
			return "I118";
		case ERewardType.EReward_Honor:
			return "M106";
		case ERewardType.EReward_Emblem:
			return "M110";
		case ERewardType.EReward_LopetSoul:
			return "M111";
		case ERewardType.EReward_FestivalVoucher:
			return "M112";
		}
		return string.Empty;
	}

	public static string GetRewardFrame(ERewardType type)
	{
		switch (type)
		{
		case ERewardType.EReward_Money:
			return Tools.GetItemQualityIcon(0);
		case ERewardType.EReward_Diamond:
			return Tools.GetItemQualityIcon(2);
		case ERewardType.EReward_Exp:
			return Tools.GetItemQualityIcon(2);
		case ERewardType.EReward_GuildRepution:
			return Tools.GetItemQualityIcon(2);
		case ERewardType.EReward_MagicCrystal:
			return Tools.GetItemQualityIcon(2);
		case ERewardType.EReward_MagicSoul:
			return Tools.GetItemQualityIcon(2);
		case ERewardType.EReward_FireDragonScale:
			return Tools.GetItemQualityIcon(2);
		case ERewardType.EReward_KingMedal:
			return Tools.GetItemQualityIcon(2);
		case ERewardType.EReward_StarSoul:
			return Tools.GetItemQualityIcon(2);
		case ERewardType.EReward_Honor:
			return Tools.GetItemQualityIcon(2);
		case ERewardType.EReward_Emblem:
			return Tools.GetItemQualityIcon(2);
		case ERewardType.EReward_LopetSoul:
			return Tools.GetItemQualityIcon(2);
		case ERewardType.EReward_FestivalVoucher:
			return Tools.GetItemQualityIcon(2);
		}
		return string.Empty;
	}

	public static Color GetRewardNameColor(ERewardType type)
	{
		switch (type)
		{
		case ERewardType.EReward_Money:
			return Tools.GetItemQualityColor(0);
		case ERewardType.EReward_Diamond:
			return Tools.GetItemQualityColor(2);
		case ERewardType.EReward_Exp:
			return Tools.GetItemQualityColor(2);
		case ERewardType.EReward_GuildRepution:
			return Tools.GetItemQualityColor(2);
		case ERewardType.EReward_MagicCrystal:
			return Tools.GetItemQualityColor(2);
		case ERewardType.EReward_MagicSoul:
			return Tools.GetItemQualityColor(2);
		case ERewardType.EReward_FireDragonScale:
			return Tools.GetItemQualityColor(2);
		case ERewardType.EReward_KingMedal:
			return Tools.GetItemQualityColor(2);
		case ERewardType.EReward_StarSoul:
			return Tools.GetItemQualityColor(2);
		case ERewardType.EReward_Honor:
			return Tools.GetItemQualityColor(2);
		case ERewardType.EReward_Emblem:
			return Tools.GetItemQualityColor(2);
		case ERewardType.EReward_LopetSoul:
			return Tools.GetItemQualityColor(2);
		case ERewardType.EReward_FestivalVoucher:
			return Tools.GetItemQualityColor(2);
		}
		return Tools.GetItemQualityColor(0);
	}

	public static uint GetTrinketEnhanceItemExp(int quality)
	{
		QualityInfo info = Globals.Instance.AttDB.QualityDict.GetInfo(quality + 1);
		if (info == null)
		{
			global::Debug.LogErrorFormat("QualityDict GetInfo Error : {0}", new object[]
			{
				quality + 1
			});
		}
		return info.TrinketExp;
	}

	public static bool IsPetBagFull()
	{
		if (Globals.Instance.Player.PetSystem.Values.Count >= GameConst.GetInt32(233))
		{
			GUIBagFullPopUp.Show(GUIBagFullPopUp.EBagType.EBT_Pet);
			return true;
		}
		return false;
	}

	public static bool IsEquipBagFull()
	{
		int num = 0;
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (current.Info.Type == 0)
			{
				num++;
			}
		}
		if (num >= GameConst.GetInt32(234))
		{
			GUIBagFullPopUp.Show(GUIBagFullPopUp.EBagType.EBT_Equip);
			return true;
		}
		return false;
	}

	public static bool IsTrinketBagFull()
	{
		int num = 0;
		foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
		{
			if (current.Info.Type == 1 || (current.Info.Type == 4 && current.Info.SubType == 9))
			{
				num++;
			}
		}
		if (num >= GameConst.GetInt32(235))
		{
			GUIBagFullPopUp.Show(GUIBagFullPopUp.EBagType.EBT_Trinket);
			return true;
		}
		return false;
	}

	public static bool IsLopetBagFull()
	{
		if (Globals.Instance.Player.LopetSystem.Values.Count >= GameConst.GetInt32(241))
		{
			GUIBagFullPopUp.Show(GUIBagFullPopUp.EBagType.EBT_Pet);
			return true;
		}
		return false;
	}

	public static int GetFDSMaxId()
	{
		int num = 1;
		int num2 = Globals.Instance.Player.TeamSystem.GetCombatValue() * 100;
		while (true)
		{
			FDSInfo info = Globals.Instance.AttDB.FDSDict.GetInfo(num);
			if (info == null)
			{
				break;
			}
			if (info.FireDragonScale <= num2)
			{
				num++;
			}
			else
			{
				if (num % 6 == 0)
				{
					return num;
				}
				num++;
			}
		}
		num--;
		return num;
	}

	public static bool IsFDSCanTaken()
	{
		bool result = false;
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		int fDSMaxId = Tools.GetFDSMaxId();
		for (int i = 1; i <= fDSMaxId; i++)
		{
			FDSInfo info = Globals.Instance.AttDB.FDSDict.GetInfo(i);
			if (info != null && !worldBossSystem.IsFDSRewardTaken(info.ID) && info.FireDragonScale <= Globals.Instance.Player.Data.FireDragonScale)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public static bool IsWBRewardCanTaken()
	{
		bool result = false;
		WorldBossSubSystem worldBossSystem = Globals.Instance.Player.WorldBossSystem;
		foreach (WorldRespawnInfo current in Globals.Instance.AttDB.WorldRespawnDict.Values)
		{
			if (current != null)
			{
				if (worldBossSystem.IsWBRewrdCanTaken(current.ID) && !worldBossSystem.IsWBRewardTaken(current.ID))
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	public static uint GetPetStarAndLvl(uint awakeLvl, out uint lvl)
	{
		lvl = awakeLvl % 10u;
		return awakeLvl / 10u;
	}

	public static LegendInfo GetLegendInfo(ItemInfo info)
	{
		if (info == null)
		{
			return null;
		}
		return Globals.Instance.AttDB.LegendDict.GetInfo(info.Quality * 10000 + info.Type * 100 + info.SubType);
	}

	public static string GetLegendSkillStr(LegendInfo info, int level)
	{
		if (info == null)
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		while (num < info.Desc.Count && num < info.RefineLevel.Count)
		{
			if (info.EffectType[num] > 0)
			{
				if (info.RefineLevel[num] <= level)
				{
					stringBuilder.AppendLine(Tools.GetColorHex(Color.green, Singleton<StringManager>.Instance.GetString("equipImprove80", new object[]
					{
						info.Desc[num],
						info.RefineLevel[num]
					})));
				}
				else
				{
					stringBuilder.AppendLine(Tools.GetColorHex(Color.gray, Singleton<StringManager>.Instance.GetString("equipImprove80", new object[]
					{
						info.Desc[num],
						info.RefineLevel[num]
					})));
				}
			}
			num++;
		}
		return stringBuilder.ToString().Trim();
	}

	public static string GetNextLegendSkillStr(LegendInfo lInfo, int level)
	{
		if (lInfo == null)
		{
			return string.Empty;
		}
		string result = string.Empty;
		int num = 2147483647;
		int num2 = 0;
		while (num2 < lInfo.Desc.Count && num2 < lInfo.RefineLevel.Count)
		{
			if (lInfo.EffectType[num2] > 0)
			{
				int num3 = lInfo.RefineLevel[num2];
				if (num3 > level && num3 < num)
				{
					num = num3;
					result = Singleton<StringManager>.Instance.GetString("equipImprove81", new object[]
					{
						lInfo.Desc[num2],
						num3
					});
				}
			}
			num2++;
		}
		return result;
	}

	public static RemotePlayer LocalPlayerToRemote()
	{
		RemotePlayer remotePlayer = new RemotePlayer();
		remotePlayer.FashionID = Globals.Instance.Player.GetCurFashionID();
		remotePlayer.Gender = Globals.Instance.Player.Data.Gender;
		remotePlayer.GUID = Globals.Instance.Player.Data.ID;
		remotePlayer.Name = Globals.Instance.Player.Data.Name;
		remotePlayer.Level = (int)Globals.Instance.Player.Data.Level;
		remotePlayer.ConstellationLevel = Globals.Instance.Player.Data.ConstellationLevel;
		remotePlayer.CombatValue = Globals.Instance.Player.TeamSystem.GetCombatValue();
		int i = 0;
		remotePlayer.PetInfoID.Clear();
		for (int j = 1; j < 4; j++)
		{
			PetDataEx pet = Globals.Instance.Player.TeamSystem.GetPet(j);
			if (pet != null)
			{
				remotePlayer.PetInfoID.Add(pet.Info.ID);
				i++;
			}
		}
		while (i < 3)
		{
			remotePlayer.PetInfoID.Add(0);
			i++;
		}
		return remotePlayer;
	}

	public static bool CanShowJiJieMark(PetDataEx pdEx)
	{
		if (Tools.CanPlay(GameConst.GetInt32(12), true))
		{
			if (pdEx == null)
			{
				return false;
			}
			uint furtherNeedLvl = pdEx.GetFurtherNeedLvl();
			if (pdEx.Data.Level < furtherNeedLvl)
			{
				return false;
			}
			int maxFurther = pdEx.GetMaxFurther(true);
			int further = (int)pdEx.Data.Further;
			int num;
			int num2;
			int num3;
			int num4;
			int num5;
			pdEx.GetFurtherData(out num, out num2, out num3, out num4, out num5);
			int money = Globals.Instance.Player.Data.Money;
			if (further < maxFurther && money >= num3 && num >= num2 && num4 >= num5)
			{
				return true;
			}
		}
		return false;
	}

	public static bool CanShowJueXingMark(PetDataEx pdEx)
	{
		ItemSubSystem itemSystem = Globals.Instance.Player.ItemSystem;
		if (itemSystem == null)
		{
			return false;
		}
		if (Tools.CanPlay(GameConst.GetInt32(24), true))
		{
			if (pdEx == null)
			{
				return false;
			}
			uint num = 0u;
			Tools.GetPetStarAndLvl(pdEx.Data.Awake, out num);
			int awakeItemID = pdEx.GetAwakeItemID(0);
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(awakeItemID);
			bool flag = info == null || pdEx.IsAwakeItemEquip(0) || itemSystem.GetItemCount(awakeItemID) > 0;
			int awakeItemID2 = pdEx.GetAwakeItemID(1);
			ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(awakeItemID2);
			bool flag2 = info2 == null || pdEx.IsAwakeItemEquip(1) || itemSystem.GetItemCount(awakeItemID2) > 0;
			int awakeItemID3 = pdEx.GetAwakeItemID(2);
			ItemInfo info3 = Globals.Instance.AttDB.ItemDict.GetInfo(awakeItemID3);
			bool flag3 = info3 == null || pdEx.IsAwakeItemEquip(2) || itemSystem.GetItemCount(awakeItemID3) > 0;
			int awakeItemID4 = pdEx.GetAwakeItemID(3);
			ItemInfo info4 = Globals.Instance.AttDB.ItemDict.GetInfo(awakeItemID4);
			bool flag4 = info4 == null || pdEx.IsAwakeItemEquip(3) || itemSystem.GetItemCount(awakeItemID4) > 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			pdEx.GetAwakeLevelupData(out num2, out num3, out num4, out num5);
			int furtherPetCount = Globals.Instance.Player.PetSystem.GetFurtherPetCount(pdEx.Data.ID, pdEx.Data.InfoID);
			int money = Globals.Instance.Player.Data.Money;
			uint jueXingNeedLvl = pdEx.GetJueXingNeedLvl();
			if (pdEx.Data.Level >= jueXingNeedLvl && pdEx.Data.Awake < 50u && money >= num2 && num5 >= num4 && (num3 == 0 || furtherPetCount >= num3) && flag && flag2 && flag3 && flag4)
			{
				return true;
			}
		}
		return false;
	}

	public static bool HasUnBattlePet()
	{
		bool result = false;
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (!current.IsBattling() && !current.IsPetAssisting())
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public static bool HasBetterUnBattlePet()
	{
		bool result = false;
		foreach (PetDataEx current in Globals.Instance.Player.PetSystem.Values)
		{
			if (!current.IsBattling() && !current.IsPetAssisting() && current.Relation > 0)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public static bool IsAssistUnlocked(int index)
	{
		return (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)(GameConst.GetInt32(197) + index * 5));
	}

	private static int SortEquipItems(ItemDataEx a, ItemDataEx b)
	{
		if (a.RelationCount > b.RelationCount)
		{
			return -1;
		}
		if (a.RelationCount < b.RelationCount)
		{
			return 1;
		}
		if (a.Info.Quality > b.Info.Quality)
		{
			return -1;
		}
		if (a.Info.Quality < b.Info.Quality)
		{
			return 1;
		}
		if (a.Info.SubQuality > b.Info.SubQuality)
		{
			return -1;
		}
		if (a.Info.SubQuality < b.Info.SubQuality)
		{
			return 1;
		}
		if (a.GetEquipRefineLevel() > b.GetEquipRefineLevel())
		{
			return -1;
		}
		if (a.GetEquipRefineLevel() < b.GetEquipRefineLevel())
		{
			return 1;
		}
		if (a.GetEquipEnhanceLevel() > b.GetEquipEnhanceLevel())
		{
			return -1;
		}
		if (a.GetEquipEnhanceLevel() < b.GetEquipEnhanceLevel())
		{
			return 1;
		}
		if (a.GetEquipPet() != null && b.GetEquipPet() == null)
		{
			return -1;
		}
		if (a.GetEquipPet() == null && b.GetEquipPet() != null)
		{
			return 1;
		}
		return a.Info.ID - b.Info.ID;
	}

	private static ItemDataEx GetGoodEquipItemEx(int slotIndex, int index)
	{
		ItemDataEx result = null;
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem == null)
		{
			return result;
		}
		SocketDataEx socket = teamSystem.GetSocket(slotIndex, true);
		if (socket == null)
		{
			return result;
		}
		PetDataEx pet = socket.GetPet();
		if (socket != null && pet != null)
		{
			List<int> relationEquip = socket.GetRelationEquip(index);
			ItemDataEx equip = socket.GetEquip(index);
			List<ItemDataEx> list = new List<ItemDataEx>();
			if (0 <= index && index < 4)
			{
				foreach (ItemDataEx current in Globals.Instance.Player.ItemSystem.Values)
				{
					if ((!current.IsEquiped() || (equip != null && current.Data.ID == equip.Data.ID)) && current.Info.Type == 0 && current.Info.SubType == index)
					{
						current.ClearUIData();
						if (current.CanActiveRelation(relationEquip))
						{
							current.RelationCount = 1;
						}
						list.Add(current);
					}
				}
			}
			else if (4 <= index && index < 6 && (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(25)))
			{
				foreach (ItemDataEx current2 in Globals.Instance.Player.ItemSystem.Values)
				{
					if ((!current2.IsEquiped() || (equip != null && current2.Data.ID == equip.Data.ID)) && current2.Info.Type == 1 && current2.Info.SubType == index - 4)
					{
						current2.ClearUIData();
						if (current2.CanActiveRelation(relationEquip))
						{
							current2.RelationCount = 1;
						}
						list.Add(current2);
					}
				}
			}
			list.Sort(new Comparison<ItemDataEx>(Tools.SortEquipItems));
			if (list.Count != 0)
			{
				result = list[0];
			}
		}
		return result;
	}

	private static int SortSameItems(ItemDataEx a, ItemDataEx b)
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem == null)
		{
			return 0;
		}
		SocketDataEx socket = teamSystem.GetSocket(Tools.SortSameSocketIndex, true);
		if (socket == null)
		{
			return 0;
		}
		PetDataEx pet = socket.GetPet();
		if (pet == null)
		{
			return 0;
		}
		if (a.GetEquipPet() != null && a.GetEquipPet().Data.ID == pet.Data.ID)
		{
			return -1;
		}
		if (b.GetEquipPet() != null && b.GetEquipPet().Data.ID == pet.Data.ID)
		{
			return 1;
		}
		if (a.GetEquipRefineLevel() > b.GetEquipRefineLevel())
		{
			return -1;
		}
		if (a.GetEquipRefineLevel() < b.GetEquipRefineLevel())
		{
			return 1;
		}
		if (a.GetEquipEnhanceLevel() > b.GetEquipEnhanceLevel())
		{
			return -1;
		}
		if (a.GetEquipEnhanceLevel() < b.GetEquipEnhanceLevel())
		{
			return 1;
		}
		return 0;
	}

	public static ItemDataEx[] GetGoodEquips(int slotIndex)
	{
		ItemDataEx[] array = new ItemDataEx[6];
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem == null)
		{
			return array;
		}
		ItemSubSystem itemSystem = Globals.Instance.Player.ItemSystem;
		if (itemSystem == null)
		{
			return array;
		}
		SocketDataEx socket = teamSystem.GetSocket(slotIndex, true);
		if (socket == null)
		{
			return array;
		}
		PetDataEx pet = socket.GetPet();
		if (pet == null)
		{
			return array;
		}
		if (0 <= slotIndex && slotIndex <= 3)
		{
			for (int i = 0; i < 6; i++)
			{
				array[i] = Tools.GetGoodEquipItemEx(slotIndex, i);
			}
			for (int j = 0; j < 4; j++)
			{
				ItemDataEx itemDataEx = array[j];
				if (itemDataEx != null)
				{
					List<int> relationEquip = socket.GetRelationEquip(j);
					if (itemDataEx.CanActiveRelation(relationEquip))
					{
						ItemSetInfo info = Globals.Instance.AttDB.ItemSetDict.GetInfo(itemDataEx.Info.Value5);
						if (info != null)
						{
							for (int k = 0; k < info.ItemID.Count; k++)
							{
								if (info.ItemID[k] != 0)
								{
									if (itemDataEx.Info.ID != info.ItemID[k])
									{
										List<ItemDataEx> itemsByInfoID = itemSystem.GetItemsByInfoID(info.ItemID[k]);
										if (itemsByInfoID.Count != 0)
										{
											Tools.SortSameSocketIndex = slotIndex;
											itemsByInfoID.Sort(new Comparison<ItemDataEx>(Tools.SortSameItems));
											for (int l = 0; l < itemsByInfoID.Count; l++)
											{
												ItemDataEx itemDataEx2 = itemsByInfoID[l];
												if (itemDataEx2 != null)
												{
													if (itemDataEx2.Info.Type == 0)
													{
														if (itemDataEx2.GetEquipPet() == null || itemDataEx2.GetEquipPet() == pet)
														{
															ItemDataEx itemDataEx3 = array[itemDataEx2.Info.SubType];
															if (itemDataEx3 == null || itemDataEx3.Info.ID != itemDataEx2.Info.ID)
															{
																if (0 <= itemDataEx2.Info.SubType && itemDataEx2.Info.SubType <= 3)
																{
																	array[itemDataEx2.Info.SubType] = itemDataEx2;
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return array;
	}

	public static bool CanBattlePetHasBetterPet(int slotIndex)
	{
		PetSubSystem petSystem = Globals.Instance.Player.PetSystem;
		if (petSystem == null)
		{
			return false;
		}
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem == null)
		{
			return false;
		}
		if (teamSystem.GetSocket(slotIndex, true) == null)
		{
			return false;
		}
		PetDataEx pet = teamSystem.GetPet(slotIndex);
		if (0 < slotIndex && slotIndex <= 3)
		{
			int num = (pet == null) ? -1 : pet.Info.Quality;
			foreach (PetDataEx current in petSystem.Values)
			{
				if (!current.IsBattling() && !current.IsPetAssisting() && !Globals.Instance.Player.TeamSystem.HasPetInfoID(current.Info.ID))
				{
					if (current.Info.Quality > num)
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	public static bool CanBattlePetHasBetterPet2(int slotIndex)
	{
		PetSubSystem petSystem = Globals.Instance.Player.PetSystem;
		if (petSystem == null)
		{
			return false;
		}
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem == null)
		{
			return false;
		}
		if (teamSystem.GetSocket(slotIndex, true) == null)
		{
			return false;
		}
		PetDataEx pet = teamSystem.GetPet(slotIndex);
		if (pet == null)
		{
			return false;
		}
		if (0 < slotIndex && slotIndex <= 3)
		{
			int num = (pet == null) ? 100 : pet.Info.Quality;
			foreach (PetDataEx current in petSystem.Values)
			{
				if (!current.IsBattling() && !current.IsPetAssisting() && !Globals.Instance.Player.TeamSystem.HasPetInfoID(current.Info.ID))
				{
					if (current.Info.Quality > num)
					{
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	public static bool CanBattlePetMark(int slotIndex)
	{
		if (Globals.Instance.Player.PetSystem == null)
		{
			return false;
		}
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem == null)
		{
			return false;
		}
		if (slotIndex == 0)
		{
			if (teamSystem.GetSocket(slotIndex, true) == null)
			{
				return false;
			}
			PetDataEx pet = teamSystem.GetPet(slotIndex);
			if (pet == null)
			{
				return false;
			}
			if (GameCache.Data.HasNewFashion)
			{
				return true;
			}
			if (Tools.CanShowJiJieMark(pet))
			{
				return true;
			}
			if (Tools.CanShowJueXingMark(pet))
			{
				return true;
			}
			for (int i = 0; i < 6; i++)
			{
				if (Tools.CanEquipShowMark(0, i))
				{
					return true;
				}
			}
			return false;
		}
		else if (0 < slotIndex && slotIndex <= 3)
		{
			if (teamSystem.GetSocket(slotIndex, true) == null)
			{
				return false;
			}
			PetDataEx pet2 = teamSystem.GetPet(slotIndex);
			if (Tools.CanBattlePetHasBetterPet(slotIndex))
			{
				return true;
			}
			if (Tools.CanShowJiJieMark(pet2))
			{
				return true;
			}
			if (Tools.CanShowJueXingMark(pet2))
			{
				return true;
			}
			for (int j = 0; j < 6; j++)
			{
				if (Tools.CanEquipShowMark(slotIndex, j))
				{
					return true;
				}
			}
			return false;
		}
		else
		{
			if (slotIndex == 4)
			{
				return Globals.Instance.Player.LopetSystem.HasLopet2Change() || Tools.CanCurLopetAwake() || Tools.CanCurLopetLevelup();
			}
			return slotIndex == 5 && Tools.CanAssistShowMark();
		}
	}

	public static bool CanAssistPetShowMark(int mIndex, PetDataEx pdEx)
	{
		return Tools.IsAssistUnlocked(mIndex) && Globals.Instance.Player.TeamSystem != null && Tools.HasUnBattlePet() && pdEx == null;
	}

	public static bool CanAssistShowMark()
	{
		TeamSubSystem teamSystem = Globals.Instance.Player.TeamSystem;
		if (teamSystem == null)
		{
			return false;
		}
		if (!Tools.HasUnBattlePet())
		{
			return false;
		}
		for (int i = 0; i < 6; i++)
		{
			PetDataEx assist = teamSystem.GetAssist(i, true);
			if (Tools.CanAssistPetShowMark(i, assist))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsPetEquipSlotLocked(int equipIndex)
	{
		uint level = Globals.Instance.Player.Data.Level;
		return (ulong)level < (ulong)((long)GameConst.GetInt32(25)) && (equipIndex == 4 || equipIndex == 5);
	}

	public static bool CanEquipShowMark(int slotIndex, int equipIndex)
	{
		ItemDataEx[] goodEquips = Tools.GetGoodEquips(slotIndex);
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(slotIndex);
		if (socket == null)
		{
			return false;
		}
		ItemDataEx equip = socket.GetEquip(equipIndex);
		if (equip == null)
		{
			return goodEquips[equipIndex] != null && !Tools.IsPetEquipSlotLocked(equipIndex);
		}
		return goodEquips[equipIndex] != null && goodEquips[equipIndex].Data.ID != equip.Data.ID && !Tools.IsPetEquipSlotLocked(equipIndex);
	}

	public static bool CanPetLvlUp(PetDataEx pdEx)
	{
		if (pdEx == null)
		{
			return false;
		}
		if (pdEx.Data.ID == 100uL)
		{
			return false;
		}
		int money = Globals.Instance.Player.Data.Money;
		int num = 0;
		uint num2 = 0u;
		uint maxExp = pdEx.GetMaxExp();
		if (maxExp >= pdEx.Data.Exp)
		{
			num2 = maxExp - pdEx.Data.Exp;
		}
		int num3 = 4;
		int[] array = new int[4];
		int[] array2 = new int[4];
		int[] array3 = new int[4];
		for (int i = 0; i < num3; i++)
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.PET_EXP_ITEM_ID[i]);
			if (info != null)
			{
				array[i] = Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.PET_EXP_ITEM_ID[i]);
				array2[i] = ((array[i] <= 0) ? 0 : info.Value1);
				int num4 = array[i] * array2[i];
				array3[i] = ((num4 < 0) ? 2147483647 : num4);
			}
			if (array[i] != 0)
			{
				if (array2[i] > 0)
				{
					int num5 = (int)(num2 / (uint)array2[i]);
					int num6 = (int)(num2 % (uint)array2[i]);
					if (num6 != 0)
					{
						num5++;
					}
					if (array[i] >= num5)
					{
						num += num5 * array2[i];
						break;
					}
					num += array[i] * array2[i];
					num2 -= (uint)(array[i] * array2[i]);
					if (num2 <= 0u)
					{
						break;
					}
				}
			}
		}
		num /= 5;
		bool flag = false;
		long num7 = 0L;
		int num8 = (int)(pdEx.GetMaxExp() - pdEx.Data.Exp);
		for (int j = 0; j < num3; j++)
		{
			num7 += (long)array3[j];
			if (num7 >= (long)num8)
			{
				flag = true;
				break;
			}
		}
		return money >= num && pdEx.Data.Level < Globals.Instance.Player.Data.Level && flag;
	}

	public static bool CanPetSkillLvlUp(PetDataEx pdEx)
	{
		if (pdEx == null)
		{
			return false;
		}
		if (pdEx.Data.ID == 100uL)
		{
			return false;
		}
		if (pdEx.Data.Further >= 3u)
		{
			int num = pdEx.GetSkillLevel(0) + 1;
			if (num < GameConst.GetInt32(232) + 1)
			{
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				pdEx.GetSkillCost(0, out num2, out num3, out num4);
				if (Globals.Instance.Player.Data.Money >= num4 && num2 >= num3)
				{
					return true;
				}
			}
			for (int i = 0; i < 3; i++)
			{
				SkillInfo skillInfo = pdEx.GetSkillInfo(1 + i);
				if (skillInfo != null && skillInfo.ID != 0)
				{
					bool flag = i == 0 || (ulong)pdEx.Data.Further > (ulong)((long)(i + 1));
					int num5 = pdEx.GetSkillLevel(1 + i) + 1;
					if (num5 < GameConst.GetInt32(232) + 1)
					{
						int num6 = 0;
						int num7 = 0;
						int num8 = 0;
						pdEx.GetSkillCost(1 + i, out num6, out num7, out num8);
						if (Globals.Instance.Player.Data.Money >= num8 && num6 >= num7 && flag)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public static bool CanPetPeiYang(PetDataEx pdEx)
	{
		if (Tools.CanPlay(GameConst.GetInt32(122), true))
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(178));
			if (info != null)
			{
				int itemCount = Globals.Instance.Player.ItemSystem.GetItemCount(info.ID);
				int @int = GameConst.GetInt32(179);
				return itemCount >= @int;
			}
		}
		return false;
	}

	public static bool IsAllGiftReceived(bool isShowMsg)
	{
		if (Globals.Instance.Player.Data.TakeGuildGift == GameConst.GetInt32(166))
		{
			if (isShowMsg)
			{
				GameUIManager.mInstance.ShowMessageTip("EGR", 96);
			}
			return true;
		}
		bool result = true;
		for (int i = 0; i < Globals.Instance.Player.GuildSystem.Members.Count; i++)
		{
			GuildMember guildMember = Globals.Instance.Player.GuildSystem.Members[i];
			if (guildMember.ID != Globals.Instance.Player.Data.ID && (guildMember.Flag & 1) == 0)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public static bool IsAllGiftSended()
	{
		bool result = true;
		for (int i = 0; i < Globals.Instance.Player.GuildSystem.Members.Count; i++)
		{
			GuildMember guildMember = Globals.Instance.Player.GuildSystem.Members[i];
			if (guildMember.ID != Globals.Instance.Player.Data.ID && (guildMember.Flag & 2) == 0)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public static bool IsInGuildBossTime()
	{
		if (Globals.Instance.Player.GetTimeStamp() < Globals.Instance.Player.GuildSystem.GuildBossGMOpenTime)
		{
			return true;
		}
		DateTime dateTime = Tools.ServerDateTime(Globals.Instance.Player.GetTimeStamp());
		return dateTime.Hour >= 16 && dateTime.Hour < 22;
	}

	public static bool IsGuildBossHasNum()
	{
		if (Globals.Instance == null)
		{
			return false;
		}
		int num = GameConst.GetInt32(150) - Globals.Instance.Player.Data.GuildBossCount;
		return num > 0;
	}

	public static bool HasNewPet(int infoID)
	{
		return Globals.Instance.Player.PetSystem.GetNewPetByInfoID(infoID) != null;
	}

	public static bool IsLopetProps(ItemDataEx data)
	{
		return Tools.IsLopetProps(data.Info);
	}

	public static bool IsLopetProps(ItemInfo info)
	{
		return info.Type == 4 && (info.SubType == 12 || info.SubType == 11);
	}

	public static bool IsLopetFragment(ItemDataEx data)
	{
		return Tools.IsLopetFragment(data.Info);
	}

	public static bool IsLopetFragment(ItemInfo info)
	{
		return info.Type == 3 && info.SubType == 3;
	}

	public static bool CanCurLopetLevelup()
	{
		return Tools.CanLopetLevelUp(Globals.Instance.Player.LopetSystem.GetCurLopet(true));
	}

	public static bool CanLopetLevelUp(LopetDataEx ldEx)
	{
		if (!Tools.CanPlay(GameConst.GetInt32(201), true))
		{
			return false;
		}
		if (ldEx == null)
		{
			return false;
		}
		if ((ulong)ldEx.Data.Level >= (ulong)((long)GameConst.GetInt32(240)))
		{
			return false;
		}
		uint maxExp = ldEx.GetMaxExp();
		int money = Globals.Instance.Player.Data.Money;
		int num = 0;
		uint num2 = 0u;
		if (maxExp >= ldEx.Data.Exp)
		{
			num2 = maxExp - ldEx.Data.Exp;
		}
		if ((long)money < (long)((ulong)num2))
		{
			return false;
		}
		for (int i = 0; i < GameConst.LOPET_EXP_ITEM_ID.Length; i++)
		{
			ItemDataEx itemByInfoID = Globals.Instance.Player.ItemSystem.GetItemByInfoID(GameConst.LOPET_EXP_ITEM_ID[i]);
			if (itemByInfoID != null)
			{
				int value = itemByInfoID.Info.Value1;
				if (value > 0)
				{
					int num3 = (int)(num2 / (uint)value);
					int num4 = (int)(num2 % (uint)value);
					if (num4 != 0)
					{
						num3++;
					}
					if (itemByInfoID.GetCount() >= num3)
					{
						num += num3 * value;
						break;
					}
					num += itemByInfoID.GetCount() * value;
					num2 -= (uint)(itemByInfoID.GetCount() * value);
					if (num2 <= 0u)
					{
						break;
					}
				}
			}
		}
		num /= 5;
		bool flag = money >= num;
		bool flag2 = false;
		long num5 = 0L;
		int num6 = (int)(ldEx.GetMaxExp() - ldEx.Data.Exp);
		for (int j = 0; j < GameConst.LOPET_EXP_ITEM_ID.Length; j++)
		{
			ItemDataEx itemByInfoID = Globals.Instance.Player.ItemSystem.GetItemByInfoID(GameConst.LOPET_EXP_ITEM_ID[j]);
			if (itemByInfoID != null)
			{
				num5 += (long)(itemByInfoID.GetCount() * itemByInfoID.Info.Value1);
				if (num5 >= (long)num6)
				{
					flag2 = true;
					break;
				}
			}
		}
		return flag && flag2;
	}

	public static bool CanCurLopetAwake()
	{
		return Tools.CanLopetAwake(Globals.Instance.Player.LopetSystem.GetCurLopet(true));
	}

	public static bool CanLopetAwake(LopetDataEx ldEx)
	{
		if (!Tools.CanPlay(GameConst.GetInt32(201), true))
		{
			return false;
		}
		if (ldEx == null)
		{
			return false;
		}
		if ((ulong)ldEx.Data.Awake >= (ulong)((long)GameConst.GetInt32(251)))
		{
			return false;
		}
		uint awakeNeedLvl = ldEx.GetAwakeNeedLvl();
		if (ldEx.Data.Level < awakeNeedLvl)
		{
			return false;
		}
		int num;
		int num2;
		int num3;
		int num4;
		int num5;
		ldEx.GetFurtherData(out num, out num2, out num3, out num4, out num5);
		return Globals.Instance.Player.Data.Money >= num3 && num >= num2 && num4 >= num5;
	}
}
