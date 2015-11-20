using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GUIAttributeTip : MonoBehaviour
{
	private GameObject mOrignalLb;

	private float mTime1;

	private float mTime2;

	private float mAnimLength;

	private void CreateObjects()
	{
		this.mOrignalLb = base.transform.Find("orignalLb").gameObject;
		this.mOrignalLb.gameObject.SetActive(false);
	}

	public void ShowTipContents(List<string> contents, float time1, float time2, float animLength)
	{
		this.mTime1 = time1;
		this.mTime2 = time2;
		this.mAnimLength = animLength;
		this.CreateObjects();
		base.StartCoroutine(this.DoShowTips(contents));
	}

	[DebuggerHidden]
	private IEnumerator DoShowTips(List<string> contents)
	{
        return null;
        //GUIAttributeTip.<DoShowTips>c__Iterator64 <DoShowTips>c__Iterator = new GUIAttributeTip.<DoShowTips>c__Iterator64();
        //<DoShowTips>c__Iterator.contents = contents;
        //<DoShowTips>c__Iterator.<$>contents = contents;
        //<DoShowTips>c__Iterator.<>f__this = this;
        //return <DoShowTips>c__Iterator;
	}

	public void DestroySelf()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
