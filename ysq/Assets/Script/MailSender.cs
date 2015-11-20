using System;

public class MailSender : MailContentElementBase
{
	private UILabel mMailSender;

	private void CreateObjects()
	{
		this.mMailSender = base.transform.GetComponent<UILabel>();
		this.ElementPriority = 2;
	}

	public void InitMailSender(string sender)
	{
		if (this.mMailSender == null)
		{
			this.CreateObjects();
		}
		this.mMailSender.text = sender;
	}
}
