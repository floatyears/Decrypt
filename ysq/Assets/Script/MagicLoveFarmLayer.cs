using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MagicLoveFarmLayer : MonoBehaviour
{
	private class MagicLoveMatchItem : MonoBehaviour
	{
		private UILabel mBout;

		private UILabel mTips;

		private UISprite[] mSelfs = new UISprite[3];

		private UISprite[] mTargets = new UISprite[3];

		private Animator mSelfAnim;

		private Animator mTargetAnim;

		private UILabel mSelfResult;

		private UILabel mTargetResult;

		private int tempValue;

		private int target;

		public void Init()
		{
			this.mBout = GameUITools.FindUILabel("Bout", base.gameObject);
			this.mTips = GameUITools.FindUILabel("Tips", base.gameObject);
			this.mTips.text = string.Empty;
			for (int i = 0; i < this.mSelfs.Length; i++)
			{
				this.mSelfs[i] = GameUITools.FindUISprite(string.Format("Self{0}", i), base.gameObject);
				this.mTargets[i] = GameUITools.FindUISprite(string.Format("Target{0}", i), base.gameObject);
			}
			this.mSelfAnim = GameUITools.FindGameObject("SelfAnim/Anim", base.gameObject).GetComponent<Animator>();
			this.mTargetAnim = GameUITools.FindGameObject("TargetAnim/Anim", base.gameObject).GetComponent<Animator>();
			this.mSelfResult = GameUITools.FindUILabel("SelfResult", base.gameObject);
			this.mTargetResult = GameUITools.FindUILabel("TargetResult", base.gameObject);
		}

		public void Refresh(int bout, int value, int self, int tar, bool halfAnim)
		{
			this.tempValue = value;
			this.target = tar;
			base.StartCoroutine(this.PlayAnim(bout, value, self, tar, halfAnim));
		}

		[DebuggerHidden]
		public IEnumerator PlayAnim(int bout, int value, int self, int tar, bool halfAnim)
		{
            return null;
            //MagicLoveFarmLayer.MagicLoveMatchItem.<PlayAnim>c__Iterator7F <PlayAnim>c__Iterator7F = new MagicLoveFarmLayer.MagicLoveMatchItem.<PlayAnim>c__Iterator7F();
            //<PlayAnim>c__Iterator7F.bout = bout;
            //<PlayAnim>c__Iterator7F.halfAnim = halfAnim;
            //<PlayAnim>c__Iterator7F.self = self;
            //<PlayAnim>c__Iterator7F.tar = tar;
            //<PlayAnim>c__Iterator7F.value = value;
            //<PlayAnim>c__Iterator7F.<$>bout = bout;
            //<PlayAnim>c__Iterator7F.<$>halfAnim = halfAnim;
            //<PlayAnim>c__Iterator7F.<$>self = self;
            //<PlayAnim>c__Iterator7F.<$>tar = tar;
            //<PlayAnim>c__Iterator7F.<$>value = value;
            //<PlayAnim>c__Iterator7F.<>f__this = this;
            //return <PlayAnim>c__Iterator7F;
		}

		public void Pass()
		{
			this.mTips.text = Singleton<StringManager>.Instance.GetString("MagicLove12", new object[]
			{
				this.tempValue
			});
			this.mTips.color = Color.green;
			int num = (this.target + 1) % 3 + 1;
			for (int i = 0; i < this.mTargets.Length; i++)
			{
				if (num - 1 == i)
				{
					this.mTargets[i].enabled = true;
				}
				else
				{
					this.mTargets[i].enabled = false;
				}
			}
			this.mSelfResult.enabled = true;
			this.mTargetResult.enabled = false;
		}
	}

	private GUIMagicLoveScene mBaseScene;

	private UIScrollView mScrollView;

	private UIGrid mContent;

	private GameObject mPass;

	private GameObject mCancel;

	private GameObject mOK;

	private UILabel mPassCost;

	private List<MagicLoveFarmLayer.MagicLoveMatchItem> mItems = new List<MagicLoveFarmLayer.MagicLoveMatchItem>();

	private int tempBout;

	public void Init(GUIMagicLoveScene basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mScrollView = GameUITools.FindGameObject("Panel", base.gameObject).GetComponent<UIScrollView>();
		this.mContent = GameUITools.FindGameObject("Content", this.mScrollView.gameObject).GetComponent<UIGrid>();
		this.mPass = GameUITools.RegisterClickEvent("Pass", new UIEventListener.VoidDelegate(this.OnPassClick), base.gameObject);
		this.mCancel = GameUITools.RegisterClickEvent("Cancel", new UIEventListener.VoidDelegate(this.OnCancelClick), base.gameObject);
		this.mOK = GameUITools.RegisterClickEvent("OK", new UIEventListener.VoidDelegate(this.OnOKClick), base.gameObject);
		this.mPassCost = GameUITools.FindUILabel("Cost", this.mPass);
	}

	public void Refresh(List<int> selfs, List<int> targets)
	{
		this.mPass.SetActive(false);
		this.mCancel.SetActive(false);
		this.mOK.SetActive(false);
		base.StartCoroutine(this.RefreshContent(selfs, targets));
	}

	[DebuggerHidden]
	private IEnumerator RefreshContent(List<int> selfs, List<int> targets)
	{
        return null;
        //MagicLoveFarmLayer.<RefreshContent>c__Iterator7E <RefreshContent>c__Iterator7E = new MagicLoveFarmLayer.<RefreshContent>c__Iterator7E();
        //<RefreshContent>c__Iterator7E.selfs = selfs;
        //<RefreshContent>c__Iterator7E.targets = targets;
        //<RefreshContent>c__Iterator7E.<$>selfs = selfs;
        //<RefreshContent>c__Iterator7E.<$>targets = targets;
        //<RefreshContent>c__Iterator7E.<>f__this = this;
        //return <RefreshContent>c__Iterator7E;
	}

	public void PassLast()
	{
		this.mItems[this.mItems.Count - 1].Pass();
		if (this.mBaseScene.mData.Bout == 0)
		{
			this.mOK.SetActive(true);
			this.mPass.SetActive(false);
			this.mCancel.SetActive(false);
		}
	}

	private MagicLoveFarmLayer.MagicLoveMatchItem AddItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/MagicLoveMatchItem");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = this.mContent.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		MagicLoveFarmLayer.MagicLoveMatchItem magicLoveMatchItem = gameObject.AddComponent<MagicLoveFarmLayer.MagicLoveMatchItem>();
		magicLoveMatchItem.Init();
		return magicLoveMatchItem;
	}

	private void OnPassClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseScene.SendMagicCamouflage(true);
	}

	private void OnCancelClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseScene.SendGo();
	}

	private void OnOKClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.Clear();
		this.mBaseScene.Refresh();
		this.mBaseScene.TryPlayFinishAnim();
		this.mBaseScene.TryPlayIncreaseAnim();
	}

	public void Clear()
	{
		for (int i = 0; i < this.mItems.Count; i++)
		{
			UnityEngine.Object.Destroy(this.mItems[i].gameObject);
		}
		this.mItems.Clear();
		this.tempBout = 0;
	}
}
