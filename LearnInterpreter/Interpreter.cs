using System;

namespace LearnInterpreter
{
    public class Interpreter : NodeVisitor
    {
        private Parser parser;
        private SemanticAnalyzer semanticAnalyzer;

        private CallStack callStack = new CallStack();

        public Interpreter(Lexer lexer)
        {
            parser = new Parser(lexer);
            semanticAnalyzer = new SemanticAnalyzer();
        }

        protected override object VisitProgram(Node node)
        {
            ProgramNode program = (ProgramNode)node;

            callStack.Push(new ActivationRecord("Program", ARType.Program, 1));
            Visit(program.Node);

            Console.WriteLine(callStack);
            callStack.Pop();
            return null;
        }

        protected override object VisitBinOp(Node node)
        {
            BinOp op = (BinOp)node;

            float left = (float)Visit(op.Left);
            float right = (float)Visit(op.Right);

            switch (op.Token.TokenType)
            {
                case TokenType.Plus:
                    return left + right;
                case TokenType.Minus:
                    return left - right;
                case TokenType.Mult:
                    return left * right;
                case TokenType.Div:
                    return left / right;
            }

            throw new Exception();
        }

        protected override object VisitUnaryOp(Node node)
        {
            UnaryOp op = (UnaryOp)node;

            switch (op.Token.TokenType)
            {
                case TokenType.Plus:
                    return (float)Visit(op.Expr);
                case TokenType.Minus:
                    return -(float)Visit(op.Expr);
            }

            throw new Exception();
        }

        protected override object VisitNumber(Node node)
        {
            Number num = (Number)node;
            return float.Parse(num.Value);
        }

        protected override object VisitBlock(Node node)
        {
            Block block = (Block)node;
            return VisitStatements(block.Statements);
        }

        protected override object VisitStatements(Node node)
        {
            Statements statements = (Statements)node;

            foreach (Node child in statements.Children)
            {
                Visit(child);
            }

            return null;
        }

        protected override object VisitAssign(Node node)
        {
            Assign assign = (Assign)node;
            Variable var = assign.Left;

            ActivationRecord ar = callStack.Peek();
            ar[var.Token.Value] = Visit(assign.Right);

            return null;
        }

        protected override object VisitVariableDeclaration(Node node)
        {
            VariableDeclaration declaration = (VariableDeclaration)node;
            Variable variable = declaration.Variable;

            object assign = 0f;
            if (declaration.Assignment != null)
            {
                assign = Visit(declaration.Assignment);
            }

            ActivationRecord ar = callStack.Peek();
            ar[variable.Token.Value] = assign;

            return null;
        }

        protected override object VisitMethodDeclaration(Node node)
        {
            return null;
        }

        protected override object VisitMethodCall(Node node)
        {
            return null;
        }

        protected override object VisitTypeNode(Node node)
        {
            TypeNode type = (TypeNode)node;
            return type.Token.Value;
        }

        protected override object VisitVariable(Node node)
        {
            Variable variable = (Variable)node;

            ActivationRecord ar = callStack.Peek();
            if (ar.TryGet(variable.Token.Value, out object val))
            {
                return val;
            }

            return null;
        }

        protected override object VisitNoOp(Node node)
        {
            return null;
        }

        public void Evaluate()
        {
            ProgramNode ast = parser.Parse();
            semanticAnalyzer.Visit(ast);
            Visit(ast);
        }
    }
}
