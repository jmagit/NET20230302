using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos {
    public class Principal {
        static void Main() {
            var nombre = "mundo";
            Console.WriteLine($"Hola, {nombre}!");

            var c = new Documentada();
            var i = c.suma(2, 2);

        }
    }
}
