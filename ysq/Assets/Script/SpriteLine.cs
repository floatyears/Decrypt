using System;

public class SpriteLine : MailContentElementBase
{
	private void Awake()
	{
		this.InitSpriteLine();
	}

	public void InitSpriteLine()
	{
		this.ElementPriority = 3;
	}
}
