using Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAgency.Repository.Entities.Abstract
{
    /// <summary>
    /// Modela las funcionalidades de un Repositorio de Variables.
    /// </summary>
    public interface IVariableRepository: IRepository
    {
        /// <summary>
        /// Crea una variable.
        /// </summary>
        /// <param name="name">Nombre de la Variable.</param>
        /// <param name="code">Código de la Variable.</param>
        /// <param name="sensorId">Identificador del Sensor que mide a la Variable.</param>
        /// <param name="actuatorId">Identificador del Actuador que controla a la Variable.</param>
        /// <returns>Instancia del tipo <see cref="Variable"/> con información de la Variable creada.</returns>
        Variable CreateVariable(string name, string code, int sensorId, int actuatorId);
        /// <summary>
        /// Obtiene una Variable de la Base de Datos mediante su identificador.
        /// </summary>
        /// <param name="variableId">Identificador de la Variable.</param>
        /// <returns>Instancia del tipo<see cref="Variable"/>.
        /// Si no, devuelve <see langword="null"/>.</returns>
        Variable? GetVariableById(int variableId);
        /// <summary>
        /// Obtiene todas las Variables de la Base de Datos.
        /// </summary>
        /// <returns>Lista de Variables.</returns>
        IEnumerable<Variable> GetAllVariables();
        /// <summary>
        /// Obtiene todas las variables que le correspondan a un sensor.
        /// </summary>
        /// <param name="sensorId">Identificador del sensor.</param>
        /// <returns></returns>
        IEnumerable<Variable> GetAllVariablesBySensorId(int sensorId);
        /// <summary>
        /// Obtiene todas las variables que le correspondan a un actuador.
        /// </summary>
        /// <param name="actuatorId"></param>
        /// <returns></returns>
        IEnumerable<Variable> GetAllVariablesByActuatorId(int actuatorId);
        /// <summary>
        /// Actualiza una Variable en la Base de Datos.
        /// </summary>
        /// <param name="variable">Instancia de la Variable modificada.</param>
        void UpdateVariable(Variable variable);
        /// <summary>
        /// Elimina una Variable mediante su identificador.
        /// </summary>
        /// <param name="variableId">Identificador de la Variable a eliminar.</param>
        void DeleteVariableById(int variableId);
    }
}
