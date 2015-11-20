using Att;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GUIPetTrainJueXingInfo : MonoBehaviour
{
	private const int mNeedItemNum = 4;

	private const int mCostItemNum = 2;

	private GUIPetTrainSceneV2 mBaseScene;

	private UILabel mJueXingLvl;

	private GUIPetTrainJueXingNeedItem[] mNeedItems = new GUIPetTrainJueXingNeedItem[4];

	private UILabel mTitle;

	private UILabel mDesc;

	private GUIPetTrainJueXingCostItem[] mCostItems = new GUIPetTrainJueXingCostItem[2];

	private UILabel mCostNum;

	private GameObject mJueXingBtnGo;

	private UIButton mJueXingBtn;

	private UILabel mJueXingTipLb;

	private GameObject mEffect74;

	private GameObject mEffect56_1;

	private Vector3[] mEffect56Path = new Vector3[4];

	private uint mOldAwake;

	private uint mNextAwake;

	private StringBuilder mSb = new StringBuilder(42);

	private Sequence mSequenceForStarUp;

	public void InitWithBaseScene(GUIPetTrainSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mJueXingLvl = base.transform.Find("num0").GetComponent<UILabel>();
		Transform transform = base.transform.Find("infoBg");
		for (int i = 0; i < 4; i++)
		{
			this.mNeedItems[i] = transform.Find(string.Format("item{0}", i)).gameObject.AddComponent<GUIPetTrainJueXingNeedItem>();
			this.mNeedItems[i].InitWithBaseScene(this.mBaseScene, i);
		}
		this.mEffect74 = transform.Find("ui74").gameObject;
		Tools.SetParticleRenderQueue2(this.mEffect74, 4000);
		NGUITools.SetActive(this.mEffect74, false);
		this.mEffect56_1 = transform.Find("ui56_1").gameObject;
		Tools.SetParticleRenderQueue2(this.mEffect56_1, 4000);
		NGUITools.SetActive(this.mEffect56_1, false);
		this.mTitle = base.transform.Find("title").GetComponent<UILabel>();
		this.mTitle.text = string.Empty;
		this.mDesc = base.transform.Find("desc").GetComponent<UILabel>();
		this.mDesc.text = string.Empty;
		Transform transform2 = base.transform.Find("infoBg2");
		for (int j = 0; j < 2; j++)
		{
			this.mCostItems[j] = transform2.Find(string.Format("item{0}", j)).gameObject.AddComponent<GUIPetTrainJueXingCostItem>();
			this.mCostItems[j].InitWithBaseScene();
		}
		this.mCostNum = base.transform.Find("txt1/costNum").GetComponent<UILabel>();
		this.mCostNum.text = string.Empty;
		this.mJueXingBtnGo = base.transform.Find("jueXingBtn").gameObject;
		UIEventListener expr_1DD = UIEventListener.Get(this.mJueXingBtnGo);
		expr_1DD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1DD.onClick, new UIEventListener.VoidDelegate(this.OnJueXingBtnClick));
		this.mJueXingBtn = this.mJueXingBtnGo.GetComponent<UIButton>();
		this.mJueXingTipLb = this.mJueXingBtn.transform.Find("Label").GetComponent<UILabel>();
	}

	public void Refresh()
	{
		uint num = 0u;
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			this.mNextAwake = curPetDataEx.Data.Awake + 1u;
			uint petStarAndLvl = Tools.GetPetStarAndLvl(curPetDataEx.Data.Awake, out num);
			this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("petJueXing0", new object[]
			{
				petStarAndLvl
			}));
			if (petStarAndLvl < 5u)
			{
				this.mSb.Append(Singleton<StringManager>.Instance.GetString("petJueXing1", new object[]
				{
					num
				}));
			}
			this.mJueXingLvl.text = this.mSb.ToString();
			for (int i = 0; i < 4; i++)
			{
				this.mNeedItems[i].Refresh();
			}
			if (petStarAndLvl < 5u)
			{
				this.mTitle.text = Singleton<StringManager>.Instance.GetString("petJueXing2", new object[]
				{
					petStarAndLvl + 1u
				});
				AwakeInfo info = Globals.Instance.AttDB.AwakeDict.GetInfo((int)((petStarAndLvl + 1u) * 10u));
				if (info != null)
				{
					this.mDesc.text = Singleton<StringManager>.Instance.GetString("petJueXing3", new object[]
					{
						info.AttPct / 100
					});
				}
				else
				{
					this.mDesc.text = string.Empty;
				}
			}
			else
			{
				this.mTitle.text = string.Empty;
				this.mDesc.text = string.Empty;
			}
			int num2 = 0;
			int num3 = 0;
			int needItemCount = 0;
			int curItemCount = 0;
			curPetDataEx.GetAwakeLevelupData(out num2, out num3, out needItemCount, out curItemCount);
			ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(118));
			this.mCostItems[0].Refresh(info2, curItemCount, needItemCount);
			if (num3 == 0)
			{
				this.mCostItems[1].gameObject.SetActive(false);
				this.mCostItems[1].Refresh(curPetDataEx.Info, 0, num3);
			}
			else
			{
				this.mCostItems[1].gameObject.SetActive(true);
				int furtherPetCount = Globals.Instance.Player.PetSystem.GetFurtherPetCount(curPetDataEx.Data.ID, curPetDataEx.Data.InfoID);
				this.mCostItems[1].Refresh(curPetDataEx.Info, furtherPetCount, num3);
			}
			this.mCostNum.text = num2.ToString();
			int money = Globals.Instance.Player.Data.Money;
			if (money < num2)
			{
				this.mCostNum.color = Color.red;
			}
			else
			{
				this.mCostNum.color = Color.white;
			}
			uint jueXingNeedLvl = curPetDataEx.GetJueXingNeedLvl();
			if (curPetDataEx.Data.Level < jueXingNeedLvl)
			{
				this.mJueXingBtn.isEnabled = false;
				Tools.SetButtonState(this.mJueXingBtnGo, false);
				this.mJueXingTipLb.text = Singleton<StringManager>.Instance.GetString("needLvl", new object[]
				{
					jueXingNeedLvl
				});
			}
			else
			{
				this.mJueXingTipLb.text = Singleton<StringManager>.Instance.GetString("petJueXing6");
				if (curPetDataEx.Data.Awake < 50u && money >= num2 && this.mCostItems[0].IsEnough && this.mCostItems[1].IsEnough && this.mNeedItems[0].IsItemEquiped && this.mNeedItems[1].IsItemEquiped && this.mNeedItems[2].IsItemEquiped && this.mNeedItems[3].IsItemEquiped)
				{
					this.mJueXingBtn.isEnabled = true;
					Tools.SetButtonState(this.mJueXingBtnGo, true);
				}
				else
				{
					this.mJueXingBtn.isEnabled = false;
					Tools.SetButtonState(this.mJueXingBtnGo, false);
				}
			}
		}
		this.mBaseScene.RefreshJueXingNewMark();
	}

	private void OnJueXingBtnClick(GameObject go)
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null && curPetDataEx.Data.Awake < this.mNextAwake)
		{
			this.mOldAwake = curPetDataEx.Data.Awake;
			this.mBaseScene.SetOldAttrNum();
			MC2S_AwakeLevelup mC2S_AwakeLevelup = new MC2S_AwakeLevelup();
			mC2S_AwakeLevelup.PetID = ((curPetDataEx.GetSocketSlot() != 0) ? curPetDataEx.Data.ID : 100uL);
			Globals.Instance.CliSession.Send(419, mC2S_AwakeLevelup);
		}
	}

	private void HideNeedItemEffects()
	{
		for (int i = 0; i < 4; i++)
		{
			this.mNeedItems[i].HideEffects();
		}
	}

	public void PlayEquipEffect(int slot)
	{
		if (0 <= slot && slot < 4)
		{
			this.mNeedItems[slot].PlayEquipEffect();
		}
	}

	public void DestroySequenceForLevelup()
	{
		if (this.mSequenceForStarUp != null)
		{
			this.mSequenceForStarUp.Kill();
			this.mSequenceForStarUp = null;
		}
	}

	private void PlayEffect56(int starIndex)
	{
		Transform transform = base.transform.Find("infoBg");
		this.mEffect56Path[0] = transform.Find("tmp").position;
		this.mEffect56Path[1] = transform.Find("tmp1").position;
		this.mEffect56Path[2] = transform.Find("tmp2").position;
		this.mEffect56Path[3] = this.mBaseScene.GetStarPosition(starIndex);
		this.mEffect56_1.transform.localPosition = Vector3.zero;
		NGUITools.SetActive(this.mEffect56_1, false);
		NGUITools.SetActive(this.mEffect56_1, true);
		HOTween.To(this.mEffect56_1.transform, 0.4f, new TweenParms().Prop("position", new PlugVector3Path(this.mEffect56Path, PathType.Curved)).Ease(EaseType.EaseInSine));
	}

	public void PlayAwakeLvlUpEffect()
	{
		uint num = 0u;
		uint newStar = 0u;
		uint num2 = 0u;
		uint petStarAndLvl = Tools.GetPetStarAndLvl(this.mOldAwake, out num);
		newStar = Tools.GetPetStarAndLvl(this.mBaseScene.CurPetDataEx.Data.Awake, out num2);
		if (petStarAndLvl == newStar)
		{
			this.DestroySequenceForLevelup();
			this.mSequenceForStarUp = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnAnimEnd)));
            this.mSequenceForStarUp.AppendCallback(() =>
			{
				NGUITools.SetActive(this.mEffect74, false);
				NGUITools.SetActive(this.mEffect74, true);
			});
			this.mSequenceForStarUp.AppendInterval(0.3f);
		}
		else
		{
			this.DestroySequenceForLevelup();
			this.mSequenceForStarUp = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.OnAnimEnd)));
            this.mSequenceForStarUp.AppendCallback(() =>
			{
				NGUITools.SetActive(this.mEffect74, false);
				NGUITools.SetActive(this.mEffect74, true);
			});
			this.mSequenceForStarUp.AppendInterval(0.4f);
            this.mSequenceForStarUp.AppendCallback(() =>
			{
				this.PlayEffect56((int)(newStar - 1u));
			});
			this.mSequenceForStarUp.AppendInterval(0.5f);
            this.mSequenceForStarUp.AppendCallback(() =>
			{
				this.mBaseScene.PlayStarEffect((int)(newStar - 1u));
			});
			this.mSequenceForStarUp.AppendInterval(0.1f);
            this.mSequenceForStarUp.AppendCallback(() =>
			{
				this.mBaseScene.ModelEffect75();
			});
		}
		this.mSequenceForStarUp.Play();
	}

	private void OnAnimEnd()
	{
		this.mBaseScene.PlayAwakeLvlUpMsgTip(this.mOldAwake);
	}

	public void HideEffects()
	{
		this.HideNeedItemEffects();
		NGUITools.SetActive(this.mEffect74, false);
		NGUITools.SetActive(this.mEffect56_1, false);
	}
}
