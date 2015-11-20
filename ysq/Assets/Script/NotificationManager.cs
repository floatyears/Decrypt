using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
	public class NotificationItem
	{
		public int ID
		{
			get;
			set;
		}

		public int Hour
		{
			get;
			set;
		}

		public int Minute
		{
			get;
			set;
		}

		public int Repeat
		{
			get;
			set;
		}

		public string Content
		{
			get;
			set;
		}
	}

	private List<NotificationManager.NotificationItem> NotificationConfig;

	private AndroidJavaClass localPushService;

	private void VerifyNotificationConfig()
	{
		if (this.NotificationConfig == null)
		{
			this.LoadNotificationConfig();
		}
	}

	private void LoadNotificationConfig()
	{
		TextAsset textAsset = StringManager.LoadTextRes("Notification");
		if (textAsset == null)
		{
			global::Debug.LogError(new object[]
			{
				"[NotificationManager]Load Config File Error!"
			});
			return;
		}
		try
		{
			this.NotificationConfig = JsonMapper.ToObject<List<NotificationManager.NotificationItem>>(textAsset.text);
		}
		catch (Exception ex)
		{
			global::Debug.LogErrorFormat("[NotificationManager]Parse Config File Error : {0}!", new object[]
			{
				ex.Message
			});
		}
	}

	private void Awake()
	{
		this.localPushService = new AndroidJavaClass("com.simple.dailynotify.DailyNotifyService");
		this.ClearNotification();
	}

	public long GetTimeInMillis(DateTime date)
	{
		DateTime d = new DateTime(1970, 1, 1, 8, 0, 0);
		return Convert.ToInt64((date - d).TotalSeconds) * 1000L;
	}

	private void OnApplicationQuit()
	{
		this.RegisterNotification();
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			this.RegisterNotification();
		}
		else
		{
			this.ClearNotification();
		}
	}

	public void NotificationMessage(int id, string message, int hour, int Minute, bool isRepeatDay, int seconds = 0)
	{
		if (id >= 40)
		{
			return;
		}
		int year = DateTime.Now.Year;
		int month = DateTime.Now.Month;
		int day = DateTime.Now.Day;
		int hour2 = Mathf.Clamp(hour, 0, 23);
		int minute = Mathf.Clamp(Minute, 0, 59);
		DateTime newDate = new DateTime(year, month, day, hour2, minute, seconds);
		this.NotificationMessage(id, message, newDate, isRepeatDay);
	}

	public void NotificationMessage(int id, string message, DateTime newDate, bool isRepeatDay)
	{
		if (newDate > DateTime.Now)
		{
			if (id >= 40)
			{
				return;
			}
			if (this.localPushService != null)
			{
				this.localPushService.CallStatic("ScheduleNotification", new object[]
				{
					id,
					message,
					this.GetTimeInMillis(newDate)
				});
			}
		}
	}

	public static void RegisterNotifyTypes()
	{
	}

	private void RegisterNotification()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		if (Globals.Instance.GameMgr.Status == GameManager.EGameStatus.EGS_None)
		{
			return;
		}
		this.VerifyNotificationConfig();
		int num = 1;
		if (this.NotificationConfig != null)
		{
			for (int i = 0; i < this.NotificationConfig.Count; i++)
			{
				NotificationManager.NotificationItem notificationItem = this.NotificationConfig[i];
				if (notificationItem.ID != 0)
				{
					if ((notificationItem.ID != 2 && notificationItem.ID != 4) || GameSetting.Data.WorldBossNotify)
					{
						this.NotificationMessage(num++, notificationItem.Content, notificationItem.Hour, notificationItem.Minute, notificationItem.Repeat == 1, 0);
					}
				}
			}
		}
		if (Globals.Instance != null && Globals.Instance.Player != null)
		{
			int maxEnergy = Globals.Instance.Player.GetMaxEnergy();
			if (GameSetting.Data.EnergyFull && Globals.Instance.Player.Data.Energy < maxEnergy)
			{
				int num2 = Globals.Instance.Player.Data.EnergyTimeStamp - Globals.Instance.Player.GetTimeStamp();
				if (num2 < 0)
				{
					num2 = 0;
				}
				int num3 = maxEnergy - Globals.Instance.Player.Data.Energy - 1;
				int num4 = num3 * 360 + num2;
				DateTime newDate = Tools.ServerDateTime(Globals.Instance.Player.GetTimeStamp() + num4);
				this.NotificationMessage(num++, Singleton<StringManager>.Instance.GetString("energyFullNotification"), newDate, true);
			}
			if (GameSetting.Data.PetShopRefresh)
			{
				for (int j = 2; j < 24; j += 2)
				{
					this.NotificationMessage(num++, Singleton<StringManager>.Instance.GetString("petShopRefreshNotification"), j, 0, true, 0);
				}
				this.NotificationMessage(num++, Singleton<StringManager>.Instance.GetString("petShopRefreshNotification"), 23, 59, true, 59);
			}
			if (GameSetting.Data.JueXingShopRefresh && (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(24)))
			{
				for (int k = 2; k < 24; k += 2)
				{
					this.NotificationMessage(num++, Singleton<StringManager>.Instance.GetString("jueXingShopRefreshNotification"), k, 0, true, 0);
				}
				this.NotificationMessage(num++, Singleton<StringManager>.Instance.GetString("jueXingShopRefreshNotification"), 23, 59, true, 59);
			}
		}
	}

	private void ClearNotification()
	{
		if (this.localPushService != null)
		{
			this.localPushService.CallStatic("CancelAllNotifications", new object[0]);
		}
	}
}
