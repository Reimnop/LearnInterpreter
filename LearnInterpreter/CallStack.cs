using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    public class CallStack
    {
        private Stack<ActivationRecord> records = new Stack<ActivationRecord>();

        public void Push(ActivationRecord ar)
        {
            records.Push(ar);
        }

        public ActivationRecord Pop()
        {
            return records.Pop();
        }

        public ActivationRecord Peek()
        {
            return records.Peek();
        }

        public override string ToString()
        {
            string s = 
                $"CALL STACK{Environment.NewLine}" +
                $"================================{Environment.NewLine}";

            foreach (ActivationRecord ar in records)
            {
                s += $"{ar}{Environment.NewLine}";
            }

            return s;
        }
    }
}
