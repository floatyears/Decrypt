using System;
using UnityEngine;

[ExecuteInEditMode]
public class BRDFLookupTexture : MonoBehaviour
{
	public float intensity = 1f;

	public float diffuseIntensity = 1f;

	public Color keyColor = BRDFLookupTexture.ColorRGB(188, 158, 118);

	public Color fillColor = BRDFLookupTexture.ColorRGB(86, 91, 108);

	public Color backColor = BRDFLookupTexture.ColorRGB(44, 54, 57);

	public float wrapAround;

	public float metalic;

	public float specularIntensity = 1f;

	public float specularShininess = 0.078125f;

	public float translucency;

	public Color translucentColor = BRDFLookupTexture.ColorRGB(255, 82, 82);

	public int lookupTextureWidth = 128;

	public int lookupTextureHeight = 128;

	public bool fastPreview = true;

	public Texture2D lookupTexture;

	private void Awake()
	{
		if (!this.lookupTexture)
		{
			this.Bake();
		}
	}

	private static Color ColorRGB(int r, int g, int b)
	{
		return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f, 0f);
	}

	private void CheckConsistency()
	{
		this.intensity = Mathf.Max(0f, this.intensity);
		this.wrapAround = Mathf.Clamp(this.wrapAround, -1f, 1f);
		this.metalic = Mathf.Clamp(this.metalic, 0f, 12f);
		this.diffuseIntensity = Mathf.Max(0f, this.diffuseIntensity);
		this.specularIntensity = Mathf.Max(0f, this.specularIntensity);
		this.specularShininess = Mathf.Clamp(this.specularShininess, 0.01f, 1f);
		this.translucency = Mathf.Clamp01(this.translucency);
	}

	private Color PixelFunc(float ndotl, float ndoth)
	{
		ndotl *= Mathf.Pow(ndoth, this.metalic);
		float num = (1f + this.metalic * 0.25f) * Mathf.Max(0f, this.diffuseIntensity - (1f - ndoth) * this.metalic);
		float t = Mathf.Clamp01(Mathf.InverseLerp(-this.wrapAround, 1f, ndotl * 2f - 1f));
		float t2 = Mathf.Clamp01(Mathf.InverseLerp(-1f, Mathf.Max(-0.99f, -this.wrapAround), ndotl * 2f - 1f));
		Color a = num * Color.Lerp(this.backColor, Color.Lerp(this.fillColor, this.keyColor, t), t2);
		a += this.backColor * (1f - num) * Mathf.Clamp01(this.diffuseIntensity);
		float num2 = this.specularShininess * 128f;
		float num3 = (num2 + 2f) * (num2 + 4f) / (25.1327419f * (Mathf.Pow(2f, -num2 / 2f) + num2));
		float a2 = this.specularIntensity * num3 * Mathf.Pow(ndoth, num2);
		float num4 = ndotl + 0.1f;
		float b = 0.5f * this.translucency * Mathf.Clamp01(1f - num4 * ndoth) * Mathf.Clamp01(1f - ndotl);
		Color a3 = a * this.intensity + this.translucentColor * b + new Color(0f, 0f, 0f, a2);
		return a3 * this.intensity;
	}

	private void TextureFunc(Texture2D tex)
	{
		for (int i = 0; i < tex.height; i++)
		{
			for (int j = 0; j < tex.width; j++)
			{
				float num = (float)tex.width;
				float num2 = (float)tex.height;
				float num3 = (float)j / num;
				float num4 = (float)i / num2;
				float ndotl = num3;
				float ndoth = num4;
				Color color = this.PixelFunc(ndotl, ndoth);
				tex.SetPixel(j, i, color);
			}
		}
	}

	private void GenerateLookupTexture(int width, int height)
	{
		Texture2D texture2D;
		if (this.lookupTexture && this.lookupTexture.width == width && this.lookupTexture.height == height)
		{
			texture2D = this.lookupTexture;
		}
		else
		{
			texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
		}
		this.CheckConsistency();
		this.TextureFunc(texture2D);
		texture2D.Apply();
		texture2D.wrapMode = TextureWrapMode.Clamp;
		if (this.lookupTexture != texture2D)
		{
			UnityEngine.Object.DestroyImmediate(this.lookupTexture);
		}
		this.lookupTexture = texture2D;
	}

	public void Preview()
	{
		this.GenerateLookupTexture(32, 64);
	}

	public void Bake()
	{
		this.GenerateLookupTexture(this.lookupTextureWidth, this.lookupTextureHeight);
	}
}
