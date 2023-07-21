using Domain.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Concrete
{
    /// <summary>
    /// Modela una Variable
    /// </summary>
    public class Variable: Entity, ICode
    {
        #region Propiedades
        /// <summary>
        /// Codigo de la Variable
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Identificador del Sensor que mide la Variable
        /// </summary>
        public int SensorId { get; set; }
        /// <summary>
        /// Identificador del Actuador que mide la Variable
        /// </summary>
        public int ActuatorId { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Crea una instancia del tipo <see cref="Variable"/>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        public Variable(string Name, string Code): base(Name)
        {
            this.Code = Code;
        }
        #endregion
    }
}
