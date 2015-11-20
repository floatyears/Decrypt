using System;
using UnityEngine;

public class GUISharePic : MonoBehaviour
{
	private static GUISharePic mInstance;

	private float screenScale;

	private UITexture mFg;

	private UISprite mDescSp;

	private UILabel mServerName;

	private UILabel mID;

	private UILabel mBattleNum;

	private UILabel mFromDesc;

	private Camera mRenderCamera;

	private RenderTexture mRenderTexture;

	public static GUISharePic Instance
	{
		get
		{
			if (GUISharePic.mInstance == null)
			{
				GUISharePic.CreateInstance();
			}
			return GUISharePic.mInstance;
		}
	}

	private static void CreateInstance()
	{
		if (GUISharePic.mInstance != null)
		{
			return;
		}
		GameObject gameObject = Res.LoadGUI("GUI/GUISharePic");
		if (gameObject == null)
		{
			global::Debug.LogError(new object[]
			{
				"Res.Load GUI/GUISharePic error"
			});
			return;
		}
		GameObject gameObject2 = NGUITools.AddChild(GameUIManager.mInstance.uiRoot.gameObject, gameObject);
		if (gameObject2 == null)
		{
			global::Debug.LogError(new object[]
			{
				"AddChild GUISharePic error"
			});
			return;
		}
		GUISharePic.mInstance = gameObject2.AddComponent<GUISharePic>();
		int num = Screen.height;
		float num2 = GameUIManager.mInstance.uiRoot.GetPixelSizeAdjustment(num);
		num = Mathf.CeilToInt((float)num * num2);
		num2 = (float)num / 640f;
		gameObject2.transform.localPosition = new Vector3(0f, 0f, 10000f);
		gameObject2.transform.localScale = new Vector3(num2, num2, num2);
		GUISharePic.mInstance.screenScale = num2;
	}

	private void Awake()
	{
		this.mRenderCamera = base.transform.GetComponent<Camera>();
		this.mRenderTexture = new RenderTexture(1136, 640, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		this.mRenderCamera.targetTexture = this.mRenderTexture;
		Transform transform = base.transform.Find("UIMiddle");
		this.mFg = transform.Find("Fg").GetComponent<UITexture>();
		this.mDescSp = this.mFg.transform.Find("desc").GetComponent<UISprite>();
		this.mServerName = transform.Find("txt0/num").GetComponent<UILabel>();
		this.mID = transform.Find("txt1/num").GetComponent<UILabel>();
		this.mBattleNum = transform.Find("txt2/num").GetComponent<UILabel>();
		this.mFromDesc = transform.Find("from").GetComponent<UILabel>();
	}

	public void Refresh(string shareTxt, string fromTxt, Texture2D shareBgImg)
	{
		if (shareBgImg != null)
		{
			this.mFg.mainTexture = shareBgImg;
			this.mFg.transform.localPosition = new Vector3(-78f, 60f, 0f);
			float num = this.screenScale * 400f / (float)shareBgImg.height;
			int num2 = Mathf.CeilToInt((float)shareBgImg.width * num);
			int height = Mathf.CeilToInt((float)shareBgImg.height * num);
			if (num2 < 600)
			{
				num2 = 600;
			}
			this.mFg.width = num2;
			this.mFg.height = height;
			this.mDescSp.spriteName = null;
		}
		else
		{
			this.mDescSp.spriteName = shareTxt;
		}
		this.mServerName.text = Singleton<StringManager>.Instance.GetString("sharePic0", new object[]
		{
			GameSetting.ServerID,
			GameSetting.Data.ServerName
		});
		this.mID.text = Globals.Instance.Player.Data.Name;
		this.mBattleNum.text = Globals.Instance.Player.TeamSystem.GetCombatValue().ToString();
		if (!string.IsNullOrEmpty(fromTxt))
		{
			this.mFromDesc.text = fromTxt;
		}
	}

	public Texture2D TakeSnapShot()
	{
		RenderTexture.active = this.mRenderTexture;
		Texture2D texture2D = new Texture2D(this.mRenderTexture.width, this.mRenderTexture.height, TextureFormat.ARGB32, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), 0, 0);
		RenderTexture.active = null;
		return texture2D;
	}

	public void DestroySelf()
	{
		UnityEngine.Object.Destroy(GUISharePic.mInstance.gameObject);
		GUISharePic.mInstance = null;
	}
}
