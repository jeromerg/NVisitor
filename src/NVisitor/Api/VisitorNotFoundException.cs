using System;
using System.Text;
using NVisitor.Common.Topo;

namespace NVisitor.Api
{
    [Serializable]
    public class VisitorNotFoundException : Exception
    {
        internal VisitorNotFoundException(TargetTypeNotResolvedException exception)
            : base(BuildMessage(exception), exception)
        {
        }

        private static string BuildMessage(TargetTypeNotResolvedException exception)
        {
            if (exception.CandidateInfos.Count == 0)
                return string.Format("No visitor found for type {0}. Director has no visitor defined", exception.Type.Name);

            var b = new StringBuilder();
            b.AppendFormat("No visitor found for type {0}. Candidates are:\n", exception.Type.Name);
            foreach (var candidateInfo in exception.CandidateInfos)
                b.AppendFormat("{0}: {1}\n", candidateInfo.Type, candidateInfo.Status);
            return b.ToString();
        }
    }
}
