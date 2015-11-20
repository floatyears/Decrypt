using NJG;
using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class NJGFOW : MonoBehaviour
{
	public enum State
	{
		Blending,
		NeedUpdate,
		UpdateTexture0,
		UpdateTexture1
	}

	public class Revealer
	{
		public bool isActive;

		public Vector2 pos = Vector2.zero;

		public int revealDistance = 10;
	}

	private const string FOW_ID = "NJGFOW";

	private static NJGFOW mInst;

	private static FastList<NJGFOW.Revealer> mRevealers = new FastList<NJGFOW.Revealer>();

	private static FastList<NJGFOW.Revealer> mAdded = new FastList<NJGFOW.Revealer>();

	private static FastList<NJGFOW.Revealer> mRemoved = new FastList<NJGFOW.Revealer>();

	private float mBlendFactor;

	private NJGFOW.State mState;

	private Texture2D mTexture0;

	private Texture2D hiddenTexture;

	private Vector2 mOrigin;

	private Color32[] mBuffer0;

	private Color32[] mBuffer1;

	private Color32[] mBuffer2;

	private Color32[] mBuffer3;

	private NJGMapBase map;

	private float mNextUpdate;

	private Thread mThread;

	public static NJGFOW instance
	{
		get
		{
			if (NJGFOW.mInst == null)
			{
				NJGFOW.mInst = (UnityEngine.Object.FindObjectOfType(typeof(NJGFOW)) as NJGFOW);
				if (NJGFOW.mInst == null)
				{
					NJGFOW.mInst = new GameObject("_NJGFOW")
					{
						hideFlags = HideFlags.HideInHierarchy
					}.AddComponent<NJGFOW>();
				}
			}
			return NJGFOW.mInst;
		}
	}

	private void Awake()
	{
		this.map = NJGMapBase.instance;
	}

	public void Init()
	{
		if (this.hiddenTexture == null)
		{
			this.hiddenTexture = new Texture2D(4, 4);
		}
		if (this.mBuffer3 == null)
		{
			this.mBuffer3 = new Color32[16];
		}
		int i = 0;
		int num = this.mBuffer3.Length;
		while (i < num)
		{
			this.mBuffer3[i] = this.map.fow.fogColor;
			i++;
		}
		this.hiddenTexture.SetPixels32(this.mBuffer3);
		this.hiddenTexture.Apply();
		if (this.map.fow.fowSystem == NJGMapBase.FOW.FOWSystem.BuiltInFOW)
		{
			if (this.mTexture0 == null)
			{
				this.mTexture0 = new Texture2D(this.map.fow.textureSize, this.map.fow.textureSize, TextureFormat.ARGB32, false);
				this.mTexture0.wrapMode = TextureWrapMode.Clamp;
			}
			this.mOrigin = Vector2.zero;
			this.mOrigin.x = this.mOrigin.x - (float)this.map.fow.textureSize * 0.5f;
			this.mOrigin.y = this.mOrigin.y - (float)this.map.fow.textureSize * 0.5f;
			int num2 = this.map.fow.textureSize * this.map.fow.textureSize;
			if (this.mBuffer0 == null)
			{
				this.mBuffer0 = new Color32[num2];
			}
			if (this.mBuffer1 == null)
			{
				this.mBuffer1 = new Color32[num2];
			}
			if (this.mBuffer2 == null)
			{
				this.mBuffer2 = new Color32[num2];
			}
			int j = 0;
			int num3 = this.mBuffer0.Length;
			while (j < num3)
			{
				this.mBuffer0[j] = Color.clear;
				this.mBuffer1[j] = Color.clear;
				this.mBuffer2[j] = Color.clear;
				j++;
			}
			this.UpdateBuffer();
			this.UpdateTexture();
			if (UIMiniMapBase.inst != null)
			{
				UIMiniMapBase.inst.material.SetTexture("_Revealed", this.mTexture0);
				UIMiniMapBase.inst.material.SetTexture("_Hidden", this.hiddenTexture);
			}
			this.mNextUpdate = Time.time + this.map.fow.updateFrequency;
			if (this.mThread == null)
			{
				this.mThread = new Thread(new ThreadStart(this.ThreadUpdate));
				this.mThread.Start();
			}
		}
		else if (this.map.fow.fowSystem == NJGMapBase.FOW.FOWSystem.TasharenFOW)
		{
		}
	}

	private void OnDestroy()
	{
		if (this.mThread != null)
		{
			this.mThread.Abort();
			while (this.mThread.IsAlive)
			{
				Thread.Sleep(1);
			}
			this.mThread = null;
		}
	}

	private void ThreadUpdate()
	{
		Stopwatch stopwatch = new Stopwatch();
		while (true)
		{
			if (this.mState == NJGFOW.State.NeedUpdate)
			{
				stopwatch.Reset();
				stopwatch.Start();
				this.UpdateBuffer();
				stopwatch.Stop();
				this.mState = NJGFOW.State.UpdateTexture0;
			}
			Thread.Sleep(1);
		}
	}

	private void Update()
	{
		if (this.map == null)
		{
			return;
		}
		if (this.map.fow.textureBlendTime > 0f)
		{
			this.mBlendFactor = Mathf.Clamp01(this.mBlendFactor + Time.deltaTime / this.map.fow.textureBlendTime);
		}
		else
		{
			this.mBlendFactor = 1f;
		}
		if (this.mState == NJGFOW.State.Blending)
		{
			float time = Time.time;
			if (this.mNextUpdate < time)
			{
				this.mNextUpdate = time + this.map.fow.updateFrequency;
				this.mState = NJGFOW.State.NeedUpdate;
			}
		}
		else if (this.mState != NJGFOW.State.NeedUpdate)
		{
			this.UpdateTexture();
		}
	}

	public static NJGFOW.Revealer CreateRevealer()
	{
		NJGFOW.Revealer revealer = new NJGFOW.Revealer();
		revealer.isActive = false;
		FastList<NJGFOW.Revealer> obj = NJGFOW.mAdded;
		lock (obj)
		{
			NJGFOW.mAdded.Add(revealer);
		}
		return revealer;
	}

	public static void DeleteRevealer(NJGFOW.Revealer rev)
	{
		FastList<NJGFOW.Revealer> obj = NJGFOW.mRemoved;
		lock (obj)
		{
			NJGFOW.mRemoved.Add(rev);
		}
	}

	public byte[] GetRevealedBuffer()
	{
		int num = this.map.fow.textureSize * this.map.fow.textureSize;
		byte[] array = new byte[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = this.mBuffer1[i].g;
		}
		return array;
	}

	public void RevealFOW(byte[] arr)
	{
		int num = this.map.fow.textureSize * this.map.fow.textureSize;
		if (arr.Length != num)
		{
			global::Debug.LogError(new object[]
			{
				string.Concat(new object[]
				{
					"Buffer size mismatch. Fog is ",
					num,
					", but passed array is ",
					arr.Length
				})
			});
		}
		else
		{
			if (this.mBuffer0 == null)
			{
				this.mBuffer0 = new Color32[num];
				this.mBuffer1 = new Color32[num];
			}
			for (int i = 0; i < num; i++)
			{
				this.mBuffer0[i].g = arr[i];
				this.mBuffer1[i].g = arr[i];
			}
		}
	}

	public void RevealFOW(string fowData)
	{
		int num = this.map.fow.textureSize * this.map.fow.textureSize;
		string[] array = fowData.Split(new char[]
		{
			'|'
		});
		if (array.Length != num)
		{
			global::Debug.LogError(new object[]
			{
				string.Concat(new object[]
				{
					"Buffer size mismatch. Fog is ",
					num,
					", but passed array is ",
					array.Length
				})
			});
		}
		else
		{
			if (this.mBuffer0 == null)
			{
				this.mBuffer0 = new Color32[num];
				this.mBuffer1 = new Color32[num];
			}
			for (int i = 0; i < num; i++)
			{
				this.mBuffer0[i].g = byte.Parse(array[i]);
				this.mBuffer1[i].g = byte.Parse(array[i]);
			}
		}
	}

	private string SerializeFOW()
	{
		int num = this.map.fow.textureSize * this.map.fow.textureSize;
		string[] array = new string[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = string.Empty + this.mBuffer1[i].g;
		}
		return string.Join("|", array);
	}

	private void Save(string gameName)
	{
		string value = this.SerializeFOW();
		if (!string.IsNullOrEmpty(value))
		{
			PlayerPrefs.SetString(gameName + "NJGFOW", value);
		}
	}

	private void Load(string gameName)
	{
		this.RevealFOW(PlayerPrefs.GetString(gameName + "NJGFOW", null));
	}

	public void ResetFOW()
	{
		if (this.map.fow.fowSystem == NJGMapBase.FOW.FOWSystem.BuiltInFOW)
		{
			for (int i = 0; i < NJGMapItem.list.Count; i++)
			{
				NJGMapItem nJGMapItem = NJGMapItem.list[i];
				if (nJGMapItem.cachedTransform != UIMiniMapBase.inst.target)
				{
					nJGMapItem.isRevealed = false;
				}
			}
		}
		this.Init();
	}

	private void UpdateTexture()
	{
		if (this.mState == NJGFOW.State.UpdateTexture0)
		{
			this.mTexture0.SetPixels32(this.mBuffer0);
			this.mTexture0.Apply();
			this.mState = NJGFOW.State.UpdateTexture1;
			this.mBlendFactor = 0f;
		}
		else if (this.mState == NJGFOW.State.UpdateTexture1)
		{
			this.mState = NJGFOW.State.Blending;
		}
	}

	private void UpdateBuffer()
	{
		if (NJGFOW.mAdded.size > 0)
		{
			FastList<NJGFOW.Revealer> obj = NJGFOW.mAdded;
			lock (obj)
			{
				while (NJGFOW.mAdded.size > 0)
				{
					int num = NJGFOW.mAdded.size - 1;
					NJGFOW.mRevealers.Add(NJGFOW.mAdded.buffer[num]);
					NJGFOW.mAdded.RemoveAt(num);
				}
			}
		}
		if (NJGFOW.mRemoved.size > 0)
		{
			FastList<NJGFOW.Revealer> obj2 = NJGFOW.mRemoved;
			lock (obj2)
			{
				while (NJGFOW.mRemoved.size > 0)
				{
					int num2 = NJGFOW.mRemoved.size - 1;
					NJGFOW.mRevealers.Remove(NJGFOW.mRemoved.buffer[num2]);
					NJGFOW.mRemoved.RemoveAt(num2);
				}
			}
		}
		float t = (this.map.fow.textureBlendTime <= 0f) ? 1f : Mathf.Clamp01(this.mBlendFactor + this.map.elapsed / this.map.fow.textureBlendTime);
		if (this.mBuffer0 != null)
		{
			int i = 0;
			int num3 = this.mBuffer0.Length;
			while (i < num3)
			{
				this.mBuffer0[i] = Color32.Lerp(this.mBuffer0[i], this.mBuffer1[i], t);
				this.mBuffer0[i].r = 0;
				i++;
			}
		}
		float worldToTex = (float)(this.map.fow.textureSize / this.map.fow.textureSize);
		for (int num4 = 0; num4 != NJGFOW.mRevealers.size; num4++)
		{
			NJGFOW.Revealer revealer = NJGFOW.mRevealers[num4];
			if (revealer.isActive)
			{
				this.RevealAtPosition(revealer, worldToTex);
			}
		}
		for (int num5 = 0; num5 != this.map.fow.blurIterations; num5++)
		{
			this.BlurVisibility();
		}
		this.RevealMap();
	}

	private void BlurVisibility()
	{
		for (int i = 0; i < this.map.fow.textureSize; i++)
		{
			int num = i * this.map.fow.textureSize;
			int num2 = i - 1;
			if (num2 < 0)
			{
				num2 = 0;
			}
			int num3 = i + 1;
			if (num3 == this.map.fow.textureSize)
			{
				num3 = i;
			}
			num2 *= this.map.fow.textureSize;
			num3 *= this.map.fow.textureSize;
			for (int j = 0; j < this.map.fow.textureSize; j++)
			{
				int num4 = j - 1;
				if (num4 < 0)
				{
					num4 = 0;
				}
				int num5 = j + 1;
				if (num5 == this.map.fow.textureSize)
				{
					num5 = j;
				}
				int num6 = j + num;
				int num7 = (int)this.mBuffer1[num6].r;
				num7 += (int)this.mBuffer1[num4 + num].r;
				num7 += (int)this.mBuffer1[num5 + num].r;
				num7 += (int)this.mBuffer1[j + num2].r;
				num7 += (int)this.mBuffer1[j + num3].r;
				num7 += (int)this.mBuffer1[num4 + num2].r;
				num7 += (int)this.mBuffer1[num5 + num2].r;
				num7 += (int)this.mBuffer1[num4 + num3].r;
				num7 += (int)this.mBuffer1[num5 + num3].r;
				Color32 color = this.mBuffer2[num6];
				color.r = (byte)(num7 / 9);
				this.mBuffer2[num6] = color;
				if (this.map.fow.trailEffect)
				{
					this.mBuffer2[num6].a = 0;
					this.mBuffer2[num6].g = 0;
					this.mBuffer2[num6].b = 0;
				}
			}
		}
		Color32[] array = this.mBuffer1;
		this.mBuffer1 = this.mBuffer2;
		this.mBuffer2 = array;
	}

	private void RevealAtPosition(NJGFOW.Revealer r, float worldToTex)
	{
		Vector2 vector = r.pos - this.mOrigin;
		int num = Mathf.RoundToInt((vector.x - (float)r.revealDistance) * worldToTex);
		int num2 = Mathf.RoundToInt((vector.y - (float)r.revealDistance) * worldToTex);
		int num3 = Mathf.RoundToInt((vector.x + (float)r.revealDistance) * worldToTex);
		int num4 = Mathf.RoundToInt((vector.y + (float)r.revealDistance) * worldToTex);
		int num5 = Mathf.RoundToInt(vector.x * worldToTex);
		int num6 = Mathf.RoundToInt(vector.y * worldToTex);
		int textureSize = this.map.fow.textureSize;
		num5 = Mathf.Clamp(num5, 0, textureSize - 1);
		num6 = Mathf.Clamp(num6, 0, textureSize - 1);
		int num7 = Mathf.RoundToInt((float)(r.revealDistance * r.revealDistance) * worldToTex * worldToTex);
		for (int i = num2; i < num4; i++)
		{
			if (i > -1 && i < textureSize)
			{
				int num8 = i * textureSize;
				for (int j = num; j < num3; j++)
				{
					if (j > -1 && j < textureSize)
					{
						int num9 = j - num5;
						int num10 = i - num6;
						int num11 = num9 * num9 + num10 * num10;
						if (num11 < num7)
						{
							this.mBuffer1[j + num8].r = 255;
						}
					}
				}
			}
		}
	}

	private void RevealMap()
	{
		for (int i = 0; i < this.map.fow.textureSize; i++)
		{
			int num = i * this.map.fow.textureSize;
			for (int j = 0; j < this.map.fow.textureSize; j++)
			{
				int num2 = j + num;
				Color32 color = this.mBuffer1[num2];
				if (color.g < color.r)
				{
					color.g = color.r;
					this.mBuffer1[num2] = color;
				}
			}
		}
	}

	public bool IsVisible(Vector2 pos)
	{
		if (this.mBuffer0 == null)
		{
			return false;
		}
		pos -= this.mOrigin;
		float num = (float)this.map.fow.textureSize / (float)this.map.fow.textureSize;
		int num2 = Mathf.RoundToInt(pos.x * num);
		int num3 = Mathf.RoundToInt(pos.y * num);
		num2 = Mathf.Clamp(num2, 0, this.map.fow.textureSize - 1);
		num3 = Mathf.Clamp(num3, 0, this.map.fow.textureSize - 1);
		int num4 = num2 + num3 * this.map.fow.textureSize;
		return this.mBuffer1[num4].r > 0 || this.mBuffer0[num4].r > 0;
	}

	public bool IsExplored(Vector2 pos)
	{
		if (this.mBuffer0 == null)
		{
			return false;
		}
		pos -= this.mOrigin;
		float num = (float)this.map.fow.textureSize / (float)this.map.fow.textureSize;
		int num2 = Mathf.RoundToInt(pos.x * num);
		int num3 = Mathf.RoundToInt(pos.y * num);
		num2 = Mathf.Clamp(num2, 0, this.map.fow.textureSize - 1);
		num3 = Mathf.Clamp(num3, 0, this.map.fow.textureSize - 1);
		return this.mBuffer0[num2 + num3 * this.map.fow.textureSize].g > 0;
	}
}
