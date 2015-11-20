using Att;
using System;
using UnityEngine;

public class RewardMoney : MonoBehaviour
{
	private int mMoneyNum;

	private GameUIToolTip mToolTip;

	private Vector3 tipsPosition;

	private int qualityLvl;

	private ERewardType type;

	public void Init(int num, bool showValue, bool showTips, float x, float y, float z, int quality = 0, ERewardType type = ERewardType.EReward_Money)
	{
		this.type = type;
		UISprite uISprite = GameUITools.FindUISprite("icon", base.gameObject);
		uISprite.spriteName = Tools.GetRewardIcon(type);
		this.mMoneyNum = num;
		this.tipsPosition = new Vector3(x, y, z);
		UILabel component = base.transform.Find("num").GetComponent<UILabel>();
		component.text = Tools.FormatCurrency(num);
		if (!showValue)
		{
			component.gameObject.SetActive(false);
		}
		if (showTips)
		{
			UIEventListener expr_83 = UIEventListener.Get(base.gameObject);
			expr_83.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_83.onPress, new UIEventListener.BoolDelegate(this.OnRewardPress));
			if (quality == 0 && type != ERewardType.EReward_Money)
			{
				quality = 2;
			}
			this.qualityLvl = quality;
		}
		UISprite component2 = base.transform.Find("QualityMark").GetComponent<UISprite>();
		if (component2 != null)
		{
			component2.spriteName = Tools.GetRewardFrame(type);
			component2.gameObject.SetActive(true);
		}
	}

	private void OnRewardPress(GameObject go, bool isPressed)
	{
		this.RewardPressHandle(go, isPressed, ref this.mToolTip, this.mMoneyNum, this.tipsPosition);
	}

	private void RewardPressHandle(GameObject go, bool isPressed, ref GameUIToolTip toolTip, int money, Vector3 pos)
	{
		if (isPressed)
		{
			if (toolTip == null)
			{
				toolTip = GameUIToolTipManager.GetInstance().CreateBasicTooltip(go.transform, string.Empty, string.Empty);
			}
			string text = string.Format("{0}{1}", money, Tools.GetRewardTypeName(this.type, 0));
			string description = string.Format(Singleton<StringManager>.Instance.GetString("take"), text);
			toolTip.Create(Tools.GetCameraRootParent(go.transform), text, description, this.qualityLvl);
			toolTip.transform.localPosition = Tools.GetRelativePos(go.transform, GameUIManager.mInstance.uiCamera.transform, new Vector3(pos.x, toolTip.transform.localPosition.y + pos.y, pos.z - 5000f));
			toolTip.EnableToolTip();
		}
		else if (toolTip != null)
		{
			toolTip.HideTipAnim();
		}
	}
}
