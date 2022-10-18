using Compiler.Tokenization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.IO
{
    public class Error
    {
        /// <summary>
        ///  The Token the error occured on
        /// </summary>
        public Token Token { get; private set; }
        /// <summary>
        /// The message associated with the error
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Create new Error with token and messsage
        /// </summary>
        /// <param name="token">The token the error occured on</param>
        /// <param name="errorMessage">The message associated with the error</param>
        public Error(Token token, string errorMessage)
        {
            Token = token;
            Message = errorMessage;
        }
        /// <summary>
        ///  Create new Error with token and defualt error message "Unknown"
        /// </summary>
        /// <param name="token">The token the error occured on</param>
        public Error(Token token)
        {
            Token = token;
            Message = "Unknown!"; 
        }
        /// <summary>
        /// A formated error message of this error as 
        /// Formatting is as follows "Error:{Message}, At:{Token.ToString()}"
        /// </summary>
        /// <returns>A formated error message</returns>
        public override string ToString()
        {
            return $"Error: {Message}\nOn: {Token.ToString()}.";
        }

    }
}
