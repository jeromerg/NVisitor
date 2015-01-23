using System.Diagnostics.CodeAnalysis;

namespace NVisitor.Api.Batch
{
    /// <summary> Contract for each single visitor implementation</summary>
    /// <typeparam name="TFamily">The node family that the director applies to</typeparam>
    /// <typeparam name="TDirector">The director related to the visitor</typeparam>
    /// <typeparam name="TNode">The node type that the visitor visit</typeparam>
    [SuppressMessage("ReSharper", "TypeParameterCanBeVariant")] // GENERIC PARAMETERS STRICTLY CARACTERIZE THE VISITOR
    public interface IVisitor<TFamily, TDirector, TNode> : IVisitorClass<TFamily, TDirector> 
        where TDirector : IDirector<TFamily>
        where TNode : TFamily
    {
        /// <summary>The visit method to implement</summary>
        /// <param name="director">You can read and update state of the director during the visit</param>
        /// <param name="node">The node to visit</param>
        void Visit(TDirector director, TNode node);
    }
}