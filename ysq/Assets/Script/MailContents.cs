using System;

public class MailContents : MailContentElementBase
{
	private UILabel mMailContents;

	private void CreateObjects()
	{
		this.mMailContents = base.transform.GetComponent<UILabel>();
		this.mMailContents.spaceIsNewLine = false;
		this.ElementPriority = 1;
	}

	public void InitMailContents(string contents)
	{
		if (this.mMailContents == null)
		{
			this.CreateObjects();
		}
		this.mMailContents.text = contents;
	}
}
