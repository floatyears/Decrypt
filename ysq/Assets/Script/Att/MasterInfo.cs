using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Att
{
	[ProtoContract(Name = "MasterInfo")]
	[Serializable]
	public class MasterInfo : IExtensible
	{
		private int _ID;

		private int _EELevel;

		private readonly List<int> _EEAttID = new List<int>();

		private readonly List<int> _EEAttValue = new List<int>();

		private int _ERLevel;

		private readonly List<int> _ERAttID = new List<int>();

		private readonly List<int> _ERAttValue = new List<int>();

		private int _TELevel;

		private readonly List<int> _TEAttID = new List<int>();

		private readonly List<int> _TEAttValue = new List<int>();

		private int _TRLevel;

		private readonly List<int> _TRAttID = new List<int>();

		private readonly List<int> _TRAttValue = new List<int>();

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

		[ProtoMember(2, IsRequired = false, Name = "EELevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int EELevel
		{
			get
			{
				return this._EELevel;
			}
			set
			{
				this._EELevel = value;
			}
		}

		[ProtoMember(3, Name = "EEAttID", DataFormat = DataFormat.TwosComplement)]
		public List<int> EEAttID
		{
			get
			{
				return this._EEAttID;
			}
		}

		[ProtoMember(4, Name = "EEAttValue", DataFormat = DataFormat.TwosComplement)]
		public List<int> EEAttValue
		{
			get
			{
				return this._EEAttValue;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "ERLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ERLevel
		{
			get
			{
				return this._ERLevel;
			}
			set
			{
				this._ERLevel = value;
			}
		}

		[ProtoMember(6, Name = "ERAttID", DataFormat = DataFormat.TwosComplement)]
		public List<int> ERAttID
		{
			get
			{
				return this._ERAttID;
			}
		}

		[ProtoMember(7, Name = "ERAttValue", DataFormat = DataFormat.TwosComplement)]
		public List<int> ERAttValue
		{
			get
			{
				return this._ERAttValue;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "TELevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TELevel
		{
			get
			{
				return this._TELevel;
			}
			set
			{
				this._TELevel = value;
			}
		}

		[ProtoMember(9, Name = "TEAttID", DataFormat = DataFormat.TwosComplement)]
		public List<int> TEAttID
		{
			get
			{
				return this._TEAttID;
			}
		}

		[ProtoMember(10, Name = "TEAttValue", DataFormat = DataFormat.TwosComplement)]
		public List<int> TEAttValue
		{
			get
			{
				return this._TEAttValue;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "TRLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TRLevel
		{
			get
			{
				return this._TRLevel;
			}
			set
			{
				this._TRLevel = value;
			}
		}

		[ProtoMember(12, Name = "TRAttID", DataFormat = DataFormat.TwosComplement)]
		public List<int> TRAttID
		{
			get
			{
				return this._TRAttID;
			}
		}

		[ProtoMember(13, Name = "TRAttValue", DataFormat = DataFormat.TwosComplement)]
		public List<int> TRAttValue
		{
			get
			{
				return this._TRAttValue;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
