using Domain.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Concrete
{
    /// <summary>
    /// Modela un Proyecto
    /// </summary>
    public class Project: Entity
    {
        #region Propiedades
        /// <summary>
        /// Fecha inicial del proyecto
        /// </summary>
        public DateTime FristDate { get; set; }
        /// <summary>
        /// Fecha fin del proyecto
        /// </summary>
        public DateTime? LastDate { get;set; }
        /// <summary>
        /// Proyecto en el año
        /// </summary>
        public int ProjectYear { get;set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Crea un instancia del tipo <see cref="Project"/>
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="FristDate"></param>
        public Project(string Name, DateTime FristDate): base(Name)
        {
            this.FristDate = FristDate;
        }
        #endregion
    }
}
