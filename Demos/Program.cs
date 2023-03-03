// See https://aka.ms/new-console-template for more information
using Biblioteca;
using Demos;
using Demos.Curso;

var nombre = "mundo";
//nombre[2] = '4';

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

