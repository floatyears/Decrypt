using Att;
using Proto;
using System;
using UnityEngine;

public class GUILopetTrainAwakeInfo : MonoBehaviour
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

	private UIButton mJinJieBtn;

	private GameObject mArrowGo;

	private GameObject mCostGo;

	private GameObject mTipTxt;

	private GUIPetTrainJinjieItem[] mNeedItems = new GUIPetTrainJinjieItem[2];

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
		Transform transform5 = base.transform.Find("infoBg");
		for (int i = 0; i < 2; i++)
		{
			this.mNeedItems[i] = transform5.Find(string.Format("item{0}", i)).gameObject.AddComponent<GUIPetTrainJinjieItem>();
			this.mNeedItems[i].InitWithBaseScene();
		}
		this.mJinJieBtn = base.transform.Find("jinJieBtn").GetComponent<UIButton>();
		this.mJinJieBtnLb = this.mJinJieBtn.transform.Find("Label").GetComponent<UILabel>();
		UIEventListener expr_247 = UIEventListener.Get(this.mJinJieBtn.gameObject);
		expr_247.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_247.onClick, new UIEventListener.VoidDelegate(this.OnJinjieBtnClick));
	}

	public void OnJinjieBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		LopetDataEx curLopetDataEx = this.mBaseScene.CurLopetDataEx;
		if (curLopetDataEx != null)
		{
			GameUIManager.mInstance.uiState.SetOldLopetAwakeData(curLopetDataEx);
			MC2S_LopetAwake mC2S_LopetAwake = new MC2S_LopetAwake();
			mC2S_LopetAwake.LopetID = curLopetDataEx.GetID();
			Globals.Instance.CliSession.Send(1064, mC2S_LopetAwake);
		}
	}

	public void Refresh()
	{
		LopetDataEx curLopetDataEx = this.mBaseScene.CurLopetDataEx;
		if (curLopetDataEx != null)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			curLopetDataEx.GetAttribute(ref num, ref num2, ref num3, ref num4);
			int maxAwake = curLopetDataEx.GetMaxAwake(false);
			int maxAwake2 = curLopetDataEx.GetMaxAwake(true);
			int awake = (int)curLopetDataEx.Data.Awake;
			if (awake < maxAwake2)
			{
				this.mArrowGo.SetActive(true);
				this.mLvlNew.text = (awake + 1).ToString();
				this.mLvlNew.color = Color.green;
				this.mHpNumNew.text = string.Format("{0}", Mathf.RoundToInt((float)(curLopetDataEx.Info.AwakeMaxHP[awake] - ((awake >= 1) ? curLopetDataEx.Info.AwakeMaxHP[awake - 1] : 0))) + num);
				this.mAttackNumNew.text = string.Format("{0}", Mathf.RoundToInt((float)(curLopetDataEx.Info.AwakeAttack[awake] - ((awake >= 1) ? curLopetDataEx.Info.AwakeAttack[awake - 1] : 0))) + num2);
				this.mWufangNumNew.text = string.Format("{0}", Mathf.RoundToInt((float)(curLopetDataEx.Info.AwakePhysicDefense[awake] - ((awake >= 1) ? curLopetDataEx.Info.AwakePhysicDefense[awake - 1] : 0) + num3)));
				this.mFafangNumNew.text = string.Format("{0}", Mathf.RoundToInt((float)(curLopetDataEx.Info.AwakeMagicDefense[awake] - ((awake >= 1) ? curLopetDataEx.Info.AwakeMagicDefense[awake - 1] : 0))) + num4);
				for (int i = 0; i < this.mNeedItems.Length; i++)
				{
					if (this.mNeedItems[i] != null)
					{
						this.mNeedItems[i].gameObject.SetActive(true);
					}
				}
				if (awake < maxAwake)
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
			this.mLvlOld.text = awake.ToString();
			this.mHpNumOld.text = num.ToString();
			this.mAttackNumOld.text = num2.ToString();
			this.mWufangNumOld.text = num3.ToString();
			this.mFafangNumOld.text = num4.ToString();
			int curItemCount;
			int needItemCount;
			int num5;
			int curLopetCount;
			int num6;
			curLopetDataEx.GetFurtherData(out curItemCount, out needItemCount, out num5, out curLopetCount, out num6);
			this.mCostNum.text = num5.ToString();
			int money = Globals.Instance.Player.Data.Money;
			if (money < num5)
			{
				this.mCostNum.color = Color.red;
			}
			else
			{
				this.mCostNum.color = Color.white;
			}
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(GameConst.GetInt32(205));
			this.mNeedItems[0].Refresh(info, curItemCount, needItemCount);
			if (num6 == 0)
			{
				this.mNeedItems[1].gameObject.SetActive(false);
				this.mNeedItems[1].Refresh(curLopetDataEx.Info, curLopetCount, num6);
			}
			else
			{
				this.mNeedItems[1].gameObject.SetActive(true);
				this.mNeedItems[1].Refresh(curLopetDataEx.Info, curLopetCount, num6);
			}
			uint awakeNeedLvl = curLopetDataEx.GetAwakeNeedLvl();
			if (curLopetDataEx.Data.Level < awakeNeedLvl)
			{
				this.mJinJieBtn.isEnabled = false;
				Tools.SetButtonState(this.mJinJieBtn.gameObject, false);
				this.mJinJieBtnLb.text = Singleton<StringManager>.Instance.GetString("needLvl", new object[]
				{
					awakeNeedLvl
				});
				this.mBaseScene.ShowLopetAwakeNewMark = false;
			}
			else
			{
				if (awake < maxAwake2 && money >= num5 && this.mNeedItems[0].IsEnough && this.mNeedItems[1].IsEnough)
				{
					this.mBaseScene.ShowLopetAwakeNewMark = true;
					this.mJinJieBtn.isEnabled = true;
					Tools.SetButtonState(this.mJinJieBtn.gameObject, true);
				}
				else
				{
					this.mBaseScene.ShowLopetAwakeNewMark = false;
					this.mJinJieBtn.isEnabled = false;
					Tools.SetButtonState(this.mJinJieBtn.gameObject, false);
				}
				this.mJinJieBtnLb.text = Singleton<StringManager>.Instance.GetString("jinJie");
			}
		}
	}
}
