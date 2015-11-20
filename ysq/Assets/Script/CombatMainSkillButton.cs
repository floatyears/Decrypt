using Att;
using NtUniSdk.Unity3d;
using System;
using System.Text;
using UnityEngine;

public class CombatMainSkillButton : MainSkillButtonBase
{
	private GameObject m_skillBtn;

	private GameUIToolTip mSkillToolTip;

	private float pressTimestamp = -1f;

	private float pressReleaseTimestamp;

	private UISprite m_skillbg;

	private UITexture m_skillIconMask;

	private UITexture m_skillIcon;

	private UISprite m_skillInActive;

	private UILabel m_skillCoolDownNum;

	private bool m_enableSkillBtn;

	private GameObject m_coolEndEffect;

	private int m_needMp;

	private bool? m_needMpFontColorRed;

	private UILabel m_skillNeedMpNum;

	private int m_skillKeyCode;

	private bool m_skillDragStart;

	public bool IsEquipChanged;

	private GameObject mClickEffect;

	private GameObject mLockTag;

	private PlayerController pcc;

	private ESceneType mSceneType;

	private float updateTimer;

	private int mTouchID = -2147483648;

	private GameObject mRegionSkillEf;

	private SkillTipEffect mRegionSkillEfComp;

	private GameObject mSkillCacheEf;

	public bool TutorialUsing;

