using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_API.Abstracts
{
    public abstract class Command
    {
        /// <value>gets the usage of the command</value>
        public string Usage { get; protected set; }
        /// <value>gets the description of the command</value>
        public string Description { get; protected set; }
        
        /// <value>gets the defined aliases of the command</value>
        public string[] Aliases { get; protected set; } = new string[] { };

        /// <summary>
        /// Invoke the command itself
        /// </summary>
        /// <param name="arguments">arguments passed to the command itself</param>
        /// <returns>success value</returns>
        public abstract int Invoke(string[] arguments);

    }
}
