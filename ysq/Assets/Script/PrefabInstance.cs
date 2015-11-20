using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrefabInstance : MonoBehaviour
{
	[Serializable]
	public class PrefabItem
	{
		public GameObject prefab;

		public GameObject obj;

		public Vector3 position = Vector3.zero;

		public Vector3 rotation = Vector3.zero;

		public Vector3 scale = Vector3.one;

		public int infoID;
	}

	public List<PrefabInstance.PrefabItem> prefabItems = new List<PrefabInstance.PrefabItem>();
}
