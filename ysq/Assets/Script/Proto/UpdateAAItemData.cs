using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "UpdateAAItemData")]
	[Serializable]
	public class UpdateAAItemData : IExtensible
	{
		private int _ID;

		private int _CurValue;

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

		[ProtoMember(2, IsRequired = false, Name = "CurValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CurValue
		{
			get
			{
				return this._CurValue;
			}
			set
			{
				this._CurValue = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
