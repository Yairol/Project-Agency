using Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAgency.Repository.Entities.Abstract
{
    /// <summary>
    /// Modela las funcionalidades de un Repositorio de Actuadores.
    /// </summary>
    public interface IActuatorRepository: IRepository
    {
        /// <summary>
        /// Crea un actuador
        /// </summary>
        /// <param name="name">Nombre del actuador.</param>
        /// <param name="code">Codigo del actuador.</param>
        /// <param name="machineId">Identificador de la Maquina que pertence.</param>
        /// <returns>Instancia del tipo <see cref="Actuator"/> con información del Actuador creado.</returns>
        Actuator CreateActuator(string name, string code, int machineId);
        /// <summary>
        /// Obtiene un Actuador mediante su identificador.
        /// </summary>
        /// <param name="actuatorId">Identificador del Actuador.</param>
        /// <returns>Instancia del tipo <see cref="Actuator"/> con información del Actuador.
        /// Si no, devuleve <see langword="null"/>.</returns>
        Actuator? GetActuatorById(int actuatorId);
        /// <summary>
        /// Obtiene todos los actuadores de la base de datos.
        /// </summary>
        /// <returns>Lista de actuadores.</returns>
        IEnumerable<Actuator> GetAllActuators();
        /// <summary>
        /// Obtiene todos los actuadores que tenga correspondecia con una máquina.
        /// </summary>
        /// <param name="machineId">Identificador de la máquina.</param>
        /// <returns>Actuadores de una máquina.</returns>
        IEnumerable<Actuator> GetAllActuatorsByMachineId(int machineId);
        /// <summary>
        /// Actualiza la información de un actuador mediante su identificador.
        /// </summary>
        /// <param name="actuator">Instancia del Actuador modificado.</param>
        void UpdateActuator(Actuator actuator);
        /// <summary>
        /// Elimina un Actuador de la Base de Datos mediante su identificador
        /// </summary>
        /// <param name="actuatorId">Identificador del Actuador a eliminar</param>
        void DeleteActuatorById(int actuatorId);
    }
}
