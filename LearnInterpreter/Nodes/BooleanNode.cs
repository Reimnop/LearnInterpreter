namespace LearnInterpreter
{
    public class BooleanNode : Node
    {
        public Condition Condition => _condition;
        public ConditionNode ConditionNode => _conditionNode;

        private Condition _condition;
        private ConditionNode _conditionNode;

        public BooleanNode(Condition condition, ConditionNode conditionNode = null)
        {
            _condition = condition;
            _conditionNode = conditionNode;
        }
    }
}
