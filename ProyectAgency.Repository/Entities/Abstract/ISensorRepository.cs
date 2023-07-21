using Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAgency.Repository.Entities.Abstract
{
    /// <summary>
    /// Modela las funcionalidades de un Repositorio de Sensores.
    /// </summary>
    public interface ISensorRepository: IRepository
    {
        /// <summary>
        /// Crea un Sensor.
        /// </summary>
        /// <param name="name">Nombre del Sensor.</param>
        /// <param name="code">Código del Sensor.</param>
        /// <param name="machineId">Identificador de la Máquina que pertence el Sensor.</param>
        /// <returns>Instancia del tipo <see cref="Sensor"/> con informacion del Proyecto creado.</returns>
        Sensor CreateSensor(string name, string code, int machineId);
        /// <summary>
        /// Obtiene un Sensor de la Base de Datos mediante su identificador.
        /// </summary>
        /// <param name="sensorId">Identificador del Sensor.</param>
        /// <returns>Instancia del tipo <see cref="Sensor"/>.
        /// Si no, devuelve <see langword="null"/>.</returns>
        Sensor? GetSensorById(int sensorId);
        /// <summary>
        /// Obtiene todos los Sensores de la Base de Datos.
        /// </summary>
        /// <returns>Lista de Sensores.</returns>
        IEnumerable<Sensor> GetAllSensors();
        /// <summary>
        /// Obtiene todos los sensores asociados a una Máquina.
        /// </summary>
        /// <param name="machineId">Identificador de la máquina.</param>
        /// <returns>Sensores relacionados con una máquina.</returns>
        IEnumerable<Sensor> GetAllSensorsByMachineId(int machineId);
        /// <summary>
        /// Actualiza un Sensor en la Base de Datos.
        /// </summary>
        /// <param name="sensor">Instancia del Sensor modificado.</param>
        void UpdateSensor(Sensor sensor);
        /// <summary>
        /// Elimina un Sensor de la Base de Datos mediante su identificador.
        /// </summary>
        /// <param name="sensorId">Identificador del Sensor a eliminar</param>
        void DeleteSensorById(int sensorId);
    }
}