	public bool TutorialCanDragSkill;

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
				NGUITools.SetActive(this.m_coolEndEffect, false);
				this.m_skillIconMask.fillAmount = 1f;
				this.m_skillNeedMpNum.gameObject.SetActive(false);
				this.m_skillIcon.gameObject.SetActive(false);
				this.m_skillbg.alpha = 0.5f;
			}
			else
			{
				this.m_skillNeedMpNum.gameObject.SetActive(true);
				this.m_skillIcon.gameObject.SetActive(true);
				this.m_skillbg.alpha = 1f;
			}
		}
	}

	public bool NeedMPFontColorRed
	{
		get
		{
			return this.m_needMpFontColorRed.HasValue && this.m_needMpFontColorRed.Value;
		}
		set
		{
			if (!this.m_needMpFontColorRed.HasValue || value != this.m_needMpFontColorRed)
			{
				this.m_needMpFontColorRed = new bool?(value);
				if (this.m_needMpFontColorRed == true)
				{
					this.m_skillNeedMpNum.color = Color.red;
				}
				else
				{
					this.m_skillNeedMpNum.color = new Color32(94, 244, 246, 255);
				}
				this.m_skillNeedMpNum.text = this.m_needMp.ToString();
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
		this.m_needMp = 0;
	}

	private void OnDestroy()
	{
		GamePadMgr.UnRegClickDelegate(this.m_skillKeyCode, new GamePadMgr.VoidDelegate(this.ProcessSkillBtnClick));
		GamePadMgr.UnRegPressDelegate(this.m_skillKeyCode, new GamePadMgr.BoolDelegate(this.ProcessSkillBtnPress));
		GamePadMgr.UnRegDragDelegate(this.m_skillKeyCode, new GamePadMgr.BoolDelegate(this.ProcessSkillBtnDrag));
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
		this.m_skillInActive = this.m_skillIcon.transform.FindChild("disableIcon").GetComponent<UISprite>();
		this.m_coolEndEffect = this.m_skillIcon.transform.FindChild("ui02").gameObject;
		Tools.SetParticleRenderQueue(this.m_coolEndEffect, 4000, 1f);
		NGUITools.SetActive(this.m_coolEndEffect, false);
		this.m_skillIconMask = this.m_skillIcon.transform.FindChild("cooldown").GetComponent<UITexture>();
		this.m_skillCoolDownNum = base.transform.FindChild("cooldownNum").GetComponent<UILabel>();
		this.m_skillNeedMpNum = base.transform.FindChild("needMpNum").GetComponent<UILabel>();
		this.m_skillbg = base.GetComponent<UISprite>();
		this.mLockTag = base.transform.Find("lock").gameObject;
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

	private void ProcessSkillBtnDrag(bool drag)
	{
		if (drag)
		{
			if (!this.m_skillDragStart)
			{
				this.m_skillDragStart = this.DragStart();
			}
			else
			{
				this.DragMove();
			}
		}
		else if (this.m_skillDragStart)
		{
			this.m_skillDragStart = false;
			this.DragEnd();
		}
	}

	public void OnSkillBtnClicked(GameObject go)
	{
		this.ProcessSkillBtnClick();
	}

	private void ProcessSkillBtnClick()
	{
		if (this.EnableSkillBtn && Time.time - this.pressReleaseTimestamp > 0.5f)
		{
			if (this.pcc == null)
			{
				return;
			}
			if (this.NeedMPFontColorRed)
			{
				return;
			}
			if (this.TutorialUsing)
			{
				return;
			}
			if (this.mSceneType == ESceneType.EScene_Arena || this.mSceneType == ESceneType.EScene_OrePillage || this.mSceneType == ESceneType.EScene_GuildPvp)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("pvpTxt11", 0f, 0f);
				return;
			}
			if (this.IsCoolTime())
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

	public bool IsCoolTime()
	{
		return 0f < this.m_skillIconMask.fillAmount;
	}

	private void EndCoolTime()
	{
		if (this.IsCoolTime())
		{
			this.m_skillIconMask.fillAmount = 0f;
			this.m_skillCoolDownNum.gameObject.SetActive(false);
			NGUITools.SetActive(this.m_coolEndEffect, true);
		}
	}

	public void InitIngameSkillBtn(int _skillSlotIdx, int keyCode)
	{
		this.pcc = Globals.Instance.ActorMgr.PlayerCtrler;
		this.m_skillIndex = _skillSlotIdx;
		if (0 <= this.m_skillIndex && this.m_skillIndex < this.pcc.ActorCtrler.Skills.Length && this.pcc.ActorCtrler.Skills[this.m_skillIndex] != null)
		{
			this.m_needMp = this.pcc.ActorCtrler.Skills[this.m_skillIndex].Info.ManaCost;
		}
		else
		{
			this.m_needMp = 0;
		}
		this.m_needMpFontColorRed = null;
		this.NeedMPFontColorRed = false;
		this.m_skillIconMask.fillAmount = 0f;
		this.m_skillCoolDownNum.gameObject.SetActive(false);
		this.m_skillInActive.gameObject.SetActive(false);
		this.EndCoolTime();
		this.m_skillKeyCode = keyCode;
		GamePadMgr.RegClickDelegate(this.m_skillKeyCode, new GamePadMgr.VoidDelegate(this.ProcessSkillBtnClick));
		GamePadMgr.RegPressDelegate(this.m_skillKeyCode, new GamePadMgr.BoolDelegate(this.ProcessSkillBtnPress));
		GamePadMgr.RegDragDelegate(this.m_skillKeyCode, new GamePadMgr.BoolDelegate(this.ProcessSkillBtnDrag));
	}

	private void Update()
	{
		if (this.EnableSkillBtn && this.pressTimestamp > 0f && this.mRegionSkillEf == null && Time.time - this.pressTimestamp > 0.5f)
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
			this.EnableSkillBtn = (this.pcc.ActorCtrler.Skills[this.m_skillIndex] != null);
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
			SkillData skillData = this.pcc.ActorCtrler.Skills[this.m_skillIndex];
			if (skillData.CoolPercent < 1f && !this.IsCoolTime())
			{
				this.m_skillIconMask.fillAmount = 1f;
				this.m_skillCoolDownNum.gameObject.SetActive(true);
				this.m_skillCoolDownNum.text = skillData.RemainCoolDownTime.ToString();
				NGUITools.SetActive(this.m_coolEndEffect, false);
			}
			else if (skillData.CoolPercent == 1f)
			{
				this.EndCoolTime();
			}
			if (this.IsCoolTime())
			{
				this.m_skillCoolDownNum.gameObject.SetActive(true);
				this.m_skillIconMask.fillAmount = 1f - skillData.CoolPercent;
				this.m_skillCoolDownNum.text = (((float)skillData.RemainCoolDownTime != 0f) ? skillData.RemainCoolDownTime.ToString() : string.Empty);
			}
			this.NeedMPFontColorRed = (this.pcc.ActorCtrler.CurMP < (long)skillData.Info.ManaCost);
			NGUITools.SetActive(this.mSkillCacheEf, this.pcc.ShouldShowSkillCache(this.m_skillIndex));
		}
	}

	public void NormalImageChange()
	{
		if (this.EnableSkillBtn)
		{
			Texture mainTexture = Res.Load<Texture>("icon/skill/" + this.pcc.ActorCtrler.Skills[this.m_skillIndex].Info.Icon, false);
			this.m_skillIcon.mainTexture = mainTexture;
			this.m_skillIconMask.mainTexture = mainTexture;
			this.m_skillbg.spriteName = ((this.pcc.ActorCtrler.Skills[this.m_skillIndex].Info.CastTargetType != 3) ? "botton-skill1" : "botton-skill");
		}
		else
		{
			this.m_skillIcon.mainTexture = null;
			this.m_skillIconMask.mainTexture = null;
			this.m_skillbg.spriteName = "botton-skill1";
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
		if (this.NeedMPFontColorRed)
		{
			return false;
		}
		if (this.mSceneType == ESceneType.EScene_Arena || this.mSceneType == ESceneType.EScene_OrePillage || this.mSceneType == ESceneType.EScene_GuildPvp)
		{
			return false;
		}
		if (this.IsCoolTime())
		{
			return false;
		}
		if (this.TutorialUsing && !this.TutorialCanDragSkill)
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
