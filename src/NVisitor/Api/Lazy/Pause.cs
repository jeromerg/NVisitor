namespace NVisitor.Api.Lazy
{
    /// <summary>Used has dummy handle to pause the visit</summary>
    public class Pause
    {
        public static readonly Pause Now = new Pause();
    }
}