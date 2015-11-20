using System;
using UnityEngine;

namespace Util
{
	public class SpeedHackDetector : MonoBehaviour
	{
		private const int THRESHOLD = 5000000;

		public Action onSpeedHackDetected;

		public float interval = 0.5f;

		public byte maxFalsePositives = 3;

		private int errorsCount;

		private long ticksOnStart;

		private long ticksOnStartVulnerable;

		private bool running;

		public void StartMonitoring()
		{
			if (this.running)
			{
				global::Debug.LogWarning(new object[]
				{
					"Speed hack detector is running."
				});
				return;
			}
			this.RestartTicks();
			this.errorsCount = 0;
			this.running = true;
			base.InvokeRepeating("OnTimer", this.interval, this.interval);
		}

		private void RestartTicks()
		{
			this.ticksOnStart = DateTime.UtcNow.Ticks;
			this.ticksOnStartVulnerable = (long)Environment.TickCount * 10000L;
		}

		public void StopMonitoring()
		{
			if (this.running)
			{
				base.CancelInvoke("OnTimer");
				this.errorsCount = 0;
				this.running = false;
			}
		}

		private void OnDisable()
		{
			this.StopMonitoring();
		}

		private void OnEnable()
		{
			if (this.running)
			{
				this.RestartTicks();
				this.errorsCount = 0;
			}
			else
			{
				this.StartMonitoring();
			}
		}

		public void OnApplicationFocus(bool focusStatus)
		{
			if (focusStatus)
			{
				if (this.running)
				{
					this.RestartTicks();
					this.errorsCount = 0;
				}
				else
				{
					this.StartMonitoring();
				}
			}
			else
			{
				this.StopMonitoring();
				HackerMessageBox.HideMessageBox();
			}
		}

		private void OnApplicationQuit()
		{
			this.StopMonitoring();
		}

		private void OnTimer()
		{
			long ticks = DateTime.UtcNow.Ticks;
			long num = (long)Environment.TickCount * 10000L;
			if (Mathf.Abs((float)(num - this.ticksOnStartVulnerable - (ticks - this.ticksOnStart))) > 5000000f)
			{
				this.errorsCount++;
				if (this.errorsCount > (int)this.maxFalsePositives)
				{
					this.OnSpeedHackDetected();
				}
				else
				{
					global::Debug.LogWarning(new object[]
					{
						"[ACT] SpeedHackDetector: detection! Silent detections left: " + ((int)this.maxFalsePositives - this.errorsCount)
					});
				}
				this.RestartTicks();
			}
			else if (ticks - this.ticksOnStart > 1200000000L)
			{
				this.RestartTicks();
				this.errorsCount = 0;
			}
		}

		private void OnSpeedHackDetected()
		{
			if (this.onSpeedHackDetected != null)
			{
				this.onSpeedHackDetected();
			}
			HackerMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("EnvHacker"));
		}
	}
}
