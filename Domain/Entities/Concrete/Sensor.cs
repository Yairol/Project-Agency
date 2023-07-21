using Domain.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Concrete
{
    /// <summary>
    /// Modela un Sensor
    /// </summary>
    public class Sensor: Entity, ICode
    {
        #region Propiedades
        /// <summary>
        /// Codigo del Sensor
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Identificador de la Maquina a la que pertence el Sensor
        /// </summary>
        public int MachineId { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Crea una instancia del tipo <see cref="Sensor"/>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        public Sensor(string Name, string Code):base (Name)
        {
            this.Code = Code;
        }
        #endregion
    }
}
