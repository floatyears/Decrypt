using Proto;
using System;
using UnityEngine;

public class GUIGuildMagicItem : MonoBehaviour
{
	private GUIGuildMagicPopUp mBaseScene;

	private int mIndex;

	private UILabel mCostNum;

	private UILabel mStateTip;

	private GameObject mDoneTip;

	private GameObject mFanBeiGo;

	private UILabel mFanBeiDesc;

	public void InitWithBaseScene(GUIGuildMagicPopUp baseScene, int index)
	{
		this.mBaseScene = baseScene;
		this.mIndex = index;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mCostNum = base.transform.Find("Price").GetComponent<UILabel>();
		this.mStateTip = base.transform.Find("stateTip").GetComponent<UILabel>();
		this.mDoneTip = base.transform.Find("done").gameObject;
		this.mFanBeiGo = base.transform.Find("Tag").gameObject;
		this.mFanBeiDesc = this.mFanBeiGo.transform.Find("Label").GetComponent<UILabel>();
		UIEventListener expr_97 = UIEventListener.Get(base.gameObject);
		expr_97.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_97.onClick, new UIEventListener.VoidDelegate(this.OnItemClick));
	}

	private bool IsCanGuildSign(bool showMsg = false)
	{
		bool flag = false;
		for (int i = 0; i < Globals.Instance.Player.GuildSystem.Members.Count; i++)
		{
			GuildMember guildMember = Globals.Instance.Player.GuildSystem.Members[i];
			if (guildMember.ID == Globals.Instance.Player.Data.ID)
			{
				flag = ((guildMember.Flag & 4) == 0);
			}
		}
		if (!flag && showMsg)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("guild15", 0f, 0f);
		}
		return flag;
	}

	public void Refresh()
	{
		int num = this.mIndex;
		if (num != 1)
		{
			if (num != 2)
			{
				this.mCostNum.text = GameConst.GetInt32(154).ToString();
				this.mCostNum.color = ((Globals.Instance.Player.Data.Money < GameConst.GetInt32(154)) ? Color.red : Color.white);
				this.mStateTip.text = Singleton<StringManager>.Instance.GetString("guild29", new object[]
				{
					1
				});
				this.mDoneTip.SetActive((Globals.Instance.Player.Data.DataFlag & 67108864) != 0);
			}
			else
			{
				this.mCostNum.text = GameConst.GetInt32(164).ToString();
				this.mCostNum.color = ((Globals.Instance.Player.Data.Diamond < GameConst.GetInt32(164)) ? Color.red : Color.white);
				this.mStateTip.text = Singleton<StringManager>.Instance.GetString("guild29", new object[]
				{
					5
				});
				this.mDoneTip.SetActive((Globals.Instance.Player.Data.DataFlag & 268435456) != 0);
			}
		}
		else
		{
			this.mCostNum.text = GameConst.GetInt32(159).ToString();
			this.mCostNum.color = ((Globals.Instance.Player.Data.Diamond < GameConst.GetInt32(159)) ? Color.red : Color.white);
			this.mStateTip.text = Singleton<StringManager>.Instance.GetString("guild29", new object[]
			{
				3
			});
			this.mDoneTip.SetActive((Globals.Instance.Player.Data.DataFlag & 134217728) != 0);
		}
		ActivityValueData valueMod = Globals.Instance.Player.ActivitySystem.GetValueMod(8);
		if (valueMod != null)
		{
			this.mFanBeiGo.SetActive(true);
			this.mFanBeiDesc.text = Singleton<StringManager>.Instance.GetString("ShopCommon6");
		}
		else
		{
			this.mFanBeiGo.SetActive(false);
		}
	}

	private void OnItemClick(GameObject go)
	{
		if (this.IsCanGuildSign(true))
		{
			this.mBaseScene.ShowTipPopUp(this.mIndex);
		}
	}
}
