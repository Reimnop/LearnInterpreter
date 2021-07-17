using System;
using System.IO;

namespace LearnInterpreter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Lexer lexer = new Lexer(File.ReadAllText("TestProgram.prg"));
            Interpreter interpreter = new Interpreter(lexer);
            interpreter.Evaluate();
        }
    }
}
