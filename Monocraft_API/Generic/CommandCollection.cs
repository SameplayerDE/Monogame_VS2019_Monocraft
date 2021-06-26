using Monocraft_API.Abstracts;
using Monocraft_API.Generic.Enumerator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_API.Generic
{
    public class CommandCollection : ICollection
    {

        private Command[] _commands = new Command[1];
        public int Count { get { return _commands.Length - 1; } }

        public object SyncRoot { get { return this; } }

        public bool IsSynchronized { get { return false; } }

        public void CopyTo(Array array, int index)
        {
            foreach(Command command in _commands)
            {
                array.SetValue(command, index);
                index++;
            }
        }

        public void Add()
        {

        }

        public IEnumerator GetEnumerator()
        {
           return new CommandEnumerator(_commands);
        }
    }
}
