using System;
using UnityEngine;

public sealed class GUISceneLoading : GameUISession
{
	private UISlider m_pb;

	private UILabel m_text;

	[NonSerialized]
	public bool UpdateFlag;

	private float timer;

	private float curProgress;

	private float maxProgress = 0.85f;

	[NonSerialized]
	public bool Loaded;

	[NonSerialized]
	public bool SceneLoading = true;

	public float MaxProgress
	{
		get
		{
			return this.maxProgress;
		}
		set
		{
			if (this.maxProgress > value)
			{
				return;
			}
			this.maxProgress = value;
		}
	}

	protected override void OnPostLoadGUI()
	{
		GameObject gameObject = base.FindGameObject("load_bk", null);
		UITexture component = gameObject.GetComponent<UITexture>();
		GameObject gameObject2 = gameObject.transform.Find("logo").gameObject;
		int num = UnityEngine.Random.Range(0, 100) % ConstString.LoadingTexture.Length;
		Texture mainTexture = Res.Load<Texture>(ConstString.LoadingTexture[num], false);
		component.mainTexture = mainTexture;
		gameObject2.SetActive(false);
		GameObject gameObject3 = base.FindGameObject("load_bk/pb", null);
		this.m_pb = gameObject3.GetComponent<UISlider>();
		this.m_pb.value = 0f;
		GameObject gameObject4 = base.FindGameObject("Background/text", gameObject3);
		this.m_text = gameObject4.GetComponent<UILabel>();
		UILabel uILabel = GameUITools.FindUILabel("Background/Tips", gameObject3);
		uILabel.text = Singleton<StringManager>.Instance.GetLoadingTips();
		if (this.SceneLoading)
		{
			Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("bg/bg_002", true);
		}
	}

	protected override void OnPreDestroyGUI()
	{
	}

	private void Update()
	{
		if (!base.gameObject.activeInHierarchy || !this.UpdateFlag)
		{
			return;
		}
		if (!this.Loaded || this.curProgress < 1f)
		{
			if (this.SceneLoading)
			{
				this.MaxProgress = Globals.Instance.SenceMgr.progress;
			}
			float num = 0.33f;
			if (this.maxProgress >= 1f)
			{
				num = 3f;
			}
			float num2 = this.curProgress;
			num2 += num * Time.deltaTime;
			if (num2 > this.maxProgress)
			{
				num2 = this.maxProgress;
			}
			if (num2 > this.curProgress)
			{
				this.curProgress = num2;
				this.SetProgress(this.curProgress);
			}
			return;
		}
		this.timer += Time.deltaTime;
		if (this.timer > 0.2f)
		{
			if (this.SceneLoading)
			{
				Globals.Instance.ActorMgr.OnUILoadingFinish();
			}
			base.CloseImmediate();
			return;
		}
	}

	private void SetProgress(float progress)
	{
		if (this.m_pb != null)
		{
			this.m_pb.value = progress;
		}
		if (this.m_text != null)
		{
			this.m_text.text = Mathf.CeilToInt(progress * 100f).ToString() + "%";
		}
	}
}
