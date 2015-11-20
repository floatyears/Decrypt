using Att;
using Proto;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIRightInfo : MonoBehaviour
{
	private UILabel mTitle;

	private UILabel mInfo;

	private UILabel mInfoNum;

	private UILabel mBook;

	private UILabel mBookNum;

	private UILabel mLightTxt;

	public static UIButton mLightBtn;

	private GUILeftInfo mLeftInfo;

	private ConstellationInfo mConstellationInfo;

	private GUIConstellationScene mBaseScene;

	private GameObject mEffect;

	private List<string> tempStrs = new List<string>();

	public void InitWithBaseScene(GUIConstellationScene basescene)
	{
		this.mBaseScene = basescene;
		this.CreateObjects();
	}

	public void CreateObjects()
	{
		this.mTitle = base.transform.Find("title").GetComponent<UILabel>();
		this.mInfo = base.transform.Find("info").GetComponent<UILabel>();
		this.mInfoNum = this.mInfo.transform.Find("num").GetComponent<UILabel>();
		this.mBook = base.transform.Find("book").GetComponent<UILabel>();
		this.mBookNum = this.mBook.transform.Find("num").GetComponent<UILabel>();
		GUIRightInfo.mLightBtn = base.transform.Find("lightBtn").GetComponent<UIButton>();
		this.mLightTxt = GUIRightInfo.mLightBtn.transform.Find("lightTxt").GetComponent<UILabel>();
		UIEventListener expr_D9 = UIEventListener.Get(GUIRightInfo.mLightBtn.gameObject);
		expr_D9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_D9.onClick, new UIEventListener.VoidDelegate(this.OnLightBtnClicked));
		this.mEffect = GameUITools.FindGameObject("Effect", GUIRightInfo.mLightBtn.gameObject);
		this.mEffect.gameObject.SetActive(false);
	}

	public void Refresh(int conLv)
	{
		this.mLightTxt.text = Singleton<StringManager>.Instance.GetString("XingZuo16");
		ConstellationInfo info = Globals.Instance.AttDB.ConstellationDict.GetInfo(GUIXingZuoPage.mXingZuo);
		ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(info.Value1[4]);
		if (conLv == 0)
		{
			this.mTitle.text = Singleton<StringManager>.Instance.GetString(string.Format("XingWei{0}", conLv + 1));
			this.mInfo.text = Singleton<StringManager>.Instance.GetString("XingZuo13");
			this.mInfoNum.text = Singleton<StringManager>.Instance.GetString("XingZuo7", new object[]
			{
				info.Value2[0]
			});
			this.mBook.text = Singleton<StringManager>.Instance.GetString("XingZuo9");
			if (Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(103)) < GUIRightInfo.GetCost())
			{
				this.mBookNum.text = Singleton<StringManager>.Instance.GetString("XingZuo10", new object[]
				{
					Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(103)),
					GUIRightInfo.GetCost()
				});
				this.mBookNum.color = Color.red;
			}
			else
			{
				this.mBookNum.text = Singleton<StringManager>.Instance.GetString("XingZuo10", new object[]
				{
					Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(103)),
					GUIRightInfo.GetCost()
				});
			}
		}
		if (conLv > 0 && conLv < 60)
		{
			this.mTitle.text = Singleton<StringManager>.Instance.GetString(string.Format("XingWei{0}", conLv % 5 + 1));
			if ((conLv - 1) % 5 + 1 == 5)
			{
				this.mInfo.text = Singleton<StringManager>.Instance.GetString("XingZuo13");
				this.mInfoNum.transform.localPosition = new Vector3(170f, 0f, 0f);
				ConstellationInfo info3 = Globals.Instance.AttDB.ConstellationDict.GetInfo((conLv + 4) / 5 + 1);
				this.mInfoNum.text = Singleton<StringManager>.Instance.GetString("XingZuo7", new object[]
				{
					info3.Value2[0]
				});
			}
			if ((conLv - 1) % 5 + 1 == 1)
			{
				this.mInfoNum.transform.localPosition = new Vector3(170f, 0f, 0f);
				this.mInfo.text = Singleton<StringManager>.Instance.GetString("XingZuo14");
				this.mInfoNum.text = Singleton<StringManager>.Instance.GetString("XingZuo7", new object[]
				{
					info.Value2[1]
				});
			}
			if ((conLv - 1) % 5 + 1 == 2)
			{
				this.mInfoNum.transform.localPosition = new Vector3(170f, 0f, 0f);
				this.mInfo.text = Singleton<StringManager>.Instance.GetString("XingZuo15");
				this.mInfoNum.text = Singleton<StringManager>.Instance.GetString("XingZuo7", new object[]
				{
					info.Value2[2]
				});
			}
			if ((conLv - 1) % 5 + 1 == 3)
			{
				this.mInfoNum.transform.localPosition = new Vector3(170f, 0f, 0f);
				this.mInfo.text = Singleton<StringManager>.Instance.GetString("XingZuo8");
				this.mInfoNum.text = Singleton<StringManager>.Instance.GetString("XingZuo7", new object[]
				{
					info.Value2[3]
				});
			}
			if ((conLv - 1) % 5 + 1 == 4)
			{
				this.mInfo.text = Singleton<StringManager>.Instance.GetString("XingZuo29");
				if (info.Type[info.Type.Count - 1] == 1)
				{
					this.mInfoNum.transform.localPosition = new Vector3(78f, 0f, 0f);
					this.mInfoNum.text = Singleton<StringManager>.Instance.GetString("XingZuo21", new object[]
					{
						info2.Name,
						info.Value2[4]
					});
				}
				if (GUIXingZuoPage.mXingZuo == 2)
				{
					this.mInfoNum.transform.localPosition = new Vector3(78f, 0f, 0f);
					this.mInfoNum.text = Singleton<StringManager>.Instance.GetString("XingZuo40");
				}
				if (GUIXingZuoPage.mXingZuo == 6)
				{
					this.mInfoNum.transform.localPosition = new Vector3(78f, 0f, 0f);
					this.mInfoNum.text = Singleton<StringManager>.Instance.GetString("XingZuo41");
				}
			}
			int constellationLevel = Globals.Instance.Player.Data.ConstellationLevel;
			if (constellationLevel >= 60)
			{
				GameUIManager.mInstance.ShowMessageTipByKey("XingZuo22", 0f, 0f);
			}
			if (ConLevelInfo.GetConInfo(constellationLevel + 1) == null)
			{
				return;
			}
			GUIRightInfo.RefreshLightBtn(conLv);
			this.mBook.text = Singleton<StringManager>.Instance.GetString("XingZuo9");
			if (Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(103)) < GUIRightInfo.GetCost())
			{
				this.mBookNum.text = Singleton<StringManager>.Instance.GetString("XingZuo10", new object[]
				{
					Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(103)),
					GUIRightInfo.GetCost()
				});
				this.mBookNum.color = Color.red;
			}
			else
			{
				this.mBookNum.text = Singleton<StringManager>.Instance.GetString("XingZuo10", new object[]
				{
					Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(103)),
					GUIRightInfo.GetCost()
				});
			}
		}
		if (conLv == 60)
		{
			this.mTitle.text = Singleton<StringManager>.Instance.GetString(string.Format("XingWei{0}", 5));
			this.mInfo.text = Singleton<StringManager>.Instance.GetString("XingZuo29");
			this.mInfoNum.transform.localPosition = new Vector3(78f, 1f, 0f);
			this.mInfoNum.text = Singleton<StringManager>.Instance.GetString("XingZuo21", new object[]
			{
				info2.Name,
				info.Value2[4]
			});
			this.mBookNum.text = Singleton<StringManager>.Instance.GetString("XingZuo34");
		}
		this.RefreshEffect();
	}

	public static void RefreshLightBtn(int conLv)
	{
		if (conLv == 0)
		{
			if (GUIXingZuoPage.mXingZuo == 1)
			{
				GUIRightInfo.mLightBtn.isEnabled = true;
				Tools.SetButtonState(GUIRightInfo.mLightBtn.gameObject, true);
			}
			else
			{
				GUIRightInfo.mLightBtn.isEnabled = false;
				Tools.SetButtonState(GUIRightInfo.mLightBtn.gameObject, false);
			}
		}
		int num = (conLv + 4) / 5;
		int num2 = (conLv - 1) % 5 + 1;
		if (conLv > 0 && conLv < 60)
		{
			if (num2 < 5)
			{
				if (GUIXingZuoPage.mXingZuo == num)
				{
					GUIRightInfo.mLightBtn.isEnabled = true;
					Tools.SetButtonState(GUIRightInfo.mLightBtn.gameObject, true);
				}
				if (GUIXingZuoPage.mXingZuo < num)
				{
					GUIRightInfo.mLightBtn.isEnabled = false;
					Tools.SetButtonState(GUIRightInfo.mLightBtn.gameObject, false);
				}
				if (GUIXingZuoPage.mXingZuo > num)
				{
					GUIRightInfo.mLightBtn.isEnabled = false;
					Tools.SetButtonState(GUIRightInfo.mLightBtn.gameObject, false);
				}
			}
			if (num2 == 5)
			{
				GUIRightInfo.mLightBtn.isEnabled = false;
				Tools.SetButtonState(GUIRightInfo.mLightBtn.gameObject, false);
				if (GUIXingZuoPage.mXingZuo == num + 1)
				{
					GUIRightInfo.mLightBtn.isEnabled = true;
					Tools.SetButtonState(GUIRightInfo.mLightBtn.gameObject, true);
				}
				if (GUIXingZuoPage.mXingZuo < num + 1)
				{
					GUIRightInfo.mLightBtn.isEnabled = false;
					Tools.SetButtonState(GUIRightInfo.mLightBtn.gameObject, false);
				}
				if (GUIXingZuoPage.mXingZuo > num + 1)
				{
					GUIRightInfo.mLightBtn.isEnabled = false;
					Tools.SetButtonState(GUIRightInfo.mLightBtn.gameObject, false);
				}
			}
		}
		if (conLv == 60)
		{
			if (GUIXingZuoPage.mXingZuo == 12)
			{
				GUIRightInfo.mLightBtn.isEnabled = true;
				Tools.SetButtonState(GUIRightInfo.mLightBtn.gameObject, true);
			}
			else
			{
				GUIRightInfo.mLightBtn.isEnabled = false;
				Tools.SetButtonState(GUIRightInfo.mLightBtn.gameObject, false);
			}
		}
	}

	private void RefreshEffect()
	{
		NGUITools.SetActive(this.mEffect, Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(103)) >= GUIRightInfo.GetCost() && (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(7)));
	}

	public static int GetCost()
	{
		int constellationLevel = Globals.Instance.Player.Data.ConstellationLevel;
		if (constellationLevel >= 60)
		{
			return 0;
		}
		ConInfo conInfo = ConLevelInfo.GetConInfo(constellationLevel + 1);
		if (conInfo == null)
		{
			return 0;
		}
		return conInfo.Cost;
	}

	public void ShowAttributeAni(int conLv)
	{
		this.tempStrs.Clear();
		ConstellationInfo info = Globals.Instance.AttDB.ConstellationDict.GetInfo(GUIXingZuoPage.mXingZuo);
		ItemInfo info2 = Globals.Instance.AttDB.ItemDict.GetInfo(info.Value1[4]);
		if (conLv > 0 && conLv % 5 != 0)
		{
			if ((conLv - 1) % 5 + 1 == 5)
			{
				if (info.Type[info.Type.Count - 1] == 1)
				{
					this.tempStrs.Add(Singleton<StringManager>.Instance.GetString("XingZuo21", new object[]
					{
						info2.Name,
						info.Value2[4]
					}));
					GameUIManager.mInstance.ShowAttributeTip(this.tempStrs, 2f, 0.4f, 0f, 200f);
				}
				if (GUIXingZuoPage.mXingZuo == 2)
				{
					GameUIManager.mInstance.ShowAttributeTip(null, 2f, 0.4f, 0f, 200f);
				}
				if (GUIXingZuoPage.mXingZuo == 6)
				{
					GameUIManager.mInstance.ShowAttributeTip(null, 2f, 0.4f, 0f, 200f);
				}
			}
			if ((conLv - 1) % 5 + 1 == 1)
			{
				this.tempStrs.Add(Singleton<StringManager>.Instance.GetString("XingZuo30", new object[]
				{
					info.Value2[0]
				}));
				GameUIManager.mInstance.ShowAttributeTip(this.tempStrs, 2f, 0.4f, 0f, 200f);
			}
			if ((conLv - 1) % 5 + 1 == 2)
			{
				this.tempStrs.Add(Singleton<StringManager>.Instance.GetString("XingZuo31", new object[]
				{
					info.Value2[1]
				}));
				GameUIManager.mInstance.ShowAttributeTip(this.tempStrs, 2f, 0.4f, 0f, 200f);
			}
			if ((conLv - 1) % 5 + 1 == 3)
			{
				this.tempStrs.Add(Singleton<StringManager>.Instance.GetString("XingZuo32", new object[]
				{
					info.Value2[2]
				}));
				GameUIManager.mInstance.ShowAttributeTip(this.tempStrs, 2f, 0.4f, 0f, 200f);
			}
			if ((conLv - 1) % 5 + 1 == 4)
			{
				this.tempStrs.Add(Singleton<StringManager>.Instance.GetString("XingZuo33", new object[]
				{
					info.Value2[3]
				}));
				GameUIManager.mInstance.ShowAttributeTip(this.tempStrs, 2f, 0.4f, 0f, 200f);
			}
		}
	}

	public void OnLightBtnClicked(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		PetDataEx pet = Globals.Instance.Player.TeamSystem.GetPet(0);
		int maxHp = 0;
		int attack = 0;
		int wuFang = 0;
		int faFang = 0;
		pet.GetAttribute(ref maxHp, ref attack, ref wuFang, ref faFang);
		GameUIManager.mInstance.uiState.MaxHp = maxHp;
		GameUIManager.mInstance.uiState.Attack = attack;
		GameUIManager.mInstance.uiState.WuFang = wuFang;
		GameUIManager.mInstance.uiState.FaFang = faFang;
		int constellationLevel = Globals.Instance.Player.Data.ConstellationLevel;
		ConInfo conInfo = ConLevelInfo.GetConInfo(constellationLevel + 1);
		if (constellationLevel < 60)
		{
			this.mBaseScene.mGUIXingZuoPage.mLeftBtn.GetComponent<BoxCollider>().enabled = false;
			this.mBaseScene.mGUIXingZuoPage.mRightBtn.GetComponent<BoxCollider>().enabled = false;
			base.StartCoroutine(this.mBaseScene.mGUIXingZuoPage.WaitChangePageTimeLeft());
			base.StartCoroutine(this.mBaseScene.mGUIXingZuoPage.WaitChangePageTimeRight());
			if (conInfo == null)
			{
				return;
			}
			if (Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(103)) < GUIRightInfo.GetCost())
			{
				GameUIManager.mInstance.ShowMessageTipByKey("XingZuo12", 0f, 0f);
			}
		}
		if (constellationLevel >= 60)
		{
			GameUIManager.mInstance.ShowMessageTipByKey("XingZuo22", 0f, 0f);
		}
		if (Globals.Instance.Player.ItemSystem.GetItemCount(GameConst.GetInt32(103)) >= GUIRightInfo.GetCost() && constellationLevel < 60)
		{
			MC2S_ConstellationLevelup ojb = new MC2S_ConstellationLevelup();
			Globals.Instance.CliSession.Send(203, ojb);
		}
	}
}
