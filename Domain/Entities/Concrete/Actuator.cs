using Domain.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Concrete
{
    /// <summary>
    /// Modela un actuador
    /// </summary>
    public class Actuator: Entity, ICode
    {
        #region Propiedades
        /// <summary>
        /// Codigo del actuador
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Identificador de la Maquina a el actuador
        /// </summary>
        public int MachineId { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Crea una instancia del tipo <see cref="Actuator"/>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        public Actuator(string Name, string Code): base(Name)
        {
            this.Code= Code;
        }
        #endregion
    }
}
