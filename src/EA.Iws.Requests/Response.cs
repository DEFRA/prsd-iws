namespace EA.Iws.Requests
{
    using System.Collections.Generic;
    using System.Linq;

    public class Response<TResult>
    {
        private readonly List<string> warnings = new List<string>(); 
        private readonly List<string> errors = new List<string>();

        public IEnumerable<string> Warnings
        {
            get { return warnings; }
        }

        public IEnumerable<string> Errors
        {
            get { return errors; }
        }

        public bool HasWarnings
        {
            get { return Warnings.Any(); }
        }

        public bool HasErrors
        {
            get { return Errors.Any(); }
        }

        public bool IsSuccess
        {
            get { return !HasWarnings && !HasErrors; }
        }

        public TResult Result { get; private set; }

        public Response(TResult result)
        {
            Result = result;
        }

        public Response(TResult result, IEnumerable<string> warnings)
        {
            Result = result;
            this.warnings.AddRange(warnings);
        }

        public Response(TResult result, IEnumerable<string> warnings, IEnumerable<string> errors)
        {
            Result = result;
            this.warnings.AddRange(warnings);
            this.errors.AddRange(errors);
        }
    }
}