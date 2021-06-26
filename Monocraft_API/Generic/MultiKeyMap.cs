using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monocraft_API.Generic
{
    public class MultiKeyMap<TKey0, TKey1, TValue>
    {

        private Dictionary<TKey0, Dictionary<TKey1, TValue>> _data;

        public MultiKeyMap()
        {
            _data = new Dictionary<TKey0, Dictionary<TKey1, TValue>>();
        }

        public void Add(TKey0 key0, TKey1 key1, TValue value)
        {
            Dictionary<TKey1, TValue> dictionary = new Dictionary<TKey1, TValue>();
            dictionary.Add(key1, value);
            _data.Add(key0, dictionary);
        }

        public TValue Get(TKey0 key0, TKey1 key1)
        {
            return _data[key0][key1];
        }

        public bool Has(TKey0 key0, TKey1 key1)
        {

            if (_data.ContainsKey(key0))
            {
                Dictionary<TKey1, TValue> dictionary = _data[key0];
                if (dictionary.ContainsKey(key1))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

    }
}
