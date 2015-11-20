using System;
using UnityEngine;

public abstract class AARewardItemBase : UICustomGridItem
{
	protected UISprite bg;

	protected UILabel Title;

	protected Transform Reward;

	protected GameObject[] RewardItem = new GameObject[3];

	protected GameObject GoBtn;

	protected GameObject ReceiveBtn;

	protected GameObject finished;

	protected UILabel step;

	public virtual void Init()
	{
		this.bg = base.transform.GetComponent<UISprite>();
		this.Title = base.transform.FindChild("Title").GetComponent<UILabel>();
		this.Reward = base.transform.Find("Reward");
		this.GoBtn = base.transform.FindChild("GoBtn").gameObject;
		this.ReceiveBtn = base.transform.FindChild("ReceiveBtn").gameObject;
		this.finished = base.transform.FindChild("finished").gameObject;
		this.step = base.transform.FindChild("step").GetComponent<UILabel>();
		UIEventListener expr_B9 = UIEventListener.Get(this.GoBtn);
		expr_B9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_B9.onClick, new UIEventListener.VoidDelegate(this.OnGoBtnClicked));
		UIEventListener expr_E6 = UIEventListener.Get(this.ReceiveBtn);
		expr_E6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_E6.onClick, new UIEventListener.VoidDelegate(this.OnReceiveBtnClicked));
	}

	protected abstract void OnGoBtnClicked(GameObject go);

	protected abstract void OnReceiveBtnClicked(GameObject go);
}
