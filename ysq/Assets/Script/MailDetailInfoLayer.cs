using Att;
using Holoville.HOTween.Core;
using Proto;
using System;
using UnityEngine;

public class MailDetailInfoLayer : MonoBehaviour
{
	private GUIMailScene mBaseScene;

	private MailData mMailData;

	private GameObject mBg;

	private UIScrollView mScrollView;

	private MailDetailContentTable mInfoTable;

	private UIScrollBar mScrollBar;

	private UILabel mBtnTxt;

	private MailTitle mMailTitle;

	private MailContents mMainDetailInfo;

	private MailSender mMailSender;

	private SpriteLine mSpriteLine;

	private MailAffixGoods mCurUseAffixGoods;

	private int mElementPriority = 10;

	public void InitWithBaseScene(GUIMailScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObjects();
		base.gameObject.SetActive(false);
	}

	private void CreateObjects()
	{
		this.mBg = base.transform.Find("Bg").gameObject;
		this.mScrollView = this.mBg.transform.Find("infoPanel").GetComponent<UIScrollView>();
		this.mInfoTable = this.mScrollView.transform.Find("infoContents").gameObject.AddComponent<MailDetailContentTable>();
		this.mInfoTable.columns = 1;
		this.mInfoTable.direction = UITable.Direction.Down;
		this.mInfoTable.sorting = UITable.Sorting.Custom;
		this.mInfoTable.hideInactive = true;
		this.mInfoTable.keepWithinPanel = true;
		this.mInfoTable.padding = new Vector2(0f, 0f);
		this.mScrollBar = this.mBg.transform.Find("infoScrollBar").GetComponent<UIScrollBar>();
		this.mMailTitle = this.mInfoTable.transform.Find("Title").gameObject.AddComponent<MailTitle>();
		this.mMainDetailInfo = this.mInfoTable.transform.Find("infoLabel").gameObject.AddComponent<MailContents>();
		this.mMailSender = this.mInfoTable.transform.Find("sender").gameObject.AddComponent<MailSender>();
		this.mSpriteLine = this.mInfoTable.transform.Find("line").gameObject.AddComponent<SpriteLine>();
		GameObject gameObject = base.transform.Find("MailContentBG").gameObject;
		UIEventListener expr_18B = UIEventListener.Get(gameObject.gameObject);
		expr_18B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_18B.onClick, new UIEventListener.VoidDelegate(this.OnBackGroundClick));
		GameObject gameObject2 = this.mBg.transform.Find("takeBtn").gameObject;
		UIEventListener expr_1CD = UIEventListener.Get(gameObject2);
		expr_1CD.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1CD.onClick, new UIEventListener.VoidDelegate(this.OnTakeAffixClick));
		this.mBtnTxt = gameObject2.transform.Find("Label").GetComponent<UILabel>();
	}

	public void SetMailData(MailData mailData)
	{
		this.mMailData = mailData;
	}

	public void EnableMailDetailInfo(bool isEnable)
	{
		if (isEnable)
		{
			base.gameObject.SetActive(true);
			this.Refresh();
			GameUITools.PlayOpenWindowAnim(this.mBg.transform, null, true);
		}
		else
		{
			GameUITools.PlayCloseWindowAnim(this.mBg.transform, new TweenDelegate.TweenCallback(this.OnEndAnimComplete), true);
		}
	}

	private bool HasAffix()
	{
		bool result = false;
		for (int i = 0; i <= 17; i++)
		{
			if (this.HasAffix((EAffixType)i))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private bool HasAffix(EAffixType affixType)
	{
		bool result = false;
		for (int i = 0; i < this.mMailData.AffixType.Count; i++)
		{
			if (this.mMailData.AffixType[i] == (int)affixType)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private void AddAffixItem(int itemId, int itemNum)
	{
		if (this.mCurUseAffixGoods == null || this.mCurUseAffixGoods.GetAffixEmptyIndex() == -1)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/mailAffixGoods");
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = this.mMainDetailInfo.transform.parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.AddComponent<UIDragScrollView>().scrollView = this.mScrollView;
			this.mCurUseAffixGoods = gameObject.AddComponent<MailAffixGoods>();
			this.mCurUseAffixGoods.ElementPriority = this.mElementPriority++;
			this.mCurUseAffixGoods.InitWithBaseScene(this.mBaseScene);
		}
		if (this.mCurUseAffixGoods != null && this.mCurUseAffixGoods.GetAffixEmptyIndex() != -1)
		{
			ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(itemId);
			if (info != null)
			{
				this.mCurUseAffixGoods.AddAffixItem(info, itemNum);
			}
		}
	}

	private void AddAffixPet(int petId)
	{
		if (this.mCurUseAffixGoods == null || this.mCurUseAffixGoods.GetAffixEmptyIndex() == -1)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/mailAffixGoods");
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = this.mMainDetailInfo.transform.parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.AddComponent<UIDragScrollView>().scrollView = this.mScrollView;
			this.mCurUseAffixGoods = gameObject.AddComponent<MailAffixGoods>();
			this.mCurUseAffixGoods.ElementPriority = this.mElementPriority++;
			this.mCurUseAffixGoods.InitWithBaseScene(this.mBaseScene);
		}
		if (this.mCurUseAffixGoods != null && this.mCurUseAffixGoods.GetAffixEmptyIndex() != -1)
		{
			PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(petId);
			if (info != null)
			{
				this.mCurUseAffixGoods.AddAffixPet(info);
			}
		}
	}

	private void AddAffixLopet(int petId)
	{
		if (this.mCurUseAffixGoods == null || this.mCurUseAffixGoods.GetAffixEmptyIndex() == -1)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/mailAffixGoods");
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = this.mMainDetailInfo.transform.parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.AddComponent<UIDragScrollView>().scrollView = this.mScrollView;
			this.mCurUseAffixGoods = gameObject.AddComponent<MailAffixGoods>();
			this.mCurUseAffixGoods.ElementPriority = this.mElementPriority++;
			this.mCurUseAffixGoods.InitWithBaseScene(this.mBaseScene);
		}
		if (this.mCurUseAffixGoods != null && this.mCurUseAffixGoods.GetAffixEmptyIndex() != -1)
		{
			LopetInfo info = Globals.Instance.AttDB.LopetDict.GetInfo(petId);
			if (info != null)
			{
				this.mCurUseAffixGoods.AddAffixLopet(info);
			}
		}
	}

	private void AddAffixFashion(int fashionId)
	{
		if (this.mCurUseAffixGoods == null || this.mCurUseAffixGoods.GetAffixEmptyIndex() == -1)
		{
			UnityEngine.Object @object = Res.LoadGUI("GUI/mailAffixGoods");
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
			gameObject.name = @object.name;
			gameObject.transform.parent = this.mMainDetailInfo.transform.parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.AddComponent<UIDragScrollView>().scrollView = this.mScrollView;
			this.mCurUseAffixGoods = gameObject.AddComponent<MailAffixGoods>();
			this.mCurUseAffixGoods.ElementPriority = this.mElementPriority++;
			this.mCurUseAffixGoods.InitWithBaseScene(this.mBaseScene);
		}
		if (this.mCurUseAffixGoods != null && this.mCurUseAffixGoods.GetAffixEmptyIndex() != -1)
		{
			FashionInfo info = Globals.Instance.AttDB.FashionDict.GetInfo(fashionId);
			if (info != null)
			{
				this.mCurUseAffixGoods.AddAffixFashion(info);
			}
		}
	}

	private void Refresh()
	{
		if (this.mMailData != null)
		{
			for (int i = 0; i < this.mInfoTable.transform.childCount; i++)
			{
				Transform child = this.mInfoTable.transform.GetChild(i);
				if (child.name != "Title" && child.name != "infoLabel" && child.name != "line" && child.name != "sender")
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
			}
			this.mCurUseAffixGoods = null;
			this.mMailTitle.InitMailTitle(this.mMailData.Title);
			this.mMainDetailInfo.InitMailContents(this.mMailData.ContentText);
			if (!string.IsNullOrEmpty(this.mMailData.Sender))
			{
				this.mMailSender.gameObject.SetActive(true);
				this.mMailSender.InitMailSender(this.mMailData.Sender);
			}
			else
			{
				this.mMailSender.gameObject.SetActive(false);
			}
			if (this.HasAffix())
			{
				this.mSpriteLine.gameObject.SetActive(true);
				this.mBtnTxt.text = Singleton<StringManager>.Instance.GetString("takeGoods");
			}
			else
			{
				this.mSpriteLine.gameObject.SetActive(false);
				this.mBtnTxt.text = Singleton<StringManager>.Instance.GetString("close");
			}
			for (int j = 0; j < this.mMailData.AffixType.Count; j++)
			{
				if (this.mMailData.AffixType[j] == 17)
				{
					UnityEngine.Object @object = Res.LoadGUI("GUI/mailAffixMoney");
					GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
					gameObject.name = @object.name;
					gameObject.transform.parent = this.mMainDetailInfo.transform.parent;
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					gameObject.AddComponent<UIDragScrollView>().scrollView = this.mScrollView;
					MailAffixMoney mailAffixMoney = gameObject.AddComponent<MailAffixMoney>();
					mailAffixMoney.ElementPriority = this.mElementPriority++;
					mailAffixMoney.InitMoneyInfo(EAffixType.EAffix_FestivalVoucher, this.mMailData.AffixValue1[j]);
					break;
				}
			}
			for (int k = 0; k < this.mMailData.AffixType.Count; k++)
			{
				if (this.mMailData.AffixType[k] == 1)
				{
					UnityEngine.Object object2 = Res.LoadGUI("GUI/mailAffixMoney");
					GameObject gameObject2 = (GameObject)UnityEngine.Object.Instantiate(object2);
					gameObject2.name = object2.name;
					gameObject2.transform.parent = this.mMainDetailInfo.transform.parent;
					gameObject2.transform.localPosition = Vector3.zero;
					gameObject2.transform.localScale = Vector3.one;
					gameObject2.AddComponent<UIDragScrollView>().scrollView = this.mScrollView;
					MailAffixMoney mailAffixMoney2 = gameObject2.AddComponent<MailAffixMoney>();
					mailAffixMoney2.ElementPriority = this.mElementPriority++;
					mailAffixMoney2.InitMoneyInfo(EAffixType.EAffix_Diamond, this.mMailData.AffixValue1[k]);
					break;
				}
			}
			for (int l = 0; l < this.mMailData.AffixType.Count; l++)
			{
				if (this.mMailData.AffixType[l] == 0)
				{
					UnityEngine.Object object3 = Res.LoadGUI("GUI/mailAffixMoney");
					GameObject gameObject3 = (GameObject)UnityEngine.Object.Instantiate(object3);
					gameObject3.name = object3.name;
					gameObject3.transform.parent = this.mMainDetailInfo.transform.parent;
					gameObject3.transform.localPosition = Vector3.zero;
					gameObject3.transform.localScale = Vector3.one;
					gameObject3.AddComponent<UIDragScrollView>().scrollView = this.mScrollView;
					MailAffixMoney mailAffixMoney3 = gameObject3.AddComponent<MailAffixMoney>();
					mailAffixMoney3.ElementPriority = this.mElementPriority++;
					mailAffixMoney3.InitMoneyInfo(EAffixType.EAffix_Money, this.mMailData.AffixValue1[l]);
					break;
				}
			}
			for (int m = 0; m < this.mMailData.AffixType.Count; m++)
			{
				if (this.mMailData.AffixType[m] == 4)
				{
					UnityEngine.Object object4 = Res.LoadGUI("GUI/mailAffixMoney");
					GameObject gameObject4 = (GameObject)UnityEngine.Object.Instantiate(object4);
					gameObject4.name = object4.name;
					gameObject4.transform.parent = this.mMainDetailInfo.transform.parent;
					gameObject4.transform.localPosition = Vector3.zero;
					gameObject4.transform.localScale = Vector3.one;
					gameObject4.AddComponent<UIDragScrollView>().scrollView = this.mScrollView;
					MailAffixMoney mailAffixMoney4 = gameObject4.AddComponent<MailAffixMoney>();
					mailAffixMoney4.ElementPriority = this.mElementPriority++;
					mailAffixMoney4.InitMoneyInfo(EAffixType.EAffix_Honor, this.mMailData.AffixValue1[m]);
					break;
				}
			}
			for (int n = 0; n < this.mMailData.AffixType.Count; n++)
			{
				if (this.mMailData.AffixType[n] == 5)
				{
					UnityEngine.Object object5 = Res.LoadGUI("GUI/mailAffixMoney");
					GameObject gameObject5 = (GameObject)UnityEngine.Object.Instantiate(object5);
					gameObject5.name = object5.name;
					gameObject5.transform.parent = this.mMainDetailInfo.transform.parent;
					gameObject5.transform.localPosition = Vector3.zero;
					gameObject5.transform.localScale = Vector3.one;
					gameObject5.AddComponent<UIDragScrollView>().scrollView = this.mScrollView;
					MailAffixMoney mailAffixMoney5 = gameObject5.AddComponent<MailAffixMoney>();
					mailAffixMoney5.ElementPriority = this.mElementPriority++;
					mailAffixMoney5.InitMoneyInfo(EAffixType.EAffix_Reputation, this.mMailData.AffixValue1[n]);
					break;
				}
			}
			for (int num = 0; num < this.mMailData.AffixType.Count; num++)
			{
				if (this.mMailData.AffixType[num] == 6)
				{
					MailAffixMoney mailAffixMoney6 = this.InitItem();
					mailAffixMoney6.InitMoneyInfo(EAffixType.EAffix_Energy, this.mMailData.AffixValue1[num]);
					break;
				}
			}
			for (int num2 = 0; num2 < this.mMailData.AffixType.Count; num2++)
			{
				if (this.mMailData.AffixType[num2] == 7)
				{
					MailAffixMoney mailAffixMoney7 = this.InitItem();
					mailAffixMoney7.InitMoneyInfo(EAffixType.EAffix_Exp, this.mMailData.AffixValue1[num2]);
					break;
				}
			}
			for (int num3 = 0; num3 < this.mMailData.AffixType.Count; num3++)
			{
				if (this.mMailData.AffixType[num3] == 8)
				{
					MailAffixMoney mailAffixMoney8 = this.InitItem();
					mailAffixMoney8.InitMoneyInfo(EAffixType.EAffix_MagicCrystal, this.mMailData.AffixValue1[num3]);
					break;
				}
			}
			for (int num4 = 0; num4 < this.mMailData.AffixType.Count; num4++)
			{
				if (this.mMailData.AffixType[num4] == 9)
				{
					MailAffixMoney mailAffixMoney9 = this.InitItem();
					mailAffixMoney9.InitMoneyInfo(EAffixType.EAffix_MagicSoul, this.mMailData.AffixValue1[num4]);
					break;
				}
			}
			for (int num5 = 0; num5 < this.mMailData.AffixType.Count; num5++)
			{
				if (this.mMailData.AffixType[num5] == 10)
				{
					MailAffixMoney mailAffixMoney10 = this.InitItem();
					mailAffixMoney10.InitMoneyInfo(EAffixType.EAffix_FireDragonScale, this.mMailData.AffixValue1[num5]);
					break;
				}
			}
			for (int num6 = 0; num6 < this.mMailData.AffixType.Count; num6++)
			{
				if (this.mMailData.AffixType[num6] == 11)
				{
					MailAffixMoney mailAffixMoney11 = this.InitItem();
					mailAffixMoney11.InitMoneyInfo(EAffixType.EAffix_KingMedal, this.mMailData.AffixValue1[num6]);
					break;
				}
			}
			for (int num7 = 0; num7 < this.mMailData.AffixType.Count; num7++)
			{
				if (this.mMailData.AffixType[num7] == 13)
				{
					MailAffixMoney mailAffixMoney12 = this.InitItem();
					mailAffixMoney12.InitMoneyInfo(EAffixType.EAffix_StarSoul, this.mMailData.AffixValue1[num7]);
					break;
				}
			}
			for (int num8 = 0; num8 < this.mMailData.AffixType.Count; num8++)
			{
				if (this.mMailData.AffixType[num8] == 16)
				{
					MailAffixMoney mailAffixMoney13 = this.InitItem();
					mailAffixMoney13.InitMoneyInfo(EAffixType.EAffix_LopetSoul, this.mMailData.AffixValue1[num8]);
					break;
				}
			}
			for (int num9 = 0; num9 < this.mMailData.AffixType.Count; num9++)
			{
				if (this.mMailData.AffixType[num9] == 14)
				{
					MailAffixMoney mailAffixMoney14 = this.InitItem();
					mailAffixMoney14.InitMoneyInfo(EAffixType.EAffix_Emblem, this.mMailData.AffixValue1[num9]);
					break;
				}
			}
			for (int num10 = 0; num10 < this.mMailData.AffixType.Count; num10++)
			{
				if (this.mMailData.AffixType[num10] == 2)
				{
					this.AddAffixItem(this.mMailData.AffixValue1[num10], this.mMailData.AffixValue2[num10]);
				}
				if (this.mMailData.AffixType[num10] == 3)
				{
					this.AddAffixPet(this.mMailData.AffixValue1[num10]);
				}
				if (this.mMailData.AffixType[num10] == 15)
				{
					this.AddAffixLopet(this.mMailData.AffixValue1[num10]);
				}
				if (this.mMailData.AffixType[num10] == 12)
				{
					this.AddAffixFashion(this.mMailData.AffixValue1[num10]);
				}
			}
			this.mInfoTable.repositionNow = true;
			this.mScrollBar.value = 0f;
		}
		else
		{
			global::Debug.Log(new object[]
			{
				"mMailData is null."
			});
		}
	}

	private MailAffixMoney InitItem()
	{
		UnityEngine.Object @object = Res.LoadGUI("GUI/mailAffixMoney");
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(@object);
		gameObject.name = @object.name;
		gameObject.transform.parent = this.mMainDetailInfo.transform.parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.AddComponent<UIDragScrollView>().scrollView = this.mScrollView;
		MailAffixMoney mailAffixMoney = gameObject.AddComponent<MailAffixMoney>();
		mailAffixMoney.ElementPriority = this.mElementPriority++;
		return mailAffixMoney;
	}

	private void OnEndAnimComplete()
	{
		base.gameObject.SetActive(false);
	}

	private void OnTakeAffixClick(GameObject go)
	{
		if (this.mMailData.AffixType.Count > 0)
		{
			MC2S_TakeMailAffix mC2S_TakeMailAffix = new MC2S_TakeMailAffix();
			mC2S_TakeMailAffix.MailID = this.mMailData.MailID;
			Globals.Instance.CliSession.Send(214, mC2S_TakeMailAffix);
		}
		this.EnableMailDetailInfo(false);
	}

	private void OnBackGroundClick(GameObject go)
	{
		this.EnableMailDetailInfo(false);
	}
}
