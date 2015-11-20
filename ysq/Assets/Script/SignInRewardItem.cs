using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class SignInRewardItem : MonoBehaviour
{
	private GameObject flare;

	private UISprite dark;

	private UISprite sprite;

	private UISprite stamp;

	private UIWidget maskColor;

	private GUISignIn baseScene;

	private SignInInfo signInInfo;

	private GameUIToolTip mToolTip;

	private StringBuilder mStringBuilder = new StringBuilder();

	public void InitItem(GUISignIn baseScene, SignInInfo info)
	{
		this.baseScene = baseScene;
		this.signInInfo = info;
		this.dark = GameUITools.FindUISprite("dark", base.gameObject);
		this.sprite = GameUITools.FindUISprite("Sprite", base.gameObject);
		this.stamp = GameUITools.FindUISprite("stamp", base.gameObject);
		this.flare = GameUITools.FindGameObject("flare", base.gameObject);
		UILabel uILabel = GameUITools.FindUILabel("num", base.gameObject);
		UISprite uISprite = GameUITools.FindUISprite("Tag", base.gameObject);
		UILabel uILabel2 = GameUITools.FindUILabel("Tag/vipLevel", base.gameObject);
		UILabel uILabel3 = GameUITools.FindUILabel("times", base.gameObject);
		GameObject gameObject = GameUITools.FindGameObject("slot", base.gameObject);
		if (info.RewardType == 3 || info.RewardType == 4)
		{
			uILabel.text = Singleton<StringManager>.Instance.GetString("signInRewardNum", new object[]
			{
				this.signInInfo.RewardValue2
			});
		}
		else
		{
			uILabel.text = Singleton<StringManager>.Instance.GetString("signInRewardNum", new object[]
			{
				this.signInInfo.RewardValue1
			});
		}
		uILabel3.text = this.signInInfo.ID.ToString();
		if (this.signInInfo.VipLevel > 0)
		{
			uILabel2.text = Singleton<StringManager>.Instance.GetString("signInVipLevel", new object[]
			{
				this.signInInfo.VipLevel
			});
			uISprite.gameObject.SetActive(true);
		}
		GameObject gameObject2 = GameUITools.CreateReward(this.signInInfo.RewardType, this.signInInfo.RewardValue1, this.signInInfo.RewardValue2, gameObject.transform, false, false, 36f, -7f, -2000f, 20f, 13f, 7f, 0);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Sign In Reward Init Error , rewardItem is null , {0}", this.signInInfo.ID)
			});
			return;
		}
		this.InitSubReward(gameObject2);
		this.RefreshItem();
	}

	public void InitSubReward(GameObject obj)
	{
		obj.AddComponent<UIDragScrollView>().scrollView = this.baseScene.mRewardScrollView;
		UIButtonScale uIButtonScale = obj.AddComponent<UIButtonScale>();
		uIButtonScale.hover = new Vector3(1f, 1f, 1f);
		uIButtonScale.pressed = new Vector3(0.98f, 0.98f, 0.98f);
		if (this.signInInfo.RewardType == 3 && Globals.Instance.AttDB.ItemDict.GetInfo(this.signInInfo.RewardValue1) == null)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("ItemDict.GetInfo, ID = {0}", this.signInInfo.RewardValue1)
			});
			return;
		}
		UIEventListener expr_B7 = UIEventListener.Get(obj);
		expr_B7.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_B7.onPress, new UIEventListener.BoolDelegate(this.OnRewardPress));
		UIEventListener expr_DE = UIEventListener.Get(obj);
		expr_DE.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_DE.onClick, new UIEventListener.VoidDelegate(this.RewardItemClick));
	}

	private void OnRewardPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			if (this.CheckSignInState() == 2)
			{
				return;
			}
			if (this.mToolTip == null)
			{
				this.mToolTip = GameUIToolTipManager.GetInstance().CreateSignInRewardTooltip(go.transform, string.Empty, string.Empty);
			}
			int rewardType = this.signInInfo.RewardType;
			if (rewardType != 1)
			{
				if (rewardType == 2)
				{
					this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
					this.mStringBuilder.Append(this.signInInfo.RewardValue1).Append(Singleton<StringManager>.Instance.GetString("diamond"));
					string title = this.mStringBuilder.ToString();
					string description = string.Format(Singleton<StringManager>.Instance.GetString("takeDiamond"), this.signInInfo.RewardValue1);
					this.mToolTip.CreateSignInToolTip(go.transform, title, description, this.signInInfo.ID, 0, 2, this.baseScene.mRewardScrollView.transform.parent);
					this.mToolTip.EnableToolTip();
				}
			}
			else
			{
				this.mStringBuilder.Remove(0, this.mStringBuilder.Length);
				this.mStringBuilder.Append(this.signInInfo.RewardValue1).Append(Singleton<StringManager>.Instance.GetString("money"));
				string title2 = this.mStringBuilder.ToString();
				string description2 = string.Format(Singleton<StringManager>.Instance.GetString("takeMoney"), this.signInInfo.RewardValue1);
				this.mToolTip.CreateSignInToolTip(go.transform, title2, description2, this.signInInfo.ID, 0, 0, this.baseScene.mRewardScrollView.transform.parent);
				this.mToolTip.EnableToolTip();
			}
		}
		else if (this.mToolTip != null)
		{
			this.mToolTip.HideTipAnim();
		}
	}

	private void RewardItemClick(GameObject obj)
	{
		int num = this.CheckSignInState();
		if (num != 2)
		{
			switch (this.signInInfo.RewardType)
			{
			case 3:
				Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
				GameUIManager.mInstance.ShowItemInfo(Globals.Instance.AttDB.ItemDict.GetInfo(this.signInInfo.RewardValue1));
				break;
			case 4:
				Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
				GameUIManager.mInstance.ShowPetInfo(Globals.Instance.AttDB.PetDict.GetInfo(this.signInInfo.RewardValue1));
				break;
			}
		}
		else
		{
			this.SignIn();
		}
	}

	public void RefreshItem()
	{
		switch (this.CheckSignInState())
		{
		case 0:
			this.dark.gameObject.SetActive(true);
			this.stamp.gameObject.SetActive(true);
			this.sprite.gameObject.SetActive(false);
			this.flare.SetActive(false);
			break;
		case 1:
			this.sprite.spriteName = "Retroactive";
			this.sprite.gameObject.SetActive(true);
			if (this.signInInfo.RewardType == 4)
			{
				this.baseScene.FlareList.Add(this.flare);
			}
			break;
		case 2:
			this.sprite.spriteName = "today";
			this.sprite.gameObject.SetActive(true);
			if (this.signInInfo.RewardType == 4)
			{
				this.baseScene.FlareList.Add(this.flare);
			}
			break;
		case 3:
			if (this.signInInfo.RewardType == 4)
			{
				this.baseScene.FlareList.Add(this.flare);
			}
			break;
		}
	}

	private int CheckSignInState()
	{
		if (this.signInInfo.ID <= Globals.Instance.Player.Data.SignIn)
		{
			return 0;
		}
		if (Globals.Instance.Player.GetTimeStamp() >= Globals.Instance.Player.Data.SignInTimeStamp && this.signInInfo.ID == Globals.Instance.Player.Data.SignIn + 1)
		{
			return 2;
		}
		return 3;
	}

	private void SignIn()
	{
		MC2S_SignIn mC2S_SignIn = new MC2S_SignIn();
		mC2S_SignIn.Index = Globals.Instance.Player.Data.SignIn + 1;
		Globals.Instance.CliSession.Send(226, mC2S_SignIn);
	}
}
