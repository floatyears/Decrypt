using System;

public class ZoneItemInfoData : BaseData
{
	public int mShowNum;

	public string mName;

	public int mServerID;

	public int mState;

	public int mNew;

	public string mServerIP;

	public int mServerPort;

	public int mGender;

	private ulong ID;

	public ZoneItemInfoData(int showNum, string name, int serverID, int state, int mNew, string serverIP, int serverPort, int id, int gender)
	{
		this.mShowNum = showNum;
		this.mName = name;
		this.mServerID = serverID;
		this.mState = state;
		this.mNew = mNew;
		this.mServerIP = serverIP;
		this.mServerPort = serverPort;
		this.mGender = gender;
		this.ID = (ulong)((long)id);
	}

	public override ulong GetID()
	{
		return this.ID;
	}
}
