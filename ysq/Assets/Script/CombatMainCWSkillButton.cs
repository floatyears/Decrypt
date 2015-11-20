using Att;
using Holoville.HOTween;
using NtUniSdk.Unity3d;
using System;
using System.Text;
using UnityEngine;

public class CombatMainCWSkillButton : MainSkillButtonBase
{
	private GameObject m_skillBtn;

	private GameObject mClickEffect;

	private GameObject mLockTag;

	private GameObject mSkillCacheEf;

	private GameObject mUI95;

	private UITexture m_skillIcon;

	private UITexture m_skillIconMask;

	private UISprite m_skillbg;

	private UISprite m_skillXuLiFg;

	private ESceneType mSceneType;

	private float updateTimer;

	private int mTouchID = -2147483648;

	private GameObject mRegionSkillEf;

	private SkillTipEffect mRegionSkillEfComp;

	private GameUIToolTip mSkillToolTip;

	private float pressTimestamp = -1f;

	private float pressReleaseTimestamp;

	private bool m_enableSkillBtn;

	public bool IsEquipChanged;

	private PlayerController pcc;

	private Vector3 dragCurPos;

	public bool EnableSkillBtn
	{
		get
		{
			return this.m_enableSkillBtn;
		}
		set
		{
			this.m_enableSkillBtn = value;
			if (!this.m_enableSkillBtn)
			{
				NGUITools.SetActive(this.mUI95, false);
				this.m_skillIconMask.gameObject.SetActive(false);
				this.m_skillIcon.gameObject.SetActive(false);
				this.m_skillbg.alpha = 0.5f;
			}
			else
			{
				this.m_skillIcon.gameObject.SetActive(true);
				this.m_skillbg.alpha = 1f;
			}
		}
	}

	public GameObject getRegionSkillEf()
	{
		return this.mRegionSkillEf;
	}

