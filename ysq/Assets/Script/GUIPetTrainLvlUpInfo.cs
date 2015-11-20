using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GUIPetTrainLvlUpInfo : MonoBehaviour
{
	public enum EUITabBtns
	{
		ETB_UIYaoShui,
		ETB_UITunShi,
		ETB_UIMax
	}

	private GUIPetTrainSceneV2 mBaseScene;

	private GameObject[] mTab0s = new GameObject[2];

	private GameObject[] mTab1s = new GameObject[2];

	private UILabel mLvlNum;

	private UILabel mHpNum;

	private UILabel mAttackNum;

	private UILabel mWufangNum;

	private UILabel mFafangNum;

	public GUIPetTrainYaoShuiLayer mGUIPetTrainYaoShuiLayer;

	private GUIPetTrainTunShiLayer mGUIPetTrainTunShiLayer;

	private GUISimpleSM<string, string> mGUISimpleSM;

	private StringBuilder mSb = new StringBuilder();

	public void InitWithBaseScene(GUIPetTrainSceneV2 baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mLvlNum = base.transform.Find("lvl").GetComponent<UILabel>();
		this.mHpNum = base.transform.Find("hpBg/num").GetComponent<UILabel>();
		this.mAttackNum = base.transform.Find("attackBg/num").GetComponent<UILabel>();
		this.mWufangNum = base.transform.Find("wufangBg/num").GetComponent<UILabel>();
		this.mFafangNum = base.transform.Find("fafangBg/num").GetComponent<UILabel>();
		for (int i = 0; i < 2; i++)
		{
			this.mTab0s[i] = base.transform.Find(string.Format("tab{0}", i)).gameObject;
			UIEventListener expr_C3 = UIEventListener.Get(this.mTab0s[i]);
			expr_C3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_C3.onClick, new UIEventListener.VoidDelegate(this.OnTab0sClick));
			this.mTab1s[i] = base.transform.Find(string.Format("tabF{0}", i)).gameObject;
		}
		this.mGUIPetTrainYaoShuiLayer = base.transform.Find("yaoshui").gameObject.AddComponent<GUIPetTrainYaoShuiLayer>();
		this.mGUIPetTrainYaoShuiLayer.InitWithBaseScene(this.mBaseScene, GameConst.PET_EXP_ITEM_ID.Length);
		this.mGUIPetTrainTunShiLayer = base.transform.Find("tunshi").gameObject.AddComponent<GUIPetTrainTunShiLayer>();
		this.mGUIPetTrainTunShiLayer.InitWithBaseScene(this.mBaseScene);
		this.mGUISimpleSM = new GUISimpleSM<string, string>("init");
		this.mGUISimpleSM.Configure("init").Permit("onYaoShui", "yaoShui").Permit("onTunShi", "tunShi");
		this.mGUISimpleSM.Configure("yaoShui").Permit("onTunShi", "tunShi").Ignore("onYaoShui").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterYaoShui();
		});
		this.mGUISimpleSM.Configure("tunShi").Permit("onYaoShui", "yaoShui").Ignore("onTunShi").OnEntry(delegate(GUISimpleSM<string, string>.Transition t)
		{
			this.OnEnterTunShi();
		});
	}

	private void SetTabStates(int index)
	{
		for (int i = 0; i < 2; i++)
		{
			this.mTab0s[i].SetActive(i != index);
			this.mTab1s[i].SetActive(i == index);
		}
	}

	public void SelectCurItem(int index)
	{
		if (index == 1)
		{
			this.mGUISimpleSM.Fire("onTunShi");
		}
		else
		{
			this.mGUISimpleSM.Fire("onYaoShui");
		}
	}

	public int GetCurPageIndex()
	{
		if (this.mGUISimpleSM.State == "tunShi")
		{
			return 1;
		}
		return 0;
	}

	private void OnTab0sClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		if (this.mGUIPetTrainYaoShuiLayer.IsAnimationing || this.mGUIPetTrainTunShiLayer.IsAnimationing)
		{
			return;
		}
		if (go == this.mTab0s[0])
		{
			this.SelectCurItem(0);
		}
		else
		{
			this.SelectCurItem(1);
		}
	}

	private void OnEnterYaoShui()
	{
		this.SetTabStates(0);
		this.mGUIPetTrainYaoShuiLayer.gameObject.SetActive(true);
		this.mGUIPetTrainTunShiLayer.gameObject.SetActive(false);
	}

	private void OnEnterTunShi()
	{
		this.SetTabStates(1);
		this.mGUIPetTrainYaoShuiLayer.gameObject.SetActive(false);
		this.mGUIPetTrainTunShiLayer.gameObject.SetActive(true);
	}

	public void Refresh()
	{
		PetDataEx curPetDataEx = this.mBaseScene.CurPetDataEx;
		if (curPetDataEx != null)
		{
			this.mLvlNum.text = this.mSb.Remove(0, this.mSb.Length).Append("Lv").Append(curPetDataEx.Data.Level).ToString();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			curPetDataEx.GetAttribute(ref num, ref num2, ref num3, ref num4);
			this.mHpNum.text = num.ToString();
			this.mAttackNum.text = num2.ToString();
			this.mWufangNum.text = num3.ToString();
			this.mFafangNum.text = num4.ToString();
			this.mGUIPetTrainYaoShuiLayer.Refresh();
			this.mGUIPetTrainTunShiLayer.Refresh();
		}
	}

	public void SetTuiShiItems(List<PetDataEx> petDatas)
	{
		this.mGUIPetTrainTunShiLayer.SetTuiShiItems(petDatas);
	}

	public void PlayExpBarEffect()
	{
		if (this.mGUIPetTrainYaoShuiLayer.gameObject.activeInHierarchy)
		{
			this.mGUIPetTrainYaoShuiLayer.PlayExpBarEffect();
		}
		else
		{
			this.mGUIPetTrainTunShiLayer.PlayExpBarEffect();
		}
	}

	public void PlayLvlUpEffectAnimation()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_017");
		if (this.mGUIPetTrainYaoShuiLayer.gameObject.activeInHierarchy)
		{
			this.mGUIPetTrainYaoShuiLayer.PlayLvlUpEffectAnimation();
		}
		else
		{
			this.mGUIPetTrainTunShiLayer.PlayLvlUpEffectAnimation();
		}
	}
}
