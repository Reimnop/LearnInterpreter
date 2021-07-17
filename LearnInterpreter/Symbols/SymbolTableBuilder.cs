using System;

namespace LearnInterpreter
{
    public class SymbolTableBuilder : NodeVisitor
    {
        private SymbolTable symbolTable = new SymbolTable();

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
            if (!symbolTable.TryLookup(name, out _))
            {
                throw new Exception($"The name \"{name}\" does not exist in the current context!");
            }

            Visit(assign.Right);

            return null;
        }

        protected override object VisitVariable(Node node)
        {
            Variable variable = (Variable)node;

            string name = variable.Token.Value;
            if (!symbolTable.TryLookup(name, out _))
            {
                throw new Exception($"The name \"{name}\" does not exist in the current context!");
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

        protected override object VisitDeclaration(Node node)
        {
            VariableDeclaration Declaration = (VariableDeclaration)node;
            string typeName = Declaration.Type.Token.Value;
            string varName = Declaration.Variable.Token.Value;

            Symbol typeSymbol;
            if (!symbolTable.TryLookup(typeName, out typeSymbol))
            {
                throw new Exception($"The type \"{typeName}\" is not found in the current context");
            }

            if (!symbolTable.TryDefine(new VariableSymbol(varName, typeSymbol)))
            {
                throw new Exception($"Double Declaration detected: \"{varName}\"");
            }
            return null;
        }
    }
}
