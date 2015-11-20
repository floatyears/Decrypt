using NtUniSdk.Unity3d;
using System;
using UnityEngine;

public class CombatMainGameControllerLayer : MonoBehaviour
{
	private bool mAutoSkill;

	private bool mAutoCombat;

	private GameObject mAutoCombatFX;

	private PlayerController pcc;

	private UIButton mAutoBtn;

	private UISprite mAutoCombatSprite;

	private UILabel mAutoCombatLabel;

	private GameObject mLockGo;

	private bool mIsAutoBtnLock;

	private bool mFollowPlayer;

	private UISprite mFollowBtn;

	private GameObject mSpeedBtn;

	private UILabel mSpeedTxt;

	private int mSpeedNum;

	private GameObject mSkipCombatBtn;

	private int mCurState;

	public bool IsAutoBtnLock
	{
		get
		{
			return this.mIsAutoBtnLock;
		}
		set
		{
			this.mIsAutoBtnLock = value;
			this.mLockGo.gameObject.SetActive(value);
		}
	}

	public int SpeedCombatNum
	{
		get
		{
			return this.mSpeedNum;
		}
		set
		{
			if (value == 1)
			{
				this.SetSpeedCombatNum1();
			}
			else if (value == 2)
			{
				if (Tools.IsUnlockPveCombatSpeedupX2())
				{
					Globals.Instance.GameMgr.ClearSpeedMod();
					Globals.Instance.GameMgr.SpeedUp(0.15f, true);
					this.mSpeedTxt.text = "x2";
					this.mSpeedNum = 2;
					GameCache.SetGameSpeed(this.mSpeedNum);
				}
				else
				{
					this.SetSpeedCombatNum1();
				}
			}
			else if (value == 3)
			{
				if (Tools.IsUnlockPveCombatSpeedupX3())
				{
					Globals.Instance.GameMgr.ClearSpeedMod();
					Globals.Instance.GameMgr.SpeedUp(0.450000018f, true);
					this.mSpeedTxt.text = "x3";
					this.mSpeedNum = 3;
					GameCache.SetGameSpeed(this.mSpeedNum);
				}
				else
				{
					this.SetSpeedCombatNum1();
				}
			}
			else if (value == 4)
			{
				if (Globals.Instance.CliSession.Privilege > 0)
				{
					Globals.Instance.GameMgr.ClearSpeedMod();
					Globals.Instance.GameMgr.SpeedUp(1.5f, true);
					this.mSpeedTxt.text = "x10";
					this.mSpeedNum = 4;
					GameCache.SetGameSpeed(this.mSpeedNum);
				}
				else
				{
					this.SetSpeedCombatNum1();
				}
			}
			else
			{
				this.SetSpeedCombatNum1();
			}
		}
	}

