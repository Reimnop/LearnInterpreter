using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    public abstract class NodeVisitor
    {
        private delegate object Visitor(Node node);
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
                { typeof(Block), VisitBlock },
                { typeof(Statements), VisitStatements },
                { typeof(Assign), VisitAssign },
                { typeof(VariableDeclaration), VisitVariableDeclaration },
                { typeof(MethodDeclaration), VisitMethodDeclaration },
                { typeof(MethodCall), VisitMethodCall },
                { typeof(IfNode), VisitIfNode },
                { typeof(BooleanNode), VisitBooleanNode },
                { typeof(ConditionNode), VisitConditionNode },
                { typeof(Variable), VisitVariable },
                { typeof(NoOp), VisitNoOp }
            };
        }

        public object Visit(Node node)
        {
            return visitors[node.GetType()].Invoke(node);
        }

        protected abstract object VisitProgram(Node node);
        protected abstract object VisitBinOp(Node node);
        protected abstract object VisitUnaryOp(Node node);
        protected abstract object VisitNumber(Node node);
        protected abstract object VisitString(Node node);
        protected abstract object VisitBlock(Node node);
        protected abstract object VisitStatements(Node node);
        protected abstract object VisitAssign(Node node);
        protected abstract object VisitVariableDeclaration(Node node);
        protected abstract object VisitMethodDeclaration(Node node);
        protected abstract object VisitMethodCall(Node node);
        protected abstract object VisitIfNode(Node node);
        protected abstract object VisitBooleanNode(Node node);
        protected abstract object VisitConditionNode(Node node);
        protected abstract object VisitVariable(Node node);
        protected abstract object VisitNoOp(Node node);
    }
}
