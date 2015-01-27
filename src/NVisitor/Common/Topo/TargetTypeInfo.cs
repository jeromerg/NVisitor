using System;

namespace NVisitor.Common.Topo
{
    public class TargetTypeInfo
    {
        private readonly Type mType;
        private readonly TargetTypeStatus mStatus;

        public TargetTypeInfo(Type type, TargetTypeStatus status)
        {
            mType = type;
            mStatus = status;
        }

        public Type Type
        {
            get { return mType; }
        }

        public TargetTypeStatus Status
        {
            get { return mStatus; }
        }
    }
}