using Proto;
using System;
using UnityEngine;

public class GUIActivityValueInfo : MonoBehaviour
{
	private GameObject mGo;

	private UILabel mTitle;

	private UILabel mContent;

	private UILabel mTime;

	private float RefreshActivityTime = 0.2f;

	public ActivityValueData AVData
	{
		get;
		private set;
	}

	public void Init()
	{
		this.mGo = base.transform.Find("Go").gameObject;
		UIEventListener expr_26 = UIEventListener.Get(this.mGo);
		expr_26.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_26.onClick, new UIEventListener.VoidDelegate(this.OnGoBtnClick));
		this.mTitle = GameUITools.FindUILabel("Title", base.gameObject);
		this.mContent = GameUITools.FindUILabel("Content", base.gameObject);
		this.mTime = GameUITools.FindUILabel("Time", base.gameObject);
	}

	public void Refresh(ActivityValueData data)
	{
		if (data == null)
		{
			return;
		}
		if (this.AVData != data)
		{
			this.AVData = data;
			this.mTitle.text = this.AVData.Base.Name;
			this.mContent.text = this.AVData.Base.Desc;
		}
	}

	private void Update()
	{
		this.RefreshTime();
	}

	private void RefreshTime()
	{
		this.RefreshActivityTime -= Time.deltaTime;
		if (this.mTime != null && this.RefreshActivityTime < 0f)
		{
			int num = (this.AVData != null) ? Tools.GetRemainAARewardTime(this.AVData.Base.CloseTimeStamp) : 0;
			if (num <= 0)
			{
				this.mTime.text = Singleton<StringManager>.Instance.GetString("activityOverTime", new object[]
				{
					Singleton<StringManager>.Instance.GetString("activityOver")
				});
				this.RefreshActivityTime = 3.40282347E+38f;
			}
			else
			{
				this.mTime.text = Singleton<StringManager>.Instance.GetString("activityOverTime", new object[]
				{
					Tools.FormatTimeStr2(num, false, false)
				});
				this.RefreshActivityTime = 1f;
			}
		}
	}

	private void OnGoBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.AVData == null)
		{
			return;
		}
		GUIReward.GotoActivityFunction((EActivityValueType)this.AVData.Type);
	}
}
