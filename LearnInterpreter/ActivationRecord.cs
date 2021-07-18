﻿using System;
using System.Collections.Generic;

namespace LearnInterpreter
{
    public class ActivationRecord
    {
        public string Name => _name;
        public ARType Type => _type;
        public int NestingLevel => _nestingLevel;

        private string _name;
        private ARType _type;
        private int _nestingLevel;

        private Dictionary<string, object> members = new Dictionary<string, object>();

        public ActivationRecord(string name, ARType type, int nestingLevel)
        {
            _name = name;
            _type = type;
            _nestingLevel = nestingLevel;
        }

        public object this[string key]
        {
            get
            {
                return members[key];
            }
            set
            {
                members[key] = value;
            }
        }

        public bool TryGet(string key, out object value)
        {
            return members.TryGetValue(key, out value);
        }

        public override string ToString()
        {
            const int align = 10;

            string s = $"{_nestingLevel} : {_type} {_name}{Environment.NewLine}";
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