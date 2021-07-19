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

            callStack.Push(new ActivationRecord(ARType.Program, 1));
            Visit(program.Node);

#if PRINT_DEBUG
            Console.WriteLine(callStack);
#endif
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

        protected override object VisitString(Node node)
        {
            StringNode stringNode = (StringNode)node;
            return stringNode.Value;
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
            ar.Define(variable.Token.Value, assign);

            return null;
        }

        protected override object VisitMethodDeclaration(Node node)
        {
            MethodDeclaration declaration = (MethodDeclaration)node;
            declaration.Symbol.SymbolRecord = callStack.Peek();

            return null;
        }

        protected override object VisitMethodCall(Node node)
        {
            MethodCall call = (MethodCall)node;
            string methodName = call.MethodName;

            Symbol symbol = call.Symbol;

            if (symbol is MethodSymbol)
            {
                MethodSymbol methodSymbol = (MethodSymbol)symbol;

                ActivationRecord ar = new ActivationRecord(ARType.Method, symbol.ScopeLevel + 1);
                ar.EnclosingRecord = methodSymbol.SymbolRecord;

                for (int i = 0; i < call.Parameters.Count; i++)
                {
                    ar.Define(methodSymbol.Parameters[i].Name, Visit(call.Parameters[i]));
                }
                callStack.Push(ar);
                Visit(methodSymbol.Body);

#if PRINT_DEBUG
                Console.WriteLine(callStack);
#endif
                callStack.Pop();
            }
            else if (symbol is BuiltinMethodSymbol)
            {
                BuiltinMethodSymbol methodSymbol = (BuiltinMethodSymbol)symbol;

                object[] parameters = new object[call.Parameters.Count];
                for (int i = 0; i < call.Parameters.Count; i++)
                {
                    parameters[i] = Visit(call.Parameters[i]);
                }

                methodSymbol.Method.Invoke(parameters);
            }

            return null;
        }

        protected override object VisitIfNode(Node node)
        {
            IfNode ifNode = (IfNode)node;

            if ((bool)Visit(ifNode.Boolean))
            {
                ActivationRecord ar = new ActivationRecord(ARType.If, ifNode.ScopeLevel + 1);
                ar.EnclosingRecord = callStack.Peek();

                callStack.Push(ar);
                Visit(ifNode.Body);

#if PRINT_DEBUG
                Console.WriteLine(callStack);
#endif
                callStack.Pop();
            }

            return null;
        }

        protected override object VisitBooleanNode(Node node)
        {
            BooleanNode boolean = (BooleanNode)node;

            if (boolean.Condition == Condition.True)
            {
                return true;
            }

            if (boolean.Condition == Condition.False)
            {
                return false;
            }

            return Visit(boolean.ConditionNode);
        }

        protected override object VisitConditionNode(Node node)
        {
            ConditionNode condition = (ConditionNode)node;

            Node left = condition.Left;
            Node right = condition.Right;

            switch (condition.Op.TokenType)
            {
                case TokenType.GreaterThan:
                    return (float)Visit(left) > (float)Visit(right);
                case TokenType.LessThan:
                    return (float)Visit(left) < (float)Visit(right);
                default:
                    return (float)Visit(left) == (float)Visit(right);
            }
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
