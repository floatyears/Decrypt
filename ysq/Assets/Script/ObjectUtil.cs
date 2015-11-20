using System;
using System.Text;
using UnityEngine;

public static class ObjectUtil
{
	public static GameObject FindChildObject(GameObject parent, string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		Component[] componentsInChildren = parent.GetComponentsInChildren(typeof(Transform), true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject.name == name)
			{
				return componentsInChildren[i].gameObject;
			}
		}
		return null;
	}

	public static uint GetTransformDepth(Transform trans)
	{
		uint num = 0u;
		Transform parent = trans.parent;
		while (parent != null)
		{
			num += 1u;
			parent = parent.parent;
		}
		return num;
	}

	public static string GetTransRootPath(Transform trans)
	{
		StringBuilder stringBuilder = new StringBuilder(trans.name);
		Transform parent = trans.parent;
		while (parent != null)
		{
			stringBuilder.Insert(0, '/');
			stringBuilder.Insert(0, parent.name);
			parent = parent.parent;
		}
		return stringBuilder.ToString();
	}

	public static Transform GetTransFromRoot(Transform trans, string path)
	{
		path = path.Replace('\\', '/');
		path = path.Trim(new char[]
		{
			'/'
		});
		int num = path.IndexOf('/');
		if (num < 0)
		{
			if (path.Equals(trans.name.ToLower()))
			{
				return trans;
			}
			return null;
		}
		else
		{
			if (!path.Substring(0, num).Equals(trans.name.ToLower()))
			{
				return null;
			}
			return trans.Find(path.Substring(num + 1, path.Length - num - 1));
		}
	}

	private static Vector3 KeepSign(Vector3 a, Vector3 b)
	{
		if (a.x >= 0f)
		{
			b.x = Mathf.Abs(b.x);
		}
		else
		{
			b.x = -b.x;
		}
		if (a.y >= 0f)
		{
			b.y = Mathf.Abs(b.y);
		}
		else
		{
			b.y = -b.y;
		}
		if (a.z >= 0f)
		{
			b.z = Mathf.Abs(b.z);
		}
		else
		{
			b.z = -b.z;
		}
		return b;
	}

	public static void AttachToParent(GameObject parent, GameObject child, Vector3 localPos, Quaternion localRot)
	{
		child.transform.parent = parent.transform;
		child.transform.localPosition = localPos;
		child.transform.localRotation = localRot;
	}

	public static void AttachToParentAndResetLocalTrans(GameObject parent, GameObject child)
	{
		child.transform.parent = parent.transform;
		child.transform.localPosition = Vector3.zero;
		child.transform.localRotation = Quaternion.identity;
		child.transform.localScale = Vector3.one;
	}

	public static void AttachToParentAndResetLocalPosAndRotation(GameObject parent, GameObject child)
	{
		child.transform.parent = parent.transform;
		child.transform.localPosition = Vector3.zero;
		child.transform.localRotation = Quaternion.identity;
	}

	public static void AttachToParentAndKeepWorldTrans(GameObject parent, GameObject child)
	{
		Vector3 position = child.transform.position;
		Quaternion rotation = child.transform.rotation;
		child.transform.parent = parent.transform;
		child.transform.position = position;
		child.transform.rotation = rotation;
	}

	public static void AttachToParentAndKeepLocalTrans(GameObject parent, GameObject child)
	{
		Vector3 localPosition = child.transform.localPosition;
		Quaternion localRotation = child.transform.localRotation;
		child.transform.parent = parent.transform;
		child.transform.localPosition = localPosition;
		child.transform.localRotation = localRotation;
	}

	public static void DestroyChildObjects(GameObject parent)
	{
		Component[] componentsInChildren = parent.GetComponentsInChildren(typeof(Transform), true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!(componentsInChildren[i].gameObject == parent))
			{
				UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
			}
		}
	}

	public static void SetObjectLayer(GameObject obj, int layer)
	{
		Component[] componentsInChildren = obj.GetComponentsInChildren(typeof(Transform), true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = layer;
		}
	}

	public static void UnifyWorldTrans(GameObject src, GameObject dst)
	{
		dst.transform.position = src.transform.position;
		dst.transform.rotation = src.transform.rotation;
	}

	public static Bounds GetMaxBounds(GameObject obj)
	{
		Bounds result = new Bounds(obj.transform.position, Vector3.zero);
		Vector3 min = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		Vector3 max = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
		bool flag = false;
		Component[] componentsInChildren = obj.GetComponentsInChildren(typeof(Renderer), true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Renderer renderer = componentsInChildren[i].renderer;
			if (!(renderer is ParticleRenderer))
			{
				Bounds bounds = renderer.bounds;
				if (bounds.min.x < min.x)
				{
					min.x = bounds.min.x;
				}
				if (bounds.min.y < min.y)
				{
					min.y = bounds.min.y;
				}
				if (bounds.min.z < min.z)
				{
					min.z = bounds.min.z;
				}
				if (bounds.max.x > max.x)
				{
					max.x = bounds.max.x;
				}
				if (bounds.max.y > max.y)
				{
					max.y = bounds.max.y;
				}
				if (bounds.max.z > max.z)
				{
					max.z = bounds.max.z;
				}
				flag = true;
			}
		}
		if (flag)
		{
			result.SetMinMax(min, max);
		}
		return result;
	}
}
