// See https://aka.ms/new-console-template for more information
using Biblioteca;
using Demos;
using Demos.Curso;
using Demos.Cursos;
using System.Reflection;

var nombre = "mundo";
//nombre[2] = '4';
if(nombre == "") {

}
#if DEBUG
Console.WriteLine($"Hola, {nombre}!"[2..5]);
#endif
int[] t = { 1, 2, 3 };
t = new[] { 1, 2, 3 };
t = new int[10];
int[,] m = { { 1, 2 }, { 3, 4 } };
int[][] mm = new int[10][];
mm[0] = new[] { 1, 2, 3 };
mm[1] = new int[10];
mm[0][0] = 66;
mm[3] = mm[0];
mm[3][0] = 666;
Console.WriteLine(mm[0][0]);
var aux = mm[0];
mm[0] = mm[1];
mm[1] = aux;
mm[2] = t;
m[0, 0] = 3;

var c = new Demos.Documentada();
if(c != null && c.suma(2, 2) == 4) {
    mm[2] = t;
    m[0, 0] = 3;
}
var i = c.suma(2, 2);
int j = 1;
Nullable<int> ii = null;
long k = 1L;
k = j = 1;
j = (int)k;


Object obj = i; // Int(i)
i = (decimal)obj + 1; // Int.value(obj)
obj = c;
obj = DateTime.Now;
bool b = true;
if(!b) { }
if(obj is Demos.Documentada cc) {
    c = ((Demos.Documentada)obj);

} else if(obj is SubDocumento) {
    c = ((Demos.Documentada)obj);

}
if(c is DateTime) {
    c = ((Demos.Documentada)obj);

}
//obj = c?.p?.a?.suma() ?? 0;
//if(c!= null && c.p != null && c.p.a != null) {
//c.p.a.suma();

//}
//if(c?.p?.a != null) {
//    c.p.a.suma();

//}
IList<string> x = new List<string>();
Demos.Documentada d = new();
obj = (c ?? new Demos.Documentada()).suma(2, 2);
j += 1;
k = 0;
Console.WriteLine((decimal)0.1 + (decimal)0.2);
obj = (c ?? throw new Exception("c es nulo")).suma(2, 2);

try {

} catch(BadImageFormatException ex) {

    throw new Exception("", ex);
} catch(Exception ex) {

    throw new Exception("", ex);
}

Persona p1 = new Profesor(67), p2 = new Alumno() { Edad = 44, FechaNacimiento = DateTime.Now, [0] = 9, [1] = 5 };

foreach(var nota in (p2 as Alumno)) {
    Console.WriteLine(p1);
    if(nota == 5) break;
}
foreach(var nota in (p2 as Alumno)) {
    Console.WriteLine(p1);
    if(nota == 5) break;
}

(p2 as Alumno).Notas[0] = -1;

var rslt = p2.Calcula(c.resta);
rslt = p2.Calcula(c.genera(3));
rslt = p2.Calcula(delegate (decimal a, decimal b) { return a / b; });

j = (int)p1;

if(p1 > p2 || p1 < 23 || p1 == j) {

}

Console.WriteLine(p1);

PropertyChangeEventHandler cntl1 = delegate (object sender, PropertyChangeEventArgs args) {
    //Console.WriteLine(sender.ToString());
    (sender as Persona).Pintate();
};
PropertyChangeEventHandler cntl2 = delegate (object sender, PropertyChangeEventArgs args) {
    if(args.Property == "Edad")
        Console.WriteLine("Cambia la edad");
    args.Cancel = true;
};

p1.PropertyChange += cntl1;
p1.PropertyChange += cntl2;
p2.PropertyChange += cntl1;
p1.Edad = 33;
p2.Edad = 32;
p1.PropertyChange -= cntl2;
p1.Edad = 32;

