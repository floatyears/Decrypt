using Att;
using System;
using UnityEngine;

public class UIMasterItem : MonoBehaviour
{
	private GameUIToolTip mToolTip;

	private bool mShowTips;

	private UISprite icon;

	private UISprite propertyBg;

	public void Init(MonsterInfo info, bool showTips)
	{
		if (info == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.mShowTips = showTips;
		this.icon = base.transform.FindChild("icon").GetComponent<UISprite>();
		if (showTips)
		{
			if (this.icon.collider != null)
			{
				this.icon.collider.enabled = true;
			}
			UIEventListener expr_72 = UIEventListener.Get(this.icon.gameObject);
			expr_72.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_72.onPress, new UIEventListener.BoolDelegate(this.OnEnemyItemPress));
		}
		else if (this.icon.collider != null)
		{
			this.icon.collider.enabled = false;
		}
		this.propertyBg = base.transform.GetComponent<UISprite>();
		this.Refresh(info);
	}

	private void Refresh(MonsterInfo info)
	{
		this.propertyBg.spriteName = Tools.GetItemQualityIcon(info.Quality);
		this.icon.spriteName = info.Icon;
		if (this.mShowTips)
		{
			this.icon.gameObject.name = info.ID.ToString();
		}
	}

	private void OnEnemyItemPress(GameObject go, bool isPressed)
	{
		int id;
		if (!int.TryParse(go.name, out id))
		{
			return;
		}
		MonsterInfo info = Globals.Instance.AttDB.MonsterDict.GetInfo(id);
		if (isPressed)
		{
			if (this.mToolTip == null)
			{
				this.mToolTip = GameUIToolTipManager.GetInstance().CreateMonsterTooltip(go.transform, info);
			}
			this.mToolTip.Create(Tools.GetCameraRootParent(go.transform), info.Name, info.Desc, string.Empty);
			this.mToolTip.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(96f, this.mToolTip.transform.localPosition.y + (float)this.propertyBg.height * (this.propertyBg.transform.parent.localScale.x - 1f) / 4f, -7000f));
			this.mToolTip.EnableToolTip();
		}
		else if (this.mToolTip != null)
		{
			this.mToolTip.HideTipAnim();
		}
	}
}
