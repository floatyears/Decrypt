using System;

public class MailTitle : MailContentElementBase
{
	private UILabel mMailTitle;

	private void CreateObjects()
	{
		this.mMailTitle = base.transform.GetComponent<UILabel>();
		this.ElementPriority = 0;
	}

	public void InitMailTitle(string title)
	{
		if (this.mMailTitle == null)
		{
			this.CreateObjects();
		}
		this.mMailTitle.text = title;
	}
}
