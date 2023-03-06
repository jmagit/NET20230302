using Demos.Cursos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Demos {
    /// <summary>
    /// Ejemplo de documentación
    /// </summary>    /// 
#if DEBUG
    public class Documentada {
#else
    internal class Documentada {
#endif
        #region Atributos

        #endregion
        #region Métodos

        /// <summary>
        ///     Suma dos valores
        /// </summary>
        /// <param name="a">Operando 1</param>
        /// <param name="b">Operando 2</param>
        /// <returns>Suma de los valores</returns>
        public decimal suma(decimal a, decimal b = 0, decimal c = 0) {
            #region Métodos
            return Abs(a + b + c);
            #endregion
        }
        public decimal avg(decimal a, params decimal[] lst) {
            return Abs(a);
        }
        public string nombre() {
            avg(1, 2, 3, 4);
            suma(4, c: 4);
            return nameof(suma);
        }
        public decimal resta(decimal a, decimal b) {
            return a - b;
        }

        public decimal multiplica(decimal a, decimal b) {
            return a * b;
        }
        public OperacionBinaria genera(int i) {
            switch(i) {
                case 1:
                case 2:
                    return resta;
                default:
                    return multiplica;
            }
        }
        #endregion
    }
}

namespace Demos.Curso {
    public class Documentada {
        #region Atributos

        #endregion
        #region Métodos

        /// <summary>
        ///     Suma dos valores
        /// </summary>
        /// <param name="a">Operando 1</param>
        /// <param name="b">Operando 2</param>
        /// <returns>Suma de los valores</returns>
        public decimal suma(decimal a, decimal b) {
            #region Métodos
            return a + b;
            #endregion
        }
        public string nombre() {
            return nameof(suma);
        }
        #endregion
    }

    public class SubDocumento : Demos.Documentada {

    }
}
