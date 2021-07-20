using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    public abstract class NodeVisitor
    {
        private delegate dynamic Visitor(Node node);
        private Dictionary<Type, Visitor> visitors;

        public NodeVisitor()
        {
            visitors = new Dictionary<Type, Visitor>()
            {
                { typeof(ProgramNode), VisitProgram },
                { typeof(BinOp), VisitBinOp },
                { typeof(UnaryOp), VisitUnaryOp },
                { typeof(Number), VisitNumber },
                { typeof(StringNode), VisitString },
                { typeof(ArrayNode), VisitArrayNode },
                { typeof(Block), VisitBlock },
                { typeof(Statements), VisitStatements },
                { typeof(Assign), VisitAssign },
                { typeof(VariableDeclaration), VisitVariableDeclaration },
                { typeof(MethodDeclaration), VisitMethodDeclaration },
                { typeof(MethodCall), VisitMethodCall },
                { typeof(ReturnStatement), VisitReturn },
                { typeof(IfNode), VisitIfNode },
                { typeof(WhileLoop), VisitWhileLoop },
                { typeof(BooleanNode), VisitBooleanNode },
                { typeof(ConditionNode), VisitConditionNode },
                { typeof(Variable), VisitVariable },
                { typeof(NoOp), VisitNoOp }
            };
        }

        public dynamic Visit(Node node)
        {
            return visitors[node.GetType()].Invoke(node);
        }

        protected abstract dynamic VisitProgram(Node node);
        protected abstract dynamic VisitBinOp(Node node);
        protected abstract dynamic VisitUnaryOp(Node node);
        protected abstract dynamic VisitNumber(Node node);
        protected abstract dynamic VisitString(Node node);
        protected abstract dynamic VisitArrayNode(Node node);
        protected abstract dynamic VisitBlock(Node node);
        protected abstract dynamic VisitStatements(Node node);
        protected abstract dynamic VisitAssign(Node node);
        protected abstract dynamic VisitVariableDeclaration(Node node);
        protected abstract dynamic VisitMethodDeclaration(Node node);
        protected abstract dynamic VisitMethodCall(Node node);
        protected abstract dynamic VisitReturn(Node node);
        protected abstract dynamic VisitIfNode(Node node);
        protected abstract dynamic VisitWhileLoop(Node node);
        protected abstract dynamic VisitBooleanNode(Node node);
        protected abstract dynamic VisitConditionNode(Node node);
        protected abstract dynamic VisitVariable(Node node);
        protected abstract dynamic VisitNoOp(Node node);
    }
}
