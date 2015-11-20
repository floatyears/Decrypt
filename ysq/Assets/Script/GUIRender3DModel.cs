using System;
using UnityEngine;

public class GUIRender3DModel : MonoBehaviour
{
	private UITexture mModelTexture;

	private Camera mRenderCamera;

	private RenderTexture mRenderTexture;

	private GameObject mRenderCameraGo;

	private GameObject mModelTmp;

	private ResourceEntity asyncEntiry;

	private void CreateRenderTexture()
	{
		if (this.mRenderTexture == null)
		{
			this.mRenderTexture = new RenderTexture(this.mModelTexture.width, this.mModelTexture.height, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Default);
			this.mRenderTexture.antiAliasing = 2;
			this.mRenderTexture.filterMode = FilterMode.Bilinear;
		}
	}

	private void CreateRenderCamera(GameObject go)
	{
		if (GameUIManager.mInstance != null)
		{
			this.mRenderCameraGo = new GameObject("RenderTextureCamera");
			this.mRenderCameraGo.transform.parent = go.transform;
			this.mRenderCameraGo.layer = LayerDefine.uiMeshLayer;
			this.mRenderCameraGo.transform.localPosition = new Vector3(2048f, 2048f, 0f);
			this.mRenderCameraGo.transform.localScale = Vector3.one;
			this.mRenderCamera = this.mRenderCameraGo.AddComponent<Camera>();
			this.mRenderCamera.clearFlags = CameraClearFlags.Color;
			this.mRenderCamera.cullingMask = 1 << LayerMask.NameToLayer("UIMesh");
			this.mRenderCamera.orthographic = true;
			this.mRenderCamera.orthographicSize = 1f;
			this.mRenderCamera.nearClipPlane = -5f;
			this.mRenderCamera.farClipPlane = 5f;
			this.mRenderCamera.depth = 0f;
			this.mRenderCamera.targetTexture = this.mRenderTexture;
			this.mRenderCamera.useOcclusionCulling = false;
			this.mRenderCamera.hdr = false;
		}
	}

	public void InitWithBaseScene(GameObject go)
	{
		this.mModelTexture = base.transform.GetComponent<UITexture>();
		this.CreateRenderTexture();
		this.CreateRenderCamera(go);
		this.mModelTexture.shader = Shader.Find("Unlit/Transparent Colored");
		this.mModelTexture.mainTexture = this.mRenderTexture;
	}

	private void ClearModel()
	{
		if (this.asyncEntiry != null)
		{
			ActorManager.CancelCreateUIActorAsync(this.asyncEntiry);
			this.asyncEntiry = null;
		}
		if (this.mModelTmp != null)
		{
			UnityEngine.Object.DestroyImmediate(this.mModelTmp);
			this.mModelTmp = null;
		}
	}

	public void CreateModel(int slot, float scale = 1f)
	{
		this.ClearModel();
		this.asyncEntiry = ActorManager.CreateLocalUIActor(slot, 0, false, false, this.mRenderCameraGo, scale, delegate(GameObject go)
		{
			this.asyncEntiry = null;
			this.mModelTmp = go;
			if (this.mModelTmp != null)
			{
				this.mModelTmp.transform.localPosition = new Vector3(0f, -300f, 0f);
				NGUITools.SetLayer(this.mModelTmp, LayerDefine.uiMeshLayer);
			}
		});
	}

	private void OnDestroy()
	{
		this.ClearModel();
		if (this.mRenderCameraGo != null)
		{
			UnityEngine.Object.Destroy(this.mRenderCameraGo);
			this.mRenderCameraGo = null;
		}
		this.mRenderTexture = null;
	}
}
