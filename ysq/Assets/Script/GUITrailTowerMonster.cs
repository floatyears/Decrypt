using Att;
using System;
using UnityEngine;

public class GUITrailTowerMonster : MonoBehaviour
{
	private UISprite mMonsterIcon;

	private UISprite mQualityMask;

	private GameObject mBossMark;

	private MonsterInfo mMonsterInfo;

	private GameUIToolTip mToolTips;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mMonsterIcon = base.transform.Find("icon").GetComponent<UISprite>();
		this.mQualityMask = base.transform.Find("qualityMask").GetComponent<UISprite>();
		this.mBossMark = base.transform.Find("mark").gameObject;
		UIEventListener expr_5C = UIEventListener.Get(base.gameObject);
		expr_5C.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_5C.onPress, new UIEventListener.BoolDelegate(this.OnIconPress));
	}

	public void Refresh(MonsterInfo mInfo)
	{
		this.mMonsterInfo = mInfo;
		if (this.mMonsterInfo != null)
		{
			base.gameObject.SetActive(true);
			this.mMonsterIcon.spriteName = this.mMonsterInfo.Icon;
			this.mQualityMask.spriteName = Tools.GetItemQualityIcon(this.mMonsterInfo.Quality);
			this.mBossMark.SetActive(this.mMonsterInfo.BossType != 0);
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnIconPress(GameObject go, bool isPressed)
	{
		if (this.mMonsterInfo != null)
		{
			if (isPressed)
			{
				if (this.mToolTips == null)
				{
					this.mToolTips = GameUIToolTipManager.GetInstance().CreateMonsterTooltip(go.transform, this.mMonsterInfo);
				}
				this.mToolTips.Create(Tools.GetCameraRootParent(go.transform), this.mMonsterInfo.Name, this.mMonsterInfo.Desc, string.Empty);
				this.mToolTips.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(36f, this.mToolTips.transform.localPosition.y, -7000f));
				this.mToolTips.EnableToolTip();
			}
			else if (this.mToolTips != null)
			{
				this.mToolTips.HideTipAnim();
			}
		}
	}
}