	private void SetSpeedCombatNum1()
	{
		Globals.Instance.GameMgr.ClearSpeedMod();
		this.mSpeedTxt.text = "x1";
		this.mSpeedNum = 1;
		GameCache.SetGameSpeed(this.mSpeedNum);
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	public void SetState(int nState)
	{
		this.mCurState = nState;
		switch (nState)
		{
		case 0:
		case 1:
		case 6:
			this.mSkipCombatBtn.gameObject.SetActive(false);
			this.mAutoBtn.gameObject.SetActive(true);
			this.IsAutoBtnLock = false;
			this.mFollowBtn.gameObject.SetActive(true);
			this.mSpeedBtn.gameObject.SetActive(true);
			this.SpeedCombatNum = Mathf.Clamp(GameCache.Data.GameSpeed, 1, 4);
			this.RefreshAutoCombatBtn();
			break;
		case 2:
		case 8:
		case 9:
			this.mSkipCombatBtn.gameObject.SetActive(false);
			this.mAutoBtn.gameObject.SetActive(true);
			this.IsAutoBtnLock = true;
			this.mFollowBtn.gameObject.SetActive(false);
			this.mSpeedBtn.gameObject.SetActive(false);
			this.RefreshAutoCombatBtn();
			break;
		case 3:
		{
			this.mSkipCombatBtn.gameObject.SetActive(false);
			this.mAutoBtn.gameObject.SetActive(true);
			this.IsAutoBtnLock = false;
			this.mFollowBtn.gameObject.SetActive(true);
			this.mFollowPlayer = Globals.Instance.ActorMgr.ForceFollow;
			this.SetFollowBtnState(this.mFollowPlayer);
			this.mSpeedBtn.gameObject.SetActive(false);
			Vector3 localPosition = this.mAutoBtn.transform.localPosition;
			localPosition.y += 82f;
			this.mFollowBtn.transform.localPosition = localPosition;
			this.RefreshAutoCombatBtn();
			break;
		}
		case 4:
		{
			if (Globals.Instance.Player.TeamSystem.GetCombatValue() > Globals.Instance.Player.TeamSystem.GeRemoteCombatValue() * 15 / 10)
			{
				this.mSkipCombatBtn.gameObject.SetActive(true);
				LopetDataEx curLopet = Globals.Instance.Player.LopetSystem.GetCurLopet(true);
				if (curLopet != null)
				{
					this.mSkipCombatBtn.transform.localPosition = new Vector3(-490f, -65f, 0f);
				}
				else
				{
					this.mSkipCombatBtn.transform.localPosition = new Vector3(-385f, -65f, 0f);
				}
			}
			else
			{
				this.mSkipCombatBtn.gameObject.SetActive(false);
			}
			this.mAutoBtn.gameObject.SetActive(true);
			this.IsAutoBtnLock = false;
			this.mFollowBtn.gameObject.SetActive(true);
			this.mSpeedBtn.gameObject.SetActive(true);
			Vector3 localPosition2 = this.mAutoBtn.transform.localPosition;
			localPosition2.x -= 82f;
			this.mSpeedBtn.transform.localPosition = localPosition2;
			localPosition2.x -= 82f;
			this.mFollowBtn.transform.localPosition = localPosition2;
			this.SpeedCombatNum = Mathf.Clamp(GameCache.Data.GameSpeed, 1, 4);
			this.RefreshAutoCombatBtn();
			break;
		}
		case 5:
		case 7:
			this.mSkipCombatBtn.gameObject.SetActive(false);
			this.mAutoBtn.gameObject.SetActive(true);
			this.IsAutoBtnLock = false;
			this.mFollowBtn.gameObject.SetActive(true);
			this.mFollowPlayer = Globals.Instance.ActorMgr.ForceFollow;
			this.SetFollowBtnState(this.mFollowPlayer);
			this.mSpeedBtn.gameObject.SetActive(true);
			this.SpeedCombatNum = Mathf.Clamp(GameCache.Data.GameSpeed, 1, 4);
			this.RefreshAutoCombatBtn();
			break;
		default:
			global::Debug.LogErrorFormat("Unkonw ESceneType {0}", new object[]
			{
				nState
			});
			break;
		}
		this.UpdateAutoCombatBtnPosition();
	}

	private void CreateObjects()
	{
		this.mAutoBtn = base.transform.FindChild("AutoCombat").GetComponent<UIButton>();
		this.mAutoBtn.gameObject.SetActive(false);
		UIEventListener expr_3C = UIEventListener.Get(this.mAutoBtn.gameObject);
		expr_3C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3C.onClick, new UIEventListener.VoidDelegate(this.OnAutoCombatBtnClicked));
		GamePadMgr.RegClickDelegate(16, new GamePadMgr.VoidDelegate(this.ProcessAutoCombatBtnClicked));
		this.mAutoCombatSprite = this.mAutoBtn.transform.GetComponent<UISprite>();
		this.mAutoCombatLabel = this.mAutoCombatSprite.transform.FindChild("Label").GetComponent<UILabel>();
		this.mAutoCombatFX = this.mAutoCombatSprite.transform.FindChild("ui03").gameObject;
		Tools.SetParticleRenderQueue(this.mAutoCombatFX, 4000, 1f);
		ParticleSystem[] componentsInChildren = this.mAutoCombatFX.GetComponentsInChildren<ParticleSystem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			ParticleSystem particleSystem = componentsInChildren[i];
			particleSystem.startSpeed *= 0.62f;
			particleSystem.startSize *= 0.62f;
			particleSystem.gravityModifier *= 0.62f;
		}
		NGUITools.SetActive(this.mAutoCombatFX, false);
		this.mLockGo = this.mAutoBtn.transform.Find("lock").gameObject;
		this.pcc = Globals.Instance.ActorMgr.PlayerCtrler;
		this.mFollowBtn = base.transform.Find("followBtn").GetComponent<UISprite>();
		UIEventListener expr_1A2 = UIEventListener.Get(this.mFollowBtn.gameObject);
		expr_1A2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1A2.onClick, new UIEventListener.VoidDelegate(this.OnFollowBtnClicked));
		GamePadMgr.RegClickDelegate(32, new GamePadMgr.VoidDelegate(this.ProcessFollowBtnClicked));
		this.mFollowBtn.gameObject.SetActive(false);
		this.mSpeedBtn = base.transform.Find("speedCombat").gameObject;
		UIEventListener expr_20D = UIEventListener.Get(this.mSpeedBtn);
		expr_20D.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_20D.onClick, new UIEventListener.VoidDelegate(this.OnSpeedBtnClicked));
		this.mSpeedTxt = this.mSpeedBtn.transform.Find("Label").GetComponent<UILabel>();
		this.mSkipCombatBtn = base.transform.Find("SkipCombat").gameObject;
		UIEventListener expr_274 = UIEventListener.Get(this.mSkipCombatBtn);
		expr_274.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_274.onClick, new UIEventListener.VoidDelegate(this.OnSkipCombatBtnClicked));
		this.mSkipCombatBtn.SetActive(false);
		this.mAutoSkill = false;
		this.mAutoCombat = false;
		this.SetAutoCombat();
	}

	private void OnDestroy()
	{
		GamePadMgr.UnRegClickDelegate(16, new GamePadMgr.VoidDelegate(this.ProcessAutoCombatBtnClicked));
		GamePadMgr.UnRegClickDelegate(32, new GamePadMgr.VoidDelegate(this.ProcessFollowBtnClicked));
	}

	private void OnAutoCombatBtnClicked(GameObject go)
	{
		this.ProcessAutoCombatBtnClicked();
	}

	private void ProcessAutoCombatBtnClicked()
	{
		if (!this.mAutoBtn.gameObject.activeInHierarchy)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_011");
		if (this.pcc != null)
		{
			if (this.mIsAutoBtnLock)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("pvpTxt11", 0f, 0f);
				return;
			}
			if (this.mAutoSkill)
			{
				GameCache.Data.AutoSkill = false;
				Globals.Instance.ActorMgr.ChangeAIMode(false);
			}
			else if (this.mAutoCombat)
			{
				GameCache.Data.AutoSkill = true;
			}
			else
			{
				Globals.Instance.ActorMgr.ChangeAIMode(true);
			}
		}
	}

	private void OnFollowBtnClicked(GameObject go)
	{
		this.ProcessFollowBtnClicked();
	}

	private void ProcessFollowBtnClicked()
	{
		if (!this.mFollowBtn.gameObject.activeInHierarchy)
		{
			return;
		}
		Globals.Instance.EffectSoundMgr.Play("ui/ui_011");
		if (this.pcc != null)
		{
			Globals.Instance.ActorMgr.ChangeForceFollow();
		}
	}

	public void UpdateControlType()
	{
		this.pcc.ControlType = (GameCache.Data.Joystick ? PlayerController.EControlType.EJoystick : PlayerController.EControlType.ETouch);
		this.UpdateAutoCombatBtnPosition();
	}

	private void UpdateAutoCombatBtnPosition()
	{
		if (GameCache.Data.Joystick && this.mCurState != 2 && this.mCurState != 8)
		{
			if (GameUIManager.mInstance.GetDPad() == null)
			{
				GameUIManager.mInstance.CreateDPad(base.transform.parent.gameObject);
			}
			GameUIManager.mInstance.GetDPad().NeutralDPad();
		}
		else if (GameUIManager.mInstance.GetDPad() != null)
		{
			GameUIManager.mInstance.GetDPad().DisableDPad();
		}
	}

	private void SetAutoCombat()
	{
		if (this.mAutoSkill)
		{
			this.mAutoCombatLabel.text = Singleton<StringManager>.Instance.GetString("AutoCancel");
			NGUITools.SetActive(this.mAutoCombatFX, true);
			this.mAutoBtn.normalSprite = "botton-guide3";
			this.mAutoBtn.pressedSprite = "botton-guide3";
			this.mAutoBtn.hoverSprite = "botton-guide3";
			this.mAutoBtn.disabledSprite = "botton-guide3";
		}
		else if (this.mAutoCombat)
		{
			this.mAutoCombatLabel.text = Singleton<StringManager>.Instance.GetString("AutoSkill");
			NGUITools.SetActive(this.mAutoCombatFX, true);
			this.mAutoBtn.normalSprite = "botton-guide2";
			this.mAutoBtn.pressedSprite = "botton-guide2";
			this.mAutoBtn.hoverSprite = "botton-guide2";
			this.mAutoBtn.disabledSprite = "botton-guide2";
		}
		else
		{
			this.mAutoCombatLabel.text = Singleton<StringManager>.Instance.GetString("AutoCombat");
			NGUITools.SetActive(this.mAutoCombatFX, false);
			this.mAutoBtn.normalSprite = "botton-guide";
			this.mAutoBtn.pressedSprite = "botton-guide";
			this.mAutoBtn.hoverSprite = "botton-guide";
			this.mAutoBtn.disabledSprite = "botton-guide";
		}
	}

	public void SetFollowBtnState(bool isFollow)
	{
		this.mFollowBtn.spriteName = ((!isFollow) ? "Follow" : "Attack");
	}

	private void OnSpeedBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_011");
		if (Tools.IsUnlockPveCombatSpeedupX2())
		{
			if (this.mSpeedNum == 1)
			{
				this.SpeedCombatNum = 2;
			}
			else if (this.mSpeedNum == 2)
			{
				if (Tools.IsUnlockPveCombatSpeedupX3())
				{
					this.SpeedCombatNum = 3;
				}
				else
				{
					GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("speedCombat"), 0f, 0f);
					this.SpeedCombatNum = 1;
				}
			}
			else if (this.mSpeedNum == 3)
			{
				if (Globals.Instance.CliSession.Privilege > 0)
				{
					this.SpeedCombatNum = 4;
				}
				else
				{
					this.SpeedCombatNum = 1;
				}
			}
			else if (this.mSpeedNum == 4)
			{
				this.SpeedCombatNum = 1;
			}
		}
		else
		{
			GameUIManager.mInstance.ShowMessageTip(Singleton<StringManager>.Instance.GetString("speedCombat2", new object[]
			{
				GameConst.GetInt32(17)
			}), 0f, 0f);
		}
	}

	private void OnSkipCombatBtnClicked(GameObject go)
	{
		if (this.mCurState == 2 || this.mCurState == 4 || this.mCurState == 8 || this.mCurState == 9)
		{
			Globals.Instance.ActorMgr.SkipCombat();
			this.mSkipCombatBtn.collider.enabled = false;
		}
	}

	private void Update()
	{
		if (this.pcc != null)
		{
			if (this.mAutoBtn.gameObject.activeInHierarchy && (this.mAutoCombat != GameCache.Data.EnableAI || this.mAutoSkill != (GameCache.Data.AutoSkill && GameCache.Data.EnableAI)))
			{
				this.RefreshAutoCombatBtn();
			}
			if (this.mFollowBtn.gameObject.activeInHierarchy && this.mFollowPlayer != Globals.Instance.ActorMgr.ForceFollow)
			{
				this.mFollowPlayer = Globals.Instance.ActorMgr.ForceFollow;
				this.SetFollowBtnState(this.mFollowPlayer);
			}
		}
	}

	private void RefreshAutoCombatBtn()
	{
		if (this.mCurState == 2 || this.mCurState == 8 || this.mCurState == 9)
		{
			this.mAutoCombat = true;
			this.mAutoSkill = true;
		}
		else
		{
			this.mAutoCombat = GameCache.Data.EnableAI;
			this.mAutoSkill = (GameCache.Data.AutoSkill && GameCache.Data.EnableAI);
		}
		this.SetAutoCombat();
	}
}
