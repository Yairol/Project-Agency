using Domain.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Concrete
{
    /// <summary>
    /// Modela un tipo de Maquina
    /// </summary>
    public enum TypeMachine {PACKAGING, FILL, STERILIZATION }
    /// <summary>
    /// Modela una Maquina
    /// </summary>
    public class Machine: Entity, ICode
    {
        #region Propiedades
        /// <summary>
        /// Codigo de la Maquina
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tipo de Maquina
        /// </summary>
        public TypeMachine Type { get; set; }
        /// <summary>
        /// Identificador del Proyecto al que pertenece la Maquina
        /// </summary>
        public int ProjectId { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Crea una instancia del tipo <see cref="Machine"/>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Code"></param>
        public Machine(string Name, string Code):base(Name)
        {
            this.Code = Code;
        }
        #endregion
    }
}
