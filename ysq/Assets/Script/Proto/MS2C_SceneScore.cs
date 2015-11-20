using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_SceneScore")]
	[Serializable]
	public class MS2C_SceneScore : IExtensible
	{
		private int _SceneID;

		private int _Score;

		private int _Times;

		private int _CoolDown;

		private uint _SceneVersion;

		private int _ResetCount;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "SceneID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(2, IsRequired = false, Name = "Score", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Score
		{
			get
			{
				return this._Score;
			}
			set
			{
				this._Score = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Times", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Times
		{
			get
			{
				return this._Times;
			}
			set
			{
				this._Times = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "CoolDown", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CoolDown
		{
			get
			{
				return this._CoolDown;
			}
			set
			{
				this._CoolDown = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "SceneVersion", DataFormat = DataFormat.TwosComplement), DefaultValue(0L)]
		public uint SceneVersion
		{
			get
			{
				return this._SceneVersion;
			}
			set
			{
				this._SceneVersion = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "ResetCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ResetCount
		{
			get
			{
				return this._ResetCount;
			}
			set
			{
				this._ResetCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
