using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_PvpPillageFarm")]
	[Serializable]
	public class MC2S_PvpPillageFarm : IExtensible
	{
		private ulong _TargetID;

		private int _ItemID;

		private int _Count;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "TargetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong TargetID
		{
			get
			{
				return this._TargetID;
			}
			set
			{
				this._TargetID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ItemID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ItemID
		{
			get
			{
				return this._ItemID;
			}
			set
			{
				this._ItemID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Count", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				this._Count = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
