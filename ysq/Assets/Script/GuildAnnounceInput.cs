using System;

public class GuildAnnounceInput : UIInput
{
	public delegate void VoidCallback();

	public GuildAnnounceInput.VoidCallback mInputLoseFocus;

	protected override void OnSelect(bool isSelected)
	{
		base.OnSelect(isSelected);
		if (!isSelected && this.mInputLoseFocus != null)
		{
			this.mInputLoseFocus();
		}
	}
}
