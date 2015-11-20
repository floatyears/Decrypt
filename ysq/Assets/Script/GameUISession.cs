using System;
using UnityEngine;

public abstract class GameUISession : MonoBehaviour
{
	private bool postLoadGUIDone;

	private bool destroyFlag;

	public bool PostLoadGUIDone
	{
		get
		{
			return this.postLoadGUIDone;
		}
	}

	public void _OnLoadedGUI()
	{
		this.OnPostLoadGUI();
		this.postLoadGUIDone = true;
	}

	public void _OnLoadedFinished()
	{
		this.OnLoadedFinished();
	}

	protected virtual void OnLoadedFinished()
	{
	}

	protected abstract void OnPostLoadGUI();

	protected abstract void OnPreDestroyGUI();

	protected virtual void OnApplicationQuit()
	{
		this.destroyFlag = true;
	}

	protected virtual void OnDestroy()
	{
		if (!this.destroyFlag && base.gameObject.activeInHierarchy)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("Destroy {0} Invalid", base.gameObject.name)
			});
		}
	}

	public void Close()
	{
		this.OnPreDestroyGUI();
		this.destroyFlag = true;
		UnityEngine.Object.Destroy(base.gameObject, 0.01f);
		GameUIManager.mInstance.RemoveSession(this);
	}

	public void CloseImmediate()
	{
		this.OnPreDestroyGUI();
		this.destroyFlag = true;
		UnityEngine.Object.DestroyImmediate(base.gameObject);
		GameUIManager.mInstance.RemoveSession(this);
	}

	public void CloseNotRemoveSession()
	{
		this.OnPreDestroyGUI();
		this.destroyFlag = true;
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}

	public GameObject RegisterClickEvent(string _name, UIEventListener.VoidDelegate delgate, GameObject parent = null)
	{
		if (parent == null)
		{
			parent = base.gameObject;
		}
		return GameUITools.RegisterClickEvent(_name, delgate, parent);
	}

	public GameObject RegisterPressEvent(string _name, UIEventListener.BoolDelegate delgate, GameObject parent = null)
	{
		if (parent == null)
		{
			parent = base.gameObject;
		}
		return GameUITools.RegisterPressEvent(_name, delgate, parent);
	}

	public GameObject RegisterDragEvent(string _name, UIEventListener.VectorDelegate delgate, GameObject parent = null)
	{
		if (parent == null)
		{
			parent = base.gameObject;
		}
		return GameUITools.RegisterDragEvent(_name, delgate, parent);
	}

	public UILabel SetLabelLocalText(string _name, string key, GameObject parent = null)
	{
		if (parent == null)
		{
			parent = base.gameObject;
		}
		return GameUITools.SetLabelLocalText(_name, key, parent);
	}

	public UILabel SetLabelLocalText(GameObject go, string key)
	{
		return GameUITools.SetLabelLocalText(go, key);
	}

	public GameObject FindGameObject(string _name, GameObject parent = null)
	{
		if (parent == null)
		{
			parent = base.gameObject;
		}
		return GameUITools.FindGameObject(_name, parent);
	}
}
