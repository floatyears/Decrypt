using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Proto;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GUIPetTrainTunShiLayer : MonoBehaviour
{
	private const int mTuiShiItemNum = 5;

	private GUIPetTrainSceneV2 mBaseScene;

	private UILabel mExpBarNum;

	private UISlider mExpBar;

	private UISprite mExpBarFg2;

	private UILabel mNewLvl;

	private GUIPetTrainTunShiItem[] mTunShiItems = new GUIPetTrainTunShiItem[5];

	private PetDataEx[] mTunShiItemDatas = new PetDataEx[5];

	private UILabel mAddExpNum;

	private UILabel mCostNum;

	public bool IsAnimationing;

	private StringBuilder mSb = new StringBuilder(42);

	private Sequence mSequenceForLevelup;

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

	public void InitWithBaseScene(GUIPetTrainSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mNewLvl = base.transform.Find("lvl").GetComponent<UILabel>();
		this.mExpBar = base.transform.Find("expBar").GetComponent<UISlider>();
		this.mExpBarNum = this.mExpBar.transform.Find("num").GetComponent<UILabel>();
		this.mExpBarFg2 = this.mExpBar.transform.Find("expBarFg2").GetComponent<UISprite>();
		for (int i = 0; i < 5; i++)
		{
			this.mTunShiItems[i] = base.transform.Find(string.Format("item{0}", i)).gameObject.AddComponent<GUIPetTrainTunShiItem>();
			this.mTunShiItems[i].InitWithBaseScene(this, this.mBaseScene, i);
		}
		this.mAddExpNum = base.transform.Find("txt0/num").GetComponent<UILabel>();
		this.mCostNum = base.transform.Find("txt1/costNum").GetComponent<UILabel>();
		GameObject gameObject = base.transform.Find("autoSet").gameObject;
		UIEventListener expr_11C = UIEventListener.Get(gameObject);
		expr_11C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_11C.onClick, new UIEventListener.VoidDelegate(this.OnAutoSetBtnClick));
		GameObject gameObject2 = base.transform.Find("lvlUpBtn").gameObject;
		UIEventListener expr_159 = UIEventListener.Get(gameObject2);
		expr_159.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_159.onClick, new UIEventListener.VoidDelegate(this.OnLvlUpBtnClick));
	}

	private void OnLvlUpBtnClick(GameObject go)
	{
		if (this.IsAnimationing)
		{
			return;
		}
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			if (Globals.Instance.Player.Data.Level <= curPetDataEx.Data.Level)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("petTrainTxt4", 0f, 0f);
				return;
			}
			bool flag = true;
			for (int i = 0; i < 5; i++)
			{
				if (!this.mTunShiItems[i].IsItemEmpty())
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("petTrainTxt6", 0f, 0f);
				return;
			}
			this.CurNeedMoney = 0;
			for (int j = 0; j < 5; j++)
			{
				this.CurNeedMoney += (int)this.mTunShiItems[j].GetCurPetExpMoney();
			}
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Money, this.CurNeedMoney, 0))
			{
				return;
			}
			this.mBaseScene.SetOldAttrNum();
			MC2S_PetLevelup mC2S_PetLevelup = new MC2S_PetLevelup();
			mC2S_PetLevelup.PetID = curPetDataEx.Data.ID;
			for (int k = 0; k < 5; k++)
			{
				if (this.mTunShiItemDatas[k] != null && this.mTunShiItemDatas[k].Data.ID != 0uL)
				{
					mC2S_PetLevelup.Pets.Add(this.mTunShiItemDatas[k].Data.ID);
				}
			}
			Globals.Instance.CliSession.Send(402, mC2S_PetLevelup);
		}
	}

	private void OnAutoSetBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		List<PetDataEx> petDatas = this.mBaseScene.GetPetDatas();
		if (petDatas.Count == 0)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("petTrainTxt0", 0f, 0f);
			return;
		}
		for (int i = 0; i < 5; i++)
		{
			if (this.mTunShiItemDatas[i] == null)
			{
				this.mTunShiItemDatas[i] = ((i >= petDatas.Count) ? null : petDatas[i]);
			}
		}
		this.Refresh();
	}

	private int GetCanLvlNum(int addExp, out int addExpIndeed)
	{
		int num = 0;
		addExpIndeed = 0;
		uint level = Globals.Instance.Player.Data.Level;
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			uint num2 = curPetDataEx.GetMaxExp() - curPetDataEx.Data.Exp;
			uint num3 = num2;
			for (uint num4 = curPetDataEx.Data.Level + 1u; num4 <= level; num4 += 1u)
			{
				LevelInfo info = Globals.Instance.AttDB.LevelDict.GetInfo((int)num4);
				if (info != null && curPetDataEx.Info.Quality < info.Exp.Count && curPetDataEx.Info.Quality >= 0)
				{
					num3 += info.Exp[curPetDataEx.Info.Quality];
				}
			}
			if ((long)addExp >= (long)((ulong)num3))
			{
				num = (int)(level - curPetDataEx.Data.Level);
				addExpIndeed = (int)num3;
			}
			else if ((long)addExp > (long)((ulong)num2))
			{
				addExp -= (int)num2;
				addExpIndeed += (int)num2;
				num++;
				while (addExp > 0)
				{
					int id = (int)(curPetDataEx.Data.Level + (uint)num);
					LevelInfo info = Globals.Instance.AttDB.LevelDict.GetInfo(id);
					if (info == null)
					{
						break;
					}
					if (curPetDataEx.Info.Quality < info.Exp.Count && curPetDataEx.Info.Quality >= 0)
					{
						addExp -= (int)info.Exp[curPetDataEx.Info.Quality];
						addExpIndeed += (int)info.Exp[curPetDataEx.Info.Quality];
						if (addExp < 0)
						{
							addExpIndeed += addExp;
						}
						else
						{
							num++;
						}
					}
				}
			}
			else if ((long)addExp == (long)((ulong)num2))
			{
				num++;
				addExpIndeed = addExp;
			}
			else
			{
				num = 0;
				addExpIndeed = addExp;
			}
		}
		return num;
	}

	public void Refresh()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			uint maxExp = curPetDataEx.GetMaxExp();
			this.mExpBar.value = ((maxExp == 0u) ? 1f : (curPetDataEx.Data.Exp / maxExp));
			this.mExpBarNum.text = this.mSb.Remove(0, this.mSb.Length).Append(curPetDataEx.Data.Exp).Append("/").Append(maxExp).ToString();
			int money = Globals.Instance.Player.Data.Money;
			uint num = 0u;
			uint num2 = 0u;
			int num3 = 0;
			for (int i = 0; i < 5; i++)
			{
				this.mTunShiItems[i].Refresh(this.mTunShiItemDatas[i]);
				num += this.mTunShiItems[i].GetCurPetExpMoney();
				num2 += this.mTunShiItems[i].GetCurPetExpNum();
			}
			num /= 5u;
			if (num > 0u)
			{
				this.mExpBarFg2.gameObject.SetActive(true);
				int canLvlNum = this.GetCanLvlNum((int)num2, out num3);
				if (canLvlNum > 0)
				{
					this.mNewLvl.gameObject.SetActive(true);
					this.mNewLvl.text = string.Format("+{0}", canLvlNum);
				}
				else
				{
					this.mNewLvl.gameObject.SetActive(false);
				}
				this.mExpBarFg2.fillAmount = ((maxExp == 0u) ? 1f : Mathf.Clamp01((curPetDataEx.Data.Exp + num2) / maxExp));
			}
			else
			{
				this.mNewLvl.gameObject.SetActive(false);
				this.mExpBarFg2.gameObject.SetActive(false);
			}
			this.mCostNum.text = num.ToString();
			if ((long)money < (long)((ulong)num))
			{
				this.mCostNum.color = Color.red;
			}
			else
			{
				this.mCostNum.color = Color.white;
			}
			this.MoneyIsEnough = ((long)money >= (long)((ulong)num));
			this.mAddExpNum.text = num3.ToString();
		}
	}

	public void GoToLvlUpSelPetScene()
	{
		PetDataEx pdEx = this.mBaseScene.CurPetDataEx;
		if (pdEx != null)
		{
			if (pdEx.Data.Level < Globals.Instance.Player.Data.Level)
			{
				GameUIManager.mInstance.ChangeSession<GUILvlUpSelPetSceneV2>(delegate(GUILvlUpSelPetSceneV2 sen)
				{
					sen.CurPetDataEx = pdEx;
					sen.SetSelectPetDatas(this.mTunShiItemDatas);
				}, false, true);
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("petTrainTxt4", 0f, 0f);
			}
		}
	}

	public void SetTuiShiItems(List<PetDataEx> petDatas)
	{
		if (petDatas != null)
		{
			for (int i = 0; i < 5; i++)
			{
				if (i < petDatas.Count)
				{
					this.mTunShiItemDatas[i] = petDatas[i];
				}
			}
		}
		else
		{
			for (int j = 0; j < 5; j++)
			{
				this.mTunShiItemDatas[j] = null;
			}
		}
		this.Refresh();
	}

	public void ClearItem(int i)
	{
		if (0 <= i && i < 5)
		{
			this.mTunShiItemDatas[i] = null;
		}
		this.Refresh();
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
		for (int i = 0; i < 5; i++)
		{
			this.mTunShiItems[i].HideEffec0();
			this.mTunShiItems[i].HideEffec1();
		}
	}

	public void PlayLvlUpEffectAnimation()
	{
		this.DestroySequenceForLevelup();
		this.mSequenceForLevelup = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnAnimEnd)));
		if (!this.mTunShiItems[0].IsItemEmpty())
		{
            this.mSequenceForLevelup.AppendCallback(() =>
			{
				this.mTunShiItems[0].PlayEffect0();
			});
			this.mSequenceForLevelup.AppendInterval(0.3f);
            this.mSequenceForLevelup.AppendCallback(() =>
			{
				this.mTunShiItems[0].PlayEffect1();
			});
		}
		if (!this.mTunShiItems[1].IsItemEmpty())
		{
            this.mSequenceForLevelup.InsertCallback(0f, () =>
			{
				this.mTunShiItems[1].PlayEffect0();
			});
            this.mSequenceForLevelup.InsertCallback(0.3f, () =>
			{
				this.mTunShiItems[1].PlayEffect1();
			});
		}
		if (!this.mTunShiItems[2].IsItemEmpty())
		{
            this.mSequenceForLevelup.InsertCallback(0f, () =>
			{
				this.mTunShiItems[2].PlayEffect0();
			});
            this.mSequenceForLevelup.InsertCallback(0.3f, () =>
			{
				this.mTunShiItems[2].PlayEffect1();
			});
		}
		if (!this.mTunShiItems[3].IsItemEmpty())
		{
            this.mSequenceForLevelup.InsertCallback(0f, () =>
			{
				this.mTunShiItems[3].PlayEffect0();
			});
            this.mSequenceForLevelup.InsertCallback(0.3f, () =>
			{
				this.mTunShiItems[3].PlayEffect1();
			});
		}
		if (!this.mTunShiItems[4].IsItemEmpty())
		{
            this.mSequenceForLevelup.InsertCallback(0f, () =>
			{
				this.mTunShiItems[4].PlayEffect0();
			});
            this.mSequenceForLevelup.InsertCallback(0.3f, () =>
			{
				this.mTunShiItems[4].PlayEffect1();
			});
		}
		if (!this.mTunShiItems[0].IsItemEmpty())
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
	}

	public void PlayExpBarEffect()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			uint maxExp = curPetDataEx.GetMaxExp();
			float endValue = (maxExp == 0u) ? 1f : (curPetDataEx.Data.Exp / maxExp);
			uint num = curPetDataEx.Data.Level - this.mBaseScene.GetOldLvl();
			float duration = (num <= 4u) ? 0.2f : (1f / num);
			GameUITools.PlayUISlilderEffect(this.mExpBar, this.mExpBarNum, num, endValue, duration, (int)maxExp);
		}
	}
}
