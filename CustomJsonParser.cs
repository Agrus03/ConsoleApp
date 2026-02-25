using System;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Reflection;

namespace ConsoleApp
{
    public enum TokenType
    {
        CurlyOpen,
        CurlyClose,
        SquareOpen,
        SquareClose,
        Colon,
        Comma,
        String,
        Number,
    }

    public class JsonToken
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"[{Type}]: {Value}";
        }
    }

    public static class CustomJsonParser
    {
        public static List<JsonToken> Tokenize(string filePath)
        {
            var tokens = new List<JsonToken>();
            using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            int ch;
            while ((ch = sr.Read()) != -1)
            {
                char c = (char)ch;
                if (char.IsWhiteSpace(c)) continue;
                switch (c)
                {
                    case '{': tokens.Add(new JsonToken { Type = TokenType.CurlyOpen, Value = "{" }); break;
                    case '}': tokens.Add(new JsonToken { Type = TokenType.CurlyClose, Value = "}" }); break;
                    case '[': tokens.Add(new JsonToken { Type = TokenType.SquareOpen, Value = "[" }); break;
                    case ']': tokens.Add(new JsonToken { Type = TokenType.SquareClose, Value = "]" }); break;
                    case ',': tokens.Add(new JsonToken { Type = TokenType.Comma, Value = "," }); break;
                    case ':': tokens.Add(new JsonToken { Type = TokenType.Colon, Value = ":" }); break;
                    case '"':
                        StringBuilder stringBuilder = new StringBuilder();
                        
                        while ((ch = sr.Read()) != -1 && (char)ch != '"') 
                        {
                            stringBuilder.Append((char)ch);
                        }
                        tokens.Add(new JsonToken { Type = TokenType.String, Value = stringBuilder.ToString() });
                        break;
                    default:
                        if (char.IsDigit(c) || c == '-')
                        {
                            StringBuilder numberBuilder = new StringBuilder();
                            numberBuilder.Append(c);
                            
                            while ((ch = sr.Peek()) != -1 && (char.IsDigit((char)ch) || (char)ch == '.'))
                            {
                                numberBuilder.Append((char)sr.Read());
                            }
                            tokens.Add(new JsonToken { Type = TokenType.Number, Value = numberBuilder.ToString() });
                        }
                        break;
                }
            }
            return tokens;
        }

        public static object Parse(Type targetType, List<JsonToken> tokens, ref int pos)
        {
            return 1;
        }
    }
}