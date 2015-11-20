using Holoville.HOTween;
using Holoville.HOTween.Core;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIPetTrainYaoShuiLayer : MonoBehaviour
{
	private GUIPetTrainSceneV2 mBaseScene;

	private UILabel mExpBarNum;

	private UISlider mExpBar;

	private UILabel mMax;

	private int mItemNum;

	private GUIPetTrainLvlUpItem[] mLvlupItems;

	private bool[] mIsUse;

	private bool[] mHasItem;

	private UILabel mCostNum;

	private StringBuilder mSb = new StringBuilder(42);

	private Sequence mSequenceForLevelup;

	public bool IsAnimationing;

	private bool MoneyIsEnough
	{
		get;
		set;
	}

	private int CurNeedMoney
	{
		get;
		set;
	}

	public void InitWithBaseScene(GUIPetTrainSceneV2 baseScene, int itemnum)
	{
		this.mBaseScene = baseScene;
		this.mItemNum = itemnum;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mLvlupItems = new GUIPetTrainLvlUpItem[this.mItemNum];
		this.mIsUse = new bool[this.mItemNum];
		this.mHasItem = new bool[this.mItemNum];
		this.mExpBar = base.transform.Find("expBar").GetComponent<UISlider>();
		this.mExpBarNum = this.mExpBar.transform.Find("num").GetComponent<UILabel>();
		if (base.transform.Find("max") != null)
		{
			this.mMax = base.transform.Find("max").GetComponent<UILabel>();
		}
		int i;
		for (i = 0; i < this.mItemNum; i++)
		{
			this.mLvlupItems[i] = base.transform.Find(string.Format("item{0}", i)).gameObject.AddComponent<GUIPetTrainLvlUpItem>();
			this.mLvlupItems[i].InitWithBaseScene(this.mBaseScene, i);
		}
		Transform transform = base.transform.Find(string.Format("item{0}", i));
		while (transform != null)
		{
			transform.gameObject.SetActive(false);
			i++;
			transform = base.transform.Find(string.Format("item{0}", i));
		}
		this.mCostNum = base.transform.Find("txt1/costNum").GetComponent<UILabel>();
		GameObject gameObject = base.transform.Find("lvlUp5Btn").gameObject;
		UIEventListener expr_18C = UIEventListener.Get(gameObject);
		expr_18C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_18C.onClick, new UIEventListener.VoidDelegate(this.OnLvlUp5BtnClick));
		GameObject gameObject2 = base.transform.Find("lvlUpBtn").gameObject;
		UIEventListener expr_1C9 = UIEventListener.Get(gameObject2);
		expr_1C9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C9.onClick, new UIEventListener.VoidDelegate(this.OnLvlUpBtnClick));
	}

	private void SetItemIsUse()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		LopetDataEx curLopetDataEx = this.mBaseScene.CurLopetDataEx;
		if (curPetDataEx != null)
		{
			uint num = curPetDataEx.GetMaxExp() - curPetDataEx.Data.Exp;
			for (int i = 0; i < this.mItemNum; i++)
			{
				this.mIsUse[i] = false;
				if (!this.mLvlupItems[i].IsItemEmpty())
				{
					int expNum = this.mLvlupItems[i].GetExpNum();
					if (expNum != 0)
					{
						int num2 = (int)(num / (uint)expNum);
						int num3 = (int)(num % (uint)expNum);
						if (num3 != 0)
						{
							num2++;
						}
						this.mIsUse[i] = true;
						if (this.mLvlupItems[i].HasItemCount >= num2)
						{
							break;
						}
						num -= (uint)(this.mLvlupItems[i].HasItemCount * expNum);
						if (num <= 0u)
						{
							break;
						}
					}
				}
			}
		}
		else if (curLopetDataEx != null)
		{
			uint num4 = curLopetDataEx.GetMaxExp() - curLopetDataEx.Data.Exp;
			for (int j = 0; j < this.mItemNum; j++)
			{
				this.mIsUse[j] = false;
				if (!this.mLvlupItems[j].IsItemEmpty())
				{
					int expNum2 = this.mLvlupItems[j].GetExpNum();
					if (expNum2 != 0)
					{
						int num5 = (int)(num4 / (uint)expNum2);
						int num6 = (int)(num4 % (uint)expNum2);
						if (num6 != 0)
						{
							num5++;
						}
						this.mIsUse[j] = true;
						if (this.mLvlupItems[j].HasItemCount >= num5)
						{
							break;
						}
						num4 -= (uint)(this.mLvlupItems[j].HasItemCount * expNum2);
						if (num4 <= 0u)
						{
							break;
						}
					}
				}
			}
		}
	}

	public void Refresh()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		LopetDataEx curLopetDataEx = this.mBaseScene.CurLopetDataEx;
		if (curPetDataEx != null)
		{
			uint maxExp = curPetDataEx.GetMaxExp();
			this.mExpBar.value = ((maxExp == 0u) ? 1f : (curPetDataEx.Data.Exp / maxExp));
			this.mExpBarNum.text = this.mSb.Remove(0, this.mSb.Length).Append(curPetDataEx.Data.Exp).Append("/").Append(maxExp).ToString();
			int money = Globals.Instance.Player.Data.Money;
			this.CurNeedMoney = 0;
			uint num = 0u;
			if (maxExp >= curPetDataEx.Data.Exp)
			{
				num = maxExp - curPetDataEx.Data.Exp;
			}
			for (int i = 0; i < this.mItemNum; i++)
			{
				this.mLvlupItems[i].Refresh();
			}
			for (int j = 0; j < this.mItemNum; j++)
			{
				if (!this.mLvlupItems[j].IsItemEmpty())
				{
					int expNum = this.mLvlupItems[j].GetExpNum();
					if (expNum > 0)
					{
						int num2 = (int)(num / (uint)expNum);
						int num3 = (int)(num % (uint)expNum);
						if (num3 != 0)
						{
							num2++;
						}
						if (this.mLvlupItems[j].HasItemCount >= num2)
						{
							this.CurNeedMoney += num2 * expNum;
							break;
						}
						this.CurNeedMoney += this.mLvlupItems[j].HasItemCount * expNum;
						num -= (uint)(this.mLvlupItems[j].HasItemCount * expNum);
						if (num <= 0u)
						{
							break;
						}
					}
				}
			}
			this.CurNeedMoney /= 5;
			this.mCostNum.text = this.CurNeedMoney.ToString();
			if (money < this.CurNeedMoney)
			{
				this.mCostNum.color = Color.red;
			}
			else
			{
				this.mCostNum.color = new Color(0.8862745f, 0.768627465f, 0.5921569f);
			}
			this.MoneyIsEnough = (money >= this.CurNeedMoney);
			bool flag = false;
			long num4 = 0L;
			int num5 = (int)(curPetDataEx.GetMaxExp() - curPetDataEx.Data.Exp);
			for (int k = 0; k < this.mItemNum; k++)
			{
				num4 += this.mLvlupItems[k].GetTotalExpNum();
				if (num4 >= (long)num5)
				{
					flag = true;
					break;
				}
			}
			this.mBaseScene.ShowPetLvlUpNewMark = (this.MoneyIsEnough && curPetDataEx.Data.Level < Globals.Instance.Player.Data.Level && flag);
		}
		else if (curLopetDataEx != null)
		{
			uint maxExp2 = curLopetDataEx.GetMaxExp();
			this.mExpBar.value = ((maxExp2 == 0u) ? 1f : (curLopetDataEx.Data.Exp / maxExp2));
			this.mExpBarNum.text = this.mSb.Remove(0, this.mSb.Length).Append(curLopetDataEx.Data.Exp).Append("/").Append(maxExp2).ToString();
			if ((ulong)curLopetDataEx.Data.Level >= (ulong)((long)GameConst.GetInt32(240)))
			{
				this.mMax.enabled = true;
				this.mExpBar.gameObject.SetActive(false);
			}
			else
			{
				this.mMax.enabled = false;
				this.mExpBar.gameObject.SetActive(true);
			}
			int money2 = Globals.Instance.Player.Data.Money;
			this.CurNeedMoney = 0;
			uint num6 = 0u;
			if (maxExp2 >= curLopetDataEx.Data.Exp)
			{
				num6 = maxExp2 - curLopetDataEx.Data.Exp;
			}
			for (int l = 0; l < this.mItemNum; l++)
			{
				this.mLvlupItems[l].Refresh();
			}
			for (int m = 0; m < this.mItemNum; m++)
			{
				if (!this.mLvlupItems[m].IsItemEmpty())
				{
					int expNum2 = this.mLvlupItems[m].GetExpNum();
					if (expNum2 > 0)
					{
						int num7 = (int)(num6 / (uint)expNum2);
						int num8 = (int)(num6 % (uint)expNum2);
						if (num8 != 0)
						{
							num7++;
						}
						if (this.mLvlupItems[m].HasItemCount >= num7)
						{
							this.CurNeedMoney += num7 * expNum2;
							break;
						}
						this.CurNeedMoney += this.mLvlupItems[m].HasItemCount * expNum2;
						num6 -= (uint)(this.mLvlupItems[m].HasItemCount * expNum2);
						if (num6 <= 0u)
						{
							break;
						}
					}
				}
			}
			this.CurNeedMoney = (int)((float)this.CurNeedMoney / ((float)GameConst.GetInt32(250) / 10000f));
			this.mCostNum.text = this.CurNeedMoney.ToString();
			if (money2 < this.CurNeedMoney)
			{
				this.mCostNum.color = Color.red;
			}
			else
			{
				this.mCostNum.color = new Color(0.8862745f, 0.768627465f, 0.5921569f);
			}
			this.MoneyIsEnough = (money2 >= this.CurNeedMoney);
			bool flag2 = false;
			long num9 = 0L;
			int num10 = (int)(curLopetDataEx.GetMaxExp() - curLopetDataEx.Data.Exp);
			for (int n = 0; n < this.mItemNum; n++)
			{
				num9 += this.mLvlupItems[n].GetTotalExpNum();
				if (num9 >= (long)num10)
				{
					flag2 = true;
					break;
				}
			}
			this.mBaseScene.ShowLopetLvlUpNewMark = (this.MoneyIsEnough && (ulong)curLopetDataEx.Data.Level <= (ulong)((long)GameConst.GetInt32(240)) && flag2);
		}
	}

	public void OnLvlUpBtnClick(GameObject go)
	{
		if (this.IsAnimationing)
		{
			return;
		}
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		LopetDataEx curLopetDataEx = this.mBaseScene.CurLopetDataEx;
		if (curPetDataEx != null)
		{
			if (Globals.Instance.Player.Data.Level <= curPetDataEx.Data.Level)
			{
				Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
				GameUIManager.mInstance.ShowMessageTipByKey("petTrainTxt4", 0f, 0f);
				return;
			}
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Money, this.CurNeedMoney, 0))
			{
				return;
			}
			long num = 0L;
			int num2 = (int)(curPetDataEx.GetMaxExp() - curPetDataEx.Data.Exp);
			this.SetItemIsUse();
			for (int i = 0; i < this.mItemNum; i++)
			{
				this.mHasItem[i] = !this.mLvlupItems[i].IsItemEmpty();
			}
			for (int j = 0; j < this.mItemNum; j++)
			{
				num += this.mLvlupItems[j].GetTotalExpNum();
				if (num >= (long)num2)
				{
					this.mBaseScene.SetOldAttrNum();
					MC2S_PetLevelup mC2S_PetLevelup = new MC2S_PetLevelup();
					mC2S_PetLevelup.PetID = curPetDataEx.Data.ID;
					mC2S_PetLevelup.Flag = false;
					Globals.Instance.CliSession.Send(402, mC2S_PetLevelup);
					break;
				}
			}
			if (num < (long)num2)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("petTrainTxt5", 0f, 0f);
			}
		}
		else if (curLopetDataEx != null)
		{
			if ((long)GameConst.GetInt32(240) <= (long)((ulong)curLopetDataEx.Data.Level))
			{
				Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
				GameUIManager.mInstance.ShowMessageTipByKey("Lopet4", 0f, 0f);
				return;
			}
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Money, this.CurNeedMoney, 0))
			{
				return;
			}
			long num3 = 0L;
			int num4 = (int)(curLopetDataEx.GetMaxExp() - curLopetDataEx.Data.Exp);
			this.SetItemIsUse();
			for (int k = 0; k < this.mItemNum; k++)
			{
				this.mHasItem[k] = !this.mLvlupItems[k].IsItemEmpty();
			}
			for (int l = 0; l < this.mItemNum; l++)
			{
				num3 += this.mLvlupItems[l].GetTotalExpNum();
				if (num3 >= (long)num4)
				{
					this.mBaseScene.SetOldAttrNum();
					MC2S_LopetLevelup mC2S_LopetLevelup = new MC2S_LopetLevelup();
					mC2S_LopetLevelup.LopetID = curLopetDataEx.Data.ID;
					mC2S_LopetLevelup.Level = 1;
					Globals.Instance.CliSession.Send(1062, mC2S_LopetLevelup);
					break;
				}
			}
			if (num3 < (long)num4)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("Lopet5", 0f, 0f);
			}
		}
	}

	public void OnLvlUp5BtnClick(GameObject go)
	{
		if (this.IsAnimationing)
		{
			return;
		}
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		LopetDataEx curLopetDataEx = this.mBaseScene.CurLopetDataEx;
		if (curPetDataEx != null)
		{
			if (Globals.Instance.Player.Data.Level <= curPetDataEx.Data.Level)
			{
				Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
				GameUIManager.mInstance.ShowMessageTipByKey("petTrainTxt4", 0f, 0f);
				return;
			}
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Money, this.CurNeedMoney, 0))
			{
				return;
			}
			this.SetItemIsUse();
			for (int i = 0; i < this.mItemNum; i++)
			{
				this.mHasItem[i] = !this.mLvlupItems[i].IsItemEmpty();
			}
			long num = 0L;
			int num2 = (int)(curPetDataEx.GetMaxExp() - curPetDataEx.Data.Exp);
			for (int j = 0; j < this.mItemNum; j++)
			{
				num += this.mLvlupItems[j].GetTotalExpNum();
				if (num >= (long)num2)
				{
					this.mBaseScene.SetOldAttrNum();
					MC2S_PetLevelup mC2S_PetLevelup = new MC2S_PetLevelup();
					mC2S_PetLevelup.PetID = curPetDataEx.Data.ID;
					mC2S_PetLevelup.Flag = true;
					Globals.Instance.CliSession.Send(402, mC2S_PetLevelup);
					break;
				}
			}
			if (num < (long)num2)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("petTrainTxt5", 0f, 0f);
			}
		}
		else if (curLopetDataEx != null)
		{
			if ((long)GameConst.GetInt32(240) <= (long)((ulong)curLopetDataEx.Data.Level))
			{
				Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
				GameUIManager.mInstance.ShowMessageTipByKey("Lopet4", 0f, 0f);
				return;
			}
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Money, this.CurNeedMoney, 0))
			{
				return;
			}
			this.SetItemIsUse();
			for (int k = 0; k < this.mItemNum; k++)
			{
				this.mHasItem[k] = !this.mLvlupItems[k].IsItemEmpty();
			}
			long num3 = 0L;
			int num4 = (int)(curLopetDataEx.GetMaxExp() - curLopetDataEx.Data.Exp);
			for (int l = 0; l < this.mItemNum; l++)
			{
				num3 += this.mLvlupItems[l].GetTotalExpNum();
				if (num3 >= (long)num4)
				{
					this.mBaseScene.SetOldAttrNum();
					MC2S_LopetLevelup mC2S_LopetLevelup = new MC2S_LopetLevelup();
					mC2S_LopetLevelup.LopetID = curLopetDataEx.Data.ID;
					mC2S_LopetLevelup.Level = 5;
					Globals.Instance.CliSession.Send(1062, mC2S_LopetLevelup);
					break;
				}
			}
			if (num3 < (long)num4)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("Lopet5", 0f, 0f);
			}
		}
	}

	public void DestroySequenceForLevelup()
	{
		if (this.mSequenceForLevelup != null)
		{
			this.mSequenceForLevelup.Kill();
			this.mSequenceForLevelup = null;
		}
	}

	private void HideItemEffects()
	{
		for (int i = 0; i < this.mItemNum; i++)
		{
			this.mLvlupItems[i].HideEffec0();
			this.mLvlupItems[i].HideEffec1();
		}
	}

	public void PlayLvlUpEffectAnimation()
	{
		this.DestroySequenceForLevelup();
		this.mSequenceForLevelup = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnAnimEnd)));
		if (this.mIsUse[0] && this.mHasItem[0])
		{
            this.mSequenceForLevelup.AppendCallback(() =>
			{
				this.mLvlupItems[0].PlayEffect0();
			});
			this.mSequenceForLevelup.AppendInterval(0.3f);
            this.mSequenceForLevelup.AppendCallback(() =>
			{
				this.mLvlupItems[0].PlayEffect1();
			});
		}
		if (this.mIsUse[1] && this.mHasItem[1])
		{
            this.mSequenceForLevelup.InsertCallback(0f, () =>
			{
				this.mLvlupItems[1].PlayEffect0();
			});
            this.mSequenceForLevelup.InsertCallback(0.3f, () =>
			{
				this.mLvlupItems[1].PlayEffect1();
			});
		}
		if (this.mIsUse[2] && this.mHasItem[2])
		{
            this.mSequenceForLevelup.InsertCallback(0f, () =>
			{
				this.mLvlupItems[2].PlayEffect0();
			});
            this.mSequenceForLevelup.InsertCallback(0.3f, () =>
			{
				this.mLvlupItems[2].PlayEffect1();
			});
		}
		if (3 < this.mIsUse.Length && 3 < this.mHasItem.Length && this.mIsUse[3] && this.mHasItem[3])
		{
            this.mSequenceForLevelup.InsertCallback(0f, () =>
			{
				this.mLvlupItems[3].PlayEffect0();
			});
            this.mSequenceForLevelup.InsertCallback(0.3f, () =>
			{
				this.mLvlupItems[3].PlayEffect1();
			});
		}
		if (this.mIsUse[0])
		{
			this.mSequenceForLevelup.AppendInterval(0.4f);
		}
		else
		{
			this.mSequenceForLevelup.AppendInterval(0.7f);
		}
        this.mSequenceForLevelup.AppendCallback(() =>
		{
			this.HideItemEffects();
			this.mBaseScene.PlayModelEffect();
		});
		this.mSequenceForLevelup.Play();
		this.IsAnimationing = true;
	}

	private void OnAnimEnd()
	{
		this.mBaseScene.PlayLvlUpMsgTip();
		this.IsAnimationing = false;
		Globals.Instance.TutorialMgr.InitializationCompleted(this, null);
	}

	public void PlayExpBarEffect()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		LopetDataEx curLopetDataEx = this.mBaseScene.CurLopetDataEx;
		if (curPetDataEx != null)
		{
			uint maxExp = curPetDataEx.GetMaxExp();
			float endValue = (maxExp == 0u) ? 1f : (curPetDataEx.Data.Exp / maxExp);
			uint num = curPetDataEx.Data.Level - this.mBaseScene.GetOldLvl();
			float duration = (num <= 4u) ? 0.2f : 0.1f;
			GameUITools.PlayUISlilderEffect(this.mExpBar, this.mExpBarNum, num, endValue, duration, (int)maxExp);
		}
		else if (curLopetDataEx != null)
		{
			uint maxExp2 = curLopetDataEx.GetMaxExp();
			float endValue2 = (maxExp2 == 0u) ? 1f : (curLopetDataEx.Data.Exp / maxExp2);
			uint num2 = curLopetDataEx.Data.Level - this.mBaseScene.GetOldLvl();
			float duration2 = (num2 <= 4u) ? 0.2f : 0.1f;
			GameUITools.PlayUISlilderEffect(this.mExpBar, this.mExpBarNum, num2, endValue2, duration2, (int)maxExp2);
		}
	}
}
