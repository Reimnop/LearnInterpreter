using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    public class ActivationRecord
    {
        public ARType Type => _type;
        public int NestingLevel => _nestingLevel;
        public ActivationRecord EnclosingRecord;

        private ARType _type;
        private int _nestingLevel;

        private Dictionary<string, dynamic> members = new Dictionary<string, dynamic>();

        public ActivationRecord(ARType type, int nestingLevel)
        {
            _type = type;
            _nestingLevel = nestingLevel;
        }

        public dynamic this[string key]
        {
            get
            {
                if (TryGet(key, out object val))
                {
                    return val;
                }

                return EnclosingRecord != null ? EnclosingRecord[key] : throw new KeyNotFoundException();
            }
            set
            {
                if (!members.ContainsKey(key))
                {
                    if (EnclosingRecord != null)
                    {
                        EnclosingRecord[key] = value;
                    }
                    else
                    {
                        throw new KeyNotFoundException();
                    }
                    return;
                }

                members[key] = value;
            }
        }

        public void Define(string key, object value)
        {
            members.Add(key, value);
        }

        public bool TryGet(string key, out object value)
        {
            if (members.TryGetValue(key, out value))
            {
                return true;
            }

            return EnclosingRecord != null ? EnclosingRecord.TryGet(key, out value) : false;
        }

        public override string ToString()
        {
            const int align = 10;

            string s = $"{_nestingLevel} : {_type}{Environment.NewLine}";
            foreach (KeyValuePair<string, object> kvp in members)
            {
                s += kvp.Key;
                for (int i = 0; i < align - kvp.Key.Length; i++)
                {
                    s += " ";
                }
                s += $" : {kvp.Value}{Environment.NewLine}";
            }

            return s;
        }
    }
}
