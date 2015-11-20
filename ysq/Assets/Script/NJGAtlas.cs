using System;
using System.Collections.Generic;
using UnityEngine;

public class NJGAtlas : MonoBehaviour
{
	[Serializable]
	public class Sprite
	{
		public int id;

		public string name;

		public Rect uvs;

		public bool initialized;

		private Vector2 mPos;

		private Vector2 mSize;

		public Vector2 position
		{
			get
			{
				if (this.mPos == Vector2.zero)
				{
					this.mPos = new Vector2(this.uvs.x, this.uvs.y);
				}
				return this.mPos;
			}
		}

		public Vector2 size
		{
			get
			{
				if (this.mSize == Vector2.zero)
				{
					this.mSize = new Vector2(this.width, this.height);
				}
				return this.mSize;
			}
		}

		public float width
		{
			get
			{
				return this.uvs.width;
			}
		}

		public float height
		{
			get
			{
				return this.uvs.height;
			}
		}
	}

	public Shader shader;

	public int size = 2048;

	public int padding = 1;

	public NJGAtlas.Sprite[] sprites;

	public Texture2D texture;

	public Material spriteMaterial;

	private List<string> mNames = new List<string>();

	public NJGAtlas.Sprite GetSprite(int id)
	{
		return this.sprites[id];
	}

	public NJGAtlas.Sprite GetSprite(string spriteName)
	{
		int i = 0;
		int num = this.sprites.Length;
		while (i < num)
		{
			if (this.sprites[i] != null && !string.IsNullOrEmpty(this.sprites[i].name) && this.sprites[i].name == spriteName)
			{
				return this.sprites[i];
			}
			i++;
		}
		return null;
	}

	public List<string> GetListOfSprites()
	{
		this.mNames.Clear();
		int i = 0;
		int num = this.sprites.Length;
		while (i < num)
		{
			this.mNames.Add(this.sprites[i].name);
			i++;
		}
		return this.mNames;
	}

	public List<string> GetListOfSprites(string match)
	{
		if (string.IsNullOrEmpty(match))
		{
			return this.GetListOfSprites();
		}
		List<string> list = new List<string>();
		int i = 0;
		int num = this.sprites.Length;
		while (i < num)
		{
			NJGAtlas.Sprite sprite = this.sprites[i];
			if (sprite != null && !string.IsNullOrEmpty(sprite.name) && string.Equals(match, sprite.name, StringComparison.OrdinalIgnoreCase))
			{
				list.Add(sprite.name);
				return list;
			}
			i++;
		}
		string[] array = match.Split(new char[]
		{
			' '
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int j = 0; j < array.Length; j++)
		{
			array[j] = array[j].ToLower();
		}
		int k = 0;
		int num2 = this.sprites.Length;
		while (k < num2)
		{
			NJGAtlas.Sprite sprite2 = this.sprites[k];
			if (sprite2 != null && !string.IsNullOrEmpty(sprite2.name))
			{
				string text = sprite2.name.ToLower();
				int num3 = 0;
				for (int l = 0; l < array.Length; l++)
				{
					if (text.Contains(array[l]))
					{
						num3++;
					}
				}
				if (num3 == array.Length)
				{
					list.Add(sprite2.name);
				}
			}
			k++;
		}
		return list;
	}

	public void CreateSprite(GameObject go, Rect uvRect, Color color)
	{
		MeshFilter meshFilter = (MeshFilter)go.GetComponent(typeof(MeshFilter));
		if (meshFilter == null)
		{
			meshFilter = (MeshFilter)go.AddComponent(typeof(MeshFilter));
		}
		Mesh mesh = meshFilter.sharedMesh;
		if (mesh == null)
		{
			mesh = new Mesh();
		}
		mesh.Clear();
		MeshRenderer meshRenderer = (MeshRenderer)go.GetComponent(typeof(MeshRenderer));
		if (meshRenderer == null)
		{
			meshRenderer = (MeshRenderer)go.AddComponent(typeof(MeshRenderer));
		}
		meshRenderer.renderer.sharedMaterial = this.spriteMaterial;
		float num = (float)this.texture.width * 0.5f;
		float num2 = (float)this.texture.height * 0.5f;
		Vector3[] vertices = new Vector3[]
		{
			new Vector3(-num, -num2, 0f),
			new Vector3(-num, num2, 0f),
			new Vector3(num, num2, 0f),
			new Vector3(num, -num2, 0f)
		};
		int[] triangles = new int[]
		{
			0,
			1,
			2,
			0,
			2,
			3
		};
		Vector2[] uv = new Vector2[]
		{
			new Vector2(uvRect.x, uvRect.y),
			new Vector2(uvRect.x, uvRect.y + uvRect.height),
			new Vector2(uvRect.x + uvRect.width, uvRect.y + uvRect.height),
			new Vector2(uvRect.x + uvRect.width, uvRect.y)
		};
		Color[] colors = new Color[]
		{
			color,
			color,
			color,
			color
		};
		mesh.vertices = vertices;
		mesh.colors = colors;
		mesh.uv = uv;
		mesh.triangles = triangles;
		meshFilter.sharedMesh = mesh;
	}

	public void ChangeSprite(Mesh mesh, Rect uvRect)
	{
		mesh.uv = new Vector2[]
		{
			new Vector2(uvRect.x, uvRect.y),
			new Vector2(uvRect.x, uvRect.y + uvRect.height),
			new Vector2(uvRect.x + uvRect.width, uvRect.y + uvRect.height),
			new Vector2(uvRect.x + uvRect.width, uvRect.y)
		};
	}

	public void ChangeColor(Mesh mesh, Color color)
	{
		mesh.colors = new Color[]
		{
			color,
			color,
			color,
			color
		};
	}
}
