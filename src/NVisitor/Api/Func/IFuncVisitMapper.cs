using System;
using NVisitor.Api.Marker;

namespace NVisitor.Api.Func
{
    /// <summary>Map a node to its related visit-action. Used by the director to send visitor visit the node</summary>
    /// <typeparam name="TFamily">Node's family</typeparam>
    /// <typeparam name="TDir">Director type</typeparam>
    /// <typeparam name="TResult">Result of the Visit call</typeparam>
    public interface IFuncVisitMapper<TFamily, TDir, TResult> : IVisitMapperMarker
        where TDir : IFuncDirector<TFamily, TDir, TResult>
    {
        Func<IFuncDirector<TFamily, TDir, TResult>, TFamily, TResult> GetVisitDelegate(TFamily node);
    }
}