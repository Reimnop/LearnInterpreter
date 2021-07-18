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

            Console.WriteLine("Entering scope: global");

            currentScope = new ScopedSymbolTable("global", 1);
            Visit(program.Node);

            Console.WriteLine("Leaving scope: global");

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
            MethodSymbol methodSymbol = new MethodSymbol(methodName);

            currentScope.Define(methodSymbol);

            Console.WriteLine($"Entering scope: {methodName}");
            currentScope = new ScopedSymbolTable(methodName, currentScope.ScopeLevel + 1, currentScope);

            foreach (Parameter param in declaration.Parameters.Children)
            {
                BuiltinTypeSymbol paramType = (BuiltinTypeSymbol)currentScope.Lookup(param.Type.Token.Value);
                string paramName = param.Variable.Token.Value;

                VariableSymbol variableSymbol = new VariableSymbol(paramName, paramType);
                currentScope.Define(variableSymbol);

                methodSymbol.Parameters.Add(variableSymbol);
            }

            Visit(declaration.Block);

            Console.WriteLine($"Leaving scope: {methodName}");
            currentScope = currentScope.EnclosingScope;

            return null;
        }
    }
}
