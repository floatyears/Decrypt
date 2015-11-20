using System;
using System.Text;
using UnityEngine;

public class GUILopetTrainLvlUpInfo : MonoBehaviour
{
	private GUIPetTrainSceneV2 mBaseScene;

	private UILabel mLvlNum;

	private UILabel mHpNum;

	private UILabel mAttackNum;

	private UILabel mWufangNum;

	private UILabel mFafangNum;

	public GUIPetTrainYaoShuiLayer mGUIPetTrainYaoShuiLayer;

	private StringBuilder mSb = new StringBuilder();

	public void InitWithBaseScene(GUIPetTrainSceneV2 basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mLvlNum = base.transform.Find("lvl").GetComponent<UILabel>();
		this.mHpNum = base.transform.Find("hpBg/num").GetComponent<UILabel>();
		this.mAttackNum = base.transform.Find("attackBg/num").GetComponent<UILabel>();
		this.mWufangNum = base.transform.Find("wufangBg/num").GetComponent<UILabel>();
		this.mFafangNum = base.transform.Find("fafangBg/num").GetComponent<UILabel>();
		this.mGUIPetTrainYaoShuiLayer = base.transform.Find("yaoshui").gameObject.AddComponent<GUIPetTrainYaoShuiLayer>();
		this.mGUIPetTrainYaoShuiLayer.InitWithBaseScene(this.mBaseScene, GameConst.LOPET_EXP_ITEM_ID.Length);
	}

	public void Refresh()
	{
		LopetDataEx curLopetDataEx = this.mBaseScene.CurLopetDataEx;
		if (curLopetDataEx != null)
		{
			this.mLvlNum.text = this.mSb.Remove(0, this.mSb.Length).Append("Lv").Append(curLopetDataEx.Data.Level).ToString();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			curLopetDataEx.GetAttribute(ref num, ref num2, ref num3, ref num4);
			this.mHpNum.text = num.ToString();
			this.mAttackNum.text = num2.ToString();
			this.mWufangNum.text = num3.ToString();
			this.mFafangNum.text = num4.ToString();
			this.mGUIPetTrainYaoShuiLayer.Refresh();
		}
		this.mBaseScene.RefreshLopetLvlUpNewMark();
	}

	public void PlayExpBarEffect()
	{
		if (this.mGUIPetTrainYaoShuiLayer.gameObject.activeInHierarchy)
		{
			this.mGUIPetTrainYaoShuiLayer.PlayExpBarEffect();
		}
	}

	public void PlayLvlUpEffectAnimation()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_017");
		if (this.mGUIPetTrainYaoShuiLayer.gameObject.activeInHierarchy)
		{
			this.mGUIPetTrainYaoShuiLayer.PlayLvlUpEffectAnimation();
		}
	}
}
