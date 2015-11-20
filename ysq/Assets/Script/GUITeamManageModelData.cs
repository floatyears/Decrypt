using System;

public class GUITeamManageModelData
{
	public SocketDataEx mSocketDataEx
	{
		get;
		private set;
	}

	public int mSocketSlotIndex
	{
		get;
		private set;
	}

	public GUITeamManageModelData(SocketDataEx pdEx, int slotIndex)
	{
		this.mSocketDataEx = pdEx;
		this.mSocketSlotIndex = slotIndex;
	}
}
