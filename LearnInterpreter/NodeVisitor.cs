﻿using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    public class NodeVisitor
    {
        private delegate object Visitor(Node node);
        private Dictionary<Type, Visitor> visitors;

        private Dictionary<string, object> globalScope = new Dictionary<string, object>();

        public NodeVisitor()
        {
            visitors = new Dictionary<Type, Visitor>()
            {
                { typeof(ProgramNode), VisitProgram },
                { typeof(BinOp), VisitBinOp },
                { typeof(UnaryOp), VisitUnaryOp },
                { typeof(Number), VisitNumber },
                { typeof(Block), VisitBlock },
                { typeof(Statements), VisitStatements },
                { typeof(Assign), VisitAssign },
                { typeof(VariableDeclaration), VisitVariableDeclaration },
                { typeof(MethodDeclaration), VisitMethodDeclaration },
                { typeof(TypeNode), VisitTypeNode },
                { typeof(Variable), VisitVariable },
                { typeof(NoOp), VisitNoOp }
            };
        }

        public object Visit(Node node)
        {
            return visitors[node.GetType()].Invoke(node);
        }

        protected virtual object VisitProgram(Node node)
        {
            ProgramNode program = (ProgramNode)node;
            Visit(program.Node);
            return null;
        }

        protected virtual object VisitBinOp(Node node)
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

        protected virtual object VisitUnaryOp(Node node)
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

        protected virtual object VisitNumber(Node node)
        {
            Number num = (Number)node;
            return float.Parse(num.Value);
        }

        protected virtual object VisitBlock(Node node)
        {
            Block block = (Block)node;
            return VisitStatements(block.Statements);
        }

        protected virtual object VisitStatements(Node node)
        {
            Statements statements = (Statements)node;

            foreach (Node child in statements.Children)
            {
                Visit(child);
            }

            return null;
        }

        protected virtual object VisitAssign(Node node)
        {
            Assign assign = (Assign)node;
            Variable var = assign.Left;

            globalScope[var.Token.Value] = Visit(assign.Right);

            return null;
        }

        protected virtual object VisitVariableDeclaration(Node node)
        {
            VariableDeclaration declaration = (VariableDeclaration)node;
            Variable variable = declaration.Variable;

            object assign = 0f;
            if (declaration.Assignment != null)
            {
                assign = Visit(declaration.Assignment);
            }

            globalScope.Add(variable.Token.Value, assign);

            return null;
        }

        protected virtual object VisitMethodDeclaration(Node node)
        {
            return null;
        }

        protected virtual object VisitTypeNode(Node node)
        {
            TypeNode type = (TypeNode)node;
            return type.Token.Value;
        }

        protected virtual object VisitVariable(Node node)
        {
            Variable variable = (Variable)node;

            return globalScope[variable.Token.Value];
        }

        protected virtual object VisitNoOp(Node node)
        {
            return null;
        }
    }
}
