using Compiler.IO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Tokenization
{
    /// <summary>
    /// A tokenizer for the reader language
    /// </summary>
    public class Tokenizer
    {
        /// <summary>
        /// The error reporter
        /// </summary>
        public ErrorReporter Reporter { get; }

        /// <summary>
        /// The reader getting the characters from the file
        /// </summary>
        private IFileReader Reader { get; }

        /// <summary>
        /// The characters currently in the token
        /// </summary>
        private StringBuilder TokenSpelling { get; } = new StringBuilder();

        /// <summary>
        /// Createa a new tokenizer
        /// </summary>
        /// <param name="reader">The reader to get characters from the file</param>
        /// <param name="reporter">The error reporter to use</param>
        public Tokenizer(IFileReader reader, ErrorReporter reporter)
        {
            Reader = reader;
            Reporter = reporter;
        }

        /// <summary>
        /// Gets all the tokens from the file
        /// </summary>
        /// <returns>A list of all the tokens in the file in the order they appear</returns>
        public List<Token> GetAllTokens()
        {
            List<Token> tokens = new List<Token>();
            Token token = GetNextToken();
            while (token.Type != TokenType.EndOfText)
            {
                tokens.Add(token);
                token = GetNextToken();
            }
            tokens.Add(token);
            Reader.Close();
            return tokens;
        }

        /// <summary>
        /// Scan the next token
        /// </summary>
        /// <returns>True if and only if there is another token in the file</returns>
        private Token GetNextToken()
        {
            // Skip forward over any white spcae and comments
            SkipSeparators();

            // clear previouse token spelling
            TokenSpelling.Clear();

            // Scan the token and work out its type
            Token token = ScanToken(); // return token rather than token type

            // debugger display token
            Debugger.Write($"Scanned {token}");

            // return token
            return token;
        }

        /// <summary>
        /// Skip forward until the next character is not whitespace or a comment
        /// </summary>
        private void SkipSeparators()
        {
            if (Reader.Current == '!')
            {
                Reader.SkipRestOfLine();
            }
            else
            {
                while (IsWhiteSpace(Reader.Current))
                {
                    Reader.MoveNext();
                }
            }
        }

        /// <summary>
        /// Find the next token
        /// </summary>
        /// <returns>The type of the next token</returns>
        /// <remarks>Sets tokenSpelling to be the characters in the token</remarks>
        private Token ScanToken()
        {
            Position tokenStartPosition = Reader.CurrentPosition;
            if (Char.IsLetter(Reader.Current))
            {
                // consume as identifier
                TakeIt();
                TokenType T = TokenType.Identifier;
                while (Char.IsLetterOrDigit(Reader.Current))
                {
                    TakeIt();
                }
                if (TokenTypes.IsKeyword(TokenSpelling))
                {
                    T = TokenTypes.GetTokenForKeyword(TokenSpelling);
                }
                return new Token(T, TokenSpelling.ToString(), tokenStartPosition);

            }
            else if (Char.IsDigit(Reader.Current))
            {
                // Consume as int Literala
                TakeIt();
                while (Char.IsDigit(Reader.Current))
                {
                    TakeIt();
                }
                return new Token(TokenType.IntLiteral, TokenSpelling.ToString(), tokenStartPosition);

            } else if (Reader.Current == '\'') 
            {
                // consube as char chars literal
                TokenType T = TokenType.CharLiteral;
                TakeIt();
                TakeIt();
                if(Reader.Current == '\'')
                {
                    TakeIt();
                }
                else
                {
                    T = TokenType.Error;
                    // report error
                    Reporter.NewError(new Token(T, TokenSpelling.ToString(), tokenStartPosition), $"Character literals must be sourounded by \"\'\", \"{Reader.Current}\" was found instead.");
                }
                return new Token(T, TokenSpelling.ToString(), tokenStartPosition);

            } else if (IsOperator(Reader.Current))
            {
                // Consume as operator
                TakeIt();
                return new Token(TokenType.Operator, TokenSpelling.ToString(), tokenStartPosition);

            } else if (IsPunctuation(Reader.Current))
            {
                // Consume as punctuation
                TokenType T = TokenType.Error;
                switch (Reader.Current)
                {
                    case '(':
                        TakeIt();
                        T = TokenType.LeftBracket;
                        break;
                    case ')':
                        TakeIt();
                        T = TokenType.RightBracket;
                        break;
                    case '~':
                        TakeIt();
                        T = TokenType.Is;
                        break;
                    case ':':
                        TakeIt();
                        if (Reader.Current == '=')
                        {
                            TakeIt();
                            T = TokenType.Becomes;
                        }
                        else
                        {
                            T = TokenType.Colon;
                        }
                        break;
                    case ';':
                        TakeIt();
                        T = TokenType.Semicolon;
                        break;
                    default:
                        // This code will never be reached as the IsPunctuation check that it is a valid Punctuation
                        break;
                }
                return new Token(T, TokenSpelling.ToString(), tokenStartPosition);
            }
            else if (Reader.Current == default(char))
            {
                // Read the end of the file
                TakeIt();
                return new Token(TokenType.EndOfText, TokenSpelling.ToString(), tokenStartPosition);
            }
            else
            {
                // Encountered a character we weren't expecting
                TakeIt();
                Token token = new Token(TokenType.Error, TokenSpelling.ToString(), tokenStartPosition);
                Reporter.NewError(token, $"Unexpected character encountered \"{Reader.Current}\"");
                return token;
            }
        }

        /// <summary>
        /// Appends the current character to the current token then moves to the next character
        /// </summary>
        private void TakeIt()
        {
            TokenSpelling.Append(Reader.Current);
            Reader.MoveNext();
        }

        /// <summary>
        /// Checks whether a character is white space
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>True if and only if c is a whitespace character</returns>
        private static bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n';
        }

        /// <summary>
        /// Checks whether a character is an operator
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>True if and only if the character is an operator in the language</returns>
        private static bool IsOperator(char c)
        {
            switch (c)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '<':
                case '>':
                case '=':
                case '\\':
                    return true;
                default:
                    return false;
            }
        }
        /// <summary>
        ///  checks whether a given character is punctuation
        /// </summary>
        /// <param name="c">The character to check</param>
        /// <returns>True if char is of token type punctuation</returns>
        private static bool IsPunctuation(char c)
        {
            switch (c)
            {
                case '(':
                case ')':
                case '~':
                case ':':
                case ';':
                    return true;
                default:
                    return false;
            }
        }
    }
}
