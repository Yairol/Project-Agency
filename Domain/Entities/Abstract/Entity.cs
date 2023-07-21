using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Abstract
{
    /// <summary>
    /// Modela una entidad a ser almacenada en una base de datos.
    /// </summary>
    public abstract class Entity
    {
        #region Propiedades
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        #endregion

        #region Constructor
        public Entity(string Name)
        {
            this.Name = Name;
        }
        #endregion
    }
}
