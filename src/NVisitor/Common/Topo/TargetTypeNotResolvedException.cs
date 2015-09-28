using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NVisitor.Common.Topo
{
    public class TargetTypeNotResolvedException : Exception
    {
        private readonly Type mType;
        private readonly List<TargetTypeInfo> mCandidateInfos = new List<TargetTypeInfo>();

        public TargetTypeNotResolvedException(Type type, IEnumerable<TargetTypeInfo> candidateStatuses)
        {
            mType = type;
            mCandidateInfos.AddRange(candidateStatuses);
        }

        public Type Type
        {
            get { return mType; }
        }

        public List<TargetTypeInfo> CandidateInfos
        {
            get { return mCandidateInfos; }
        }

        public override string Message
        {
            get
            {
                var b = new StringBuilder();

                b.AppendFormat("No unambiguous Type to assign type {0} to. Candidates:\n", mType.FullName);
                foreach (TargetTypeInfo candidateInfo in mCandidateInfos)
                {
                    b.AppendFormat("{0}: {1}", candidateInfo.Type, candidateInfo.Status);
                }
                return b.ToString();
            }
        }
    }
}