using Monocraft_API.Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_API.Generic.Enumerator
{
    public class CommandEnumerator : IEnumerator
    {

        private Command[] _commands;
        private int _index;

        public object Current
        {
            get
            {
                if ((_index < 0) || (_index == _commands.Length))
                {
                    throw new InvalidOperationException();
                }
                return _commands[_index];
            }
        }

        public CommandEnumerator(Command[] commands)
        {
            _commands = commands;
            _index = -1;
        }

        public bool MoveNext()
        {
            if (_index < _commands.Length)
            {
                _index++;
            }
            return _index != _commands.Length;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