	public void SetState(int nState)
	{
		this.mSceneType = (ESceneType)nState;
		ESceneType eSceneType = this.mSceneType;
		if (eSceneType != ESceneType.EScene_OrePillage && eSceneType != ESceneType.EScene_GuildPvp && eSceneType != ESceneType.EScene_Arena)
		{
			this.mLockTag.SetActive(false);
		}
		else
		{
			this.mLockTag.SetActive(true);
		}
	}

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mClickEffect = base.transform.Find("ui34").gameObject;
		Tools.SetParticleRenderQueue(this.mClickEffect, 4000, 1f);
		NGUITools.SetActive(this.mClickEffect, false);
		this.mSkillCacheEf = base.transform.Find("ui46").gameObject;
		Tools.SetParticleRenderQueue(this.mSkillCacheEf, 4000, 1f);
		NGUITools.SetActive(this.mSkillCacheEf, false);
		this.m_skillBtn = base.transform.FindChild("skill_btn").gameObject;
		UIEventListener expr_A3 = UIEventListener.Get(this.m_skillBtn.gameObject);
		expr_A3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A3.onClick, new UIEventListener.VoidDelegate(this.OnSkillBtnClicked));
		UIEventListener expr_D4 = UIEventListener.Get(this.m_skillBtn.gameObject);
		expr_D4.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_D4.onPress, new UIEventListener.BoolDelegate(this.OnSkillBtnPressd));
		UIEventListener expr_105 = UIEventListener.Get(this.m_skillBtn.gameObject);
		expr_105.onDragStart = (UIEventListener.VoidDelegate)Delegate.Combine(expr_105.onDragStart, new UIEventListener.VoidDelegate(this.OnDragStart));
		this.m_skillIcon = this.m_skillBtn.transform.FindChild("skill_icon").GetComponent<UITexture>();
		this.mUI95 = this.m_skillIcon.transform.FindChild("ui95").gameObject;
		Tools.SetParticleRenderQueue(this.mUI95, 4000, 1f);
		NGUITools.SetActive(this.mUI95, false);
		this.m_skillIconMask = this.m_skillIcon.transform.FindChild("cooldown").GetComponent<UITexture>();
		this.mLockTag = base.transform.Find("lock").gameObject;
		this.m_skillXuLiFg = base.transform.Find("xuliFg").GetComponent<UISprite>();
		this.m_skillXuLiFg.fillAmount = 0f;
		this.m_skillbg = base.GetComponent<UISprite>();
	}

	public void OnSkillBtnClicked(GameObject go)
	{
		this.ProcessSkillBtnClick();
	}

	public bool IsEPEnough()
	{
		return !(this.pcc == null) && !(this.pcc.ActorCtrler == null) && this.pcc.ActorCtrler.CurEP >= (long)GameConst.GetInt32(217);
	}

	private void ProcessSkillBtnClick()
	{
		if (this.EnableSkillBtn && Time.time - this.pressReleaseTimestamp > 0.5f)
		{
			if (this.pcc == null)
			{
				return;
			}
			if (this.mSceneType == ESceneType.EScene_Arena || this.mSceneType == ESceneType.EScene_OrePillage || this.mSceneType == ESceneType.EScene_GuildPvp)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("pvpTxt11", 0f, 0f);
				return;
			}
			if (!this.IsEPEnough())
			{
				if (this.pcc.ActorCtrler.LockSkillIndex >= 0)
				{
					return;
				}
				this.pcc.CastSkill(0);
				return;
			}
			else
			{
				this.pcc.CastSkill(this.m_skillIndex);
				NGUITools.SetActive(this.mClickEffect, false);
				NGUITools.SetActive(this.mClickEffect, true);
			}
		}
	}

	private void OnSkillBtnPressd(GameObject go, bool isPressed)
	{
		this.ProcessSkillBtnPress(isPressed);
	}

	private void ProcessSkillBtnPress(bool isPressed)
	{
		if (isPressed)
		{
			this.pressTimestamp = Time.time;
		}
		else
		{
			this.pressTimestamp = -1f;
			if (this.mSkillToolTip != null)
			{
				if (this.mSkillToolTip.gameObject.activeInHierarchy)
				{
					this.pressReleaseTimestamp = Time.time;
				}
				this.mSkillToolTip.HideTipAnim();
			}
		}
	}

	private void ShowSkillTips()
	{
		if (!this.EnableSkillBtn)
		{
			return;
		}
		SkillInfo info = this.pcc.ActorCtrler.Skills[this.m_skillIndex].Info;
		if (this.mSkillToolTip == null)
		{
			this.mSkillToolTip = GameUIToolTipManager.GetInstance().CreateSkillTooltip(base.transform, info);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[66FE00]").Append(info.Desc).Append("[-]");
		this.mSkillToolTip.Create(base.transform, info.Name, stringBuilder.ToString());
		this.mSkillToolTip.transform.localPosition = new Vector3(-175f, this.mSkillToolTip.transform.localPosition.y, -2000f);
		this.mSkillToolTip.EnableToolTip();
	}

	public void NormalImageChange()
	{
		if (this.EnableSkillBtn)
		{
			Texture mainTexture = Res.Load<Texture>("icon/skill/" + this.pcc.ActorCtrler.Skills[this.m_skillIndex].Info.Icon, false);
			this.m_skillIcon.mainTexture = mainTexture;
			this.m_skillIconMask.mainTexture = mainTexture;
		}
		else
		{
			this.m_skillIcon.mainTexture = null;
			this.m_skillIconMask.mainTexture = null;
		}
	}

	public void InitIngameSkillBtn(int _skillSlotIdx, int keyCode)
	{
		this.pcc = Globals.Instance.ActorMgr.PlayerCtrler;
		this.m_skillIndex = _skillSlotIdx;
		this.m_skillIconMask.gameObject.SetActive(false);
		this.m_skillXuLiFg.fillAmount = 0f;
	}

	private void Update()
	{
		if (this.EnableSkillBtn && this.pressTimestamp > 0f && Time.time - this.pressTimestamp > 0.5f)
		{
			this.pressTimestamp = -1f;
			this.ShowSkillTips();
		}
		this.updateTimer += Time.deltaTime;
		if (this.updateTimer < 0.3f)
		{
			return;
		}
		this.updateTimer = 0f;
		if (this.pcc == null)
		{
			return;
		}
		if (0 <= this.m_skillIndex && this.m_skillIndex < this.pcc.ActorCtrler.Skills.Length)
		{
			this.EnableSkillBtn = (Globals.Instance.Player.LopetSystem.GetCurLopet(true) != null && this.pcc.ActorCtrler.Skills[this.m_skillIndex] != null);
		}
		else
		{
			this.EnableSkillBtn = false;
		}
		if (this.IsEquipChanged)
		{
			this.IsEquipChanged = false;
			this.NormalImageChange();
		}
		if (this.EnableSkillBtn)
		{
			if (this.IsEPEnough())
			{
				this.m_skillIconMask.gameObject.SetActive(false);
				if (!this.mUI95.gameObject.activeInHierarchy)
				{
					NGUITools.SetActive(this.mUI95, false);
					NGUITools.SetActive(this.mUI95, true);
				}
				if (this.m_skillXuLiFg.fillAmount < 1f)
				{
					if (HOTween.IsTweening(this.m_skillXuLiFg))
					{
						HOTween.Complete(this.m_skillXuLiFg);
						HOTween.Kill(this.m_skillXuLiFg);
					}
					HOTween.To(this.m_skillXuLiFg, 0.15f, new TweenParms().Prop("fillAmount", 1f));
				}
			}
			else
			{
				this.m_skillIconMask.gameObject.SetActive(false);
				if (this.mUI95.gameObject.activeInHierarchy)
				{
					NGUITools.SetActive(this.mUI95, false);
				}
				float num = Mathf.Clamp01((float)this.pcc.ActorCtrler.CurEP / (float)GameConst.GetInt32(217));
				if (num == 0f)
				{
					this.m_skillXuLiFg.fillAmount = 0f;
				}
				else
				{
					if (HOTween.IsTweening(this.m_skillXuLiFg))
					{
						HOTween.Complete(this.m_skillXuLiFg);
						HOTween.Kill(this.m_skillXuLiFg);
					}
					HOTween.To(this.m_skillXuLiFg, 0.15f, new TweenParms().Prop("fillAmount", num));
				}
			}
			NGUITools.SetActive(this.mSkillCacheEf, this.pcc.ShouldShowSkillCache(this.m_skillIndex));
		}
	}

	private void OnDragStart(GameObject go)
	{
		if (!base.enabled || this.mTouchID != -2147483648)
		{
			return;
		}
		Vector2 totalDelta = UICamera.currentTouch.totalDelta;
		if (Mathf.Abs(totalDelta.x) < 1f && Mathf.Abs(totalDelta.y) < 1f)
		{
			return;
		}
		this.DragStart();
	}

	private bool DragStart()
	{
		if (this.pcc == null)
		{
			return false;
		}
		if (!this.EnableSkillBtn)
		{
			return false;
		}
		if (this.mSceneType == ESceneType.EScene_Arena || this.mSceneType == ESceneType.EScene_OrePillage || this.mSceneType == ESceneType.EScene_GuildPvp)
		{
			return false;
		}
		if (!this.IsEPEnough())
		{
			return false;
		}
		SkillInfo info = this.pcc.ActorCtrler.Skills[this.m_skillIndex].Info;
		if (info.CastTargetType == 3 && this.mRegionSkillEf == null)
		{
			GUICombatMain session = GameUIManager.mInstance.GetSession<GUICombatMain>();
			if (session != null)
			{
				session.ShowHideSkillDragTip(true);
			}
			this.CreateSkillRegionEffect();
			return true;
		}
		return false;
	}

	private void DragMove()
	{
		Vector3 vector = new Vector3(-GamePadMgr.moveX, -GamePadMgr.moveY, 0f);
		Vector3 a = (vector.magnitude <= 1f) ? vector : vector.normalized;
		this.dragCurPos -= a * 60f;
		if (this.dragCurPos.x < 0f)
		{
			this.dragCurPos.x = 0f;
		}
		else if (this.dragCurPos.x > (float)Screen.width)
		{
			this.dragCurPos.x = (float)Screen.width;
		}
		if (this.dragCurPos.y < 0f)
		{
			this.dragCurPos.y = 0f;
		}
		else if (this.dragCurPos.y > (float)Screen.height)
		{
			this.dragCurPos.y = (float)Screen.height;
		}
		this.mRegionSkillEfComp.DragMove(this.dragCurPos);
	}

	private void DragEnd()
	{
		this.mRegionSkillEfComp.DragEnd();
		NGUITools.Destroy(this.mRegionSkillEfComp.gameObject);
	}

	public bool IsAOESkill()
	{
		if (this.pcc.ActorCtrler.Skills[this.m_skillIndex] != null)
		{
			SkillInfo info = this.pcc.ActorCtrler.Skills[this.m_skillIndex].Info;
			return info.CastTargetType == 3;
		}
		return false;
	}

	private void CreateSkillRegionEffect()
	{
		GameObject gameObject = Res.Load<GameObject>("Skill/com/st_065", false);
		if (gameObject != null)
		{
			this.mRegionSkillEf = (UnityEngine.Object.Instantiate(gameObject) as GameObject);
			this.mRegionSkillEf.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (UICamera.currentTouch != null)
			{
				UICamera.currentTouch.dragged = this.mRegionSkillEf;
			}
			if (this.mRegionSkillEf != null)
			{
				SkillTipEffect skillTipEffect = this.mRegionSkillEf.AddComponent<SkillTipEffect>();
				skillTipEffect.InitWithBaseScene(this.m_skillIndex);
				skillTipEffect.mTouchID = UICamera.currentTouchID;
				this.mRegionSkillEfComp = skillTipEffect;
				this.dragCurPos = UICamera.currentCamera.WorldToScreenPoint(this.m_skillbg.worldCorners[0]);
			}
		}
	}
}
