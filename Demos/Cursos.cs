using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Cursos {
    public enum Estado {
        Aprobado, Suspendido
    }
    public interface IGrafico {
        void Pintate();
        int Edad { get; set; }
    }
    public interface IGraficoDisposable : IGrafico, IDisposable {
    }

    public delegate decimal OperacionBinaria (decimal a, decimal b);

    public class PropertyChangeEventArgs : CancelEventArgs { 
        public string Property { get; set; }
        public PropertyChangeEventArgs(string property) {
            Property = property;
        }

    }
    public delegate void PropertyChangeEventHandler(object sender, PropertyChangeEventArgs args);

    public abstract class Persona : IGrafico, IDisposable {
        public const int EDAD_MINIMA = 16;
        public readonly int EDAD_JUBILACION = 67;

        private int id;
        private string nombre, apellidos;
        private byte edad;
        private bool activo = true;
        private DateTime fechaBaja;
        private bool disposedValue;

        public event PropertyChangeEventHandler PropertyChange;

        public int Edad { 
            get {
                return (int)edad;
            }
            set {
                if(value == edad) return;
                if(value < EDAD_MINIMA || value > EDAD_JUBILACION)
                    throw new Exception("Edad fuera de rango");
                var x = OnPropertyChange(nameof(Edad));
                if(x is PropertyChangeEventArgs ev && ev.Cancel) return;
                edad = (byte)value;
            } 
        }

        public DateTime FechaNacimiento { get; init; } = DateTime.Now;

        protected PropertyChangeEventArgs OnPropertyChange(string propiedad) {
            if(PropertyChange != null) {
                var arg = new PropertyChangeEventArgs(propiedad);
                PropertyChange(this, arg);
                return arg;
            }
            return null;
        }

        public string NombreCompleto {
            get {
                return $"{nombre} {apellidos}";
            }
            set {
                int espacio = value.IndexOf(' ');
                OnPropertyChange(nameof(NombreCompleto));
            }
        }

        public Persona(int edadJubilacion = 67) {
           EDAD_JUBILACION = edadJubilacion;
        }

        public Persona(string nombre, string apellidos, int edadJubilacion = 67): this(edadJubilacion) {
            this.nombre = nombre;
            this.apellidos = apellidos;
        }

        public virtual void Jubilate(DateTime? fecha = null, bool a = false) {
            if(!activo)
                throw new Exception("Ya esta jubilado");
            fechaBaja = fecha.HasValue ? fecha.Value : DateTime.Now;
            //fechaBaja = fecha ?? DateTime.Now;
            activo = false;
        }
        public virtual int TiempoParaJubilarme() {
            if(!activo)
                return 0;
            return EDAD_JUBILACION - edad;
        }
        public virtual int ParaJubilarme {
            get {
                if(!activo)
                    return 0;
                return EDAD_JUBILACION - edad;
            }
        }

        public void GeneraNombre(string tratamiento, ref bool prefijo, out string resultado) {
            if(String.IsNullOrWhiteSpace(tratamiento))
                tratamiento = "Sr";
            resultado = $"{nombre} {apellidos}";
            if(prefijo) {
                resultado = $"{tratamiento} {resultado}";
                prefijo = false;

            }
        }

        public abstract void Pintate();

        public bool Mayor(Persona persona) {
            if(persona == null)
                throw new Exception("La persona no puede ser nula");
            return edad > persona.edad;
        }

        public decimal Calcula(OperacionBinaria calc) {
            return calc(id, edad);
        }

        #region Sobrecarga de operadores
        public static bool operator > (Persona p1, Persona p2) {
            return p1.edad > p2.edad;
        }
        public static bool operator >(Persona p1, int edad) {
            return p1.edad < edad;
        }
        public static bool operator > (int edad, Persona p1) {
            return p1.edad < edad;
        }
        public static bool operator < (Persona p1, Persona p2) {
            return p1.edad < p2.edad;
        }
        public static bool operator < (Persona p1, int edad) {
            return p1.edad > edad;
        }
        public static bool operator <(int edad, Persona p1) {
            return p1.edad > edad;
        }

        public static implicit operator int (Persona p1) {
            return p1.edad;
        }

        // override object.Equals
        public override bool Equals(object obj) {
            if(!(obj is Persona)) {
                return false;
            }
            return id == ((Persona)obj).id;
        }

        // override object.GetHashCode
        public override int GetHashCode() {
            return this.id.GetHashCode();
        }

        public override string ToString() {
            return $"{id} {nombre} {apellidos}";
        }
        #endregion
        #region Dispose
        protected virtual void Dispose(bool disposing) {
            if(!disposedValue) {
                if(disposing) {
                    // TODO: eliminar el estado administrado (objetos administrados)
                }

                // TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
                // TODO: establecer los campos grandes como NULL
                disposedValue = true;
            }
        }

        // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
        ~Persona() {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: false);
        }

        public void Dispose() {
            // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public partial class Profesor : Persona {
        public Profesor(int edadJubilacion) : base(edadJubilacion) {
        }

        public void Jubilate() {
            Edad = 55;
            if(Edad < 0) {

            }
            base.Jubilate(DateTime.Now);
            base.Jubilate(a: true);
        }

        public override void Pintate() {
            Console.WriteLine("Soy un profesor");
        }

        public void dameNombre() {
            var t = "Don";
            var p = true;
            var s = "kk";
            GeneraNombre(t.Clone() as string, ref p, out s);

        }
        public void PonNotas(Alumno alum) {
            alum.PonNotas(1, 2, 3, 4);
            alum.PonNotas(1);
            alum[0] = 1;
            alum[0, "C#"] = 1;

        }

    }

    public class Alumno : Persona, ICloneable {
        private int[] notas = new int[10];

        internal Alumno() {
            
        }

        public int this[int index] {
            get {
                return notas[index];
            }
            set {
                notas[index] = value;
            }
        }
        public IEnumerator<int> GetEnumerator() {
            for(int i = 0; i<= notas.Length; i++) {
                if(notas[i]< 5) yield break;
                yield return notas[i];
            }
        }

        public int[] Notas {
            get {
                return notas.Clone() as int[];
            }
        }

        public int this[int index, string asignatura] {
            get {
                return notas[index];
            }
            set {
                notas[index] = value;
            }
        }
        public static Alumno CreaAlumno(string nombre, string apellidos) {
            var a = new Alumno();
            // ...
            return a;
        }

        public void PonNotas(int nota, params int[] notas) {

        }
        public void PonNotas(string nota, params string[] notas) {

        }
        public override void Pintate() {
            Console.WriteLine("Soy un alumno");
        }

        public object Clone() {
            Alumno copia = MemberwiseClone() as Alumno;
            if(notas is ICloneable n) {
                copia.notas = n.Clone() as int[];
            }
            return copia;
        }
    }
}
