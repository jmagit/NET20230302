using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos {
    internal class Tareas {
        public static Task<int> Tarea1Async(int num, int duracion) {
            return Task.Run<int>(() => {
                Thread.Sleep(duracion);
                return num;
            });
        }

        public static void proc1() {
            int x = 1;
            Console.WriteLine($"Inicio  x: {x}");
            var t = Task.Run<int>(() => { return x++; });
            Console.WriteLine($"Lanzo  x: {x}");
            t.Wait();
            Console.WriteLine($"Termino x: {x} rslt: {t.Result}");

            var t1 = Tarea1Async(1, 500);
            t1.Wait();
            Console.WriteLine(t1.Result);
            List<Task<int>> muchas = new List<Task<int>>();
            muchas.Add(Tarea1Async(1, 500));
            muchas.Add(Tarea1Async(2, 200));
            muchas.Add(Tarea1Async(3, 1000));
            Task.WaitAll(muchas.ToArray());
            Task.WaitAny(muchas.ToArray());

        }
        public static async void proc2() {
            int x = 1;
            Console.WriteLine($"Inicio  x: {x}");
            var r = await Task.Run<int>(() => { return x++; });
            Console.WriteLine($"Termino x: {x} rslt: {r}");
            r = await Tarea1Async(x, 500);
            Console.WriteLine($"Termino x: {x} rslt: {r}");
        }
    }
}
