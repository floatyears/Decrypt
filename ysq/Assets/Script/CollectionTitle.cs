using Att;
using System;
using System.Text;
using UnityEngine;

public sealed class CollectionTitle : CollectionContentBase
{
	private GUISummonCollectionScene mBaseScene;

	private UISprite mTitleBg;

	private UILabel mTitleProperty;

	private UILabel mTitleTxt;

	private int mCurNum;

	private int mMaxNum;

	private StringBuilder mStringBuilder = new StringBuilder();

	public void InitWithBaseScene(GUISummonCollectionScene baseScene, EElementType et, int cur, int max)
	{
		this.mBaseScene = baseScene;
		this.mElementType = et;
		this.mCurNum = cur;
		this.mMaxNum = max;
		this.mIsTitle = true;
		this.CreateObjects();
		this.Refresh();
	}

	private void CreateObjects()
	{
		this.mTitleBg = base.transform.Find("titleBg").GetComponent<UISprite>();
		this.mTitleProperty = this.mTitleBg.transform.Find("titleProperty").GetComponent<UILabel>();
		this.mTitleTxt = base.transform.Find("titleTxt").GetComponent<UILabel>();
		BoxCollider boxCollider = this.mTitleBg.gameObject.AddComponent<BoxCollider>();
		boxCollider.size = new Vector3(1024f, 100f, 0f);
		UIDragScrollView uIDragScrollView = this.mTitleBg.gameObject.AddComponent<UIDragScrollView>();
		uIDragScrollView.scrollView = this.mBaseScene.mSummonCollectionLayer.mSW;
	}

	private void Refresh()
	{
		this.mTitleBg.spriteName = Tools.GetCollectionSummonTitleBg(this.mElementType);
		this.mStringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("summonCollectionTxt3"), Tools.GetSummonPropertyStr(this.mElementType));
		this.mTitleProperty.text = this.mStringBuilder.ToString();
		this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
		this.mStringBuilder.AppendFormat(Singleton<StringManager>.Instance.GetString("summonCollectionTxt2"), this.mCurNum, this.mMaxNum);
		this.mTitleTxt.text = this.mStringBuilder.ToString();
	}
}
