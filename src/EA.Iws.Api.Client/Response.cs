namespace EA.Iws.Api.Client
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "File contains generic and non-generic of the same class.")]
    public class Response<T> : Response
    {
        public Response(T result) : this(null)
        {
            Result = result;
        }

        public Response(params string[] errors)
            : base(errors)
        {
        }

        public T Result { get; private set; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "File contains generic and non-generic of the same class.")]
    public class Response
    {
        public Response() 
            : this(null)
        {
        }

        public Response(params string[] errors)
        {
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public IEnumerable<string> Errors { get; private set; }

        public bool HasErrors
        {
            get { return Errors.Any(); }
        }
    }
}