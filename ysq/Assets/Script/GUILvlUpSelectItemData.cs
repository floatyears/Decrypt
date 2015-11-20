using System;

public class GUILvlUpSelectItemData : BaseData
{
	public PetDataEx mPetDataEx;

	public bool mIsSelected;

	public GUILvlUpSelectItemData(PetDataEx pdEx, bool isSel)
	{
		this.mPetDataEx = pdEx;
		this.mIsSelected = isSel;
	}

	public override ulong GetID()
	{
		return (this.mPetDataEx == null) ? 0uL : this.mPetDataEx.Data.ID;
	}

	public uint CanGetExpNum()
	{
		if (this.mPetDataEx != null)
		{
			return this.mPetDataEx.GetToExp();
		}
		return 0u;
	}
}
