using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIPetTrainJinjieInfo : MonoBehaviour
{
	private const int mItemNum = 2;

	private GUIPetTrainSceneV2 mBaseScene;

	private UILabel mLvlOld;

	private UILabel mLvlNew;

	private UILabel mHpNumOld;

	private UILabel mAttackNumOld;

	private UILabel mWufangNumOld;

	private UILabel mFafangNumOld;

	private UILabel mHpNumNew;

	private UILabel mAttackNumNew;

	private UILabel mWufangNumNew;

	private UILabel mFafangNumNew;

	private UILabel mCostNum;

	private UILabel mJinJieBtnLb;

	private UILabel mUnlockTipTxt;

	private UIButton mJinJieBtn;

	private GameObject mArrowGo;

	private GameObject mCostGo;

	private GameObject mTipTxt;

	private GUIPetTrainJinjieItem[] mNeedItems = new GUIPetTrainJinjieItem[2];

	private StringBuilder mSb = new StringBuilder(42);

	private bool mIsForceJinjie;

	public bool IsForceJinjie
	{
		get
		{
			return this.mIsForceJinjie;
		}
		set
		{
			this.mIsForceJinjie = value;
		}
	}

	public void InitWithBaseScene(GUIPetTrainSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mLvlOld = base.transform.Find("num0").GetComponent<UILabel>();
		this.mArrowGo = base.transform.Find("arrow").gameObject;
		this.mLvlNew = base.transform.Find("num1").GetComponent<UILabel>();
		Transform transform = base.transform.Find("hpBg");
		this.mHpNumOld = transform.Find("num").GetComponent<UILabel>();
		this.mHpNumNew = transform.Find("num2").GetComponent<UILabel>();
		Transform transform2 = base.transform.Find("attackBg");
		this.mAttackNumOld = transform2.Find("num").GetComponent<UILabel>();
		this.mAttackNumNew = transform2.Find("num2").GetComponent<UILabel>();
		Transform transform3 = base.transform.Find("wufangBg");
		this.mWufangNumOld = transform3.Find("num").GetComponent<UILabel>();
		this.mWufangNumNew = transform3.Find("num2").GetComponent<UILabel>();
		Transform transform4 = base.transform.Find("fafangBg");
		this.mFafangNumOld = transform4.Find("num").GetComponent<UILabel>();
		this.mFafangNumNew = transform4.Find("num2").GetComponent<UILabel>();
		this.mCostGo = base.transform.Find("txt1").gameObject;
		this.mCostNum = this.mCostGo.transform.Find("costNum").GetComponent<UILabel>();
		this.mTipTxt = base.transform.Find("tipTxt").gameObject;
		UILabel component = this.mTipTxt.GetComponent<UILabel>();
		if (component != null)
		{
			component.width = 350;
			component.spacingX = 0;
		}
		Transform transform5 = base.transform.Find("infoBg");
		for (int i = 0; i < 2; i++)
		{
			this.mNeedItems[i] = transform5.Find(string.Format("item{0}", i)).gameObject.AddComponent<GUIPetTrainJinjieItem>();
			this.mNeedItems[i].InitWithBaseScene();
		}
		this.mJinJieBtn = base.transform.Find("jinJieBtn").GetComponent<UIButton>();
		this.mJinJieBtnLb = this.mJinJieBtn.transform.Find("Label").GetComponent<UILabel>();
		UIEventListener expr_275 = UIEventListener.Get(this.mJinJieBtn.gameObject);
		expr_275.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_275.onClick, new UIEventListener.VoidDelegate(this.OnJinjieBtnClick));
		this.mUnlockTipTxt = base.transform.Find("unlockTipTxt").GetComponent<UILabel>();
	}

	public void OnJinjieBtnClick(GameObject go)
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			if (!this.mIsForceJinjie && (ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(12)))
			{
				Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
				GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("WBTip1", new object[]
				{
					GameConst.GetInt32(12)
				}), 0f, 0f);
				return;
			}
			GameUIManager.mInstance.uiState.SetOldFurtherData(curPetDataEx);
			MC2S_PetFurther mC2S_PetFurther = new MC2S_PetFurther();
			mC2S_PetFurther.PetID = ((curPetDataEx.GetSocketSlot() != 0) ? curPetDataEx.Data.ID : 100uL);
			Globals.Instance.CliSession.Send(404, mC2S_PetFurther);
		}
	}

	public void Refresh()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			curPetDataEx.GetAttribute(ref num, ref num2, ref num3, ref num4);
			int maxFurther = curPetDataEx.GetMaxFurther(true);
			int maxFurther2 = curPetDataEx.GetMaxFurther(false);
			int further = (int)curPetDataEx.Data.Further;
			if (further < maxFurther)
			{
				this.mArrowGo.SetActive(true);
				this.mLvlNew.text = (further + 1).ToString();
				this.mLvlNew.color = Color.green;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				int num8 = 0;
				int num9 = 0;
				int num10 = 0;
				int num11 = 0;
				int num12 = 0;
				curPetDataEx.GetBastAtt(ref num5, ref num6, ref num7, ref num8);
				curPetDataEx.Data.Further += 1u;
				curPetDataEx.GetBastAtt(ref num9, ref num10, ref num11, ref num12);
				curPetDataEx.Data.Further -= 1u;
				this.mHpNumNew.text = string.Format("{0}", Mathf.RoundToInt((float)(num9 - num5)) + num);
				this.mAttackNumNew.text = string.Format("{0}", Mathf.RoundToInt((float)(num10 - num6)) + num2);
				this.mWufangNumNew.text = string.Format("{0}", Mathf.RoundToInt((float)(num11 - num7)) + num3);
				this.mFafangNumNew.text = string.Format("{0}", Mathf.RoundToInt((float)(num12 - num8)) + num4);
				if (0 <= further && further < curPetDataEx.Info.TalentID.Count)
				{
					int num13 = 0;
					if (further > 9)
					{
						num13 = (further + 1) * 10;
					}
					if (num13 > GameConst.GetInt32(226))
					{
						this.mUnlockTipTxt.gameObject.SetActive(false);
					}
					else
					{
						TalentInfo info = Globals.Instance.AttDB.TalentDict.GetInfo(curPetDataEx.Info.TalentID[further]);
						if (info != null)
						{
							this.mUnlockTipTxt.gameObject.SetActive(true);
							this.mSb.Remove(0, this.mSb.Length).Append("[").Append(info.Name).Append("]").Append("(").Append(Singleton<StringManager>.Instance.GetString("teamManage1", new object[]
							{
								further + 1
							})).Append(")").Append(info.Desc);
							this.mUnlockTipTxt.text = this.mSb.ToString();
						}
						else
						{
							this.mUnlockTipTxt.gameObject.SetActive(false);
						}
					}
				}
				else
				{
					this.mUnlockTipTxt.gameObject.SetActive(false);
				}
				for (int i = 0; i < this.mNeedItems.Length; i++)
				{
					if (this.mNeedItems[i] != null)
					{
						this.mNeedItems[i].gameObject.SetActive(true);
					}
				}
				if (further < maxFurther2)
				{
					this.mCostGo.gameObject.SetActive(true);
					this.mTipTxt.SetActive(false);
				}
				else
				{
					this.mCostGo.gameObject.SetActive(false);
					this.mTipTxt.SetActive(true);
				}
				this.mJinJieBtn.gameObject.SetActive(true);
			}
			else
			{
				this.mLvlNew.text = Singleton<StringManager>.Instance.GetString("equipImprove26");
				this.mLvlNew.color = Color.red;
				this.mArrowGo.SetActive(false);
				this.mHpNumNew.gameObject.SetActive(false);
				this.mAttackNumNew.gameObject.SetActive(false);
				this.mWufangNumNew.gameObject.SetActive(false);
				this.mFafangNumNew.gameObject.SetActive(false);
				this.mUnlockTipTxt.gameObject.SetActive(false);
				for (int j = 0; j < this.mNeedItems.Length; j++)
				{
					if (this.mNeedItems[j] != null)
					{
						this.mNeedItems[j].gameObject.SetActive(false);
					}
				}
				this.mCostGo.gameObject.SetActive(false);
				this.mTipTxt.SetActive(false);
				this.mJinJieBtn.gameObject.SetActive(false);
			}
			this.mLvlOld.text = further.ToString();
			this.mHpNumOld.text = num.ToString();
			this.mAttackNumOld.text = num2.ToString();
			this.mWufangNumOld.text = num3.ToString();
			this.mFafangNumOld.text = num4.ToString();
			int curItemCount;
			int needItemCount;
			int num14;
			int curPetCount;
			int num15;
			curPetDataEx.GetFurtherData(out curItemCount, out needItemCount, out num14, out curPetCount, out num15);
			this.mCostNum.text = num14.ToString();
			int money = Globals.Instance.Player.Data.Money;
			if (money < num14)
			{
				this.mCostNum.color = Color.red;
			}
			else
			{
				this.mCostNum.color = Color.white;
			}
			ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(100));
			this.mNeedItems[0].Refresh(info2, curItemCount, needItemCount);
			if (num15 == 0)
			{
				this.mNeedItems[1].gameObject.SetActive(false);
				this.mNeedItems[1].Refresh(curPetDataEx.Info, curPetCount, num15);
			}
			else
			{
				this.mNeedItems[1].gameObject.SetActive(true);
				this.mNeedItems[1].Refresh(curPetDataEx.Info, curPetCount, num15);
			}
			uint furtherNeedLvl = curPetDataEx.GetFurtherNeedLvl();
			if (curPetDataEx.Data.Level < furtherNeedLvl)
			{
				this.mJinJieBtn.isEnabled = false;
				Tools.SetButtonState(this.mJinJieBtn.gameObject, false);
				this.mJinJieBtnLb.text = Singleton<StringManager>.Instance.GetString("needLvl", new object[]
				{
					furtherNeedLvl
				});
				this.mBaseScene.ShowPetJinJieNewMark = false;
			}
			else
			{
				if (further < maxFurther && money >= num14 && this.mNeedItems[0].IsEnough && this.mNeedItems[1].IsEnough)
				{
					this.mBaseScene.ShowPetJinJieNewMark = ((ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(12)));
					this.mJinJieBtn.isEnabled = true;
					Tools.SetButtonState(this.mJinJieBtn.gameObject, true);
				}
				else
				{
					this.mBaseScene.ShowPetJinJieNewMark = false;
					this.mJinJieBtn.isEnabled = false;
					Tools.SetButtonState(this.mJinJieBtn.gameObject, false);
				}
				this.mJinJieBtnLb.text = Singleton<StringManager>.Instance.GetString("jinJie");
			}
		}
	}
}
