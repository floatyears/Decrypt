using Att;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using System;
using UnityEngine;

public class GUIPetTrainTunShiItem : MonoBehaviour
{
	private GUIPetTrainTunShiLayer mBaseLayer;

	private GUIPetTrainSceneV2 mBaseScene;

	private UISprite mIcon;

	private UISprite mQualityMask;

	private GameObject mMinBtn;

	private GameObject mPlusGo;

	private int mIndex;

	private Transform mTmpTransform;

	private GameObject mEffect0;

	private GameObject mEffect1;

	private Vector3[] mEffect1Path = new Vector3[3];

	private PetDataEx mPetDataEx;

	public PetDataEx CurPetDataEx
	{
		get
		{
			return this.mPetDataEx;
		}
		set
		{
			this.mPetDataEx = value;
			this.Refresh();
		}
	}

	public bool IsItemEmpty()
	{
		return this.mPetDataEx == null;
	}

	public uint GetCurPetExpNum()
	{
		if (this.mPetDataEx != null)
		{
			return this.mPetDataEx.GetToExp();
		}
		return 0u;
	}

	public uint GetCurPetExpMoney()
	{
		if (this.mPetDataEx != null)
		{
			QualityInfo info = Globals.Instance.AttDB.QualityDict.GetInfo(this.mPetDataEx.Info.Quality + 1);
			return (info == null) ? 0u : info.PetExp;
		}
		return 0u;
	}

	public void InitWithBaseScene(GUIPetTrainTunShiLayer baseLayer, GUIPetTrainSceneV2 baseScene, int index)
	{
		this.mBaseLayer = baseLayer;
		this.mBaseScene = baseScene;
		this.mIndex = index;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("item");
		this.mIcon = transform.Find("icon").GetComponent<UISprite>();
		UIEventListener expr_37 = UIEventListener.Get(this.mIcon.gameObject);
		expr_37.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_37.onClick, new UIEventListener.VoidDelegate(this.OnIconClick));
		this.mQualityMask = transform.Find("qualityMask").GetComponent<UISprite>();
		this.mMinBtn = transform.Find("minBtn").gameObject;
		this.mPlusGo = base.transform.Find("plus").gameObject;
		UIEventListener expr_AA = UIEventListener.Get(this.mMinBtn);
		expr_AA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_AA.onClick, new UIEventListener.VoidDelegate(this.OnMinBtnClick));
		UIEventListener expr_D6 = UIEventListener.Get(base.gameObject);
		expr_D6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D6.onClick, new UIEventListener.VoidDelegate(this.OnPlusBtnClick));
		this.mEffect0 = base.transform.Find("ui56_2").gameObject;
		Tools.SetParticleRenderQueue2(this.mEffect0, 3100);
		this.mEffect1 = base.transform.Find("ui56_1").gameObject;
		Tools.SetParticleRenderQueue2(this.mEffect1, 3100);
		this.mTmpTransform = base.transform.Find("tmp");
		this.mEffect1Path[0] = this.mIcon.transform.position;
		this.mEffect1Path[1] = this.mTmpTransform.position;
		this.mEffect1Path[2] = this.mBaseScene.GetCardModelTransform().position;
	}

	public void Refresh(PetDataEx pdEx)
	{
		this.CurPetDataEx = pdEx;
	}

	private void Refresh()
	{
		if (this.mPetDataEx != null)
		{
			this.mPlusGo.SetActive(false);
			this.mIcon.gameObject.SetActive(true);
			this.mQualityMask.gameObject.SetActive(true);
			this.mMinBtn.SetActive(true);
			this.mIcon.spriteName = this.mPetDataEx.Info.Icon;
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mPetDataEx.Info.Quality);
		}
		else
		{
			this.mPlusGo.SetActive(true);
			this.mIcon.gameObject.SetActive(false);
			this.mQualityMask.gameObject.SetActive(false);
			this.mMinBtn.SetActive(false);
		}
	}

	private void OnMinBtnClick(GameObject go)
	{
		this.mBaseLayer.ClearItem(this.mIndex);
	}

	private void OnIconClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseLayer.GoToLvlUpSelPetScene();
	}

	private void OnPlusBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseLayer.GoToLvlUpSelPetScene();
	}

	public void PlayEffect0()
	{
		NGUITools.SetActive(this.mEffect0, false);
		NGUITools.SetActive(this.mEffect0, true);
	}

	public void HideEffec0()
	{
		NGUITools.SetActive(this.mEffect0, false);
	}

	public void HideEffec1()
	{
		NGUITools.SetActive(this.mEffect1, false);
	}

	public void PlayEffect1()
	{
		this.mEffect1.transform.localPosition = Vector3.zero;
		NGUITools.SetActive(this.mEffect1, false);
		NGUITools.SetActive(this.mEffect1, true);
		HOTween.To(this.mEffect1.transform, 0.3f, new TweenParms().Prop("position", new PlugVector3Path(this.mEffect1Path, PathType.Curved)));
	}
}
