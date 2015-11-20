using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class DynamicUpdate : MonoBehaviour
{
	public delegate void VoidCallback();

	public DynamicUpdate.VoidCallback FinishUpdateEvent;

	private UISlider m_pb;

	private UILabel loginTips;

	private UILabel versionTips;

	private UITexture m_bg;

	private GameObject logo;

	private float updateProgress;

	private float curProgress;

	private float maxProgress;

	private float timer;

	private bool startCheckList;

	private bool startUpdate;

	private bool startCompress;

	private bool wifiOkClick = true;

	private static bool firstInit;

	private void OnDestroy()
	{
		Resources.UnloadUnusedAssets();
	}

	private void Start()
	{
		this.m_pb = base.transform.FindChild("pb").GetComponent<UISlider>();
		this.m_pb.value = 0f;
		this.loginTips = base.transform.Find("Tips").GetComponent<UILabel>();
		Singleton<StringManager>.Instance.LoadUpdateTable();
		this.versionTips = base.transform.Find("Label").GetComponent<UILabel>();
		this.logo = base.transform.Find("Logo").gameObject;
		this.m_bg = base.transform.Find("bg").GetComponent<UITexture>();
		UITexture component = this.m_bg.transform.Find("logo").GetComponent<UITexture>();
		component.leftAnchor.absolute = 17;
		component.rightAnchor.absolute = 309;
		component.bottomAnchor.absolute = -201;
		component.topAnchor.absolute = -9;
		if (!DynamicUpdate.firstInit)
		{
			this.logo.SetActive(true);
		}
		this.Reset();
	}

	private void Reset()
	{
		this.updateProgress = 0f;
		this.curProgress = 0f;
		this.maxProgress = 0.01f;
		this.timer = 0f;
		this.startUpdate = false;
		this.startCompress = false;
		this.startCheckList = false;
		this.m_pb.gameObject.SetActive(false);
		this.loginTips.gameObject.SetActive(false);
		this.RefreshAppVersion();
	}

	private void RefreshAppVersion()
	{
		GameSetting.GameVersion = DownloadManager.Instance.GetCacheVersionInfo();
		this.versionTips.text = Singleton<StringManager>.Instance.GetUpdateTip("UTP_5", new object[]
		{
			GameSetting.GameVersion
		});
		this.m_bg.mainTexture = Res.Load<Texture>("MainBg/loginBG", true);
	}

	[DebuggerHidden]
	public IEnumerator CheckUpdateList()
	{
        return null;
        //DynamicUpdate.<CheckUpdateList>c__Iterator9 <CheckUpdateList>c__Iterator = new DynamicUpdate.<CheckUpdateList>c__Iterator9();
        //<CheckUpdateList>c__Iterator.<>f__this = this;
        //return <CheckUpdateList>c__Iterator;
	}

	[DebuggerHidden]
	public IEnumerator CheckInterUpdateList(bool normalUpdate)
	{
        return null;
        //DynamicUpdate.<CheckInterUpdateList>c__IteratorA <CheckInterUpdateList>c__IteratorA = new DynamicUpdate.<CheckInterUpdateList>c__IteratorA();
        //<CheckInterUpdateList>c__IteratorA.normalUpdate = normalUpdate;
        //<CheckInterUpdateList>c__IteratorA.<$>normalUpdate = normalUpdate;
        //<CheckInterUpdateList>c__IteratorA.<>f__this = this;
        //return <CheckInterUpdateList>c__IteratorA;
	}

	[DebuggerHidden]
	private IEnumerator CheckUpdateListInternal()
	{
        return null;
        //DynamicUpdate.<CheckUpdateListInternal>c__IteratorB <CheckUpdateListInternal>c__IteratorB = new DynamicUpdate.<CheckUpdateListInternal>c__IteratorB();
        //<CheckUpdateListInternal>c__IteratorB.<>f__this = this;
        //return <CheckUpdateListInternal>c__IteratorB;
	}

	private float ProgressOfBundles()
	{
		return DownloadManager.Instance.ProgressOfBundles();
	}

	private float ProgressOfCompress()
	{
		return DownloadManager.Instance.ProgressOfCompress();
	}

	private string FormatByteSize(long size)
	{
		if (size < 512L)
		{
			return string.Format("{0:0.0}", size);
		}
		if (size < 524288L)
		{
			return string.Format("{0:0.0}KB", (double)size / 1024.0);
		}
		size /= 1024L;
		return string.Format("{0:0.0}MB", (double)size / 1024.0);
	}

	private void Update()
	{
		if (!this.startCheckList)
		{
			return;
		}
		if (this.curProgress >= 1f)
		{
			this.timer += Time.deltaTime;
			if (this.timer > 0.1f)
			{
				if (DownloadManager.Instance.NeedRestart)
				{
					this.ShowAndroidRestartMessageBox();
				}
				else if (!DownloadManager.Instance.FileError)
				{
					this.Reset();
					if (this.FinishUpdateEvent != null)
					{
						this.FinishUpdateEvent();
					}
				}
			}
			return;
		}
		float num = 0f;
		if (this.startUpdate)
		{
			num = this.ProgressOfBundles();
			long totalBundleSize = DownloadManager.Instance.GetTotalBundleSize();
			this.SetLoginText(Singleton<StringManager>.Instance.GetUpdateTip("UTP_2", new object[]
			{
				this.FormatByteSize((long)(num * (float)totalBundleSize)),
				this.FormatByteSize(totalBundleSize)
			}));
			num *= 0.5f;
			this.updateProgress = num;
		}
		if (this.startCompress)
		{
			num = this.ProgressOfCompress();
			num = this.updateProgress + num * (1f - this.updateProgress);
			if (num >= 1f)
			{
				num = 0.98f;
			}
		}
		if (this.maxProgress < num)
		{
			this.maxProgress = num;
		}
		float num2 = 0.33f;
		if (this.maxProgress >= 1f)
		{
			num2 = 3f;
		}
		float num3 = this.curProgress;
		num3 += num2 * Time.deltaTime;
		if (num3 > this.maxProgress)
		{
			num3 = this.maxProgress;
		}
		if (num3 > this.curProgress)
		{
			this.curProgress = num3;
			this.SetProgress(this.curProgress);
		}
	}

	public void SetProgress(float progress)
	{
		if (this.m_pb != null)
		{
			this.m_pb.value = progress;
		}
	}

	public void SetLoginText(string str)
	{
		if (this.loginTips == null)
		{
			return;
		}
		if (!this.loginTips.gameObject.activeInHierarchy)
		{
			this.loginTips.gameObject.SetActive(true);
		}
		this.loginTips.text = str;
	}

	public void HideLoginText()
	{
		if (this.loginTips == null || !this.loginTips.gameObject.activeInHierarchy)
		{
			return;
		}
		this.loginTips.gameObject.SetActive(false);
	}

	public void ShowUpdateNeedWifiMessageBox()
	{
		long totalBundleSize = DownloadManager.Instance.GetTotalBundleSize();
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetUpdateTip("UTP_8", new object[]
		{
			this.FormatByteSize(totalBundleSize)
		}), MessageBox.Type.Custom1Btn, null);
		GameMessageBox expr_33 = gameMessageBox;
		expr_33.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_33.OkClick, new MessageBox.MessageDelegate(this.OnWifiOkClick));
		gameMessageBox.ContentPivot = UIWidget.Pivot.Center;
		gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetUpdateTip("UTP_11");
		gameMessageBox.WidthOK = 132;
		gameMessageBox.CanCloseByFadeBGClicked = false;
		this.wifiOkClick = false;
	}

	private void OnWifiOkClick(object obj)
	{
		this.wifiOkClick = true;
	}

	public void ShowAndroidRestartMessageBox()
	{
		GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetUpdateTip("UTP_6"), MessageBox.Type.Custom1Btn, null);
		GameMessageBox expr_18 = gameMessageBox;
		expr_18.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_18.OkClick, new MessageBox.MessageDelegate(this.OnRestartOkClick));
		gameMessageBox.TextOK = Singleton<StringManager>.Instance.GetUpdateTip("UTP_12");
		gameMessageBox.ContentPivot = UIWidget.Pivot.Center;
		gameMessageBox.CanCloseByFadeBGClicked = false;
		this.startCheckList = false;
	}

	private void OnRestartOkClick(object obj)
	{
		PlatformTools.RestartApp();
	}
}
