using Compiler.Tokenization;
using System;
using System.Collections.Generic;

namespace Compiler.IO
{
    /// <summary>
    /// An object for reporting errors in the compilation process
    /// </summary>
    public class ErrorReporter
    {
        /// <summary>
        /// List of error tokens that have been reported
        /// </summary>
        private List<Error> ErrorTokens;
        /// <summary>
        /// The number of errors that occourd
        /// </summary>
        public int NumberOfErrors => ErrorTokens.Count;
        /// <summary>
        /// Whether or not any errors have been encountered
        /// </summary>
        public bool HasErrors { get; private set; }
        /// <summary>
        /// Constrictor, initialise ErrorTokens to new token list and HasErrors to false.
        /// </summary>
        public ErrorReporter()
        {
            ErrorTokens = new List<Error>();
            HasErrors = false;
        }
        /// <summary>
        /// Reporsts a new error to be added to the error list
        /// </summary>
        /// <param name="token">Token that the error occured on</param>
        /// <param name="message">Description of the error</param>
        public void NewError(Token token, string message)
        {
            HasErrors = true;
            ErrorTokens.Add(new Error(token, message));
        }

        public override string ToString()
        {
            string errorLog = $"ERROR LOG...\n\nHas errors occured:{HasErrors}\nNumber of errors:{NumberOfErrors}\n\n"; 
            foreach (Error e in ErrorTokens)
            {
                errorLog += $"{e.ToString()}\n\n";
            }
            return errorLog;
        }
    }
}