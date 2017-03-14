using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmParser
{
    public class EnvironmentVariableManager
    {
        Dictionary<string, string> _variables;

        public EnvironmentVariableManager()
        {
            _variables = new Dictionary<string, string>();
        }

        public EnvironmentVariableManager(EnvironmentVariableManager src) : this()
        {
            foreach (var kv in src._variables)
            {
                _variables[kv.Key] = kv.Value;
            }
        }

        public void SetVariable(string name, string value)
        {
            _variables[name] = value;
        }

        public string GetVariable(string name)
        {
            if (_variables.ContainsKey(name))
                return _variables[name];
            return null;
        }

        public string Replace(string str)
        {
            var resSb = new StringBuilder();
            var tmpSb = new StringBuilder();
            var mode = 1;
            for (var i = 0; i < str.Length; i++)
            {
                var curChar = str[i];
                switch (mode)
                {
                    case 1:
                        if (curChar == '%')
                            mode = 2;
                        else if (curChar == '\\')
                            mode = 3;
                        else
                            resSb.Append(curChar);
                        break;
                    case 2:
                        if (curChar == '%')
                        {
                            var varName = tmpSb.ToString();
                            resSb.Append(GetVariable(varName) ?? $"%{varName}%");
                            tmpSb.Clear();
                            mode = 1;
                        }
                        else
                            tmpSb.Append(curChar);
                        break;
                    case 3:
                        resSb.Append(curChar);
                        mode = 1;
                        break;
                }
            }
            return resSb.ToString();
        }
    }

}
