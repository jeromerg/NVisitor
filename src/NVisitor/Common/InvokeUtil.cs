using System.Reflection;
using System.Runtime.ExceptionServices;

namespace NVisitor.Common
{
    public static class InvokeUtil
    {
        /// <summary>Unwrap the TargetInvocationException if any is thrown </summary>
        public static object InvokeWithUnwrapper(MethodInfo methodInfo, object obj, object[] args)
        {
            try
            {
                return methodInfo.Invoke(obj, args);
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw; // dummy
            }
        }
    }
}
