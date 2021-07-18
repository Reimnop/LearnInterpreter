using System;

namespace LearnInterpreter
{
    public class SemanticAnalyzer : NodeVisitor
    {
        private ScopedSymbolTable currentScope;

        private void ThrowError(string errorCode, Token token)
        {
            throw new SemanticError(errorCode, token, $"{errorCode} -> {token}");
        }

        protected override object VisitProgram(Node node)
        {
            ProgramNode program = (ProgramNode)node;

            Console.WriteLine("Entering scope: 1");

            currentScope = new ScopedSymbolTable(1);
            Visit(program.Node);
            
            Console.WriteLine("Leaving scope: 1");
            Console.WriteLine(currentScope);

            return null;
        }

        protected override object VisitBlock(Node node)
        {
            Block block = (Block)node;
            Visit(block.Statements);
            return null;
        }

        protected override object VisitAssign(Node node)
        {
            Assign assign = (Assign)node;

            string name = assign.Left.Token.Value;
            if (!currentScope.TryLookup(name, out _))
            {
                ThrowError(ErrorCodes.IdentifierNotFound, assign.Left.Token);
            }

            Visit(assign.Right);

            return null;
        }

        protected override object VisitVariable(Node node)
        {
            Variable variable = (Variable)node;

            string name = variable.Token.Value;
            if (!currentScope.TryLookup(name, out _))
            {
                ThrowError(ErrorCodes.IdentifierNotFound, variable.Token);
            }

            return null;
        }

        protected override object VisitBinOp(Node node)
        {
            BinOp op = (BinOp)node;

            Visit(op.Left);
            Visit(op.Right);

            return null;
        }

        protected override object VisitNumber(Node node)
        {
            return null;
        }

        protected override object VisitNoOp(Node node)
        {
            return null;
        }

        protected override object VisitTypeNode(Node node)
        {
            return null;
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

        protected override object VisitUnaryOp(Node node)
        {
            UnaryOp op = (UnaryOp)node;
            Visit(op.Expr);
            return null;
        }

        protected override object VisitVariableDeclaration(Node node)
        {
            VariableDeclaration declaration = (VariableDeclaration)node;
            string typeName = declaration.Type.Token.Value;
            string varName = declaration.Variable.Token.Value;

            Symbol typeSymbol;
            if (!currentScope.TryLookup(typeName, out typeSymbol))
            {
                ThrowError(ErrorCodes.IdentifierNotFound, declaration.Type.Token);
            }

            if (currentScope.TryLookup(varName, out _, true))
            {
                ThrowError(ErrorCodes.DuplicateIdentifier, declaration.Variable.Token);
            }

            currentScope.Define(new VariableSymbol(varName, (BuiltinTypeSymbol)typeSymbol));
            return null;
        }

        protected override object VisitMethodDeclaration(Node node)
        {
            MethodDeclaration declaration = (MethodDeclaration)node;

            string methodName = declaration.MethodName;
            MethodSymbol methodSymbol = new MethodSymbol(methodName, declaration.Block);

            currentScope.Define(methodSymbol);

            Console.WriteLine($"Entering scope: {currentScope.ScopeLevel + 1}");
            currentScope = new ScopedSymbolTable(currentScope.ScopeLevel + 1, currentScope);

            foreach (Parameter param in declaration.Parameters.Children)
            {
                BuiltinTypeSymbol paramType = (BuiltinTypeSymbol)currentScope.Lookup(param.Type.Token.Value);
                string paramName = param.Variable.Token.Value;

                VariableSymbol variableSymbol = new VariableSymbol(paramName, paramType);
                currentScope.Define(variableSymbol);

                methodSymbol.Parameters.Add(variableSymbol);
            }

            Visit(declaration.Block);
            
            Console.WriteLine($"Leaving scope: {currentScope.ScopeLevel}");
            Console.WriteLine(currentScope);
            currentScope = currentScope.EnclosingScope;

            return null;
        }

        protected override object VisitMethodCall(Node node)
        {
            MethodCall call = (MethodCall)node;

            //checks
            MethodSymbol methodSymbol = null;
            if (!currentScope.TryLookup(call.MethodName, out Symbol symbol))
            {
                ThrowError(ErrorCodes.IdentifierNotFound, call.Token);
            }
            else
            {
                methodSymbol = (MethodSymbol)symbol;
            }

            if (call.Parameters.Count != methodSymbol.Parameters.Count)
            {
                ThrowError(ErrorCodes.ParamCountMismatch, call.Token);
            }

            foreach (Node param in call.Parameters)
            {
                Visit(param);
            }

            call.Symbol = (MethodSymbol)currentScope.Lookup(call.MethodName);

            return null;
        }

        protected override object VisitIfNode(Node node)
        {
            IfNode ifNode = (IfNode)node;

            ifNode.ScopeLevel = currentScope.ScopeLevel;

            Visit(ifNode.Boolean);

            Console.WriteLine($"Entering scope: {currentScope.ScopeLevel + 1}");
            currentScope = new ScopedSymbolTable(currentScope.ScopeLevel + 1, currentScope);
            Visit(ifNode.Body);
            Console.WriteLine($"Leaving scope {currentScope.ScopeLevel}");
            Console.WriteLine(currentScope);
            currentScope = currentScope.EnclosingScope;

            return null;
        }

        protected override object VisitBooleanNode(Node node)
        {
            return null;
        }

        protected override object VisitConditionNode(Node node)
        {
            return null;
        }
    }
}
