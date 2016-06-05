using System;
using System.Collections.Generic;
using System.Linq;

namespace Xp.Cert
{
    public class Cert
    {
        private static Dictionary<string, Type> commands = new Dictionary<string, Type>()
        {
            { "up", typeof(Commands.Update) },
            { "--help", typeof(Commands.Help) },
            { "-?", typeof(Commands.Help) }
        };

        /// <summary>Entry point</summary>
        public static int Main(string[] args)
        {
            if (args.Length > 0)
            {
                var name = args[0];
                if (commands.ContainsKey(name))
                {
                    return (Activator.CreateInstance(commands[name]) as Command).Execute(args.Skip(1).ToArray());
                }

                Console.Error.WriteLine("Unkown command `{0}'", name);
                return 1;
            }

            return new Commands.Help().Execute(args);
        }
    }
}