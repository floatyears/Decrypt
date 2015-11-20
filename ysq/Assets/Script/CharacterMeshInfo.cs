using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMeshInfo
{
	private enum LerpType
	{
		None,
		Fadein,
		Fadeout,
		ToDefault,
		ToGloss,
		Hold
	}

	private Renderer renderComponent;

	private Material[] materials;

	private List<Shader> initShader = new List<Shader>();

	private List<Color> RendererComponentInitColor = new List<Color>();

	private List<float> RendererComponentInitGloss = new List<float>();

	private Color RendererComponentColor = Color.red;

	private float RendererComponentGloss = 4f;

	private Shader transparentShader;

	private CharacterMeshInfo.LerpType lerpType;

	private float holeColorDuration;

	private float lerpColorDuration;

	private float colorTimestamp;

	public Renderer RenderComponent
	{
		get
		{
			return this.renderComponent;
		}
		set
		{
			if (value != this.renderComponent)
			{
				this.renderComponent = value;
				this.materials = this.renderComponent.materials;
				this.initShader.Clear();
				this.RendererComponentInitColor.Clear();
				this.RendererComponentInitGloss.Clear();
				for (int i = 0; i < this.materials.Length; i++)
				{
					Material material = this.materials[i];
					if (!material.shader.name.Contains("Transparent"))
					{
						this.initShader.Add(material.shader);
						if (material.HasProperty("_Color"))
						{
							this.RendererComponentInitColor.Add(material.color);
						}
						else
						{
							this.RendererComponentInitColor.Add(Color.white);
						}
						if (material.HasProperty("_Gloss"))
						{
							this.RendererComponentInitGloss.Add(material.GetFloat("_Gloss"));
						}
						else
						{
							this.RendererComponentInitGloss.Add(1f);
						}
					}
				}
			}
		}
	}

	public Material[] Materials
	{
		get
		{
			return this.materials;
		}
		set
		{
			this.materials = value;
			if (this.renderComponent != null)
			{
				this.renderComponent.materials = this.materials;
			}
		}
	}

	public List<Shader> InitShader
	{
		get
		{
			return this.initShader;
		}
	}

	public CharacterMeshInfo(Renderer render)
	{
		global::Debug.Assert(!(render is ParticleRenderer), "can not support particle...");
		this.RenderComponent = render;
	}

	public float ChangeToColor(Color toColor, float Gloss, float duration, float holdTime)
	{
		this.RendererComponentColor = toColor;
		this.RendererComponentGloss = Gloss;
		this.lerpColorDuration = duration;
		this.holeColorDuration = holdTime;
		this.colorTimestamp = Time.time;
		this.lerpType = CharacterMeshInfo.LerpType.ToGloss;
		return duration;
	}

	public float ChangeToDefaultColor(float duration)
	{
		this.holeColorDuration = 0f;
		this.lerpColorDuration = duration;
		this.colorTimestamp = Time.time;
		this.lerpType = CharacterMeshInfo.LerpType.ToDefault;
		return duration;
	}

	public float StartFadein(float duration)
	{
		if (this.lerpType != CharacterMeshInfo.LerpType.Fadein)
		{
			this.lerpColorDuration = duration;
			this.colorTimestamp = Time.time;
			this.lerpType = CharacterMeshInfo.LerpType.Fadein;
		}
		return duration;
	}

	public float StartFadeout(float duration)
	{
		if (this.lerpType != CharacterMeshInfo.LerpType.Fadeout)
		{
			this.lerpColorDuration = duration;
			this.colorTimestamp = Time.time;
			this.lerpType = CharacterMeshInfo.LerpType.Fadeout;
		}
		return duration;
	}

	public void FixedUpdate()
	{
		if (this.renderComponent == null)
		{
			return;
		}
		float num = Time.time - this.colorTimestamp;
		switch (this.lerpType)
		{
		case CharacterMeshInfo.LerpType.Fadein:
			if (num > this.lerpColorDuration)
			{
				this.RollbackDefaultMaterial();
				this.lerpType = CharacterMeshInfo.LerpType.None;
			}
			else
			{
				if (this.transparentShader == null)
				{
					this.transparentShader = Shader.Find("Game/Characters/Transparent Diffuse");
				}
				float t = num / this.lerpColorDuration;
				for (int i = 0; i < this.materials.Length; i++)
				{
					Material material = this.materials[i];
					if (!(material == null))
					{
						if (material.shader != this.transparentShader && !material.shader.name.Contains("Particles"))
						{
							material.shader = this.transparentShader;
						}
						if (material.shader == this.transparentShader)
						{
							float a = Mathf.Lerp(0f, this.RendererComponentInitColor[i].a, t);
							material.color = new Color(material.color.r, material.color.g, material.color.b, a);
						}
					}
				}
				this.renderComponent.materials = this.materials;
			}
			break;
		case CharacterMeshInfo.LerpType.Fadeout:
		{
			if (this.transparentShader == null)
			{
				this.transparentShader = Shader.Find("Game/Characters/Transparent Diffuse");
			}
			float t2 = num / this.lerpColorDuration;
			for (int j = 0; j < this.materials.Length; j++)
			{
				Material material2 = this.materials[j];
				if (!(material2 == null))
				{
					if (material2.shader != this.transparentShader && !material2.shader.name.Contains("Particles"))
					{
						material2.shader = this.transparentShader;
					}
					if (material2.shader == this.transparentShader)
					{
						float a2 = Mathf.Lerp(this.RendererComponentInitColor[j].a, 0f, t2);
						material2.color = new Color(material2.color.r, material2.color.g, material2.color.b, a2);
					}
				}
			}
			this.renderComponent.materials = this.materials;
			if (num > this.lerpColorDuration)
			{
				this.lerpType = CharacterMeshInfo.LerpType.None;
			}
			break;
		}
		case CharacterMeshInfo.LerpType.ToDefault:
		{
			float t3 = num / this.lerpColorDuration;
			for (int k = 0; k < this.materials.Length; k++)
			{
				Material material3 = this.materials[k];
				if (!(material3 == null))
				{
					if (material3.HasProperty("_Color"))
					{
						Color color = Color.Lerp(material3.color, this.RendererComponentInitColor[k], t3);
						material3.color = color;
					}
					if (material3.HasProperty("_Gloss"))
					{
						float value = Mathf.Lerp(material3.GetFloat("_Gloss"), this.RendererComponentInitGloss[k], t3);
						material3.SetFloat("_Gloss", value);
					}
				}
			}
			this.renderComponent.materials = this.materials;
			if (num > this.lerpColorDuration)
			{
				this.lerpType = CharacterMeshInfo.LerpType.None;
			}
			break;
		}
		case CharacterMeshInfo.LerpType.ToGloss:
		{
			float t4 = num / this.lerpColorDuration;
			for (int l = 0; l < this.materials.Length; l++)
			{
				Material material4 = this.materials[l];
				if (!(material4 == null))
				{
					if (material4.HasProperty("_Color"))
					{
						Color color2 = Color.Lerp(material4.color, this.RendererComponentColor, num / this.lerpColorDuration);
						material4.color = color2;
					}
					if (material4.HasProperty("_Gloss"))
					{
						float value2 = Mathf.Lerp(material4.GetFloat("_Gloss"), this.RendererComponentGloss, t4);
						material4.SetFloat("_Gloss", value2);
					}
				}
			}
			this.renderComponent.materials = this.materials;
			if (num > this.lerpColorDuration)
			{
				this.lerpType = CharacterMeshInfo.LerpType.Hold;
				this.colorTimestamp = Time.time;
			}
			break;
		}
		case CharacterMeshInfo.LerpType.Hold:
			if (num > this.holeColorDuration)
			{
				this.ChangeToDefaultColor(this.lerpColorDuration);
			}
			break;
		}
	}

	private void RollbackDefaultMaterial()
	{
		for (int i = 0; i < this.materials.Length; i++)
		{
			Material material = this.materials[i];
			if (!(material == null))
			{
				material.shader = this.initShader[i];
				material.color = this.RendererComponentInitColor[i];
			}
		}
		this.renderComponent.materials = this.materials;
	}
}
