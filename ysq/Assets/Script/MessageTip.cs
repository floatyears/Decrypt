using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class MessageTip : MonoBehaviour
{
	private UISprite background;

	private UILabel textLb;

	public void Init()
	{
		this.background = base.transform.Find("BG").GetComponent<UISprite>();
		this.textLb = base.transform.Find("BG/Info").GetComponent<UILabel>();
	}

	public void SetText(string text)
	{
		this.textLb.text = text;
		this.background.width = (int)this.textLb.printedSize.x + 30;
		if (this.background.width < 100)
		{
			this.background.width = 100;
		}
		NGUITools.SetActive(base.gameObject, true);
		GameUITools.PlayOpenWindowAnim(this.background.transform, delegate
		{
			base.StartCoroutine(this.DoCloseMessageTip());
		}, true);
	}

	[DebuggerHidden]
	private IEnumerator DoCloseMessageTip()
	{
        return null;
        //MessageTip.<DoCloseMessageTip>c__Iterator77 <DoCloseMessageTip>c__Iterator = new MessageTip.<DoCloseMessageTip>c__Iterator77();
        //<DoCloseMessageTip>c__Iterator.<>f__this = this;
        //return <DoCloseMessageTip>c__Iterator;
	}
}
