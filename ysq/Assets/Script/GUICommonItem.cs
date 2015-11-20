using System;
using System.Text;
using UnityEngine;

public class GUICommonItem : MonoBehaviour
{
	public UILabel mCommonLabel;

	private GUIChatWindowV2 mBaseLayer;

	public string mCommonTxt
	{
		get;
		private set;
	}

	public void InitWithBaseLayer(GUIChatWindowV2 baseLayer, string txtMsg)
	{
		this.mBaseLayer = baseLayer;
		this.mCommonTxt = txtMsg;
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		this.mCommonLabel = base.transform.Find("commonLabel").GetComponent<UILabel>();
		UIEventListener expr_2B = UIEventListener.Get(this.mCommonLabel.gameObject);
		expr_2B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_2B.onClick, new UIEventListener.VoidDelegate(this.OnCommonLabelItemClick));
	}

	private void Refresh()
	{
		this.mCommonLabel.text = this.mCommonTxt;
	}

	private void OnCommonLabelItemClick(GameObject go)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Remove(0, stringBuilder.Length);
		stringBuilder.Append(this.mCommonLabel.text);
		stringBuilder.Remove(0, 2);
		switch (this.mBaseLayer.GetCurChanel())
		{
		case 0:
		case 1:
		case 3:
			this.mBaseLayer.CommitChatMsg1(stringBuilder.ToString());
			break;
		case 2:
			this.mBaseLayer.CommitChatMsgForPersonal(stringBuilder.ToString());
			break;
		}
	}
}
