using Att;
using System;
using UnityEngine;

public class GUIMiniMap : MonoBehaviour
{
	private void Start()
	{
		NJGMap component = base.GetComponent<NJGMap>();
		GameObject gameObject = base.transform.FindChild("MiniMap").gameObject;
		UIMiniMap component2 = gameObject.GetComponent<UIMiniMap>();
		UIAnchor component3 = gameObject.GetComponent<UIAnchor>();
		component3.uiCamera = GameUIManager.mInstance.uiCamera.camera;
		component.generateMapTexture = false;
		SceneInfo sceneInfo = Globals.Instance.SenceMgr.sceneInfo;
		component.userMapTexture = Res.Load<Texture2D>(string.Format("MiniMap/{0}", Application.loadedLevelName), false);
		component.SetTexture(component.userMapTexture);
		component.optimize = true;
		component.zoneColor = new Color(0.870588243f, 0.6666667f, 0.329411775f);
		component.worldName = sceneInfo.Name;
		component2.target = Globals.Instance.ActorMgr.PlayerCtrler.gameObject.transform;
		component2.zoom = ((sceneInfo.Zoom > 0.1f) ? sceneInfo.Zoom : 3f);
	}

	private void OnDestroy()
	{
		while (NJGMapItem.list.Count > 0)
		{
			NJGMapItem obj = NJGMapItem.list[0];
			UnityEngine.Object.DestroyImmediate(obj);
		}
	}
}
