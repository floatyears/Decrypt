using Proto;
using System;
using UnityEngine;

public class GUIMailScene : GameUISession
{
	private MailContentsLayer mMailContentsLayer;

	private MailDetailInfoLayer mMailDetailInfoLayer;

	private Transform WindowBg;

	public GameObject mNoMailGo
	{
		get;
		private set;
	}

	public void ShowMailDetailInfoLayer(MailData mailData)
	{
		this.mMailDetailInfoLayer.SetMailData(mailData);
		this.mMailDetailInfoLayer.EnableMailDetailInfo(true);
	}

	public void ReInitMailItems()
	{
		this.mMailContentsLayer.ReInitMailItems();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.FindChild("UIMiddle");
		Transform transform2 = transform.transform.FindChild("MailListBG");
		this.WindowBg = transform.transform.FindChild("WindowBg");
		this.mNoMailGo = this.WindowBg.transform.Find("noMail").gameObject;
		this.mMailContentsLayer = this.WindowBg.transform.FindChild("mailBg").gameObject.AddComponent<MailContentsLayer>();
		this.mMailContentsLayer.InitWithBaseScene(this);
		this.mMailDetailInfoLayer = transform.transform.FindChild("detailInfoPopUp").gameObject.AddComponent<MailDetailInfoLayer>();
		this.mMailDetailInfoLayer.InitWithBaseScene(this);
		UIButton component = this.WindowBg.transform.FindChild("closeBtn").GetComponent<UIButton>();
		UIEventListener expr_DB = UIEventListener.Get(component.gameObject);
		expr_DB.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_DB.onClick, new UIEventListener.VoidDelegate(this.OnCloseScene));
		UIEventListener expr_107 = UIEventListener.Get(transform2.gameObject);
		expr_107.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_107.onClick, new UIEventListener.VoidDelegate(this.OnCloseScene));
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		LocalPlayer expr_10 = Globals.Instance.Player;
		expr_10.TakeMailAffixEvent = (LocalPlayer.TakeMailAffixCallback)Delegate.Combine(expr_10.TakeMailAffixEvent, new LocalPlayer.TakeMailAffixCallback(this.mMailContentsLayer.OnTakeMailAffixEvent));
		GameUITools.PlayOpenWindowAnim(this.WindowBg.transform, null, true);
	}

	private void OnCloseScene(GameObject go)
	{
		GameUITools.PlayCloseWindowAnim(this.WindowBg.transform, delegate
		{
			base.Close();
		}, true);
	}

	protected override void OnPreDestroyGUI()
	{
		LocalPlayer expr_0A = Globals.Instance.Player;
		expr_0A.TakeMailAffixEvent = (LocalPlayer.TakeMailAffixCallback)Delegate.Remove(expr_0A.TakeMailAffixEvent, new LocalPlayer.TakeMailAffixCallback(this.mMailContentsLayer.OnTakeMailAffixEvent));
	}
}
