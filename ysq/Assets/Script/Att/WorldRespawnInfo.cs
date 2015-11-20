using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "WorldRespawnInfo")]
	[Serializable]
	public class WorldRespawnInfo : IExtensible
	{
		private int _ID;

		private int _SceneID;

		private readonly List<int> _InfoID1 = new List<int>();

		private readonly List<int> _MaxHP1 = new List<int>();

		private readonly List<int> _Rate1 = new List<int>();

		private int _RewardType;

		private int _RewardValue1;

		private int _RewardValue2;

		private int _IncScale;

		private int _MaxScale;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "ID", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(2, IsRequired = false, Name = "SceneID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneID
		{
			get
			{
				return this._SceneID;
			}
			set
			{
				this._SceneID = value;
			}
		}

		[ProtoMember(3, Name = "InfoID1", DataFormat = DataFormat.TwosComplement)]
		public List<int> InfoID1
		{
			get
			{
				return this._InfoID1;
			}
		}

		[ProtoMember(4, Name = "MaxHP1", DataFormat = DataFormat.TwosComplement)]
		public List<int> MaxHP1
		{
			get
			{
				return this._MaxHP1;
			}
		}

		[ProtoMember(5, Name = "Rate1", DataFormat = DataFormat.TwosComplement)]
		public List<int> Rate1
		{
			get
			{
				return this._Rate1;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "RewardType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardType
		{
			get
			{
				return this._RewardType;
			}
			set
			{
				this._RewardType = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "RewardValue1", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardValue1
		{
			get
			{
				return this._RewardValue1;
			}
			set
			{
				this._RewardValue1 = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "RewardValue2", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardValue2
		{
			get
			{
				return this._RewardValue2;
			}
			set
			{
				this._RewardValue2 = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "IncScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int IncScale
		{
			get
			{
				return this._IncScale;
			}
			set
			{
				this._IncScale = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "MaxScale", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MaxScale
		{
			get
			{
				return this._MaxScale;
			}
			set
			{
				this._MaxScale = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
