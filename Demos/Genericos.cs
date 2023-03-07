using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Genericos {
    public class Elemento<T, V> {
        private T elemento;

        public T Key { get; set; }
        public V Value { get; set; }

        public Elemento(T key, V value )        {
            Key = key;
            Value = value;
        }

        public T Calcula() {
            return Key;
        }

        public P conv<P>(P p) {
            return p;
        }
    }

    public class ElementoInt {
        public int Key { get; set; }
        public string Value { get; set; }
    }
    public class ElementoString {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class ElementoChar {
        public char Key { get; set; }
        public string Value { get; set; }
    }
}
