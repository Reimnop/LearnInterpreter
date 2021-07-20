using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    //Refer to MyScript.gram for grammar 
    public class Parser
    {
        private Lexer lexer;
        private Token currentToken;

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
            currentToken = lexer.NextToken();
        }

        private void Eat(TokenType tokenType)
        {
            if (tokenType == currentToken.TokenType)
            {
                currentToken = lexer.NextToken();
            }
            else
            {
                ThrowError(ErrorCodes.UnexpectedToken, currentToken);
            }
        }

        private void ThrowError(string errorCode, Token token)
        {
            throw new ParserError(errorCode, token, $"{errorCode} -> {token}");
        }

        private ProgramNode Program()
        {
            if (currentToken.TokenType == TokenType.OpenBracket)
            {
                return new ProgramNode(Block());
            }
            else
            {
                return new ProgramNode(StatementList());
            }
        }

        private Block Block()
        {
            Eat(TokenType.OpenBracket);
            Statements statements = StatementList();
            Eat(TokenType.CloseBracket);

            return new Block(statements);
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
            switch (currentToken.TokenType)
            {
                case TokenType.Identifier:
                    if (lexer.CurrentChar == '(')
                    {
                        return MethodCallStatement();
                    }
                    return AssignmentStatement();
                case TokenType.Var:
                    return VariableDeclarationStatement();
                case TokenType.Function:
                    return MethodDeclarationStatement();
                case TokenType.If:
                    return IfStatement();
                case TokenType.Return:
                    return ReturnStatement();
                default:
                    return new NoOp();
            }
        }

        private ReturnStatement ReturnStatement()
        {
            Eat(TokenType.Return);

            return new ReturnStatement(Expr());
        }

        private IfNode IfStatement()
        {
            Eat(TokenType.If);
            Eat(TokenType.LeftParen);

            BooleanNode boolean = Boolean();

            Eat(TokenType.RightParen);

            Block block = Block();

            return new IfNode(boolean, block);
        }

        private BooleanNode Boolean()
        {
            Token token = currentToken;
            if (token.TokenType == TokenType.True)
            {
                Eat(TokenType.True);
                return new BooleanNode(Condition.True);
            }

            if (token.TokenType == TokenType.False)
            {
                Eat(TokenType.False);
                return new BooleanNode(Condition.False);
            }

            return new BooleanNode(Condition.NeedEval, ConditionNode());
        }

        private ConditionNode ConditionNode()
        {
            Node left = Expr();

            Token op = currentToken;
            switch (op.TokenType)
            {
                case TokenType.GreaterThan:
                    Eat(TokenType.GreaterThan);
                    break;
                case TokenType.LessThan:
                    Eat(TokenType.LessThan);
                    break;
                default:
                    Eat(TokenType.Equal);
                    break;
            }

            Node right = Expr();

            return new ConditionNode(left, right, op);
        }

        private MethodCall MethodCallStatement()
        {
            Token token = currentToken;
            Eat(TokenType.Identifier);
            Eat(TokenType.LeftParen);

            List<Node> parameters = null;
            if (currentToken.TokenType != TokenType.RightParen)
            {
                parameters = new List<Node>() { Expr() };
            }

            while (currentToken.TokenType == TokenType.Comma)
            {
                Eat(TokenType.Comma);
                parameters.Add(Expr());
            }

            Eat(TokenType.RightParen);

            return new MethodCall(token.Value, parameters, token);
        }

        private VariableDeclaration VariableDeclarationStatement()
        {
            Eat(TokenType.Var);

            Token token = currentToken;
            Eat(TokenType.Identifier);

            Node assignment = null;
            if (currentToken.TokenType == TokenType.Assign)
            {
                Eat(TokenType.Assign);
                assignment = Expr();
            }

            return new VariableDeclaration(token, assignment);
        }

        private MethodDeclaration MethodDeclarationStatement()
        {
            Eat(TokenType.Function);

            Token token = currentToken;
            Eat(TokenType.Identifier);

            Eat(TokenType.LeftParen);

            Parameters parameters = null;
            if (currentToken.TokenType == TokenType.RightParen)
            {
                Eat(TokenType.RightParen);
            }
            else
            {
                parameters = ParameterList();
                Eat(TokenType.RightParen);
            }

            Block block = Block();

            return new MethodDeclaration(token.Value, parameters, block);
        }

        private Parameters ParameterList()
        {
            List<Parameter> parameters = new List<Parameter>() { Parameter() };

            while (currentToken.TokenType == TokenType.Comma)
            {
                Eat(TokenType.Comma);
                parameters.Add(Parameter());
            }

            return new Parameters(parameters);
        }

        private Parameter Parameter()
        {
            Eat(TokenType.Var);
            Variable var = Variable();

            return new Parameter(var);
        }

        private Node AssignmentStatement()
        {
            Variable left = Variable();
            Token op = currentToken;
            Eat(TokenType.Assign);
            Node right = Expr();

            return new Assign(op, left, right);
        }

        private Variable Variable()
        {
            Token token = currentToken;
            Eat(TokenType.Identifier);

            Node indexNode = null;
            if (currentToken.TokenType == TokenType.LeftSquareBracket)
            {
                Eat(TokenType.LeftSquareBracket);
                indexNode = Expr();
                Eat(TokenType.RightSquareBracket);
            }

            return new Variable(token, indexNode);
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
                return Number();
            }

            if (currentToken.TokenType == TokenType.Plus || currentToken.TokenType == TokenType.Minus)
            {
                Eat(currentToken.TokenType);
                return new UnaryOp(token, Factor());
            }

            if (currentToken.TokenType == TokenType.OpenBracket)
            {
                return Array();
            }

            if (currentToken.TokenType == TokenType.String)
            {
                Eat(TokenType.String);
                return new StringNode(token.Value);
            }

            if (currentToken.TokenType == TokenType.Identifier && lexer.CurrentChar == '(')
            {
                return MethodCallStatement();
            }

            Node var = Variable();
            return var;
        }

        private ArrayNode Array()
        {
            Eat(TokenType.OpenBracket);

            List<Node> elements = new List<Node>();

            if (currentToken.TokenType != TokenType.CloseBracket)
            {
                elements.Add(Expr());

                while (currentToken.TokenType == TokenType.Comma)
                {
                    Eat(TokenType.Comma);
                    elements.Add(Expr());
                }
            }

            Eat(TokenType.CloseBracket);

            return new ArrayNode(elements);
        }

        private Node Number()
        {
            Token token = currentToken;
            Eat(TokenType.Integer);

            string num = token.Value;

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

        public ProgramNode Parse()
        {
            ProgramNode node = Program();
            if (currentToken.TokenType != TokenType.Eof)
                throw new Exception("Eof expected!");

            return node;
        }
    }
}
