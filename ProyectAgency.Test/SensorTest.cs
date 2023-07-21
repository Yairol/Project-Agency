using ProjectAgency.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProjectAgency.Repository.Entities.Abstract;

namespace ProjectAgency.Test
{
    /// <summary>
    /// Clase sobre la cual se realizarán las pruebas a la interfaz <see cref="ISensorRepository"/>
    /// </summary>
    [TestClass]
    public class SensorTest
    {

        /// <summary>
        /// Repositorio sobre el que se realizarán las pruebas.
        /// </summary>
        ProjectAgencyRepository _repository;

        /// <summary>
        /// Crea una instancia del tipo <see cref="ProjectTest"/>
        /// </summary>
        public SensorTest()
        {
            _repository = new ProjectAgencyRepository(@"UniTestsDB.xml");
        }

        #region Create
        /// <summary>
        /// Método de prueba para la creación de Sensores.
        /// </summary>
        /// <param name="name">Nombre del sensor.</param>
        /// <param name="code">Código del sensor.</param>
        /// <param name="machinePos">Posición de la máquina en la base de datos asociada al sensor.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetCanCreateSensorData), DynamicDataSourceType.Method)]
        public void Can_Create_Sensor(string name, string code, string machinePos)
        {
            int position = int.Parse(machinePos);
            _repository.BeginTransaction();

            var machines = _repository.GetAllMachines();
            Assert.IsNotNull(machines);
            Assert.AreNotEqual(machines.Count(), 0);

            var readMachine = _repository.GetMachineById(machines.ElementAt(position).Id);
            Assert.IsNotNull(readMachine);

            var sensor = _repository.CreateSensor(name, code, readMachine.Id);
            Assert.IsNotNull(sensor);
            
            var readSensor = _repository.GetSensorById(sensor.Id);
            Assert.IsNotNull(readSensor);
            Assert.AreEqual(readSensor.Name, name);
            Assert.AreEqual(readSensor.Code, code);
            
            _repository.CommitTransaction();

        }

        /// <summary>
        /// Obtiene la Data para el método de prueba <see cref="Can_Create_Sensor(string, string, string)"/>.
        /// </summary>
        /// <returns>Data para el método de prueba <see cref="Can_Create_Sensor(string, string, string)"/>.</returns>
        public static IEnumerable<object[]> GetCanCreateSensorData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach(var param in source.Element("SensorsTest").Element("Create").Elements())
            {
                yield return new object[]
                {
                    param.Attribute("name").Value,
                    param.Attribute("code").Value,
                    param.Attribute("machinePos").Value
                };
            }
        }
        #endregion

        #region Get
        /// <summary>
        /// Método de prueba para la obtención de sensores.
        /// </summary>
        /// <param name="pos">Posición del sensor en la base de datos.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetGetSensorData), DynamicDataSourceType.Method)]
        public void Can_Get_Sensor(string pos)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            var sensors = _repository.GetAllSensors();
            Assert.IsNotNull(sensors);
            Assert.AreNotEqual(sensors.Count(), 0);

            var readSensor = _repository.GetSensorById(sensors.ElementAt(position).Id);
            Assert.IsNotNull(readSensor);

            _repository.CommitTransaction();
        }

        /// <summary>
        /// Obtiene la Data para el método de prueba <see cref="Can_Get_Sensor(string)"/>.
        /// </summary>
        /// <returns>Data para el método de prueba <see cref="Can_Get_Sensor(string)"/>.</returns>
        public static IEnumerable<object[]> GetGetSensorData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach (var param in source.Element("SensorsTest").Element("Get").Elements())
            {
                yield return new object[]
                {
                    param.Attribute("pos").Value
                };
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Método de prueba para la actualización de sesnores.
        /// </summary>
        /// <param name="pos">Posición del sensor en la base de datos.</param>
        /// <param name="name">Nombre del sensor.</param>
        /// <param name="code">Código del sensor.</param>
        /// <param name="description">Descripción del sensor.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetUpdateSensorData), DynamicDataSourceType.Method)]
        public void Can_Update_Sensor(string pos, string name, string code, string description)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();
            //Obtengo todos los sensores en la base de datos
            var sensors = _repository.GetAllSensors();
            Assert.IsNotNull(sensors);

            //Obtengo el sensor que se modificará
            var readSensor = _repository.GetSensorById(sensors.ElementAt(position).Id);
            Assert.IsNotNull(readSensor);

            //Verifico que parámetros se van a modificar y los modifico.
            if (!string.IsNullOrEmpty(name))
                readSensor.Name = name;
            if(!string.IsNullOrEmpty(code))
                readSensor.Code= code;
            if (!string.IsNullOrEmpty(description))
                readSensor.Description = description;

            //Modifico el sensor y guardo los cambios.
            _repository.UpdateSensor(readSensor);
            _repository.PartialCommit();

            //Obtengo el sensor modificado
            readSensor = _repository.GetSensorById(readSensor.Id);
            Assert.IsNotNull(readSensor);

            //Compara si los datos modificados han sido actualizados correctamente
            if (!string.IsNullOrEmpty(name))
                Assert.AreEqual(readSensor.Name, name);
            if (!string.IsNullOrEmpty(code))
                Assert.AreEqual(readSensor.Code, code);
            if (!string.IsNullOrEmpty(description))
                Assert.AreEqual(readSensor.Description, description);

            _repository.CommitTransaction();
        }

        /// <summary>
        /// Obtiene la Data para el método de prueba <see cref="Can_Update_Sensor(string, string, string, string)"/>.
        /// </summary>
        /// <returns>Data para el método de prueba <see cref="Can_Update_Sensor(string, string, string, string)"/>.</returns>
        public static IEnumerable<object[]> GetUpdateSensorData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach (var param in source.Element("SensorsTest").Element("Update").Elements())
            {
                yield return new object[]
                {
                    param.Attribute("pos").Value,
                    param.Attribute("name").Value,
                    param.Attribute("code").Value,
                    param.Attribute("description").Value
                };
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Método de preuba para la eliminación de sensores.
        /// </summary>
        /// <param name="pos">Posición del sensor a eliminar.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetDeleteSensorData), DynamicDataSourceType.Method)]
        public void Can_Delete_Sensor(string pos)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            //Obtengo todos los sensores en la base de datos y verifico que existan.
            var sensors = _repository.GetAllSensors();
            Assert.IsNotNull(sensors);
            Assert.AreNotEqual(sensors.Count(), 0);

            //Obtengo el sensor a eliminar y verifico que exista.
            var readSensor = _repository.GetSensorById(sensors.ElementAt(position).Id);
            Assert.IsNotNull(readSensor);

            //Elimino el sensor y guardo los cambios realizados.
            _repository.DeleteSensorById(readSensor.Id);
            _repository.PartialCommit();

            //Verifico que el sensor ya no esté en la base de datos.
            readSensor = _repository.GetSensorById(readSensor.Id);
            Assert.IsNull(readSensor);

            _repository.CommitTransaction();
        }

        /// <summary>
        /// Obtiene la data para el método de prueba <see cref="Can_Delete_Sensor(string)"/>.
        /// </summary>
        /// <returns>Data para el método de prueba <see cref="Can_Delete_Sensor(string)"/>.</returns>
        public static IEnumerable<object[]> GetDeleteSensorData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach (var param in source.Element("SensorsTest").Element("Delete").Elements())
            {
                yield return new object[]
                {
                    param.Attribute("pos").Value
                };
            }
        }
        #endregion
    }
}
