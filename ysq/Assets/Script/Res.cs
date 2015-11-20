using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Res
{
	private static string GetCachedBundleFilePath(DownloadManager dm, List<int> dependIndexList)
	{
		string name = dm.BuiltBundles[dependIndexList[dependIndexList.Count - 1]].name;
		return dm.GetCachedBundleFilePath(name);
	}

	private static AssetBundle LoadAssetBundleComplete(DownloadManager dm, List<int> dependIndexList)
	{
		AssetBundle result = null;
		for (int i = 0; i < dependIndexList.Count; i++)
		{
			BundleDataPublish bundle = dm.BuiltBundles[dependIndexList[i]];
			result = dm.LoadAssetBundle(bundle);
		}
		return result;
	}

	private static AssetBundle LoadAssetFromBundle(string path)
	{
		DownloadManager instance = DownloadManager.Instance;
		List<int> dependList = instance.getDependList(path);
		if (dependList == null)
		{
			return null;
		}
		AssetBundle assetBundle = Res.LoadAssetBundleComplete(instance, dependList);
		if (assetBundle == null)
		{
			global::Debug.LogError(new object[]
			{
				"Resource Load From Download Error, AssetBundle Don't Load = " + path
			});
			return null;
		}
		return assetBundle;
	}

	public static void LoadScene(string path)
	{
		Res.LoadAssetFromBundle(path);
	}

	public static T Load<T>(string path, bool unload = false) where T : UnityEngine.Object
	{
		AssetBundle assetBundle = Res.LoadAssetFromBundle(path);
		if (assetBundle != null)
		{
			UnityEngine.Object @object = assetBundle.Load(Path.GetFileName(path), typeof(T));
			if (unload)
			{
				assetBundle.Unload(false);
			}
			return @object as T;
		}
		return (T)((object)null);
	}

	public static AsyncOperation LoadGUIAsync(string path)
	{
		return Resources.LoadAsync(path, typeof(GameObject));
	}

	public static GameObject LoadGUI(string path)
	{
		return Resources.Load<GameObject>(path);
	}

	public static UnityEngine.Object Load(string path)
	{
		return Res.Load<UnityEngine.Object>(path, false);
	}

	public static AsyncOperation LoadAsync(string path, Type type)
	{
		AssetBundle assetBundle = Res.LoadAssetFromBundle(path);
		if (assetBundle != null)
		{
			return assetBundle.LoadAsync(Path.GetFileName(path), type);
		}
		return null;
	}

	public static UnityEngine.Object GetAsset(AsyncOperation request)
	{
		if (request == null)
		{
			return null;
		}
		if (request is ResourceRequest)
		{
			return ((ResourceRequest)request).asset;
		}
		if (request is AssetBundleRequest)
		{
			return ((AssetBundleRequest)request).asset;
		}
		return null;
	}

	public static ulong GetAssetCrc(string path)
	{
		DownloadManager instance = DownloadManager.Instance;
		List<int> dependList = instance.getDependList(path);
		if (dependList == null)
		{
			return 0uL;
		}
		string cachedBundleFilePath = Res.GetCachedBundleFilePath(instance, dependList);
		if (!File.Exists(cachedBundleFilePath))
		{
			return 0uL;
		}
		ulong result;
		try
		{
			byte[] buffer = File.ReadAllBytes(cachedBundleFilePath);
			result = CRC32.GetCrc(buffer);
		}
		catch
		{
			result = 0uL;
		}
		return result;
	}

	public static byte[] LoadDataFromStreamPath(string fileName)
	{
		string text = string.Format("{0}/{1}", Application.streamingAssetsPath, fileName);
		MemoryStream memoryStream = new MemoryStream();
		try
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.net.URL", new object[]
			{
				text
			});
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("openConnection", new object[0]);
			int num = androidJavaObject2.Call<int>("getContentLength", new object[0]);
			if (num < 0)
			{
				num = 0;
			}
			int num2 = (num != 0) ? Mathf.Min(num, 32768) : 32768;
			byte[] array = new byte[num2];
			AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getInputStream", new object[0]);
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject3.GetRawClass(), "read", AndroidJNIHelper.GetSignature<int>(new object[]
			{
				typeof(byte[])
			}));
			jvalue[] array2 = AndroidJNIHelper.CreateJNIArgArray(new object[]
			{
				array
			});
			IntPtr rawObject = androidJavaObject3.GetRawObject();
			int count;
			while ((count = AndroidJNI.CallIntMethod(rawObject, methodID, array2)) != -1)
			{
				array = AndroidJNIHelper.ConvertFromJNIArray<byte[]>(array2[0].l);
				memoryStream.Write(array, 0, count);
			}
		}
		catch
		{
			return null;
		}
		return memoryStream.ToArray();
	}

	public static void ForceUnloadAssets(bool immediate)
	{
		Globals.Instance.BackgroundMusicMgr.ClearAll();
		Globals.Instance.EffectSoundMgr.ClearCache();
		Globals.Instance.ResourceCache.Clear();
		UIAtlasDynamic.ClearAllCachedAtlas();
		DownloadManager.Instance.ForceUnloadAssets(immediate);
		Resources.UnloadUnusedAssets();
	}
}
