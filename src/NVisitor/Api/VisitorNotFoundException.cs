using System;
using System.Text;
using NVisitor.Common.Topo;

namespace NVisitor.Api
{
    public class VisitorNotFoundException : Exception
    {
        internal VisitorNotFoundException(Type directorType, TargetTypeNotResolvedException exception)
            : base(BuildMessage(directorType, exception), exception)
        {
        }

        private static string BuildMessage(Type directorType, TargetTypeNotResolvedException exception)
        {
            if (exception.CandidateInfos.Count == 0)
            {
                return string.Format("{0}: No visitor found for node type {1}. Director has no visitor defined",
                                     directorType.Name,
                                     exception.Type.Name);
            }

            var b = new StringBuilder();

            b.AppendFormat("{0}: No visitor found for node type {1}. Candidates are:\n",
                           directorType.FullName,
                           exception.Type.FullName);

            foreach (TargetTypeInfo candidateInfo in exception.CandidateInfos)
            {
                b.AppendFormat("{0,-22}: {1}", candidateInfo.Status, candidateInfo.Type);
                b.AppendLine();
            }

            return b.ToString();
        }
    }
}