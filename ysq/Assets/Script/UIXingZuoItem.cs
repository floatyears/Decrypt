using Att;
using Holoville.HOTween;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class UIXingZuoItem : MonoBehaviour
{
	public UITexture mConBg;

	public UITexture mConName;

	public GUIXingZuoPage mPage;

	public GUIConstellationScene mBaseScene;

	public int mLv;

	public float mWaitTimeToHide = 0.6f;

	private ConstellationInfo mConInfo;

	private UILabel mName;

	private GameObject mxingZuoNum;

	private GameObject[] con = new GameObject[6];

	private UISprite[] pos = new UISprite[6];

	private UISprite[] pos1 = new UISprite[6];

	private UISprite[] pos2 = new UISprite[6];

	private UISprite[] tips = new UISprite[6];

	private UILabel[] info = new UILabel[6];

	private GameObject[] effect = new GameObject[6];

	private GameObject[] effectLight = new GameObject[6];

	private Texture[] mXingZuoBg = new Texture[13];

	private Texture[] mXingZuoName = new Texture[13];

	public void InitItemData(GUIConstellationScene baseScene)
	{
		this.mBaseScene = baseScene;
		this.CreateObject();
	}

	public void InitItemData(GUIXingZuoPage baseScene)
	{
		this.mPage = baseScene;
	}

	private void CreateObject()
	{
		this.mConBg = base.transform.Find("Texture").GetComponent<UITexture>();
		this.mConName = this.mConBg.transform.Find("name").GetComponent<UITexture>();
		this.mxingZuoNum = base.transform.Find("xing1").gameObject;
		for (int i = 1; i < 6; i++)
		{
			this.con[i] = this.mxingZuoNum.transform.Find(string.Format("con{0}", i)).gameObject;
			this.pos[i] = this.con[i].transform.Find("pos").GetComponent<UISprite>();
			this.pos1[i] = this.con[i].transform.Find("pos1").GetComponent<UISprite>();
			this.pos2[i] = this.con[i].transform.Find("pos2").GetComponent<UISprite>();
			this.tips[i] = this.con[i].transform.Find("tips").GetComponent<UISprite>();
			this.info[i] = this.tips[i].transform.Find("info").GetComponent<UILabel>();
			this.effect[i] = this.con[i].transform.Find("ui64_1").gameObject;
			this.effectLight[i] = this.con[i].transform.Find("ui64_2").gameObject;
			UIEventListener expr_198 = UIEventListener.Get(this.con[i].gameObject);
			expr_198.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_198.onPress, new UIEventListener.BoolDelegate(this.OnConPress));
			UIEventListener expr_1CB = UIEventListener.Get(this.con[i].gameObject);
			expr_1CB.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(expr_1CB.onDrag, new UIEventListener.VectorDelegate(this.OnConDrag));
			UIEventListener expr_1FE = UIEventListener.Get(this.con[i].gameObject);
			expr_1FE.onDragEnd = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1FE.onDragEnd, new UIEventListener.VoidDelegate(this.OnConDragEnd));
		}
	}

	private void OnConDragEnd(GameObject go)
	{
		for (int i = 1; i < 6; i++)
		{
			this.con[i].transform.collider.enabled = true;
		}
		this.OnDragEnd();
	}

	private void OnConDrag(GameObject go, Vector2 delta)
	{
		float x = base.transform.localPosition.x + delta.x;
		base.transform.localPosition = new Vector3(x, base.transform.localPosition.y, base.transform.localPosition.z);
		for (int i = 1; i < 6; i++)
		{
			this.con[i].transform.collider.enabled = false;
		}
	}

	private void OnConPress(GameObject go, bool isPressed)
	{
		string name = go.transform.name;
		ConstellationInfo constellationInfo = Globals.Instance.AttDB.ConstellationDict.GetInfo(GUIXingZuoPage.mXingZuo);
		if (isPressed)
		{
			Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
			for (int i = 0; i < 4; i++)
			{
				if (name.EndsWith("1") && constellationInfo.Value1[i] == 2)
				{
					this.info[1].text = Singleton<StringManager>.Instance.GetString("XingZuo17", new object[]
					{
						constellationInfo.Value2[0]
					});
					this.tips[1].gameObject.SetActive(true);
				}
				else if (name.EndsWith("2") && constellationInfo.Value1[i] == 3)
				{
					this.info[2].text = Singleton<StringManager>.Instance.GetString("XingZuo18", new object[]
					{
						constellationInfo.Value2[1]
					});
					this.tips[2].gameObject.SetActive(true);
				}
				else if (name.EndsWith("3") && constellationInfo.Value1[i] == 4)
				{
					this.info[3].text = Singleton<StringManager>.Instance.GetString("XingZuo19", new object[]
					{
						constellationInfo.Value2[2]
					});
					this.tips[3].gameObject.SetActive(true);
				}
				else if (name.EndsWith("4") && constellationInfo.Value1[i] == 1)
				{
					this.info[4].text = Singleton<StringManager>.Instance.GetString("XingZuo20", new object[]
					{
						constellationInfo.Value2[3]
					});
					this.tips[4].gameObject.SetActive(true);
				}
				else if (name.EndsWith("5"))
				{
					this.tips[5].gameObject.SetActive(false);
					this.tips[5].transform.localScale = Vector3.zero;
					HOTween.To(this.tips[5].transform, 0.1f, new TweenParms().Prop("localScale", Vector3.one));
					this.tips[5].gameObject.SetActive(true);
				}
			}
		}
		if (!isPressed)
		{
			for (int j = 0; j < 4; j++)
			{
				if (name.EndsWith("1") && constellationInfo.Value1[j] == 2)
				{
					this.tips[1].gameObject.SetActive(false);
				}
				else if (name.EndsWith("2") && constellationInfo.Value1[j] == 3)
				{
					this.tips[2].gameObject.SetActive(false);
				}
				else if (name.EndsWith("3") && constellationInfo.Value1[j] == 4)
				{
					this.tips[3].gameObject.SetActive(false);
				}
				else if (name.EndsWith("4") && constellationInfo.Value1[j] == 1)
				{
					this.tips[4].gameObject.SetActive(false);
				}
				else if (name.EndsWith("5"))
				{
					this.tips[5].gameObject.SetActive(true);
				}
			}
		}
	}

	public void Refresh(ConstellationInfo conInfo)
	{
		this.mConInfo = conInfo;
		if (this.mConInfo == null)
		{
			return;
		}
		this.RefreshConstellationInfo();
		this.RefreshShowIcon();
	}

	public void RefreshShowIcon()
	{
		int constellationLevel = Globals.Instance.Player.Data.ConstellationLevel;
		int num = (constellationLevel + 4) / 5;
		int num2 = (constellationLevel - 1) % 5 + 1;
		if (constellationLevel == 0)
		{
			if (GUIXingZuoPage.mXingZuo == 1)
			{
				for (int i = 1; i < 6; i++)
				{
					this.effect[i].gameObject.SetActive(false);
					this.effectLight[i].gameObject.SetActive(false);
					this.pos[i].gameObject.SetActive(false);
					this.pos1[i].gameObject.SetActive(true);
					this.pos2[i].gameObject.SetActive(false);
					this.tips[i].gameObject.SetActive(false);
					this.pos2[1].gameObject.SetActive(true);
					this.tips[5].gameObject.SetActive(true);
				}
			}
			if (GUIXingZuoPage.mXingZuo > 1)
			{
				for (int j = 1; j < 6; j++)
				{
					this.effect[j].gameObject.SetActive(false);
					this.effectLight[j].gameObject.SetActive(false);
					this.pos[j].gameObject.SetActive(false);
					this.pos1[j].gameObject.SetActive(true);
					this.pos2[j].gameObject.SetActive(false);
					this.tips[j].gameObject.SetActive(false);
					this.tips[5].gameObject.SetActive(true);
				}
			}
		}
		if (constellationLevel > 0)
		{
			if (num2 < 5)
			{
				if (GUIXingZuoPage.mXingZuo < num)
				{
					for (int k = 1; k < 6; k++)
					{
						this.effect[k].gameObject.SetActive(false);
						this.effectLight[k].gameObject.SetActive(true);
						this.pos[k].gameObject.SetActive(true);
						this.pos1[k].gameObject.SetActive(false);
						this.pos2[k].gameObject.SetActive(false);
						this.tips[k].gameObject.SetActive(false);
						this.tips[5].gameObject.SetActive(true);
					}
				}
				if (GUIXingZuoPage.mXingZuo == num)
				{
					for (int l = 1; l < 6; l++)
					{
						if (l == num2)
						{
							this.effect[l].gameObject.SetActive(false);
							base.StartCoroutine(this.WaitTimeToHide(constellationLevel));
							this.pos2[l].gameObject.SetActive(false);
							this.tips[l].gameObject.SetActive(false);
							this.tips[5].gameObject.SetActive(true);
							this.pos2[num2 + 1].gameObject.SetActive(true);
						}
						if (l > num2)
						{
							this.effect[l].gameObject.SetActive(false);
							this.effectLight[l].gameObject.SetActive(false);
							this.pos[l].gameObject.SetActive(false);
							this.pos1[l].gameObject.SetActive(true);
							this.pos2[l].gameObject.SetActive(false);
							this.tips[l].gameObject.SetActive(false);
							this.tips[5].gameObject.SetActive(true);
						}
						if (l < num2)
						{
							this.effect[l].gameObject.SetActive(false);
							this.effectLight[l].gameObject.SetActive(true);
							this.pos[l].gameObject.SetActive(true);
							this.pos1[l].gameObject.SetActive(false);
							this.pos2[l].gameObject.SetActive(false);
							this.tips[l].gameObject.SetActive(false);
							this.tips[5].gameObject.SetActive(true);
						}
					}
				}
				if (GUIXingZuoPage.mXingZuo > num)
				{
					for (int m = 1; m < 6; m++)
					{
						this.effect[m].gameObject.SetActive(false);
						this.effectLight[m].gameObject.SetActive(false);
						this.pos[m].gameObject.SetActive(false);
						this.pos1[m].gameObject.SetActive(true);
						this.pos2[m].gameObject.SetActive(false);
						this.tips[m].gameObject.SetActive(false);
						this.tips[5].gameObject.SetActive(true);
					}
				}
			}
			if (num2 == 5)
			{
				if (GUIXingZuoPage.mXingZuo < num + 1)
				{
					for (int n = 1; n < 6; n++)
					{
						this.effect[n].gameObject.SetActive(false);
						base.StartCoroutine(this.WaitTimeToHide1(constellationLevel));
						this.pos2[n].gameObject.SetActive(false);
						this.tips[n].gameObject.SetActive(false);
						this.tips[5].gameObject.SetActive(true);
					}
				}
				if (GUIXingZuoPage.mXingZuo == num + 1)
				{
					for (int num3 = 1; num3 < 6; num3++)
					{
						this.effect[num3].gameObject.SetActive(false);
						this.effectLight[num3].gameObject.SetActive(false);
						this.pos[num3].gameObject.SetActive(false);
						this.pos1[num3].gameObject.SetActive(true);
						this.pos2[num3].gameObject.SetActive(false);
						this.tips[num3].gameObject.SetActive(false);
						this.tips[5].gameObject.SetActive(true);
						this.pos2[1].gameObject.SetActive(true);
					}
				}
				if (GUIXingZuoPage.mXingZuo > num + 1)
				{
					for (int num4 = 1; num4 < 6; num4++)
					{
						this.effect[num4].gameObject.SetActive(false);
						this.effectLight[num4].gameObject.SetActive(false);
						this.pos[num4].gameObject.SetActive(false);
						this.pos1[num4].gameObject.SetActive(true);
						this.pos2[num4].gameObject.SetActive(false);
						this.tips[num4].gameObject.SetActive(false);
						this.tips[5].gameObject.SetActive(true);
					}
				}
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator WaitTimeToHide(int conLv)
	{
        return null;
        //UIXingZuoItem.<WaitTimeToHide>c__Iterator39 <WaitTimeToHide>c__Iterator = new UIXingZuoItem.<WaitTimeToHide>c__Iterator39();
        //<WaitTimeToHide>c__Iterator.conLv = conLv;
        //<WaitTimeToHide>c__Iterator.<$>conLv = conLv;
        //<WaitTimeToHide>c__Iterator.<>f__this = this;
        //return <WaitTimeToHide>c__Iterator;
	}

	[DebuggerHidden]
	private IEnumerator WaitTimeToHide1(int conLv)
	{
        return null;
        //UIXingZuoItem.<WaitTimeToHide1>c__Iterator3A <WaitTimeToHide1>c__Iterator3A = new UIXingZuoItem.<WaitTimeToHide1>c__Iterator3A();
        //<WaitTimeToHide1>c__Iterator3A.<>f__this = this;
        //return <WaitTimeToHide1>c__Iterator3A;
	}

	public void RefreshEffect()
	{
		int constellationLevel = Globals.Instance.Player.Data.ConstellationLevel;
		int num = (constellationLevel + 4) / 5;
		int num2 = (constellationLevel - 1) % 5 + 1;
		if (constellationLevel == 0 && (GUIXingZuoPage.mXingZuo == 1 || GUIXingZuoPage.mXingZuo > 1))
		{
			for (int i = 1; i < 6; i++)
			{
				this.effect[i].gameObject.SetActive(false);
			}
		}
		if (constellationLevel > 0 && num2 > 0)
		{
			if (GUIXingZuoPage.mXingZuo < num || GUIXingZuoPage.mXingZuo > num)
			{
				for (int j = 1; j < 6; j++)
				{
					this.effect[j].gameObject.SetActive(false);
				}
			}
			if (GUIXingZuoPage.mXingZuo == num)
			{
				for (int k = 1; k < 6; k++)
				{
					if (k < num2)
					{
						this.effect[k].gameObject.SetActive(false);
					}
					if (k > num2)
					{
						this.effect[k].gameObject.SetActive(false);
					}
					if (k - 1 == num2 - 1)
					{
						this.effect[k].gameObject.SetActive(true);
						GUIRightInfo.mLightBtn.isEnabled = false;
						Tools.SetButtonState(GUIRightInfo.mLightBtn.gameObject, false);
						base.StartCoroutine(this.CloseEffect());
					}
				}
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator CloseEffect()
	{
        return null;
        //UIXingZuoItem.<CloseEffect>c__Iterator3B <CloseEffect>c__Iterator3B = new UIXingZuoItem.<CloseEffect>c__Iterator3B();
        //<CloseEffect>c__Iterator3B.<>f__this = this;
        //return <CloseEffect>c__Iterator3B;
	}

	private void RefreshConstellationInfo()
	{
		this.mConBg.mainTexture = this.GetTextureBg(GUIXingZuoPage.mXingZuo);
		this.mConName.mainTexture = this.GetTextureName(GUIXingZuoPage.mXingZuo);
		ConstellationInfo constellationInfo = Globals.Instance.AttDB.ConstellationDict.GetInfo(GUIXingZuoPage.mXingZuo);
		ItemInfo itemInfo = Globals.Instance.AttDB.ItemDict.GetInfo(constellationInfo.Value1[4]);
		switch (GUIXingZuoPage.mXingZuo)
		{
		case 1:
			this.mLv = 5;
			this.con[1].transform.localPosition = new Vector3(-363f, -23f, 0f);
			this.con[2].transform.localPosition = new Vector3(-146f, 62f, 0f);
			this.con[3].transform.localPosition = new Vector3(20f, 133f, 0f);
			this.con[4].transform.localPosition = new Vector3(227f, 112f, 0f);
			this.con[5].transform.localPosition = new Vector3(349f, -93f, 0f);
			this.mxingZuoNum.transform.localPosition = new Vector3(0f, 32f, 0f);
			this.mConBg.transform.localPosition = new Vector3(-10f, 41f, 0f);
			this.mConName.transform.localPosition = new Vector3(-343f, 141f, 0f);
			this.tips[2].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[3].transform.localPosition = new Vector3(0f, -71f, 0f);
			this.tips[5].transform.localPosition = new Vector3(-17f, 80f, 0f);
			this.mConBg.height = 438;
			this.mConBg.width = 740;
			this.info[5].text = Singleton<StringManager>.Instance.GetString("XingZuo24", new object[]
			{
				itemInfo.Name,
				constellationInfo.Value2[4]
			});
			break;
		case 2:
			this.mLv = 10;
			this.con[1].transform.localPosition = new Vector3(-364f, 68f, 0f);
			this.con[2].transform.localPosition = new Vector3(-170f, 161f, 0f);
			this.con[3].transform.localPosition = new Vector3(74f, 80f, 0f);
			this.con[4].transform.localPosition = new Vector3(160f, -104f, 0f);
			this.con[5].transform.localPosition = new Vector3(361f, 15f, 0f);
			this.tips[2].transform.localPosition = new Vector3(135f, 17f, 0f);
			this.tips[4].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[5].transform.localPosition = new Vector3(0f, 80f, 0f);
			this.mxingZuoNum.transform.localPosition = new Vector3(0f, 32f, 0f);
			this.mConBg.transform.localPosition = new Vector3(-5f, 41f, 0f);
			this.mConName.transform.localPosition = new Vector3(-286f, -70f, 0f);
			this.mConBg.height = 510;
			this.mConBg.width = 760;
			this.info[5].text = Singleton<StringManager>.Instance.GetString("XingZuo37");
			break;
		case 3:
			this.mLv = 15;
			this.con[1].transform.localPosition = new Vector3(-351f, 96f, 0f);
			this.con[2].transform.localPosition = new Vector3(-172f, 30f, 0f);
			this.con[3].transform.localPosition = new Vector3(103f, 174f, 0f);
			this.con[4].transform.localPosition = new Vector3(269f, 102f, 0f);
			this.con[5].transform.localPosition = new Vector3(64f, -27f, 0f);
			this.tips[2].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[3].transform.localPosition = new Vector3(118f, 40f, 0f);
			this.tips[4].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.mxingZuoNum.transform.localPosition = new Vector3(-12f, 0f, 0f);
			this.mConBg.transform.localPosition = new Vector3(-34f, 64f, 0f);
			this.mConName.transform.localPosition = new Vector3(368f, -35f, 0f);
			this.mConBg.height = 511;
			this.mConBg.width = 687;
			this.info[5].text = Singleton<StringManager>.Instance.GetString("XingZuo24", new object[]
			{
				itemInfo.Name,
				constellationInfo.Value2[4]
			});
			break;
		case 4:
			this.mLv = 20;
			this.con[1].transform.localPosition = new Vector3(-316f, -30f, 0f);
			this.con[2].transform.localPosition = new Vector3(-131f, 119f, 0f);
			this.con[3].transform.localPosition = new Vector3(74f, -5f, 0f);
			this.con[4].transform.localPosition = new Vector3(306f, 147f, 0f);
			this.con[5].transform.localPosition = new Vector3(276f, -112f, 0f);
			this.tips[2].transform.localPosition = new Vector3(0f, 67f, 0f);
			this.tips[3].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[4].transform.localPosition = new Vector3(-134f, 31f, 0f);
			this.tips[5].transform.localPosition = new Vector3(0f, 80f, 0f);
			this.mxingZuoNum.transform.localPosition = new Vector3(0f, 32f, 0f);
			this.mConBg.transform.localPosition = new Vector3(-1f, 20f, 0f);
			this.mConName.transform.localPosition = new Vector3(-239f, -159f, 0f);
			this.mConBg.height = 458;
			this.mConBg.width = 655;
			this.info[5].text = Singleton<StringManager>.Instance.GetString("XingZuo24", new object[]
			{
				itemInfo.Name,
				constellationInfo.Value2[4]
			});
			break;
		case 5:
			this.mLv = 25;
			this.con[1].transform.localPosition = new Vector3(-363f, 6f, 0f);
			this.con[2].transform.localPosition = new Vector3(-126f, 103f, 0f);
			this.con[3].transform.localPosition = new Vector3(72f, -15f, 0f);
			this.con[4].transform.localPosition = new Vector3(173f, 134f, 0f);
			this.con[5].transform.localPosition = new Vector3(396f, 22f, 0f);
			this.tips[2].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[3].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[4].transform.localPosition = new Vector3(0f, -76f, 0f);
			this.tips[5].transform.localPosition = new Vector3(-19f, 80f, 0f);
			this.mxingZuoNum.transform.localPosition = new Vector3(0f, 32f, 0f);
			this.mConBg.transform.localPosition = new Vector3(14f, 59f, 0f);
			this.mConName.transform.localPosition = new Vector3(-308f, -112f, 0f);
			this.mConBg.height = 516;
			this.mConBg.width = 803;
			this.info[5].text = Singleton<StringManager>.Instance.GetString("XingZuo24", new object[]
			{
				itemInfo.Name,
				constellationInfo.Value2[4]
			});
			break;
		case 6:
			this.mLv = 30;
			this.con[1].transform.localPosition = new Vector3(-370f, -88f, 0f);
			this.con[2].transform.localPosition = new Vector3(-172f, 85f, 0f);
			this.con[3].transform.localPosition = new Vector3(15f, 28f, 0f);
			this.con[4].transform.localPosition = new Vector3(199f, 57f, 0f);
			this.con[5].transform.localPosition = new Vector3(53f, 196f, 0f);
			this.tips[1].transform.localPosition = new Vector3(40f, 70f, 0f);
			for (int i = 2; i < 5; i++)
			{
				this.tips[i].transform.localPosition = new Vector3(0f, 70f, 0f);
			}
			this.tips[5].transform.localPosition = new Vector3(145f, 10f, 0f);
			this.mxingZuoNum.transform.localPosition = new Vector3(0f, 0f, 0f);
			this.mConBg.transform.localPosition = new Vector3(-60f, 48f, 0f);
			this.mConName.transform.localPosition = new Vector3(364f, -110f, 0f);
			this.mConBg.height = 494;
			this.mConBg.width = 676;
			this.info[5].text = Singleton<StringManager>.Instance.GetString("XingZuo38");
			break;
		case 7:
			this.mLv = 35;
			this.con[1].transform.localPosition = new Vector3(-70f, -75f, 0f);
			this.con[2].transform.localPosition = new Vector3(-245f, 33f, 0f);
			this.con[3].transform.localPosition = new Vector3(-35f, 152f, 0f);
			this.con[4].transform.localPosition = new Vector3(174f, 3f, 0f);
			this.con[5].transform.localPosition = new Vector3(358f, -101f, 0f);
			this.mxingZuoNum.transform.localPosition = new Vector3(0f, 32f, 0f);
			this.tips[1].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[2].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[3].transform.localPosition = new Vector3(130f, 7f, 0f);
			this.tips[4].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[5].transform.localPosition = new Vector3(-35f, 80f, 0f);
			this.mConBg.transform.localPosition = new Vector3(54f, 40f, 0f);
			this.mConName.transform.localPosition = new Vector3(-324f, -122f);
			this.mConBg.height = 441;
			this.mConBg.width = 645;
			this.info[5].text = Singleton<StringManager>.Instance.GetString("XingZuo24", new object[]
			{
				itemInfo.Name,
				constellationInfo.Value2[4]
			});
			break;
		case 8:
			this.mLv = 40;
			this.con[1].transform.localPosition = new Vector3(-360f, 6f, 0f);
			this.con[2].transform.localPosition = new Vector3(-96f, -140f, 0f);
			this.con[3].transform.localPosition = new Vector3(58f, 8f, 0f);
			this.con[4].transform.localPosition = new Vector3(107f, 150f, 0f);
			this.con[5].transform.localPosition = new Vector3(307f, 42f, 0f);
			this.tips[2].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[3].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[4].transform.localPosition = new Vector3(0f, -76f, 0f);
			this.tips[5].transform.localPosition = new Vector3(0f, 80f, 0f);
			this.mxingZuoNum.transform.localPosition = new Vector3(0f, 32f, 0f);
			this.mConBg.transform.localPosition = new Vector3(-19f, 52f, 0f);
			this.mConName.transform.localPosition = new Vector3(347f, -127f, 0f);
			this.mConBg.height = 424;
			this.mConBg.width = 689;
			this.info[5].text = Singleton<StringManager>.Instance.GetString("XingZuo24", new object[]
			{
				itemInfo.Name,
				constellationInfo.Value2[4]
			});
			break;
		case 9:
			this.mLv = 45;
			this.con[1].transform.localPosition = new Vector3(-297f, 163f, 0f);
			this.con[2].transform.localPosition = new Vector3(-353f, -23f, 0f);
			this.con[3].transform.localPosition = new Vector3(-121f, 82f, 0f);
			this.con[4].transform.localPosition = new Vector3(107f, 13f, 0f);
			this.con[5].transform.localPosition = new Vector3(365f, -112f, 0f);
			this.tips[1].transform.localPosition = new Vector3(135f, 0f, 0f);
			this.tips[4].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[5].transform.localPosition = new Vector3(-35f, 80f, 0f);
			this.mxingZuoNum.transform.localPosition = new Vector3(0f, 32f, 0f);
			this.mConBg.transform.localPosition = new Vector3(8f, 51f, 0f);
			this.mConName.transform.localPosition = new Vector3(314f, 92f, 0f);
			this.mConBg.height = 475;
			this.mConBg.width = 739;
			this.info[5].text = Singleton<StringManager>.Instance.GetString("XingZuo24", new object[]
			{
				itemInfo.Name,
				constellationInfo.Value2[4]
			});
			break;
		case 10:
			this.mLv = 50;
			this.con[1].transform.localPosition = new Vector3(-274f, 82f, 0f);
			this.con[2].transform.localPosition = new Vector3(-2f, 70f, 0f);
			this.con[3].transform.localPosition = new Vector3(250f, 129f, 0f);
			this.con[4].transform.localPosition = new Vector3(226f, -41f, 0f);
			this.tips[5].transform.localPosition = new Vector3(0f, 80f, 0f);
			this.tips[1].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[3].transform.localPosition = new Vector3(123f, 38f, 0f);
			this.tips[4].transform.localPosition = new Vector3(10f, 70f, 0f);
			this.con[5].transform.localPosition = new Vector3(75f, -140f, 0f);
			this.mxingZuoNum.transform.localPosition = new Vector3(0f, 32f, 0f);
			this.mConBg.transform.localPosition = new Vector3(-11f, 47f, 0f);
			this.mConName.transform.localPosition = new Vector3(-271f, -33f, 0f);
			this.mConBg.height = 474;
			this.mConBg.width = 544;
			this.info[5].text = Singleton<StringManager>.Instance.GetString("XingZuo24", new object[]
			{
				itemInfo.Name,
				constellationInfo.Value2[4]
			});
			break;
		case 11:
			this.mLv = 55;
			this.con[1].transform.localPosition = new Vector3(-390f, 45f, 0f);
			this.con[2].transform.localPosition = new Vector3(-173f, -60f, 0f);
			this.con[3].transform.localPosition = new Vector3(-76f, 98f, 0f);
			this.con[4].transform.localPosition = new Vector3(90f, 22f, 0f);
			this.con[5].transform.localPosition = new Vector3(201f, -103f, 0f);
			this.mxingZuoNum.transform.localPosition = new Vector3(0f, 32f, 0f);
			this.tips[1].transform.localPosition = new Vector3(10f, 70f, 0f);
			this.tips[2].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[4].transform.localPosition = new Vector3(0f, 70f, 0f);
			this.tips[5].transform.localPosition = new Vector3(24f, 80f, 0f);
			this.mConBg.transform.localPosition = new Vector3(-22f, 49f, 0f);
			this.mConName.transform.localPosition = new Vector3(414f, 47f, 0f);
			this.mConBg.height = 474;
			this.mConBg.width = 766;
			this.info[5].text = Singleton<StringManager>.Instance.GetString("XingZuo24", new object[]
			{
				itemInfo.Name,
				constellationInfo.Value2[4]
			});
			break;
		case 12:
			this.mLv = 60;
			this.con[1].transform.localPosition = new Vector3(-63f, 200f, 0f);
			this.con[2].transform.localPosition = new Vector3(-183f, 45f, 0f);
			this.con[3].transform.localPosition = new Vector3(-334f, -87f, 0f);
			this.con[4].transform.localPosition = new Vector3(-18f, -63f, 0f);
			this.con[5].transform.localPosition = new Vector3(213f, -87f, 0f);
			this.tips[1].transform.localPosition = new Vector3(127f, 10f, 0f);
			this.mxingZuoNum.transform.localPosition = new Vector3(0f, 0f, 0f);
			this.mConBg.transform.localPosition = new Vector3(-29f, 47f, 0f);
			this.mConName.transform.localPosition = new Vector3(212f, 81f, 0f);
			this.mConBg.height = 410;
			this.mConBg.width = 631;
			this.info[5].text = Singleton<StringManager>.Instance.GetString("XingZuo24", new object[]
			{
				itemInfo.Name,
				constellationInfo.Value2[4]
			});
			break;
		}
	}

	private Texture GetTextureBg(int conIndex)
	{
		if (this.mXingZuoBg[conIndex] != null)
		{
			return this.mXingZuoBg[conIndex];
		}
		this.mXingZuoBg[conIndex] = UIXingZuoItem.GetTextureResBg(conIndex);
		return this.mXingZuoBg[conIndex];
	}

	private Texture GetTextureName(int conIndex)
	{
		if (this.mXingZuoName[conIndex] != null)
		{
			return this.mXingZuoName[conIndex];
		}
		this.mXingZuoName[conIndex] = UIXingZuoItem.GetTextureResName(conIndex);
		return this.mXingZuoName[conIndex];
	}

	private static Texture GetTextureResBg(int conIndex)
	{
		return Res.Load<Texture>(string.Format("xingzuo/XingZuo{0}", conIndex), false);
	}

	private static Texture GetTextureResName(int conIndex)
	{
		return Res.Load<Texture>(string.Format("xingzuo/XingZuo{0}_{0}", conIndex), false);
	}

	private void OnDrag(Vector2 delta)
	{
		float x = base.transform.localPosition.x + delta.x;
		base.transform.localPosition = new Vector3(x, base.transform.localPosition.y, base.transform.localPosition.z);
	}

	private void OnDragEnd()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (base.transform.localPosition.x <= -300f)
			{
				if (GUIXingZuoPage.mXingZuo < 12)
				{
					this.mBaseScene.mGUIXingZuoPage.mDragPostion = base.transform.localPosition;
					this.mPage.BeginRightAnimation();
				}
				if (GUIXingZuoPage.mXingZuo == 12)
				{
					base.transform.localPosition = new Vector3(0f, 0f, 0f);
				}
			}
			if (base.transform.localPosition.x >= 300f)
			{
				if (GUIXingZuoPage.mXingZuo > 1)
				{
					this.mBaseScene.mGUIXingZuoPage.mDragPostion = base.transform.localPosition;
					this.mPage.BeginLeftAnimaton();
				}
				if (GUIXingZuoPage.mXingZuo == 1)
				{
					base.transform.localPosition = new Vector3(0f, 0f, 0f);
				}
			}
			if (base.transform.localPosition.x < 300f && base.transform.localPosition.x > -300f)
			{
				base.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
		}
		int constellationLevel = Globals.Instance.Player.Data.ConstellationLevel;
		GUIRightInfo.RefreshLightBtn(constellationLevel);
	}
}
