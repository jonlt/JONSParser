using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JONSParser
{
    enum TokenKind
    {
        Identifier,
        Number,
        String,
        LBrace,
        RBrace,
        LBracket,
        RBracket,
        Colon,
        Comma,
        End,
        Error,
    }

    class Token
    {
        public TokenKind Kind { get; private set; }
        public string Spelling { get; private set; }
        public int Position { get; private set; }

        public Token(TokenKind kind, string spelling, int position)
        {
            Kind = kind;
            Spelling = spelling;
            Position = position;
        }

        public override string ToString()
        {
            return "Kind=" + Kind.ToString() + "\t spelling=" + Spelling + "\t position=" + Position;
        }
    }

    class SourceString
    {
        public int CurrentPosition { get; private set; }
        public const char End = '\u0000';
        private string json;
        
        public SourceString(string source)
        {
            CurrentPosition = 0;
            json = source;
        }

        public char GetNextChar()
        {
            if (CurrentPosition >= json.Length)
                return End;
            else
                return json[CurrentPosition++];
        }
    }

    class Lexer
    {
        private SourceString source;

        private char currentChar;
        private StringBuilder currentSpelling;
        private bool currentlyScanningToken;

        public Lexer(string json)
        {
            source = new SourceString(json);
            currentChar = source.GetNextChar();
        }

        private void TakeIt()
        {
            if (currentlyScanningToken)
            {
                currentSpelling.Append(currentChar);
            }
            currentChar = source.GetNextChar();
        }

        private TokenKind ScanToken()
        {
            if (Char.IsLetter(currentChar))
            {
                TakeIt();
                while (Char.IsLetter(currentChar) || Char.IsDigit(currentChar))
                {
                    TakeIt();
                }
                return TokenKind.Identifier;
            }
            if (Char.IsDigit(currentChar)) 
            {
                TakeIt();
                while(Char.IsDigit(currentChar))
                {
                    TakeIt();
                }
                return TokenKind.Number;
            }
            else if (currentChar == '"') 
            {
                TakeIt();
                while (currentChar != '"' && currentChar != SourceString.End)
                {
                    if (currentChar == '\\')
                    {
                        TakeIt();
                    }
                    TakeIt();
                }
                TakeIt();
                return TokenKind.String;
            }
            else if (currentChar == '\'') 
            {
                TakeIt();
                while (currentChar != '\'' && currentChar != SourceString.End)
                {
                    if (currentChar == '\\')
                    {
                        TakeIt();
                    }
                    TakeIt();
                }
                TakeIt();
                return TokenKind.String;
            }
            else if (currentChar == '{')
            {
                TakeIt();
                return TokenKind.LBrace;
            }
            else if (currentChar == '}')
            {
                TakeIt();
                return TokenKind.RBrace;
            }
            else if (currentChar == '[')
            {
                TakeIt();
                return TokenKind.LBracket;
            }
            else if (currentChar == ']')
            {
                TakeIt();
                return TokenKind.RBracket;
            }
            else if (currentChar == ':')
            {
                TakeIt();
                return TokenKind.Colon;
            }
            else if (currentChar == ',')
            {
                TakeIt();
                return TokenKind.Comma;
            }
            else if (currentChar == '\u0000')
            {
                TakeIt();
                return TokenKind.End;
            }
            else
            {
                TakeIt();
                return TokenKind.Error;
            }
        }

        public Token Scan()
        {
            var prevPos = source.CurrentPosition;
            currentSpelling = new StringBuilder();
            currentlyScanningToken = true;
            var kind = ScanToken();
            currentlyScanningToken = false;
            while (Char.IsWhiteSpace(currentChar) && currentChar != SourceString.End)
            {
                TakeIt();
            }

            return new Token(kind, currentSpelling.ToString(), prevPos);
        }

    }
}
