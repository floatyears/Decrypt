using Att;
using Holoville.HOTween.Core;
using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIFirstRecharge : GameUISession
{
	private UILabel mBtnLb;

	private Transform[] ItemParents = new Transform[4];

	private UILabel[] mItemNames = new UILabel[4];

	private void CreateObjects()
	{
		GameObject gameObject = base.transform.Find("FadeBG").gameObject;
		UIEventListener expr_1C = UIEventListener.Get(gameObject);
		expr_1C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		GameObject gameObject2 = base.transform.Find("WinBg").gameObject;
		GameObject gameObject3 = gameObject2.transform.Find("closeBtn").gameObject;
		UIEventListener expr_6F = UIEventListener.Get(gameObject3);
		expr_6F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_6F.onClick, new UIEventListener.VoidDelegate(this.OnCloseBtnClick));
		GameObject gameObject4 = gameObject2.transform.Find("Btn").gameObject;
		UIEventListener expr_AC = UIEventListener.Get(gameObject4);
		expr_AC.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_AC.onClick, new UIEventListener.VoidDelegate(this.OnChargeBtnClick));
		this.mBtnLb = gameObject4.transform.Find("Label").GetComponent<UILabel>();
		Transform transform = gameObject2.transform.Find("itemBg");
		for (int i = 0; i < this.ItemParents.Length; i++)
		{
			this.ItemParents[i] = transform.Find(string.Format("Item{0}", i));
			this.mItemNames[i] = this.ItemParents[i].Find("name").GetComponent<UILabel>();
		}
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		Globals.Instance.CliSession.Register(241, new ClientSession.MsgHandler(this.OnMsgTakeFirstPayReward));
		this.Refresh();
		GameUITools.PlayOpenWindowAnim(base.transform, null, true);
	}

	protected override void OnPreDestroyGUI()
	{
		Globals.Instance.CliSession.Unregister(241, new ClientSession.MsgHandler(this.OnMsgTakeFirstPayReward));
	}

	private void OnCloseBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("UI/ui_002");
		GameUITools.PlayCloseWindowAnim(base.transform, new TweenDelegate.TweenCallback(this.OnCloseAnimEnd), true);
	}

	private void OnCloseAnimEnd()
	{
		base.Close();
	}

	private void OnChargeBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (!Globals.Instance.Player.IsFirstPayCompleted())
		{
			GameUIVip.OpenRecharge();
			base.Close();
			return;
		}
		if (!Globals.Instance.Player.IsFirstPayRewardTaken())
		{
			MC2S_TakeFirstPayReward ojb = new MC2S_TakeFirstPayReward();
			Globals.Instance.CliSession.Send(240, ojb);
		}
	}

	private void Refresh()
	{
		if (!Globals.Instance.Player.IsFirstPayCompleted())
		{
			this.mBtnLb.text = Singleton<StringManager>.Instance.GetString("payTake0");
		}
		else if (!Globals.Instance.Player.IsFirstPayRewardTaken())
		{
			this.mBtnLb.text = Singleton<StringManager>.Instance.GetString("payTake1");
		}
		MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(10);
		if (info != null && info.Day7RewardType.Count > 3)
		{
			int num = 0;
			for (int i = 0; i < info.Day7RewardType.Count; i++)
			{
				GameObject x = GameUITools.CreateReward(info.Day7RewardType[i], info.Day7RewardValue1[i], info.Day7RewardValue2[i], this.ItemParents[num], true, true, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
				if (x != null)
				{
					ERewardType eRewardType = (ERewardType)info.Day7RewardType[i];
					if (eRewardType != ERewardType.EReward_Item)
					{
						if (eRewardType != ERewardType.EReward_Pet)
						{
							if (eRewardType != ERewardType.EReward_Fashion)
							{
								this.mItemNames[i].text = Tools.GetRewardTypeName((ERewardType)info.Day7RewardType[i], info.Day7RewardValue1[i]);
							}
							else
							{
								FashionInfo info2 = Globals.Instance.AttDB.FashionDict.GetInfo(info.Day7RewardValue1[i]);
								if (info2 != null)
								{
									this.mItemNames[i].text = info2.Name;
									this.mItemNames[i].color = Tools.GetItemQualityColor(info2.Quality);
								}
							}
						}
						else
						{
							PetInfo info3 = Globals.Instance.AttDB.PetDict.GetInfo(info.Day7RewardValue1[i]);
							if (info3 != null)
							{
								this.mItemNames[i].text = Tools.GetPetName(info3);
								this.mItemNames[i].color = Tools.GetItemQualityColor(info3.Quality);
							}
						}
					}
					else
					{
						ItemInfo info4 = Globals.Instance.AttDB.ItemDict.GetInfo(info.Day7RewardValue1[i]);
						if (info4 != null)
						{
							this.mItemNames[i].text = info4.Name;
							this.mItemNames[i].color = Tools.GetItemQualityColor(info4.Quality);
						}
					}
					num++;
				}
				if (num >= this.ItemParents.Length)
				{
					break;
				}
			}
		}
	}

	public void OnMsgTakeFirstPayReward(MemoryStream stream)
	{
		MS2C_TakeFirstPayReward mS2C_TakeFirstPayReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeFirstPayReward), stream) as MS2C_TakeFirstPayReward;
		if (mS2C_TakeFirstPayReward.Result == 0)
		{
			base.StartCoroutine(this.DoReward());
			GUIMainMenuScene session = GameUIManager.mInstance.GetSession<GUIMainMenuScene>();
			if (session != null)
			{
				session.RefreshFirstPayBtn();
			}
			GameUIVip session2 = GameUIManager.mInstance.GetSession<GameUIVip>();
			if (session2 != null)
			{
				session2.Refresh();
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator DoReward()
	{
        return null;
        //GUIFirstRecharge.<DoReward>c__Iterator69 <DoReward>c__Iterator = new GUIFirstRecharge.<DoReward>c__Iterator69();
        //<DoReward>c__Iterator.<>f__this = this;
        //return <DoReward>c__Iterator;
	}
}
