using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast;

public class VariableManager
{
    public static Dictionary<string, Variable> Variables = new();

    public static void set(string name, object? value, bool final)
    {
        if (!Variables.ContainsKey(name))
        {
            Variables[name] = new Variable(name, value, final);
            return;
        }
        if (Variables[name].final)
        {
            Console.Error.WriteLine($"Variable {name} is final.");
            return;
        }
        Variables[name].value = value;
        
    }

    public static object? get(string name)
    {
        if (!Variables.ContainsKey(name))
        {
            throw new Exception($"Variable {name} hasn't been defined yet.");
        }
        return Variables[name].value;
    }

    public static void set(string name, object? value)
    {
        set(name, value, isFinal(name));
    }

    public static bool isFinal(string name)
    {
        if (!Variables.ContainsKey(name))
            throw new Exception($"Variable {name} hasn't been declared yet.");

        return Variables[name].final;
    }

    public class Variable
    {
        public string name { get; set; }
        public object? value { get; set; }
        public bool final { get; }

        public Variable(string name, object? value, bool final)
        {
            this.name = name;
            this.value = value;
            this.final = final;
        }
    }
}
