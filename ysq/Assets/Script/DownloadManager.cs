using ICSharpCode.SharpZipLib.Zip;
using LitJson;
using NtUniSdk.Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class DownloadManager : MonoBehaviour
{
	private enum eZipCompFileType
	{
		EZCFT_UpdateData,
		EZCFT_BMData,
		EZCFT_TmpData,
		EZCFT_CompData,
		EZCFT_Max
	}

	private class ProgressData
	{
		public string version;

		public string fileName;

		public string url;

		public string md5;

		public long bundleSize;

		public bool exists;
	}

	private class UpdateBundleData
	{
		public string bundleName;

		public int processingIndex = -1;

		public int dataIndex = -1;
	}

	public class WWWRequest
	{
		public string bundleName;

		public string url = string.Empty;

		public int triedTimes;

		public int priority;

		public WWW www;

		public float startTime;

		public float curProgress;

		public void CreatWWW()
		{
			this.curProgress = 0f;
			this.startTime = Time.time;
			this.triedTimes++;
			this.www = new WWW(this.url);
		}
	}

	private delegate void DoUpdateListCallback(MemoryStream re, object parm);

	private const float TIMEOUT = 15f;

	private BMConfiger bmConfiger;

	private WWW curConfigWWW;

	private string downloadRootUrl;

	private string version;

	private string gameVersion;

	private string cacheDataPath;

	private List<BundleDataPublish> bundles;

	private Dictionary<string, BundleDataPublish> bundleDict = new Dictionary<string, BundleDataPublish>();

	private Dictionary<string, List<int>> assetsToBundleDict = new Dictionary<string, List<int>>();

	private Dictionary<string, string> cacheBundleMD5Dict;

	private List<DownloadManager.ProgressData> processingList = new List<DownloadManager.ProgressData>();

	private List<DownloadManager.UpdateBundleData> compressList = new List<DownloadManager.UpdateBundleData>();

	private Dictionary<string, DownloadManager.WWWRequest> processingRequest = new Dictionary<string, DownloadManager.WWWRequest>();

	private Dictionary<string, DownloadManager.WWWRequest> succeedRequest = new Dictionary<string, DownloadManager.WWWRequest>();

	private Dictionary<string, DownloadManager.WWWRequest> failedRequest = new Dictionary<string, DownloadManager.WWWRequest>();

	private List<DownloadManager.WWWRequest> waitingRequests = new List<DownloadManager.WWWRequest>();

	private Dictionary<BundleDataPublish, AssetBundle> assetBundles = new Dictionary<BundleDataPublish, AssetBundle>();

	private List<AssetBundle> unusedAssetBundles = new List<AssetBundle>();

	private bool pauseDownload;

	private bool needRestart;

	private bool fileError;

	private float compProgress;

	private long totalBundleSize;

	private float bundlesProgress;

	public bool ConfigLoaded
	{
		get
		{
			return this.downloadRootUrl != null && this.bundles != null && this.assetsToBundleDict != null && this.bmConfiger != null;
		}
	}

	private bool PauseDownload
	{
		get
		{
			return this.pauseDownload;
		}
		set
		{
			if (!this.pauseDownload && value)
			{
				this.ShowContinueUpdateMessageBox();
			}
			this.pauseDownload = value;
		}
	}

	public bool FileError
	{
		get
		{
			return this.fileError;
		}
		set
		{
			if (this.downloadRootUrl == null)
			{
				return;
			}
			if (!this.fileError && value)
			{
				this.ShowDiskFullMessageBox();
			}
			this.fileError = value;
		}
	}

	public bool NeedRestart
	{
		get
		{
			return this.needRestart;
		}
		set
		{
			this.needRestart = value;
		}
	}

	public Dictionary<string, DownloadManager.WWWRequest> BundlesProcessingRequest
	{
		get
		{
			return this.processingRequest;
		}
	}

	public Dictionary<string, DownloadManager.WWWRequest> BundlesSucceedRequest
	{
		get
		{
			return this.succeedRequest;
		}
	}

	public List<BundleDataPublish> BuiltBundles
	{
		get
		{
			return this.bundles;
		}
	}

	public Dictionary<string, string> CacheBundleMD5Dict
	{
		get
		{
			if (this.cacheBundleMD5Dict == null)
			{
				string cacheBundleMD5Path = this.GetCacheBundleMD5Path();
				this.cacheBundleMD5Dict = new Dictionary<string, string>();
				if (File.Exists(cacheBundleMD5Path))
				{
					try
					{
						StreamReader streamReader = new StreamReader(cacheBundleMD5Path, false);
						string text = streamReader.ReadLine();
						streamReader.Close();
						if (!string.IsNullOrEmpty(text))
						{
							string json = CryptHelper.AESDecrypt(text);
							this.cacheBundleMD5Dict = JsonMapper.ToObject<Dictionary<string, string>>(json);
						}
					}
					catch
					{
					}
				}
			}
			return this.cacheBundleMD5Dict;
		}
	}

	public Dictionary<string, List<int>> AssetsToBundleMap
	{
		get
		{
			return this.assetsToBundleDict;
		}
	}

	public Dictionary<string, BundleDataPublish> BundleDict
	{
		get
		{
			return this.bundleDict;
		}
	}

	public ulong AssemblyCrc
	{
		get;
		private set;
	}

	public ulong AttInfoCrc
	{
		get;
		private set;
	}

	public static DownloadManager Instance
	{
		get
		{
			return Globals.Instance.DownloadMgr;
		}
	}

	[DllImport("mono")]
	private unsafe static extern IntPtr* mono_gchandle_get_target(void* obj);

	public string GetError(string url)
	{
		url = this.formatUrl(url);
		if (this.failedRequest.ContainsKey(url))
		{
			return this.failedRequest[url].www.error;
		}
		return null;
	}

	public bool IsUrlRequested(string url)
	{
		url = this.formatUrl(url);
		return this.isInWaitingList(url) || this.processingRequest.ContainsKey(url) || this.succeedRequest.ContainsKey(url) || this.failedRequest.ContainsKey(url);
	}

	public WWW GetWWW(string url)
	{
		url = this.formatUrl(url);
		if (this.succeedRequest.ContainsKey(url))
		{
			return this.succeedRequest[url].www;
		}
		return null;
	}

	[DebuggerHidden]
	public IEnumerator WaitDownload(string url)
	{
        return null;
        //DownloadManager.<WaitDownload>c__Iterator0 <WaitDownload>c__Iterator = new DownloadManager.<WaitDownload>c__Iterator0();
        //<WaitDownload>c__Iterator.url = url;
        //<WaitDownload>c__Iterator.<$>url = url;
        //<WaitDownload>c__Iterator.<>f__this = this;
        //return <WaitDownload>c__Iterator;
	}

	[DebuggerHidden]
	public IEnumerator WaitDownload(string url, int priority)
	{
        return null;
        //DownloadManager.<WaitDownload>c__Iterator1 <WaitDownload>c__Iterator = new DownloadManager.<WaitDownload>c__Iterator1();
        //<WaitDownload>c__Iterator.url = url;
        //<WaitDownload>c__Iterator.priority = priority;
        //<WaitDownload>c__Iterator.<$>url = url;
        //<WaitDownload>c__Iterator.<$>priority = priority;
        //<WaitDownload>c__Iterator.<>f__this = this;
        //return <WaitDownload>c__Iterator;
	}

	public DownloadManager.WWWRequest StartDownload(string url)
	{
		return this.StartDownload(url, -1);
	}

	public DownloadManager.WWWRequest StartDownload(string url, int priority)
	{
		return this.download(new DownloadManager.WWWRequest
		{
			bundleName = Path.GetFileName(url),
			url = this.formatUrl(url),
			priority = priority
		});
	}

	public void StopDownload(string url)
	{
		url = this.formatUrl(url);
		this.waitingRequests.RemoveAll((DownloadManager.WWWRequest x) => x.url == url);
		if (this.processingRequest.ContainsKey(url))
		{
			this.processingRequest[url].www.Dispose();
			this.processingRequest.Remove(url);
		}
	}

	public void DisposeWWW(string url)
	{
		url = this.formatUrl(url);
		this.StopDownload(url);
		if (this.succeedRequest.ContainsKey(url))
		{
			this.succeedRequest[url].www.Dispose();
			this.succeedRequest.Remove(url);
		}
		if (this.failedRequest.ContainsKey(url))
		{
			this.failedRequest[url].www.Dispose();
			this.failedRequest.Remove(url);
		}
	}

	public void StopAll()
	{
		this.waitingRequests.Clear();
		foreach (DownloadManager.WWWRequest current in this.processingRequest.Values)
		{
			current.www.Dispose();
		}
		this.processingRequest.Clear();
	}

	public string BuildBundleDownloadName(string bundleName)
	{
		return bundleName + "." + this.bmConfiger.bundleSuffix;
	}

	public float ProgressOfBundles(string[] bundlefiles)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < bundlefiles.Length; i++)
		{
			string text = bundlefiles[i];
			if (!text.EndsWith("." + this.bmConfiger.bundleSuffix, StringComparison.OrdinalIgnoreCase))
			{
				global::Debug.LogWarning(new object[]
				{
					"ProgressOfBundles only accept bundle files. " + text + " is not a bundle file."
				});
			}
			else
			{
				list.Add(text);
			}
		}
		HashSet<int> hashSet = new HashSet<int>();
		foreach (string current in list)
		{
			foreach (int current2 in this.getDependList(current))
			{
				if (!hashSet.Contains(current2))
				{
					hashSet.Add(current2);
				}
			}
		}
		long num = 0L;
		long num2 = 0L;
		foreach (int current3 in hashSet)
		{
			BundleDataPublish bundleDataPublish = this.bundles[current3];
			long size = bundleDataPublish.size;
			num2 += size;
			string key = this.formatUrl(this.BuildBundleDownloadName(bundleDataPublish.name));
			if (this.processingRequest.ContainsKey(key))
			{
				num += (long)(this.processingRequest[key].www.progress * (float)size);
			}
			if (this.succeedRequest.ContainsKey(key))
			{
				num += size;
			}
		}
		if (num2 == 0L)
		{
			return 0f;
		}
		return (float)num / (float)num2;
	}

	private void OnLevelWasLoaded(int level)
	{
		if (this.unusedAssetBundles.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < this.unusedAssetBundles.Count; i++)
		{
			AssetBundle assetBundle = this.unusedAssetBundles[i];
			assetBundle.Unload(true);
		}
		this.unusedAssetBundles.Clear();
		Resources.UnloadUnusedAssets();
	}

	public void UnloadAllAssets()
	{
		foreach (BundleDataPublish current in this.assetBundles.Keys)
		{
			AssetBundle assetBundle = this.assetBundles[current];
			if (assetBundle != null)
			{
				assetBundle.Unload(true);
			}
		}
		this.assetBundles.Clear();
	}

	public void ForceUnloadAssets(bool immediate)
	{
		this.unusedAssetBundles.Clear();
		foreach (BundleDataPublish current in this.assetBundles.Keys)
		{
			this.UnloadAsset(current, immediate);
		}
	}

	private void UnloadAsset(BundleDataPublish bundle, bool immediate)
	{
		if (bundle.global)
		{
			return;
		}
		AssetBundle assetBundle = this.assetBundles[bundle];
		if (assetBundle != null)
		{
			if (immediate)
			{
				assetBundle.Unload(true);
			}
			else
			{
				this.unusedAssetBundles.Add(assetBundle);
			}
		}
	}

	public bool AssetBundleIsLoaded(BundleDataPublish bundle)
	{
		if (this.assetBundles.ContainsKey(bundle))
		{
			AssetBundle x = this.assetBundles[bundle];
			return x != null;
		}
		return false;
	}

	public AssetBundle LoadAssetBundle(BundleDataPublish bundle)
	{
		AssetBundle assetBundle = this.LoadAssetBundleInternal(bundle);
		if (this.unusedAssetBundles.Contains(assetBundle))
		{
			this.unusedAssetBundles.Remove(assetBundle);
		}
		return assetBundle;
	}

	private AssetBundle LoadAssetBundleInternal(BundleDataPublish bundle)
	{
		AssetBundle assetBundle;
		if (this.assetBundles.ContainsKey(bundle))
		{
			assetBundle = this.assetBundles[bundle];
			if (assetBundle != null)
			{
				return assetBundle;
			}
			this.assetBundles.Remove(bundle);
		}
		if (!this.BundleFileNotExists(bundle))
		{
			string cachedBundleFilePath = this.GetCachedBundleFilePath(bundle.name);
			if (!bundle.encrypt)
			{
				assetBundle = AssetBundle.CreateFromFile(cachedBundleFilePath);
			}
			else
			{
				byte[] binary = BMUtility.AESDecryptAndBZip2DeCompression(cachedBundleFilePath);
				assetBundle = AssetBundle.CreateFromMemoryImmediate(binary);
			}
		}
		else
		{
			byte[] array = Res.LoadDataFromStreamPath(bundle.name);
			array = this.SaveCacheBundleFile(bundle.name, array);
			if (bundle.encrypt)
			{
				array = BMUtility.AESDecryptAndBZip2DeCompression(array);
			}
			assetBundle = AssetBundle.CreateFromMemoryImmediate(array);
		}
		this.assetBundles.Add(bundle, assetBundle);
		return assetBundle;
	}

	public void StopUpdate()
	{
		for (int i = 0; i < this.bundles.Count; i++)
		{
			BundleDataPublish bundleDataPublish = this.bundles[i];
			if (bundleDataPublish.global)
			{
				this.LoadAssetBundle(bundleDataPublish);
			}
		}
		this.AttInfoCrc = Res.GetAssetCrc("Attribute/ItemInfo");
		this.downloadRootUrl = null;
	}

	public int[] GetVersionFromStr(string verStr)
	{
		char[] separator = new char[]
		{
			'.'
		};
		string[] array = verStr.Split(separator);
		int[] array2 = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			int.TryParse(array[i], out array2[i]);
		}
		return array2;
	}

	private void InitRootUrl(bool normalUpdate)
	{
		byte[] array = Res.LoadDataFromStreamPath("Urls");
		if (array != null)
		{
			string[] array2 = Encoding.Default.GetString(array).Split(new char[]
			{
				'\n'
			});
			Uri uri = new Uri((!normalUpdate) ? array2[1] : array2[0]);
			this.downloadRootUrl = uri.AbsoluteUri;
		}
		else
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Urls");
			BMUrls bMUrls = JsonMapper.ToObject<BMUrls>(textAsset.text);
			BuildPlatform platform;
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
			{
				platform = bMUrls.bundleTarget;
			}
			else
			{
				platform = this.getRuntimePlatform();
			}
			string interpretedDownloadUrl = bMUrls.GetInterpretedDownloadUrl(platform);
			Uri uri2 = new Uri(interpretedDownloadUrl);
			this.downloadRootUrl = uri2.AbsoluteUri;
		}
	}

	public string formatUrl(string urlstr)
	{
		Uri uri;
		if (!this.isAbsoluteUrl(urlstr))
		{
			uri = new Uri(new Uri(this.downloadRootUrl + '/'), urlstr);
		}
		else
		{
			uri = new Uri(urlstr);
		}
		return uri.AbsoluteUri;
	}

	private bool isAbsoluteUrl(string url)
	{
		Uri uri;
		return Uri.TryCreate(url, UriKind.Absolute, out uri);
	}

	[DebuggerHidden]
	public IEnumerator StartDownloadConfig(bool normalUpdate)
	{
        return null;
        //DownloadManager.<StartDownloadConfig>c__Iterator2 <StartDownloadConfig>c__Iterator = new DownloadManager.<StartDownloadConfig>c__Iterator2();
        //<StartDownloadConfig>c__Iterator.normalUpdate = normalUpdate;
        //<StartDownloadConfig>c__Iterator.<$>normalUpdate = normalUpdate;
        //<StartDownloadConfig>c__Iterator.<>f__this = this;
        //return <StartDownloadConfig>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator DownloadConfigerFile(string fileName)
	{
        return null;
        //DownloadManager.<DownloadConfigerFile>c__Iterator3 <DownloadConfigerFile>c__Iterator = new DownloadManager.<DownloadConfigerFile>c__Iterator3();
        //<DownloadConfigerFile>c__Iterator.fileName = fileName;
        //<DownloadConfigerFile>c__Iterator.<$>fileName = fileName;
        //<DownloadConfigerFile>c__Iterator.<>f__this = this;
        //return <DownloadConfigerFile>c__Iterator;
	}

	private void Update()
	{
		if (!this.ConfigLoaded || this.PauseDownload || this.FileError)
		{
			return;
		}
		List<string> list = null;
		foreach (DownloadManager.WWWRequest current in this.processingRequest.Values)
		{
			if (current.www == null)
			{
				current.CreatWWW();
			}
			else if (current.www.error != null)
			{
				string error = current.www.error;
				if (current.triedTimes - 1 < this.bmConfiger.downloadRetryTime)
				{
					current.CreatWWW();
				}
				else
				{
					global::Debug.LogError(new object[]
					{
						string.Concat(new object[]
						{
							"Download ",
							current.url,
							" failed for ",
							current.triedTimes,
							" times.\nError: ",
							error
						})
					});
					current.triedTimes = 0;
					this.PauseDownload = true;
				}
			}
			else if (!current.www.isDone)
			{
				float progress = current.www.progress;
				if (Mathf.Approximately(progress, current.curProgress))
				{
					if (Time.time - current.startTime >= 15f)
					{
						current.www.Dispose();
						current.www = null;
						current.triedTimes = 0;
						this.PauseDownload = true;
					}
				}
				else
				{
					current.startTime = Time.time;
				}
				current.curProgress = progress;
			}
			else if (current.www.isDone)
			{
				if (list == null)
				{
					list = new List<string>();
				}
				this.SaveCacheBundleFileFromWWW(current);
				list.Add(current.url);
			}
		}
		if (list != null)
		{
			foreach (string current2 in list)
			{
				this.succeedRequest.Add(current2, this.processingRequest[current2]);
				this.processingRequest.Remove(current2);
			}
		}
		int num = 0;
		while (this.processingRequest.Count < this.bmConfiger.downloadThreadsCount && num < this.waitingRequests.Count)
		{
			DownloadManager.WWWRequest wWWRequest = this.waitingRequests[num++];
			this.waitingRequests.Remove(wWWRequest);
			wWWRequest.CreatWWW();
			this.processingRequest.Add(wWWRequest.url, wWWRequest);
		}
	}

	public string GetCachedPatchFilePath(string bundleName)
	{
		return string.Format("{0}{1}", this.GetCachedPatchPath(), bundleName);
	}

	public string GetCachedPatchPath()
	{
		return string.Format("{0}/Patch/", this.cacheDataPath);
	}

	public string GetCachedBundleFilePath(string bundleName)
	{
		return string.Format("{0}{1}", this.GetCachedBundlePath(), this.BuildBundleDownloadName(bundleName));
	}

	public string GetCachedBMDataPath()
	{
		return string.Format("{0}/{1}", this.cacheDataPath, "BMData");
	}

	public string GetCacheBundleMD5Path()
	{
		return string.Format("{0}/{1}", this.cacheDataPath, "MD5Data");
	}

	public string GetCachedBundlePath()
	{
		return string.Format("{0}/Cache/", this.cacheDataPath);
	}

	public string GetCacheVersionInfoPath()
	{
		return string.Format("{0}/{1}", this.cacheDataPath, "version.txt");
	}

	private void SaveCacheBundleFileFromWWW(DownloadManager.WWWRequest request)
	{
		try
		{
			string cachedPatchFilePath = this.GetCachedPatchFilePath(request.bundleName);
			string directoryName = Path.GetDirectoryName(cachedPatchFilePath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			string text = cachedPatchFilePath + ".tmp";
			File.WriteAllBytes(text, request.www.bytes);
			File.Copy(text, cachedPatchFilePath, true);
			File.Delete(text);
			request.www.Dispose();
		}
		catch
		{
			this.FileError = true;
		}
	}

	public byte[] SaveCacheBundleFile(string bundleName, byte[] bytes)
	{
		if (bytes == null)
		{
			return bytes;
		}
		try
		{
			BundleDataPublish bundleDataPublish = this.bundleDict[bundleName];
			if (!bundleDataPublish.encrypt)
			{
				bytes = BMUtility.BZip2DeCompression(bytes);
			}
			string cachedBundleFilePath = this.GetCachedBundleFilePath(bundleName);
			string directoryName = Path.GetDirectoryName(cachedBundleFilePath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			string text = cachedBundleFilePath + ".tmp";
			File.WriteAllBytes(text, bytes);
			File.Copy(text, cachedBundleFilePath, true);
			File.Delete(text);
			this.SaveCacheBundleInfo(bundleDataPublish);
		}
		catch
		{
			this.FileError = true;
		}
		return bytes;
	}

	private void SaveCacheBundleInfo(BundleDataPublish bundle)
	{
		Dictionary<string, string> dictionary = this.CacheBundleMD5Dict;
		string name = bundle.name;
		if (dictionary.ContainsKey(name))
		{
			dictionary[name] = bundle.MD5;
		}
		else
		{
			dictionary.Add(name, bundle.MD5);
		}
		this.SaveCacheBundleInfoFile();
	}

	private void SaveCacheBundleInfoFile()
	{
		try
		{
			string cacheBundleMD5Path = this.GetCacheBundleMD5Path();
			string text = cacheBundleMD5Path + ".tmp";
			string plainText = JsonMapper.ToJson(this.CacheBundleMD5Dict);
			StreamWriter streamWriter = new StreamWriter(text, false);
			streamWriter.WriteLine(CryptHelper.AESEncrypt(plainText));
			streamWriter.Close();
			File.Copy(text, cacheBundleMD5Path, true);
			File.Delete(text);
		}
		catch
		{
			this.FileError = true;
		}
	}

	public void SaveCacheVersionInfoFile(string cacheVersion)
	{
		try
		{
			string cacheVersionInfoPath = this.GetCacheVersionInfoPath();
			string text = cacheVersionInfoPath + ".tmp";
			StreamWriter streamWriter = new StreamWriter(text);
			streamWriter.Write(cacheVersion);
			streamWriter.Close();
			File.Copy(text, cacheVersionInfoPath, true);
			File.Delete(text);
		}
		catch
		{
			this.FileError = true;
		}
	}

	public string GetCacheVersionInfo()
	{
		if (this.version == null)
		{
			this.version = this.gameVersion;
			string cacheVersionInfoPath = this.GetCacheVersionInfoPath();
			if (File.Exists(cacheVersionInfoPath))
			{
				try
				{
					StreamReader streamReader = new StreamReader(cacheVersionInfoPath);
					this.version = streamReader.ReadToEnd();
					streamReader.Close();
					if (!string.IsNullOrEmpty(this.version))
					{
						int[] versionFromStr = this.GetVersionFromStr(this.gameVersion);
						int[] versionFromStr2 = this.GetVersionFromStr(this.version);
						if (!this.CompareVersion(versionFromStr, versionFromStr2))
						{
							this.version = this.gameVersion;
							string cachedBMDataPath = this.GetCachedBMDataPath();
							if (File.Exists(cachedBMDataPath))
							{
								File.Delete(cachedBMDataPath);
							}
						}
					}
				}
				catch
				{
				}
			}
		}
		return this.version;
	}

	private bool CompareVersion(int[] ver1, int[] ver2)
	{
		for (int i = 0; i < ver1.Length; i++)
		{
			if (ver1[i] > ver2[i])
			{
				return false;
			}
			if (ver1[i] < ver2[i])
			{
				return true;
			}
		}
		return false;
	}

	private bool ComparePatchVersion(int[] ver1, int[] ver2)
	{
		return ver1[0] == ver2[0] && ver1[1] == ver2[1] && ver1[2] < ver2[2];
	}

	[DebuggerHidden]
	private IEnumerator CheckPackList(string packlistText)
	{
        return null;
        //DownloadManager.<CheckPackList>c__Iterator4 <CheckPackList>c__Iterator = new DownloadManager.<CheckPackList>c__Iterator4();
        //<CheckPackList>c__Iterator.packlistText = packlistText;
        //<CheckPackList>c__Iterator.<$>packlistText = packlistText;
        //<CheckPackList>c__Iterator.<>f__this = this;
        //return <CheckPackList>c__Iterator;
	}

	private void AddToProcessingList(int[] versionValue, string patchItem)
	{
		char[] separator = new char[]
		{
			' '
		};
		string[] array = patchItem.Split(separator);
		int[] versionFromStr = this.GetVersionFromStr(array[0]);
		if (!this.ComparePatchVersion(versionValue, versionFromStr))
		{
			return;
		}
		string text;
		if (array.Length >= 5)
		{
			text = array[4];
		}
		else
		{
			string fileName = Path.GetFileName(array[1]);
			char[] separator2 = new char[]
			{
				'.'
			};
			string[] array2 = fileName.Split(separator2);
			text = array2[4];
		}
		if (!text.Equals("ad", StringComparison.OrdinalIgnoreCase) && !text.Equals(SdkU3d.getChannel(), StringComparison.OrdinalIgnoreCase))
		{
			return;
		}
		DownloadManager.ProgressData progressData = new DownloadManager.ProgressData
		{
			version = array[0],
			fileName = Path.GetFileName(array[1]),
			url = this.formatUrl(array[1]),
			md5 = array[2],
			bundleSize = Convert.ToInt64(array[3])
		};
		this.processingList.Add(progressData);
		string cachedPatchFilePath = this.GetCachedPatchFilePath(progressData.fileName);
		if (File.Exists(cachedPatchFilePath) && progressData.md5 == BMUtility.GetFileMD5Internal(cachedPatchFilePath))
		{
			progressData.exists = true;
			return;
		}
		this.totalBundleSize += progressData.bundleSize;
	}

	public bool CheckUpdateList()
	{
		return this.processingList.Count > 0;
	}

	public long GetTotalBundleSize()
	{
		return this.totalBundleSize;
	}

	[DebuggerHidden]
	public IEnumerator StartDownloadList()
	{
        return null;
        //DownloadManager.<StartDownloadList>c__Iterator5 <StartDownloadList>c__Iterator = new DownloadManager.<StartDownloadList>c__Iterator5();
        //<StartDownloadList>c__Iterator.<>f__this = this;
        //return <StartDownloadList>c__Iterator;
	}

	public float ProgressOfBundles()
	{
		if (this.PauseDownload)
		{
			return this.bundlesProgress;
		}
		long num = 0L;
		if (this.processingList.Count > 0)
		{
			for (int i = 0; i < this.processingList.Count; i++)
			{
				DownloadManager.ProgressData progressData = this.processingList[i];
				if (!progressData.exists)
				{
					long bundleSize = progressData.bundleSize;
					string url = progressData.url;
					if (this.processingRequest.ContainsKey(url))
					{
						WWW www = this.processingRequest[url].www;
						if (www != null && string.IsNullOrEmpty(www.error))
						{
							num += (long)(www.progress * (float)bundleSize);
						}
					}
					if (this.BundlesSucceedRequest.ContainsKey(url))
					{
						num += bundleSize;
					}
				}
			}
		}
		if (this.totalBundleSize == 0L)
		{
			this.bundlesProgress = 0f;
		}
		else
		{
			this.bundlesProgress = (float)num / (float)this.totalBundleSize;
		}
		return this.bundlesProgress;
	}

	private void UncompUpdateListData(string patchFile, int index, DownloadManager.DoUpdateListCallback cb, object parm)
	{
		try
		{
			using (FileStream fileStream = new FileStream(patchFile, FileMode.Open))
			{
				using (ZipInputStream zipInputStream = new ZipInputStream(fileStream))
				{
					byte[] array = new byte[4096];
					for (int i = 0; i < index; i++)
					{
						zipInputStream.GetNextEntry();
					}
					ZipEntry nextEntry = zipInputStream.GetNextEntry();
					using (MemoryStream memoryStream = new MemoryStream())
					{
						int count;
						while ((count = zipInputStream.Read(array, 0, array.Length)) != 0)
						{
							memoryStream.Write(array, 0, count);
						}
						memoryStream.Seek(0L, SeekOrigin.Begin);
						if (cb != null)
						{
							cb(memoryStream, parm);
						}
					}
				}
			}
		}
		catch
		{
			this.FileError = true;
		}
	}

	private void GetUpdateListItemData(MemoryStream re, object parm)
	{
		byte[] array = re.ToArray();
		if (array.Length == 0)
		{
			return;
		}
		List<BundleDataPublish> list = JsonMapper.ToObject<List<BundleDataPublish>>(Encoding.Default.GetString(array));
		int i = 0;
		int processingIndex = (int)parm;
		while (i < list.Count)
		{
			BundleDataPublish bundleDataPublish = list[i];
			string dataName = bundleDataPublish.name;
			int num = this.compressList.FindIndex((DownloadManager.UpdateBundleData x) => x.bundleName == dataName);
			if (num > 0)
			{
				this.compressList.RemoveAt(num);
			}
			this.compressList.Add(new DownloadManager.UpdateBundleData
			{
				bundleName = dataName,
				processingIndex = processingIndex,
				dataIndex = i
			});
			i++;
		}
	}

	private void GetUpdateBMData(MemoryStream re, object parm)
	{
		byte[] array = re.ToArray();
		if (array.Length == 0)
		{
			return;
		}
		try
		{
			string cachedBMDataPath = this.GetCachedBMDataPath();
			string text = cachedBMDataPath + ".tmp";
			File.WriteAllBytes(text, array);
			File.Copy(text, cachedBMDataPath, true);
			File.Delete(text);
		}
		catch
		{
			this.FileError = true;
		}
	}

	private void InitConfigerData()
	{
		AssetBundle assetBundle = null;
		string cachedBMDataPath = this.GetCachedBMDataPath();
		if (!File.Exists(cachedBMDataPath))
		{
			byte[] binary = Res.LoadDataFromStreamPath("BMData");
			assetBundle = AssetBundle.CreateFromMemoryImmediate(binary);
		}
		else
		{
			assetBundle = AssetBundle.CreateFromFile(cachedBMDataPath);
		}
		TextAsset textAsset = assetBundle.Load(Path.GetFileNameWithoutExtension("BundleDataPublish.txt")) as TextAsset;
		this.bundles = JsonMapper.ToObject<List<BundleDataPublish>>(textAsset.text);
		this.bundleDict.Clear();
		foreach (BundleDataPublish current in this.bundles)
		{
			this.bundleDict.Add(current.name, current);
		}
		this.UnloadAllAssets();
		textAsset = (assetBundle.Load("AssetsToBundleMap") as TextAsset);
		this.assetsToBundleDict = JsonMapper.ToObject<Dictionary<string, List<int>>>(textAsset.text);
		textAsset = (assetBundle.Load("BMConfiger") as TextAsset);
		this.bmConfiger = JsonMapper.ToObject<BMConfiger>(textAsset.text);
		assetBundle.Unload(true);
	}

	private bool BundleFileNotExists(BundleDataPublish bundle)
	{
		Dictionary<string, string> dictionary = this.CacheBundleMD5Dict;
		string cachedBundleFilePath = this.GetCachedBundleFilePath(bundle.name);
		return !File.Exists(cachedBundleFilePath) || !dictionary.ContainsKey(bundle.name) || dictionary[bundle.name] != bundle.MD5;
	}

	public bool CheckCompressList(List<string> preDirs)
	{
		if (this.processingList.Count > 0)
		{
			DownloadManager.ProgressData progressData = this.processingList[this.processingList.Count - 1];
			string cachedPatchFilePath = this.GetCachedPatchFilePath(progressData.fileName);
			this.UncompUpdateListData(cachedPatchFilePath, 1, new DownloadManager.DoUpdateListCallback(this.GetUpdateBMData), null);
			if (this.FileError)
			{
				return false;
			}
		}
		this.InitConfigerData();
		foreach (string current in this.assetsToBundleDict.Keys)
		{
			bool flag = false;
			for (int i = 0; i < preDirs.Count; i++)
			{
				string value = preDirs[i];
				if (current.StartsWith(value, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				List<int> list = this.assetsToBundleDict[current];
				for (int j = 0; j < list.Count; j++)
				{
					BundleDataPublish bundle = this.bundles[list[j]];
					if (!this.compressList.Exists((DownloadManager.UpdateBundleData x) => x.bundleName == bundle.name))
					{
						if (this.BundleFileNotExists(bundle))
						{
							this.compressList.Add(new DownloadManager.UpdateBundleData
							{
								bundleName = bundle.name
							});
						}
					}
				}
			}
		}
		for (int k = 0; k < this.processingList.Count; k++)
		{
			DownloadManager.ProgressData progressData2 = this.processingList[k];
			string cachedPatchFilePath2 = this.GetCachedPatchFilePath(progressData2.fileName);
			this.UncompUpdateListData(cachedPatchFilePath2, 0, new DownloadManager.DoUpdateListCallback(this.GetUpdateListItemData), k);
			if (this.FileError)
			{
				return false;
			}
		}
		this.CompressTmpData();
		if (this.FileError)
		{
			return false;
		}
		this.SaveCacheVersionInfoFile(this.version);
		return !this.FileError && (this.NeedRestart || this.compressList.Count > 0);
	}

	private int SortByUpdateData(DownloadManager.UpdateBundleData aItem, DownloadManager.UpdateBundleData bItem)
	{
		bool flag = aItem.processingIndex >= 0;
		bool flag2 = bItem.processingIndex >= 0;
		if (flag && !flag2)
		{
			return -1;
		}
		if (flag2 && !flag)
		{
			return 1;
		}
		if (flag && flag2)
		{
			if (aItem.processingIndex < bItem.processingIndex)
			{
				return -1;
			}
			if (bItem.processingIndex < aItem.processingIndex)
			{
				return 1;
			}
			if (aItem.dataIndex < bItem.dataIndex)
			{
				return -1;
			}
			if (bItem.dataIndex < aItem.dataIndex)
			{
				return 1;
			}
		}
		return string.Compare(aItem.bundleName, bItem.bundleName);
	}

	[DebuggerHidden]
	public IEnumerator StartCompressList()
	{
        return null;
        //DownloadManager.<StartCompressList>c__Iterator6 <StartCompressList>c__Iterator = new DownloadManager.<StartCompressList>c__Iterator6();
        //<StartCompressList>c__Iterator.<>f__this = this;
        //return <StartCompressList>c__Iterator;
	}

	private void GetUpdateTmpData(MemoryStream re, object parm)
	{
		byte[] array = re.ToArray();
		if (array.Length == 0)
		{
			return;
		}
		try
		{
			string cachedBundlePath = this.GetCachedBundlePath();
			if (!Directory.Exists(cachedBundlePath))
			{
				Directory.CreateDirectory(cachedBundlePath);
			}
			string text = cachedBundlePath + this.BuildBundleDownloadName("M000");
			if (!File.Exists(text) || !(BMUtility.GetFileMD5Internal(text) == BMUtility.GetFileMD5Internal(array)))
			{
				text += ".tmp";
				string text2 = text + ".tmp";
				File.WriteAllBytes(text2, array);
				File.Copy(text2, text, true);
				File.Delete(text2);
				this.NeedRestart = true;
			}
		}
		catch
		{
			this.FileError = true;
		}
	}

	private void CompressTmpData()
	{
		if (this.processingList.Count <= 0)
		{
			return;
		}
		for (int i = this.processingList.Count - 1; i >= 0; i--)
		{
			if (this.NeedRestart || this.FileError)
			{
				break;
			}
			DownloadManager.ProgressData progressData = this.processingList[i];
			string cachedPatchFilePath = this.GetCachedPatchFilePath(progressData.fileName);
			this.UncompUpdateListData(cachedPatchFilePath, 2, new DownloadManager.DoUpdateListCallback(this.GetUpdateTmpData), null);
		}
	}

	private void GetUpdateCompressData(MemoryStream re, object parm)
	{
		string bundleName = (string)parm;
		this.SaveCacheBundleFile(bundleName, re.ToArray());
	}

	[DebuggerHidden]
	private IEnumerator CompressFromPatchPath(string bundleName, DownloadManager.UpdateBundleData updateData)
	{
        return null;
        //DownloadManager.<CompressFromPatchPath>c__Iterator7 <CompressFromPatchPath>c__Iterator = new DownloadManager.<CompressFromPatchPath>c__Iterator7();
        //<CompressFromPatchPath>c__Iterator.updateData = updateData;
        //<CompressFromPatchPath>c__Iterator.bundleName = bundleName;
        //<CompressFromPatchPath>c__Iterator.<$>updateData = updateData;
        //<CompressFromPatchPath>c__Iterator.<$>bundleName = bundleName;
        //<CompressFromPatchPath>c__Iterator.<>f__this = this;
        //return <CompressFromPatchPath>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator CompressFromStreamPath(string bundleName)
	{
        return null;
        //DownloadManager.<CompressFromStreamPath>c__Iterator8 <CompressFromStreamPath>c__Iterator = new DownloadManager.<CompressFromStreamPath>c__Iterator8();
        //<CompressFromStreamPath>c__Iterator.bundleName = bundleName;
        //<CompressFromStreamPath>c__Iterator.<$>bundleName = bundleName;
        //<CompressFromStreamPath>c__Iterator.<>f__this = this;
        //return <CompressFromStreamPath>c__Iterator;
	}

	private unsafe void CalcAssemblyCrc()
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		GCHandle value = GCHandle.Alloc(executingAssembly);
		IntPtr value2 = GCHandle.ToIntPtr(value);
		IntPtr* value3 = DownloadManager.mono_gchandle_get_target((void*)value2);
		value.Free();
		IntPtr** ptr = (IntPtr**)((byte*)((void*)((IntPtr)((void*)value3))) + 8);
		IntPtr** ptr2 = (IntPtr**)((byte*)((void*)((IntPtr)(*(IntPtr*)ptr))) + 64);
		byte* buffer = *(byte**)((byte*)(((IntPtr)(*(IntPtr*)ptr2))) + 8);
		int count = *(int*)((byte*)((void*)((IntPtr)(*(IntPtr*)ptr2))) + 12);
		this.AssemblyCrc = CRC32.GetCrc(buffer, 0, count);
	}

	public float ProgressOfCompress()
	{
		if (this.compressList.Count == 0)
		{
			return 1f;
		}
		return this.compProgress;
	}

	private void DeleteCacheBundleFiles()
	{
		string cachedBundlePath = this.GetCachedBundlePath();
		string[] files = Directory.GetFiles(cachedBundlePath, "*.*", SearchOption.AllDirectories);
		for (int i = 0; i < files.Length; i++)
		{
			string text = files[i].Replace('\\', '/');
			string text2 = text.Replace(cachedBundlePath, string.Empty).Replace("." + this.bmConfiger.bundleSuffix, string.Empty);
			if (!text2.Contains("M000") && !this.bundleDict.ContainsKey(text2))
			{
				File.Delete(text);
			}
		}
		string cachedPatchPath = this.GetCachedPatchPath();
		if (Directory.Exists(cachedPatchPath))
		{
			Directory.Delete(cachedPatchPath, true);
		}
	}

	private DownloadManager.WWWRequest download(DownloadManager.WWWRequest request)
	{
		request.url = this.formatUrl(request.url);
		DownloadManager.WWWRequest result;
		if ((result = this.FindRequestFormUrl(request.url)) != null)
		{
			return result;
		}
		if (request.priority == -1)
		{
			request.priority = 0;
		}
		this.addRequestToWaitingList(request);
		return request;
	}

	private bool isInWaitingList(string url)
	{
		foreach (DownloadManager.WWWRequest current in this.waitingRequests)
		{
			if (current.url == url)
			{
				return true;
			}
		}
		return false;
	}

	private void addRequestToWaitingList(DownloadManager.WWWRequest request)
	{
		if (this.succeedRequest.ContainsKey(request.url) || this.isInWaitingList(request.url))
		{
			return;
		}
		int num = this.waitingRequests.FindIndex((DownloadManager.WWWRequest x) => x.priority < request.priority);
		num = ((num != -1) ? num : this.waitingRequests.Count);
		this.waitingRequests.Insert(num, request);
	}

	private bool isDownloadingWWW(string url)
	{
		return this.waitingRequests.Exists((DownloadManager.WWWRequest x) => x.url == url) || this.processingRequest.ContainsKey(url);
	}

	private DownloadManager.WWWRequest FindRequestFormUrl(string url)
	{
		DownloadManager.WWWRequest wWWRequest = this.waitingRequests.Find((DownloadManager.WWWRequest x) => x.url == url);
		if (wWWRequest != null)
		{
			return wWWRequest;
		}
		if (this.processingRequest.ContainsKey(url))
		{
			return this.processingRequest[url];
		}
		if (this.succeedRequest.ContainsKey(url))
		{
			return this.succeedRequest[url];
		}
		return null;
	}

	public List<int> getDependList(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return null;
		}
		path = path.ToLower();
		if (!this.assetsToBundleDict.ContainsKey(path))
		{
			global::Debug.LogError(new object[]
			{
				"Cannot find parent bundle [" + path + "], Please check your bundle config."
			});
			return null;
		}
		return this.assetsToBundleDict[path];
	}

	private BuildPlatform getRuntimePlatform()
	{
		if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			return BuildPlatform.Standalones;
		}
		if (Application.platform == RuntimePlatform.OSXWebPlayer || Application.platform == RuntimePlatform.WindowsWebPlayer)
		{
			return BuildPlatform.WebPlayer;
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return BuildPlatform.IOS;
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			return BuildPlatform.Android;
		}
		global::Debug.LogError(new object[]
		{
			"Platform " + Application.platform + " is not supported by BundleManager."
		});
		return BuildPlatform.Standalones;
	}

	private void Awake()
	{
		byte[] array = Res.LoadDataFromStreamPath("version.txt");
		if (array != null && array.Length != 0)
		{
			string @string = Encoding.Default.GetString(array);
			this.gameVersion = @string;
		}
		else
		{
			this.gameVersion = GameSetting.GameVersion;
		}
		this.cacheDataPath = Application.temporaryCachePath;
		string cachedBundlePath = this.GetCachedBundlePath();
		if (Directory.Exists(cachedBundlePath))
		{
			Directory.Delete(cachedBundlePath, true);
		}
		string cachedPatchPath = this.GetCachedPatchPath();
		if (Directory.Exists(cachedPatchPath))
		{
			Directory.Delete(cachedPatchPath, true);
		}
		string cachedBMDataPath = this.GetCachedBMDataPath();
		if (File.Exists(cachedBMDataPath))
		{
			File.Delete(cachedBMDataPath);
		}
		string cacheBundleMD5Path = this.GetCacheBundleMD5Path();
		if (File.Exists(cacheBundleMD5Path))
		{
			File.Delete(cacheBundleMD5Path);
		}
		string cacheVersionInfoPath = this.GetCacheVersionInfoPath();
		if (File.Exists(cacheVersionInfoPath))
		{
			File.Delete(cacheVersionInfoPath);
		}
		this.cacheDataPath = Application.persistentDataPath;
		GameSetting.GameVersion = this.GetCacheVersionInfo();
		this.InitConfigerData();
	}

	public static void SetManualUrl(string url)
	{
		if (DownloadManager.Instance != null)
		{
			global::Debug.LogError(new object[]
			{
				"Cannot use SetManualUrl after accessed DownloadManager.Instance. Make sure call SetManualUrl before access to DownloadManager.Instance."
			});
			return;
		}
	}

	public void ShowContinueUpdateMessageBox()
	{
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetUpdateTip("UTP_9"), MessageBox.Type.Custom1Btn, null);
		GameMessageBox expr_18 = gameMessageBox;
		expr_18.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_18.OkClick, new MessageBox.MessageDelegate(this.OnContinueOkClick));
		gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetUpdateTip("UTP_10");
		gameMessageBox.ContentPivot = UIWidget.Pivot.Center;
		gameMessageBox.WidthOK = 132;
		gameMessageBox.CanCloseByFadeBGClicked = false;
	}

	private void OnContinueOkClick(object obj)
	{
		this.PauseDownload = false;
	}

	public void ShowPackUpdateMessageBox(string packUrl)
	{
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetUpdateTip("UTP_7"), MessageBox.Type.Custom1Btn, packUrl);
		GameMessageBox expr_18 = gameMessageBox;
		expr_18.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_18.OkClick, new MessageBox.MessageDelegate(this.OnPackUpdateOkClick));
		gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetUpdateTip("UTP_13");
		gameMessageBox.ContentPivot = UIWidget.Pivot.Center;
		gameMessageBox.WidthOK = 132;
		gameMessageBox.CanCloseByFadeBGClicked = false;
	}

	private void OnPackUpdateOkClick(object obj)
	{
		string urlstr = obj as string;
		Application.OpenURL(this.formatUrl(urlstr));
		GUIAgreementInfoPopUp.SetUserAgreement(false);
	}

	public void ShowDiskFullMessageBox()
	{
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetUpdateTip("UTP_15"), MessageBox.Type.Custom1Btn, null);
		GameMessageBox expr_18 = gameMessageBox;
		expr_18.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_18.OkClick, new MessageBox.MessageDelegate(this.OnDiskFullOkClick));
		gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetUpdateTip("UTP_12");
		gameMessageBox.ContentPivot = UIWidget.Pivot.Center;
		gameMessageBox.CanCloseByFadeBGClicked = false;
	}

	private void OnDiskFullOkClick(object obj)
	{
		SdkU3d.exit();
	}
}
