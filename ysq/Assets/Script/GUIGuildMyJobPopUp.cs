using System;
using System.Text;
using UnityEngine;

public class GUIGuildMyJobPopUp : GameUIBasePopup
{
	private UILabel mMyJob;

	private UILabel mJobContents;

	private UIScrollBar mUIScrollBar;

	private StringBuilder mStringBuilder = new StringBuilder();

	private void Awake()
	{
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("winBg");
		GameObject gameObject = transform.Find("closeBtn").gameObject;
		UIEventListener expr_28 = UIEventListener.Get(gameObject);
		expr_28.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_28.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		this.mMyJob = transform.Find("txt0/job").GetComponent<UILabel>();
		this.mJobContents = transform.Find("contentsBg/contentsPanel/contents/Label").GetComponent<UILabel>();
		this.mUIScrollBar = transform.Find("contentsBg/contentsScrollBar").GetComponent<UIScrollBar>();
	}

	private void Refresh()
	{
		int selfGuildJob = Tools.GetSelfGuildJob();
		this.mMyJob.text = Tools.GetGuildMemberJobDesc(selfGuildJob);
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
		if (selfGuildJob == 1)
		{
			for (int i = 0; i < 3; i++)
			{
				this.mStringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString(string.Format("guildJob{0}", i)));
			}
		}
		if (selfGuildJob <= 2)
		{
			for (int j = 20; j < 27; j++)
			{
				if (j != 24)
				{
					this.mStringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString(string.Format("guildJob{0}", j)));
				}
			}
		}
		if (selfGuildJob <= 4)
		{
			for (int k = 40; k < 44; k++)
			{
				this.mStringBuilder.AppendLine(Singleton<StringManager>.Instance.GetString(string.Format("guildJob{0}", k)));
			}
		}
		this.mJobContents.text = this.mStringBuilder.ToString();
		this.mUIScrollBar.value = 0f;
		this.mUIScrollBar.alpha = 1f;
	}

	private void OnCloseBtnClick(GameObject go)
	{
		GameUIPopupManager.GetInstance().PopState(false, null);
	}
}
