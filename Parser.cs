using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;


namespace JONSParser
{
    class Parser
    {
        private Lexer lexer;
        private Token currentToken;
        private Token prevToken;

        public Parser(Lexer l)
        {
            lexer = l;
        }

        public dynamic Parse()
        {
            currentToken = lexer.Scan();
            var obj = ParseAny();
            Accept(TokenKind.End);
            return obj;
        }

        private jsObject ParseAny()
        {
            switch (currentToken.Kind)
            {
                case TokenKind.Number:
                    return ParseNumber();
                case TokenKind.String:
                    return ParseString();
                case TokenKind.LBrace:
                    return ParseObject();
                case TokenKind.RBrace:
                    SyntaxError("misplaced " + TokenKind.RBrace);
                    break;
                case TokenKind.LBracket:
                    return ParseArray();
                case TokenKind.RBracket:
                    SyntaxError("misplaced " + TokenKind.RBracket);
                    break;
                case TokenKind.Colon:
                    SyntaxError("misplaced " + TokenKind.Colon);
                    break;
                case TokenKind.Comma:
                    SyntaxError("misplaced " + TokenKind.Comma);
                    break;
                case TokenKind.End:
                    return null;
                case TokenKind.Error:
                    SyntaxError("error");
                    break;
                default:
                    SyntaxError("Unknown error");
                    break;
            }
            return null;
        }

        private void SyntaxError(string msg)
        {
            throw new SyntaxErrorException(msg+" at position "+currentToken.Position);
        }

        private void Accept(TokenKind expected)
        {
            if (currentToken.Kind == expected)
            {
                AcceptIt();
            }
            else
            {
                SyntaxError(expected.ToString() + " expected, but "+currentToken.Kind+" was found");
            }
        }

        private void AcceptIt()
        {
            prevToken = currentToken;
            currentToken = lexer.Scan();
        }

        private dynamic ParseArray()
        {
            dynamic array = new jsArray();
            
            Accept(TokenKind.LBracket);
            int index = 0;
            if (currentToken.Kind != TokenKind.RBracket)
            {
                //array.__fields__[index.ToString()] = ParseAny();
                array.__defineProperty__(index.ToString(), ParseAny());
                index++;
                array.length = index;
            }

            while (currentToken.Kind != TokenKind.RBracket)
            {
                Accept(TokenKind.Comma);
                //array.__fields__[index.ToString()] = ParseAny();
                array.__defineProperty__(index.ToString(), ParseAny());
                array.length = index;
                index++;
            }

            Accept(TokenKind.RBracket);

            return array;
        }

        private dynamic ParseObject()
        {
            dynamic obj = new jsObject();
            Accept(TokenKind.LBrace);

            if (currentToken.Kind != TokenKind.RBrace)
            {
                var prop = ParseProperty();
                obj.__defineProperty__(prop.Key, prop.Value);
                //obj.__fields__[prop.Key] = prop.Value;
            }

            while (currentToken.Kind != TokenKind.RBrace)
            {
                Accept(TokenKind.Comma);
                var prop = ParseProperty();
                //obj.__fields__[prop.Key] = prop.Value;
                obj.__defineProperty__(prop.Key, prop.Value);
            }

            Accept(TokenKind.RBrace);

            return obj;
        }

        private KeyValuePair<string,dynamic> ParseProperty()
        {
            var name = "";
            switch (currentToken.Kind)
            {
                case TokenKind.String:
                    AcceptIt();
                    name = prevToken.Spelling.Trim(new char[]{'\'','"'});
                    break;
                case TokenKind.Identifier:
                    AcceptIt();
                    name = prevToken.Spelling.Trim(new char[] { '\'', '"' });
                    break;
                default:
                    SyntaxError("expected property");
                    break;
            }
            
            Accept(TokenKind.Colon);
            var value = ParseAny();
            return new KeyValuePair<string, dynamic>(name, value);
        }

        private dynamic ParseString()
        {
            Accept(TokenKind.String);
            return new jsObject(prevToken.Spelling.Trim(new char[]{'\'','"'}));
        }

        private dynamic ParseNumber()
        {
            Accept(TokenKind.Number);
            return new jsObject(prevToken.Spelling);
        }
    }
}
