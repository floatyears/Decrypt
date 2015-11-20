using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "ActivityHalloweenItem")]
	[Serializable]
	public class ActivityHalloweenItem : IExtensible
	{
		private int _ID;

		private int _Cost;

		private int _Exchange;

		private readonly List<RewardData> _Rewards = new List<RewardData>();

		private int _DropID;

		private int _FireReturnRate;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Cost", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Cost
		{
			get
			{
				return this._Cost;
			}
			set
			{
				this._Cost = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Exchange", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Exchange
		{
			get
			{
				return this._Exchange;
			}
			set
			{
				this._Exchange = value;
			}
		}

		[ProtoMember(4, Name = "Rewards", DataFormat = DataFormat.Default)]
		public List<RewardData> Rewards
		{
			get
			{
				return this._Rewards;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "DropID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int DropID
		{
			get
			{
				return this._DropID;
			}
			set
			{
				this._DropID = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "FireReturnRate", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FireReturnRate
		{
			get
			{
				return this._FireReturnRate;
			}
			set
			{
				this._FireReturnRate = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
