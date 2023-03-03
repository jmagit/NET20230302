using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class SubDocumento : Documentada {

    }
}
