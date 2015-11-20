using System;
using UnityEngine;

public class UIIngameActorHPMP : MonoBehaviour
{
	public enum PlayerUIType
	{
		HPMP,
		HPONLY
	}

	public float m_scale = 1f;

	public float m_heightOffest = 0.2f;

	private GameObject m_playerHpMp;

	private Camera m_2dCamera;

	private Camera m_3dCamera;

	private bool m_bVisiable = true;

	private UISprite m_spriteHP;

	private UISprite m_spriteMP;

	private UISprite mHpMpBg;

	private bool m_isPlayer;

	private GameObject headTip;

	public UISlider hpbar;

	private float size_y;

	private float npcHeight;

	private ActorController m_targetUnit;

	public bool IsWin
	{
		get;
		set;
	}

	public int SpriteDepth
	{
		get
		{
			if (this.mHpMpBg != null)
			{
				return this.mHpMpBg.depth;
			}
			if (this.m_spriteHP != null)
			{
				return this.m_spriteHP.depth;
			}
			if (this.m_spriteMP != null)
			{
				return this.m_spriteMP.depth;
			}
			return 0;
		}
		set
		{
			if (this.mHpMpBg != null)
			{
				this.mHpMpBg.depth = value;
			}
			if (this.m_spriteHP != null)
			{
				this.m_spriteHP.depth = value + 1;
			}
			if (this.m_spriteMP != null)
			{
				this.m_spriteMP.depth = value + 1;
			}
		}
	}

	public string SliderThumbIcon
	{
		get
		{
			if (this.hpbar != null)
			{
				UISprite uISprite = this.hpbar.foregroundWidget as UISprite;
				if (uISprite != null)
				{
					return uISprite.spriteName;
				}
			}
			return string.Empty;
		}
		set
		{
			if (this.hpbar != null && !string.IsNullOrEmpty(value))
			{
				UISprite uISprite = this.hpbar.foregroundWidget as UISprite;
				if (uISprite != null)
				{
					uISprite.spriteName = value;
				}
			}
		}
	}

	public bool Green
	{
		get
		{
			return this.SliderThumbIcon == "pet-blood01";
		}
		set
		{
			this.SliderThumbIcon = ((!value) ? "monster-blood01" : "pet-blood01");
		}
	}

	public bool PlayerGreen
	{
		get
		{
			return this.m_spriteHP.spriteName == "playerHpFg";
		}
		set
		{
			this.m_spriteHP.spriteName = ((!value) ? "playerHpFg2" : "playerHpFg");
		}
	}

	public void Init(ActorController target)
	{
		if (target == null)
		{
			return;
		}
		if (GameUIManager.mInstance != null)
		{
			this.m_targetUnit = target;
			this.m_isPlayer = (this.m_targetUnit.ActorType == ActorController.EActorType.EPlayer);
			if (this.m_isPlayer)
			{
				GameObject prefab = Res.LoadGUI("GUI/PlayerHpMpbar");
				this.m_playerHpMp = NGUITools.AddChild(GameUIManager.mInstance.HUDTextMgr.gameObject, prefab);
				this.mHpMpBg = this.m_playerHpMp.transform.FindChild("playerHpMpBg").GetComponent<UISprite>();
				this.m_spriteHP = this.mHpMpBg.transform.FindChild("HpForeground").GetComponent<UISprite>();
				this.m_spriteMP = this.mHpMpBg.transform.FindChild("MpForeground").GetComponent<UISprite>();
				ActorController component = base.gameObject.GetComponent<ActorController>();
				this.PlayerGreen = (component != null && component.FactionType == ActorController.EFactionType.EBlue);
			}
			else
			{
				GameObject prefab2 = Res.LoadGUI("GUI/hpbar");
				this.headTip = NGUITools.AddChild(GameUIManager.mInstance.HUDTextMgr.gameObject, prefab2);
				this.hpbar = this.headTip.GetComponent<UISlider>();
				this.hpbar.value = 1f;
				ActorController component2 = base.gameObject.GetComponent<ActorController>();
				this.Green = (component2 != null && component2.FactionType == ActorController.EFactionType.EBlue);
				this.mHpMpBg = this.headTip.transform.FindChild("Background").GetComponent<UISprite>();
				this.m_spriteHP = this.headTip.transform.FindChild("Foreground").GetComponent<UISprite>();
			}
			this.size_y = ((!(base.collider == null)) ? base.collider.bounds.size.y : 0f);
			this.npcHeight = base.transform.position.y + this.size_y + this.m_heightOffest;
			this.m_3dCamera = Camera.main;
			this.m_2dCamera = GameUIManager.mInstance.uiCamera.camera;
		}
		this.Visiable(false);
	}

	public void Visiable(bool bVisiable)
	{
		if (bVisiable != this.m_bVisiable)
		{
			this.m_bVisiable = bVisiable;
			if (this.m_isPlayer)
			{
				this.m_spriteHP.gameObject.SetActive(this.m_bVisiable);
				this.m_spriteMP.gameObject.SetActive(this.m_bVisiable);
				this.mHpMpBg.gameObject.SetActive(this.m_bVisiable);
			}
			else if (this.headTip != null)
			{
				this.headTip.SetActive(this.m_bVisiable);
			}
		}
	}

	private void OnDestroy()
	{
		if (this.m_playerHpMp != null)
		{
			UnityEngine.Object.Destroy(this.m_playerHpMp);
			this.m_playerHpMp = null;
		}
		if (this.headTip != null)
		{
			UnityEngine.Object.Destroy(this.headTip);
			this.headTip = null;
		}
	}

	private void LateUpdate()
	{
		if (this.m_targetUnit == null || this.m_targetUnit.gameObject == null)
		{
			return;
		}
		if (!this.m_targetUnit.gameObject.activeInHierarchy || this.m_targetUnit.IsDead || this.IsWin || !GameSetting.Data.ShowHPBar)
		{
			this.Visiable(false);
		}
		else
		{
			this.Visiable(true);
			this.npcHeight = base.transform.position.y + this.size_y + this.m_heightOffest;
			Vector3 position = new Vector3(base.transform.position.x, this.npcHeight, base.transform.position.z);
			position = this.m_2dCamera.ViewportToWorldPoint(this.m_3dCamera.WorldToViewportPoint(position));
			position.z = 15f;
			if (this.m_isPlayer)
			{
				float num = (float)this.m_targetUnit.CurHP / (float)this.m_targetUnit.MaxHP;
				float num2 = (float)this.m_targetUnit.CurMP / (float)this.m_targetUnit.MaxMP;
				if (num != this.m_spriteHP.fillAmount)
				{
					this.m_spriteHP.fillAmount = num;
				}
				if (num2 != this.m_spriteMP.fillAmount)
				{
					this.m_spriteMP.fillAmount = num2;
				}
				if (this.m_playerHpMp != null)
				{
					this.m_playerHpMp.transform.localScale = Vector3.one * this.m_scale;
					this.m_playerHpMp.transform.position = position;
				}
			}
			else
			{
				float num3 = (float)this.m_targetUnit.CurHP / (float)this.m_targetUnit.MaxHP;
				if (num3 != this.hpbar.value)
				{
					this.hpbar.value = num3;
				}
				if (this.headTip != null)
				{
					this.headTip.transform.localScale = Vector3.one * this.m_scale;
					this.headTip.transform.position = position;
				}
			}
		}
	}
}
