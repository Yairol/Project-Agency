using Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAgency.Repository.Entities.Abstract
{
    /// <summary>
    /// Modela las funcionalidades de un Repositorio de Maquinas.
    /// </summary>
    public interface IMachineRepository: IRepository
    {
        /// <summary>
        /// Crea una Máquina
        /// </summary>
        /// <param name="name">Nombre de la Máquina</param>
        /// <param name="code">Código de la Máquina</param>
        /// <param name="proyectId">Identificador del Proyecto</param>
        /// <returns>Instancia del tipo <see cref="Machine"/> con información de la Máquina creada</returns>
        Machine CreateMachine(string name, string code, int proyectId);
        /// <summary>
        /// Obtiene una Máquina de la Base de Datos mediante su identificador.
        /// </summary>
        /// <param name="machineId">Identificador de la Máquina.</param>
        /// <returns>Instancia del tipo <see cref="Machine"/>.
        /// Si no, devuelve <see langword="null"/>.</returns>
        Machine? GetMachineById(int machineId);
        /// <summary>
        /// Obtiene todas las máquinas de la Base de datos
        /// </summary>
        /// <returns>Lista de Máquinas</returns>
        IEnumerable<Machine> GetAllMachines();
        /// <summary>
        /// Obtiene todas las máquinas asociadas a un proyecto.
        /// </summary>
        /// <returns>Lista de máquinas.</returns>
        IEnumerable<Machine> GetAllMachineByIdProject(int proyectId);
        /// <summary>
        /// Actualiza una Máquina en la Base de Datos mediante su identificador
        /// </summary>
        /// <param name="machine">Instancia de la Máquina modificada.</param>
        void UpdateMachine(Machine machine);
        /// <summary>
        /// Elimina una Máquina de la Base de Datos mediante su identificador
        /// </summary>
        /// <param name="machineId">Identificador de la Máquina a eliminar</param>
        void DeleteMachineById(int machineId);
    }
}
