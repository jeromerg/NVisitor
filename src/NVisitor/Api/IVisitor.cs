using NVisitor.Api.Marker;

namespace NVisitor.Api
{
    /// <summary>
    /// Interface to implement, in order to define a visitor for a given director and a concrete TConcreteNode node type
    /// </summary>
    /// <typeparam name="TDir">the director it is bound to</typeparam>
    /// <typeparam name="TNodeBase">the node upper class of the director</typeparam>
    /// <typeparam name="TConcreteNode">The concrete node type, the visitor is defined for</typeparam>
    public interface IVisitor<in TDir, in TNodeBase, in TConcreteNode> : IVisitor<TDir, TNodeBase> 
        where TDir : IDirector<TNodeBase>
        where TConcreteNode : TNodeBase
    {
        void Visit(TDir director, TConcreteNode node);
    }
}