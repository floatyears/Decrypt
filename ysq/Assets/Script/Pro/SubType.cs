using ProtoBuf.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ProtoBuf.Meta
{
	public sealed class SubType
	{
		internal sealed class Comparer : IComparer<SubType>, IComparer
		{
			public static readonly SubType.Comparer Default = new SubType.Comparer();

			public int Compare(object x, object y)
			{
				return this.Compare(x as SubType, y as SubType);
			}

			public int Compare(SubType x, SubType y)
			{
				if (object.ReferenceEquals(x, y))
				{
					return 0;
				}
				if (x == null)
				{
					return -1;
				}
				if (y == null)
				{
					return 1;
				}
				return x.FieldNumber.CompareTo(y.FieldNumber);
			}
		}

		private readonly int fieldNumber;

		private readonly MetaType derivedType;

		private readonly DataFormat dataFormat;

		private IProtoSerializer serializer;

		public int FieldNumber
		{
			get
			{
				return this.fieldNumber;
			}
		}

		public MetaType DerivedType
		{
			get
			{
				return this.derivedType;
			}
		}

		internal IProtoSerializer Serializer
		{
			get
			{
				if (this.serializer == null)
				{
					this.serializer = this.BuildSerializer();
				}
				return this.serializer;
			}
		}

		public SubType(int fieldNumber, MetaType derivedType, DataFormat format)
		{
			if (derivedType == null)
			{
				throw new ArgumentNullException("derivedType");
			}
			if (fieldNumber <= 0)
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			this.fieldNumber = fieldNumber;
			this.derivedType = derivedType;
			this.dataFormat = format;
		}

		private IProtoSerializer BuildSerializer()
		{
			WireType wireType = WireType.String;
			if (this.dataFormat == DataFormat.Group)
			{
				wireType = WireType.StartGroup;
			}
			IProtoSerializer tail = new SubItemSerializer(this.derivedType.Type, this.derivedType.GetKey(false, false), this.derivedType, false);
			return new TagDecorator(this.fieldNumber, wireType, false, tail);
		}
	}
}
