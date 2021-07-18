namespace LearnInterpreter
{
    //represents a block "{ ... }"
    public class Block : Node
    {
        public Statements Statements => _statements;

        private Statements _statements;

        public Block(Statements statements)
        {
            _statements = statements;
        }
    }
}
