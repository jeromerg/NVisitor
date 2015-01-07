using NVisitor.Api.Marker;

namespace NVisitor.Api
{
    /// <summary>
    /// Interface to implement, in order to define a visitor for a given director and a concrete TConcreteNode node type
    /// </summary>
    /// <typeparam name="TDirector">the director it is bound to</typeparam>
    /// <typeparam name="TFamily">the node upper class of the director</typeparam>
    /// <typeparam name="TNode">The concrete node type, the visitor is defined for</typeparam>
    public interface IVisitor<in TFamily, in TDirector, in TNode> : IVisitor<TFamily, TDirector> 
        where TDirector : IDirector<TFamily>
        where TNode : TFamily
    {
        void Visit(TDirector director, TNode node);
    }
}