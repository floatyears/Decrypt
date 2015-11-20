using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using System;
using UnityEngine;

public class MagicLoveStartLayer : MonoBehaviour
{
	private GUIMagicLoveScene mBaseScene;

	private UILabel mTipsValue;

	private UISprite mBar;

	private UILabel mValue;

	private UILabel mCount;

	private GameObject mUI100_1;

	private bool animFlag;

	private int tempBout;

	public int TempBout
	{
		get
		{
			return this.tempBout;
		}
	}

	public void Init(GUIMagicLoveScene basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mTipsValue = GameUITools.FindUILabel("Tips/Value", base.gameObject);
		this.mBar = GameUITools.FindUISprite("ProgressBar/Bar", base.gameObject);
		this.mValue = GameUITools.FindUILabel("ProgressBar/Value", base.gameObject);
		this.mCount = GameUITools.FindUILabel("Count", base.gameObject);
		this.mUI100_1 = GameUITools.FindGameObject("ui100_1", this.mBar.gameObject);
		Tools.SetParticleRenderQueue2(this.mUI100_1, 3050);
		this.mUI100_1.SetActive(false);
		GameUITools.RegisterClickEvent("Btn0", new UIEventListener.VoidDelegate(this.OnBtn0Click), base.gameObject);
		GameUITools.RegisterClickEvent("Btn1", new UIEventListener.VoidDelegate(this.OnBtn1Click), base.gameObject);
		GameUITools.RegisterClickEvent("Btn2", new UIEventListener.VoidDelegate(this.OnBtn2Click), base.gameObject);
		GameUITools.RegisterClickEvent("FarmBtn", new UIEventListener.VoidDelegate(this.OnFarmBtnClick), base.gameObject);
	}

	public void Refresh()
	{
		this.mUI100_1.SetActive(false);
		this.tempBout = this.mBaseScene.mData.Bout;
		if (Globals.Instance.Player.MagicLoveSystem.LastResult == MagicLoveSubSystem.ELastResult.ELR_Win || Globals.Instance.Player.MagicLoveSystem.LastResult == MagicLoveSubSystem.ELastResult.ELR_Draw)
		{
			this.tempBout = this.mBaseScene.mData.Bout + 1;
		}
		else
		{
			this.tempBout = this.mBaseScene.mData.Bout;
		}
		this.tempBout = Mathf.Clamp(this.tempBout, 1, GameConst.GetInt32(225));
		MagicLoveInfo info = Globals.Instance.AttDB.MagicLoveDict.GetInfo(this.tempBout);
		if (info == null)
		{
			global::Debug.LogErrorFormat("MagicLoveDict get info error , ID : {0} ", new object[]
			{
				this.tempBout
			});
			return;
		}
		this.mTipsValue.text = Singleton<StringManager>.Instance.GetString("MagicLove7", new object[]
		{
			info.RewardLoveValue
		});
		if (this.mBaseScene.mData.Bout > 0 && Globals.Instance.Player.MagicLoveSystem.LastResult == MagicLoveSubSystem.ELastResult.ELR_Win && this.animFlag)
		{
			MagicLoveInfo info2 = Globals.Instance.AttDB.MagicLoveDict.GetInfo(this.tempBout - 1);
			if (info2 == null)
			{
				global::Debug.LogErrorFormat("MagicLoveDict get info error , ID : {0} ", new object[]
				{
					this.tempBout - 1
				});
				return;
			}
			this.mBar.fillAmount = (float)(this.mBaseScene.mData.LoveValue[this.mBaseScene.CurIndex] - info2.RewardLoveValue) / (float)Globals.Instance.Player.MagicLoveSystem.MaxLoveValue;
			Globals.Instance.EffectSoundMgr.Play("ui/ui_030");
			this.mUI100_1.SetActive(false);
			this.mUI100_1.SetActive(true);
			HOTween.To(this.mBar, this.mBaseScene.BarTime, new TweenParms().Prop("fillAmount", (float)this.mBaseScene.mData.LoveValue[this.mBaseScene.CurIndex] / (float)Globals.Instance.Player.MagicLoveSystem.MaxLoveValue).OnUpdate(new TweenDelegate.TweenCallback(this.OnBarUpdate)).OnComplete(new TweenDelegate.TweenCallback(this.OnBarComplete)));
		}
		else
		{
			if (!this.animFlag)
			{
				this.animFlag = true;
			}
			this.mBar.fillAmount = (float)this.mBaseScene.mData.LoveValue[this.mBaseScene.CurIndex] / (float)Globals.Instance.Player.MagicLoveSystem.MaxLoveValue;
			this.mValue.text = Singleton<StringManager>.Instance.GetString("MagicLove2", new object[]
			{
				this.mBaseScene.mData.LoveValue[this.mBaseScene.CurIndex],
				Globals.Instance.Player.MagicLoveSystem.MaxLoveValue
			});
		}
		this.mCount.text = Singleton<StringManager>.Instance.GetString("MagicLove5", new object[]
		{
			this.tempBout,
			GameConst.GetInt32(225)
		});
	}

	private void OnBarUpdate()
	{
		this.mUI100_1.transform.localPosition = new Vector3(0f, (float)(-(float)this.mBar.height) * this.mBar.fillAmount, 0f);
		this.mValue.text = Singleton<StringManager>.Instance.GetString("MagicLove2", new object[]
		{
			(int)(this.mBar.fillAmount * (float)Globals.Instance.Player.MagicLoveSystem.MaxLoveValue),
			Globals.Instance.Player.MagicLoveSystem.MaxLoveValue
		});
	}

	private void OnBarComplete()
	{
		this.mUI100_1.SetActive(false);
		this.mValue.text = Singleton<StringManager>.Instance.GetString("MagicLove2", new object[]
		{
			this.mBaseScene.mData.LoveValue[this.mBaseScene.CurIndex],
			Globals.Instance.Player.MagicLoveSystem.MaxLoveValue
		});
	}

	private void OnBtn0Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseScene.SendMagicMatch(1);
	}

	private void OnBtn1Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseScene.SendMagicMatch(2);
	}

	private void OnBtn2Click(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseScene.SendMagicMatch(3);
	}

	private void OnFarmBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseScene.SendOneKeyMagicMatch();
		this.mBaseScene.SaveLoveValue();
	}
}
