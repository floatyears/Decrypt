using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdatePayItem")]
	[Serializable]
	public class MS2C_UpdatePayItem : IExtensible
	{
		private int _ActivityID;

		private int _ProductID;

		private int _PayCount;

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

		[ProtoMember(2, IsRequired = false, Name = "ProductID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ProductID
		{
			get
			{
				return this._ProductID;
			}
			set
			{
				this._ProductID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "PayCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PayCount
		{
			get
			{
				return this._PayCount;
			}
			set
			{
				this._PayCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
