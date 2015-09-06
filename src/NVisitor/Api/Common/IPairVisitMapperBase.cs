using System.Diagnostics.CodeAnalysis;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Common
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily1">Node1's family</typeparam>
    /// <typeparam name="TFamily2">Node2's family</typeparam>
    /// <typeparam name="TVisitDelegate">the visit delegate</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE DIRECTOR
    public interface IPairVisitMapperBase<TFamily1, TFamily2, TVisitDelegate> : IVisitMapperMarker
    {
        TVisitDelegate GetVisitDelegate(TFamily1 node1, TFamily2 node2);
    }
}