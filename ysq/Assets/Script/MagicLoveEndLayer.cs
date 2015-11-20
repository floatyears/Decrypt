using Att;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class MagicLoveEndLayer : MonoBehaviour
{
	private GUIMagicLoveScene mBaseScene;

	private GameObject mSelfBG;

	private GameObject mTargetBG;

	private UILabel mSelfName;

	private UILabel mTargetName;

	private GameObject mSelfSlot;

	private GameObject mTargetSlot;

	private GameObject mSelfModel;

	private GameObject mTargetModel;

	private UISprite[] mSelfs = new UISprite[3];

	private UISprite[] mTargets = new UISprite[3];

	private Animator mSelfAnim;

	private Animator mTargetAnim;

	private UILabel mTipsValue;

	private UISprite mVS;

	private GameObject mUI101;

	private GameObject mPass;

	private GameObject mCancel;

	private UILabel mPassCost;

	private ResourceEntity asyncEntitySelf;

	private ResourceEntity asyncEntityTarget;

	private PetInfo tempPetInfo;

	public int tempRewardLoveValue
	{
		get;
		private set;
	}

	public void Init(GUIMagicLoveScene basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mSelfBG = GameUITools.FindGameObject("SelfBG", base.gameObject);
		this.mTargetBG = GameUITools.FindGameObject("TargetBG", base.gameObject);
		this.mSelfName = GameUITools.FindUILabel("Name", this.mSelfBG.gameObject);
		this.mTargetName = GameUITools.FindUILabel("Name", this.mTargetBG.gameObject);
		this.mSelfSlot = GameUITools.FindGameObject("Slot", this.mSelfBG);
		this.mTargetSlot = GameUITools.FindGameObject("Slot", this.mTargetBG);
		for (int i = 0; i < this.mSelfs.Length; i++)
		{
			this.mSelfs[i] = GameUITools.FindUISprite(string.Format("Self{0}", i), this.mSelfBG.gameObject);
			this.mTargets[i] = GameUITools.FindUISprite(string.Format("Target{0}", i), this.mTargetBG.gameObject);
		}
		this.mSelfAnim = GameUITools.FindGameObject("Anim", this.mSelfBG.gameObject).GetComponent<Animator>();
		this.mTargetAnim = GameUITools.FindGameObject("Anim/Anim", this.mTargetBG.gameObject).GetComponent<Animator>();
		this.mSelfAnim.gameObject.SetActive(false);
		this.mTargetAnim.gameObject.SetActive(false);
		this.mTipsValue = GameUITools.FindUILabel("Tips/Value", base.gameObject);
		this.mVS = GameUITools.FindUISprite("VS", base.gameObject);
		this.mUI101 = GameUITools.FindGameObject("ui101", this.mVS.gameObject);
		Tools.SetParticleRenderQueue2(this.mUI101, 3003);
		this.mUI101.SetActive(false);
		this.mPass = GameUITools.RegisterClickEvent("Pass", new UIEventListener.VoidDelegate(this.OnPassClick), base.gameObject);
		this.mCancel = GameUITools.RegisterClickEvent("Cancel", new UIEventListener.VoidDelegate(this.OnCancelClick), base.gameObject);
		this.mPassCost = GameUITools.FindUILabel("Cost", this.mPass);
		this.mSelfName.text = Globals.Instance.Player.Data.Name;
		this.asyncEntitySelf = ActorManager.CreateLocalUIActor(0, 0, false, false, this.mSelfSlot, 1f, delegate(GameObject go)
		{
			this.asyncEntitySelf = null;
			this.mSelfModel = go;
			if (go == null)
			{
				global::Debug.Log(new object[]
				{
					"CreateLocalUIActor error"
				});
			}
			else
			{
				Tools.SetMeshRenderQueue(go, 3003);
			}
		});
	}

	public void Refresh()
	{
		int num;
		if (Globals.Instance.Player.MagicLoveSystem.LastResult == MagicLoveSubSystem.ELastResult.ELR_Win)
		{
			if (this.mBaseScene.mData.LoveValue[this.mBaseScene.CurIndex] >= Globals.Instance.Player.MagicLoveSystem.MaxLoveValue)
			{
				num = this.mBaseScene.mStartLayer.TempBout;
			}
			else
			{
				num = this.mBaseScene.mData.Bout;
			}
		}
		else
		{
			num = this.mBaseScene.mData.Bout + 1;
		}
		num = Mathf.Clamp(num, 1, GameConst.GetInt32(225));
		MagicLoveInfo info = Globals.Instance.AttDB.MagicLoveDict.GetInfo(num);
		if (info == null)
		{
			global::Debug.LogErrorFormat("MagicLoveDict get info error , ID : {0} ", new object[]
			{
				num
			});
			return;
		}
		this.mTipsValue.text = Singleton<StringManager>.Instance.GetString("MagicLove7", new object[]
		{
			info.RewardLoveValue
		});
		this.tempRewardLoveValue = info.RewardLoveValue;
		if (this.tempPetInfo != this.mBaseScene.CurPetItem.petInfo)
		{
			this.ClearTargetModel();
			this.tempPetInfo = this.mBaseScene.CurPetItem.petInfo;
			this.mTargetName.text = Tools.GetPetName(this.mBaseScene.CurPetItem.petInfo);
			this.asyncEntityTarget = ActorManager.CreateUIPet(this.mBaseScene.CurPetItem.petInfo, 0, false, false, this.mTargetSlot, 1f, 0, delegate(GameObject go)
			{
				this.asyncEntityTarget = null;
				this.mTargetModel = go;
				if (go == null)
				{
					global::Debug.Log(new object[]
					{
						"CreateUIPlayer error"
					});
				}
				else
				{
					Tools.SetMeshRenderQueue(go, 3003);
				}
			});
		}
		for (int i = 0; i < this.mSelfs.Length; i++)
		{
			if (i == 0)
			{
				this.mSelfs[i].enabled = true;
			}
			else
			{
				this.mSelfs[i].enabled = false;
			}
			if (i == 0)
			{
				this.mTargets[i].enabled = true;
			}
			else
			{
				this.mTargets[i].enabled = false;
			}
		}
		this.mPass.SetActive(false);
		this.mCancel.SetActive(false);
		base.StartCoroutine(this.PlayAnim(info.RewardLoveValue));
	}

	[DebuggerHidden]
	private IEnumerator PlayAnim(int value)
	{
        return null;
        //MagicLoveEndLayer.<PlayAnim>c__Iterator7D <PlayAnim>c__Iterator7D = new MagicLoveEndLayer.<PlayAnim>c__Iterator7D();
        //<PlayAnim>c__Iterator7D.value = value;
        //<PlayAnim>c__Iterator7D.<$>value = value;
        //<PlayAnim>c__Iterator7D.<>f__this = this;
        //return <PlayAnim>c__Iterator7D;
	}

	public void ClearAllModel()
	{
		if (this.asyncEntitySelf != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntitySelf);
			this.asyncEntitySelf = null;
		}
		if (this.mSelfModel != null)
		{
			UnityEngine.Object.Destroy(this.mSelfModel);
			this.mSelfModel = null;
		}
		this.ClearTargetModel();
	}

	private void ClearTargetModel()
	{
		if (this.asyncEntityTarget != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntityTarget);
			this.asyncEntityTarget = null;
		}
		if (this.mTargetModel != null)
		{
			UnityEngine.Object.Destroy(this.mTargetModel);
			this.mTargetModel = null;
		}
	}

	private void OnPassClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseScene.SendMagicCamouflage(false);
	}

	private void OnCancelClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		this.mBaseScene.SendGo();
	}
}
