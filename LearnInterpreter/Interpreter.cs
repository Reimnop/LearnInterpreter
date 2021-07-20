using System;
using System.Collections.Generic;

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

        protected override dynamic VisitProgram(Node node)
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

        protected override dynamic VisitBinOp(Node node)
        {
            BinOp op = (BinOp)node;

            dynamic left = Visit(op.Left);
            dynamic right = Visit(op.Right);

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

        protected override dynamic VisitUnaryOp(Node node)
        {
            UnaryOp op = (UnaryOp)node;

            switch (op.Token.TokenType)
            {
                case TokenType.Plus:
                    return +Visit(op.Expr);
                case TokenType.Minus:
                    return -Visit(op.Expr);
            }

            throw new Exception();
        }

        protected override dynamic VisitNumber(Node node)
        {
            Number num = (Number)node;
            return float.Parse(num.Value);
        }

        protected override dynamic VisitString(Node node)
        {
            StringNode stringNode = (StringNode)node;
            return stringNode.Value;
        }

        protected override dynamic VisitArrayNode(Node node)
        {
            ArrayNode array = (ArrayNode)node;

            List<dynamic> elements = new List<dynamic>();
            foreach (Node element in array.Elements)
            {
                elements.Add(element);
            }

            return elements;
        }

        protected override dynamic VisitBlock(Node node)
        {
            Block block = (Block)node;
            return VisitStatements(block.Statements);
        }

        protected override dynamic VisitStatements(Node node)
        {
            Statements statements = (Statements)node;

            foreach (Node child in statements.Children)
            {
                dynamic value = Visit(child);

                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }

        protected override dynamic VisitAssign(Node node)
        {
            Assign assign = (Assign)node;
            Variable var = assign.Left;

            ActivationRecord ar = callStack.Peek();

            if (var.IndexNode == null)
            {
                ar[var.Token.Value] = Visit(assign.Right);
            }
            else
            {
                int index = Visit(var.IndexNode);
                ar[var.Token.Value][index] = Visit(assign.Right);
            }

            return null;
        }

        protected override dynamic VisitVariableDeclaration(Node node)
        {
            VariableDeclaration declaration = (VariableDeclaration)node;
            Token token = declaration.Token;

            dynamic assign = null;
            if (declaration.Assignment != null)
            {
                assign = Visit(declaration.Assignment);
            }

            ActivationRecord ar = callStack.Peek();
            ar.Define(token.Value, assign);

            return null;
        }

        protected override dynamic VisitMethodDeclaration(Node node)
        {
            MethodDeclaration declaration = (MethodDeclaration)node;
            declaration.Symbol.SymbolRecord = callStack.Peek();

            return null;
        }

        protected override dynamic VisitMethodCall(Node node)
        {
            MethodCall call = (MethodCall)node;
            string methodName = call.MethodName;

            Symbol symbol = call.Symbol;

            dynamic returnValue = null;
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
                returnValue = Visit(methodSymbol.Body);

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

            return returnValue;
        }

        protected override dynamic VisitReturn(Node node)
        {
            ReturnStatement returnStatement = (ReturnStatement)node;
            return Visit(returnStatement.Node);
        }

        protected override dynamic VisitIfNode(Node node)
        {
            IfNode ifNode = (IfNode)node;

            if ((bool)Visit(ifNode.Boolean))
            {
                ActivationRecord ar = new ActivationRecord(ARType.If, ifNode.ScopeLevel + 1);
                ar.EnclosingRecord = callStack.Peek();

                callStack.Push(ar);
                dynamic value = Visit(ifNode.Body);

                if (value != null)
                {
                    callStack.Pop();
                    return value;
                }

#if PRINT_DEBUG
                Console.WriteLine(callStack);
#endif
                callStack.Pop();
            }

            return null;
        }

        protected override dynamic VisitWhileLoop(Node node)
        {
            WhileLoop whileLoop = (WhileLoop)node;

            while ((bool)Visit(whileLoop.Boolean))
            {
                ActivationRecord ar = new ActivationRecord(ARType.While, whileLoop.ScopeLevel + 1);
                ar.EnclosingRecord = callStack.Peek();

                callStack.Push(ar);

                dynamic value = Visit(whileLoop.Body);

                if (value != null)
                {
                    callStack.Pop();
                    return value;
                }

#if PRINT_DEBUG
                Console.WriteLine(callStack);
#endif
                callStack.Pop();
            }

            return null;
        }

        protected override dynamic VisitBooleanNode(Node node)
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

        protected override dynamic VisitConditionNode(Node node)
        {
            ConditionNode condition = (ConditionNode)node;

            Node left = condition.Left;
            Node right = condition.Right;

            switch (condition.Op.TokenType)
            {
                case TokenType.Equal:
                    return Visit(left) == Visit(right);
                case TokenType.NotEqual:
                    return Visit(left) != Visit(right);
                case TokenType.LessThan:
                    return Visit(left) < Visit(right);
                case TokenType.GreaterThan:
                    return Visit(left) > Visit(right);
                case TokenType.LessThanOrEqualTo:
                    return Visit(left) <= Visit(right);
                case TokenType.GreaterThanOrEqualTo:
                    return Visit(left) >= Visit(right);
            }

            throw new Exception();
        }

        protected override dynamic VisitVariable(Node node)
        {
            Variable variable = (Variable)node;

            ActivationRecord ar = callStack.Peek();
            if (ar.TryGet(variable.Token.Value, out dynamic val))
            {
                if (variable.IndexNode == null)
                {
                    return val;
                }
                else
                {
                    return val[(int)Visit(variable.IndexNode)];
                }
            }

            return null;
        }

        protected override dynamic VisitNoOp(Node node)
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
