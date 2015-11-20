using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdateActivityPayDay")]
	[Serializable]
	public class MS2C_UpdateActivityPayDay : IExtensible
	{
		private int _ActivityID;

		private int _PayDay;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ActivityID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ActivityID
		{
			get
			{
				return this._ActivityID;
			}
			set
			{
				this._ActivityID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "PayDay", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PayDay
		{
			get
			{
				return this._PayDay;
			}
			set
			{
				this._PayDay = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
