using Att;
using System;
using System.Text;
using UnityEngine;

public class GUIPetTitleInfo : MonoBehaviour
{
	private const int StarNums = 5;

	private UILabel mNameTxt;

	private UILabel mLvlNum;

	private UILabel mZiZhiNum;

	private GameObject mZiZhiGo;

	private GameObject mZhujueGo;

	private UISprite mPropertySp;

	private UIButton mPropertySpBtn;

	private UISprite mPetTypeIcon;

	private UIButton mPetTypeIconBtn;

	private UISprite[] mStars = new UISprite[5];

	private GameObject[] mStarsEffect = new GameObject[5];

	private GameObject mStarsGo;

	private StringBuilder mSb = new StringBuilder();

	public Vector3 GetStarPosition(int index)
	{
		if (0 <= index && index < 5)
		{
			return this.mStars[index].transform.position;
		}
		return Vector3.zero;
	}

	public void PlayStarEffect(int index)
	{
		if (0 <= index && index < 5)
		{
			GameObject gameObject = this.mStarsEffect[index];
			if (gameObject != null)
			{
				NGUITools.SetActive(gameObject, false);
				NGUITools.SetActive(gameObject, true);
			}
		}
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mPropertySp = base.transform.Find("shuXingIcon").GetComponent<UISprite>();
		this.mPropertySp.gameObject.SetActive(false);
		UIEventListener expr_3C = UIEventListener.Get(this.mPropertySp.gameObject);
		expr_3C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_3C.onClick, new UIEventListener.VoidDelegate(this.OnPropertyClick));
		this.mPropertySpBtn = this.mPropertySp.GetComponent<UIButton>();
		this.mPetTypeIcon = base.transform.Find("elementBtn").GetComponent<UISprite>();
		this.mPetTypeIcon.gameObject.SetActive(false);
		UIEventListener expr_AA = UIEventListener.Get(this.mPetTypeIcon.gameObject);
		expr_AA.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_AA.onClick, new UIEventListener.VoidDelegate(this.OnPetTypeIconClick));
		this.mPetTypeIconBtn = this.mPetTypeIcon.GetComponent<UIButton>();
		this.mNameTxt = base.transform.Find("name").GetComponent<UILabel>();
		this.mNameTxt.text = string.Empty;
		this.mLvlNum = base.transform.Find("lvlTxt/num").GetComponent<UILabel>();
		this.mLvlNum.text = string.Empty;
		this.mZiZhiGo = base.transform.Find("ziZhiTxt").gameObject;
		this.mZiZhiNum = this.mZiZhiGo.transform.Find("num").GetComponent<UILabel>();
		this.mZiZhiGo.SetActive(false);
		this.mZhujueGo = base.transform.Find("zhujueTxt").gameObject;
		this.mZhujueGo.SetActive(false);
		Transform transform = base.transform.Find("stars");
		this.mStarsGo = transform.gameObject;
		for (int i = 0; i < 5; i++)
		{
			this.mStars[i] = transform.Find(string.Format("starBg{0}", i)).GetComponent<UISprite>();
			Transform transform2 = this.mStars[i].transform.Find("ui75_1");
			if (transform2 != null)
			{
				this.mStarsEffect[i] = transform2.gameObject;
				Tools.SetParticleRenderQueue2(this.mStarsEffect[i], 4000);
				NGUITools.SetActive(this.mStarsEffect[i], false);
			}
			this.mStars[i].gameObject.SetActive(false);
		}
	}

	public void Refresh(PetDataEx pdEx, bool isLocal, bool isPlayer = false)
	{
		if (pdEx != null)
		{
			if (isPlayer)
			{
				this.mPropertySp.gameObject.SetActive(false);
				this.mPetTypeIcon.gameObject.SetActive(false);
				this.mZiZhiGo.SetActive(false);
				this.mZhujueGo.SetActive(true);
			}
			else
			{
				this.mPropertySp.gameObject.SetActive(true);
				string propertyIconWithBorder = Tools.GetPropertyIconWithBorder((EElementType)pdEx.Info.ElementType);
				this.mPropertySp.spriteName = propertyIconWithBorder;
				this.mPropertySpBtn.normalSprite = propertyIconWithBorder;
				this.mPropertySpBtn.hoverSprite = propertyIconWithBorder;
				this.mPropertySpBtn.pressedSprite = propertyIconWithBorder;
				this.mPetTypeIcon.gameObject.SetActive(true);
				string petTypeIcon = Tools.GetPetTypeIcon(pdEx.Info.Type);
				this.mPetTypeIcon.spriteName = petTypeIcon;
				this.mPetTypeIconBtn.normalSprite = petTypeIcon;
				this.mPetTypeIconBtn.hoverSprite = petTypeIcon;
				this.mPetTypeIconBtn.pressedSprite = petTypeIcon;
				this.mZiZhiGo.SetActive(true);
				this.mZhujueGo.SetActive(false);
				this.mZiZhiNum.text = pdEx.Info.SubQuality.ToString();
			}
			this.mSb.Remove(0, this.mSb.Length);
			if (isPlayer)
			{
				if (isLocal)
				{
					this.mSb.Append(Globals.Instance.Player.Data.Name);
					if (Globals.Instance.Player.Data.FurtherLevel > 0)
					{
						this.mSb.Append(" +").Append(Globals.Instance.Player.Data.FurtherLevel);
					}
					if ((ulong)Globals.Instance.Player.Data.Level < (ulong)((long)GameConst.GetInt32(24)))
					{
						this.mStarsGo.SetActive(false);
					}
					else
					{
						this.mStarsGo.SetActive(true);
						uint num = 0u;
						uint petStarAndLvl = Tools.GetPetStarAndLvl((uint)Globals.Instance.Player.Data.AwakeLevel, out num);
						for (uint num2 = 0u; num2 < 5u; num2 += 1u)
						{
							this.mStars[(int)((UIntPtr)num2)].gameObject.SetActive(true);
							if (num2 < petStarAndLvl)
							{
								this.mStars[(int)((UIntPtr)num2)].spriteName = "star";
							}
							else
							{
								this.mStars[(int)((UIntPtr)num2)].spriteName = "starBg";
							}
						}
					}
				}
				else
				{
					this.mSb.Append(pdEx.Info.Name);
					if (pdEx.Data.Further > 0u)
					{
						this.mSb.Append(" +").Append(pdEx.Data.Further);
					}
					if ((ulong)pdEx.Data.Level < (ulong)((long)GameConst.GetInt32(24)))
					{
						this.mStarsGo.SetActive(false);
					}
					else
					{
						this.mStarsGo.SetActive(true);
						uint num3 = 0u;
						uint petStarAndLvl2 = Tools.GetPetStarAndLvl(pdEx.Data.Awake, out num3);
						for (uint num4 = 0u; num4 < 5u; num4 += 1u)
						{
							this.mStars[(int)((UIntPtr)num4)].gameObject.SetActive(true);
							if (num4 < petStarAndLvl2)
							{
								this.mStars[(int)((UIntPtr)num4)].spriteName = "star";
							}
							else
							{
								this.mStars[(int)((UIntPtr)num4)].spriteName = "starBg";
							}
						}
					}
				}
			}
			else
			{
				this.mSb.Append(Tools.GetPetName(pdEx.Info));
				if (pdEx.Data.Further > 0u)
				{
					this.mSb.Append(" +").Append(pdEx.Data.Further);
				}
				if ((ulong)pdEx.Data.Level < (ulong)((long)GameConst.GetInt32(24)))
				{
					this.mStarsGo.SetActive(false);
				}
				else
				{
					this.mStarsGo.SetActive(true);
					uint num5 = 0u;
					uint petStarAndLvl3 = Tools.GetPetStarAndLvl(pdEx.Data.Awake, out num5);
					for (uint num6 = 0u; num6 < 5u; num6 += 1u)
					{
						this.mStars[(int)((UIntPtr)num6)].gameObject.SetActive(true);
						if (num6 < petStarAndLvl3)
						{
							this.mStars[(int)((UIntPtr)num6)].spriteName = "star";
						}
						else
						{
							this.mStars[(int)((UIntPtr)num6)].spriteName = "starBg";
						}
					}
				}
			}
			uint num7 = 0u;
			if (isLocal)
			{
				num7 = Globals.Instance.Player.Data.Level;
			}
			else
			{
				SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(0, false);
				if (socket != null)
				{
					PetDataEx pet = socket.GetPet();
					if (pet != null)
					{
						num7 = pet.Data.Level;
					}
				}
			}
			this.mNameTxt.text = this.mSb.ToString();
			this.mNameTxt.color = Tools.GetItemQualityColor(pdEx.Info.Quality);
			this.mLvlNum.text = ((!isPlayer) ? string.Format("{0}/{1}", pdEx.Data.Level, num7) : pdEx.Data.Level.ToString());
		}
	}

	private void OnPropertyClick(GameObject go)
	{
		GUIPropertyTypeInfoPopUp.ShowMe();
	}

	private void OnPetTypeIconClick(GameObject go)
	{
		GUIPetTypeInfoPopUp.ShowMe();
	}
}
