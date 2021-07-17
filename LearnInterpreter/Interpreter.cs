using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    /*
        Refer to MyScript.gram for grammar 
    */

    public class Interpreter
    {
        private Lexer lexer;
        private Token currentToken;

        private delegate object Visitor(Node node);
        private Dictionary<Type, Visitor> visitors;

        private Dictionary<string, object> globalScope = new Dictionary<string, object>();

        public Interpreter(Lexer lexer)
        {
            this.lexer = lexer;
            currentToken = lexer.NextToken();

            visitors = new Dictionary<Type, Visitor>()
            {
                { typeof(BinOp), VisitBinOp },
                { typeof(UnaryOp), VisitUnaryOp },
                { typeof(Number), VisitNumber },
                { typeof(Compound), VisitCompound },
                { typeof(Statements), VisitStatements },
                { typeof(Assign), VisitAssign },
                { typeof(Decleration), VisitDecleration },
                { typeof(Variable), VisitVariable },
                { typeof(NoOp), VisitNoOp }
            };
        }

        private void Eat(TokenType tokenType)
        {
            if (tokenType == currentToken.TokenType)
                currentToken = lexer.NextToken();
            else
                throw new Exception($"Expected {tokenType}, got {currentToken.TokenType}");
        }

        private Node Program()
        {
            if (currentToken.TokenType == TokenType.OpenBracket)
            {
                return CompoundStatement();
            }
            else
            {
                return StatementList();
            }
        }

        private Node CompoundStatement()
        {
            Eat(TokenType.OpenBracket);
            Statements statements = StatementList();
            Eat(TokenType.CloseBracket);

            return new Compound(statements);
        }

        private Statements StatementList()
        {
            Node node = Statement();

            List<Node> nodes = new List<Node>() { node };

            while (currentToken.TokenType == TokenType.Semicolon)
            {
                Eat(TokenType.Semicolon);
                nodes.Add(Statement());
            }

            return new Statements(nodes);
        }

        private Node Statement()
        {
            if (currentToken.TokenType == TokenType.OpenBracket)
            {
                return CompoundStatement();
            }
            else if (currentToken.TokenType == TokenType.Identifier)
            {
                return AssignmentStatement();
            }
            else if (currentToken.TokenType == TokenType.Type)
            {
                return DeclerationStatement();
            }
            else
            {
                return new NoOp(); //empty
            }
        }

        private Node DeclerationStatement()
        {
            Token type = currentToken;
            Eat(TokenType.Type);

            Token id = currentToken;
            Eat(TokenType.Identifier);

            return new Decleration(type, id);
        }

        private Node AssignmentStatement()
        {
            Node left = Variable();
            Token op = currentToken;
            Eat(TokenType.Assign);
            Node right = Expr();

            return new Assign(op, left, right);
        }

        private Node Variable()
        {
            Node node = new Variable(currentToken);
            Eat(TokenType.Identifier);

            return node;
        }

        private Node Factor()
        {
            Token token = currentToken;

            if (currentToken.TokenType == TokenType.LeftParen)
            {
                Eat(TokenType.LeftParen);
                Node node = Expr();
                Eat(TokenType.RightParen);

                return node;
            }

            if (currentToken.TokenType == TokenType.Integer)
            {
                Node node = Number();
                return node;
            }

            if (currentToken.TokenType == TokenType.Plus || currentToken.TokenType == TokenType.Minus)
            {
                Eat(currentToken.TokenType);
                return new UnaryOp(token, Factor());
            }

            Node var = Variable();
            return var;
        }

        private Node Number()
        {
            string num = string.Empty;

            Token token = currentToken;
            Eat(TokenType.Integer);

            num = token.Value;

            if (currentToken.TokenType == TokenType.Dot)
            {
                Eat(TokenType.Dot);

                Token dec = currentToken;
                Eat(TokenType.Integer);

                num += $".{dec.Value}";
            }

            return new Number(num);
        }

        private Node Term()
        {
            Node node = Factor();

            while (currentToken.TokenType == TokenType.Mult || currentToken.TokenType == TokenType.Div)
            {
                Token token = currentToken;
                if (token.TokenType == TokenType.Mult)
                {
                    Eat(TokenType.Mult);
                }

                if (token.TokenType == TokenType.Div)
                {
                    Eat(TokenType.Div);
                }

                node = new BinOp(token, node, Factor());
            }

            return node;
        }

        private Node Expr()
        {
            Node node = Term();

            while (currentToken.TokenType == TokenType.Plus || currentToken.TokenType == TokenType.Minus)
            {
                Token token = currentToken;
                if (token.TokenType == TokenType.Plus)
                {
                    Eat(TokenType.Plus);
                }

                if (token.TokenType == TokenType.Minus)
                {
                    Eat(TokenType.Minus);
                }

                node = new BinOp(token, node, Term());
            }

            return node;
        }

        public Node Parse()
        {
            Node node = Program();
            if (currentToken.TokenType != TokenType.Eof)
                throw new Exception("Eof expected!");

            return node;
        }

        public void Evaluate()
        {
            Node ast = Parse();
            Visit(ast);
        }

        private object Visit(Node node)
        {
            return visitors[node.GetType()].Invoke(node);
        }

        private object VisitBinOp(Node node)
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

        private object VisitUnaryOp(Node node)
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

        private object VisitNumber(Node node)
        {
            Number num = (Number)node;
            return float.Parse(num.Value);
        }

        private object VisitCompound(Node node)
        {
            Compound compound = (Compound)node;
            return VisitStatements(compound.Statements);
        }

        private object VisitStatements(Node node)
        {
            Statements statements = (Statements)node;

            foreach (Node child in statements.Children)
            {
                Visit(child);
            }

            return null;
        }

        private object VisitAssign(Node node)
        {
            Assign assign = (Assign)node;
            Variable var = (Variable)assign.Left;

            if (globalScope.ContainsKey(var.Token.Value))
            {
                globalScope[var.Token.Value] = Visit(assign.Right);
            }
            else
            {
                throw new Exception($"The variable {var.Token.Value} does not exist in the current context!");
            }

            return null;
        }

        private object VisitDecleration(Node node)
        {
            Decleration decleration = (Decleration)node;
            globalScope.Add(decleration.IdentifierToken.Value, 0f);

            return null;
        }

        private object VisitVariable(Node node)
        {
            Variable variable = (Variable)node;

            if (globalScope.TryGetValue(variable.Token.Value, out object val))
            {
                return val;
            }

            throw new Exception($"The variable {variable.Token.Value} does not exist in the current context!");
        }

        private object VisitNoOp(Node node)
        {
            return null;
        }
    }
}
