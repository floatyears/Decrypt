using System;
using System.Text;
using UnityEngine;

public class UIEnergyTooltip : MonoBehaviour
{
	public static UIEnergyTooltip Instance;

	public int EnergyDuration = 360;

	private UISprite backGround;

	private UILabel contentLabel;

	private StringBuilder content = new StringBuilder();

	private float timerRefresh;

	private bool mIsJingLi;

	public static void Show(GameObject parent, bool state, bool isJingli)
	{
		if (state)
		{
			UIEnergyTooltip uIEnergyTooltip = UIEnergyTooltip.CreateInstance();
			if (uIEnergyTooltip != null)
			{
				uIEnergyTooltip.mIsJingLi = isJingli;
				uIEnergyTooltip.Show(parent);
			}
		}
		else if (UIEnergyTooltip.Instance != null)
		{
			UIEnergyTooltip.Instance.Hide();
		}
	}

	private static UIEnergyTooltip CreateInstance()
	{
		if (UIEnergyTooltip.Instance == null)
		{
			GameObject gameObject = Res.LoadGUI("GUI/EnergyTooltip");
			if (gameObject == null)
			{
				global::Debug.LogError(new object[]
				{
					"Res.Load GUI/EnergyTooltip error"
				});
				return null;
			}
			GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject) as GameObject;
			if (gameObject2 == null)
			{
				global::Debug.LogError(new object[]
				{
					"Instantiate GUI/EnergyTooltip error"
				});
				return null;
			}
			UIEnergyTooltip.Instance = gameObject2.AddComponent<UIEnergyTooltip>();
		}
		return UIEnergyTooltip.Instance;
	}

	public void Show(GameObject parent)
	{
		if (parent != null)
		{
			base.gameObject.transform.parent = parent.transform;
			base.gameObject.transform.localPosition = new Vector3(50f, -27f, 0f);
			base.gameObject.transform.localRotation = Quaternion.identity;
			base.gameObject.transform.localScale = Vector3.one;
		}
		NGUITools.SetActive(base.gameObject, true);
		GameUITools.PlayOpenWindowAnim(base.transform, null, true);
		this.Refresh();
	}

	public void Hide()
	{
		GameUITools.PlayCloseWindowAnim(base.transform, delegate
		{
			NGUITools.SetActive(base.gameObject, false);
		}, true);
	}

	private void Awake()
	{
		this.contentLabel = base.transform.FindChild("content").GetComponent<UILabel>();
		this.backGround = base.transform.FindChild("backGround").GetComponent<UISprite>();
		UIEnergyTooltip.Instance = this;
	}

	public static string FormatTime(int timecount)
	{
		int num = timecount / 3600;
		int num2 = timecount / 60 % 60;
		int num3 = timecount % 60;
		return string.Format("{0:D2}:{1:D2}:{2:D2}", num, num2, num3);
	}

	public static string FormatTime2(int timecount)
	{
		int num = timecount / 60 % 60;
		int num2 = timecount % 60;
		return string.Format("{0:D2}:{1:D2}", num, num2);
	}

	private void Refresh()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		LocalPlayer player = Globals.Instance.Player;
		this.content.Remove(0, this.content.Length);
		int timeStamp = player.GetTimeStamp();
		this.content.Append(Singleton<StringManager>.Instance.GetString("Energy1"));
		this.content.AppendLine(Tools.ServerDateTimeFormat1(timeStamp));
		if (!this.mIsJingLi)
		{
			int maxEnergy = player.GetMaxEnergy();
			if (player.Data.Energy >= maxEnergy)
			{
				this.content.AppendLine(Singleton<StringManager>.Instance.GetString("Energy6"));
			}
			else
			{
				this.content.Append(Singleton<StringManager>.Instance.GetString("Energy5"));
				this.content.AppendLine(string.Format("{0}{1}", GameConst.GetInt32(134) / 60, Singleton<StringManager>.Instance.GetString("MinuteF")));
				int num = player.Data.EnergyTimeStamp - timeStamp;
				if (num < 0)
				{
					num = 0;
				}
				this.content.Append(Singleton<StringManager>.Instance.GetString("Energy3"));
				this.content.AppendLine(UIEnergyTooltip.FormatTime(num));
				int num2 = maxEnergy - player.Data.Energy - 1;
				int timecount = num2 * this.EnergyDuration + num;
				this.content.Append(Singleton<StringManager>.Instance.GetString("Energy4"));
				this.content.AppendLine(UIEnergyTooltip.FormatTime(timecount));
			}
		}
		else
		{
			int maxStamina = player.GetMaxStamina();
			if (player.Data.Stamina >= maxStamina)
			{
				this.content.AppendLine(Singleton<StringManager>.Instance.GetString("Energy7"));
			}
			else
			{
				this.content.Append(Singleton<StringManager>.Instance.GetString("Energy5"));
				this.content.AppendLine(string.Format("{0}{1}", GameConst.GetInt32(136) / 60, Singleton<StringManager>.Instance.GetString("MinuteF")));
				int num3 = player.Data.StaminaTimeStamp - timeStamp;
				if (num3 < 0)
				{
					num3 = 0;
				}
				this.content.Append(Singleton<StringManager>.Instance.GetString("Energy9"));
				this.content.AppendLine(UIEnergyTooltip.FormatTime(num3));
				int num4 = maxStamina - player.Data.Stamina - 1;
				int timecount2 = num4 * GameConst.GetInt32(136) + num3;
				this.content.Append(Singleton<StringManager>.Instance.GetString("Energy8"));
				this.content.AppendLine(UIEnergyTooltip.FormatTime(timecount2));
			}
		}
		this.contentLabel.text = this.content.ToString();
		this.backGround.height = this.contentLabel.height + 4;
		this.timerRefresh = Time.time;
	}

	private void Update()
	{
		if (Time.time - this.timerRefresh > 1f)
		{
			this.Refresh();
		}
	}
}
