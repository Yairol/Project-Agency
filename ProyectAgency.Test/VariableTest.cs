using ProjectAgency.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProjectAgency.Repository.Entities.Abstract;

namespace ProjectAgency.Test
{
    /// <summary>
    /// Clase sobre la cual se realizaran las pruebas a la interfaz <see cref="IVariableRepository"/>
    /// </summary>
    [TestClass]
    public class VariableTest
    {
        /// <summary>
        /// Repositorio sobre el que se realizarán las pruebas.
        /// </summary>
        ProjectAgencyRepository _repository;

        /// <summary>
        /// Crea una instancia de <see cref="VariableTest"/>.
        /// </summary>
        public VariableTest()
        {
            _repository = new ProjectAgencyRepository(@"UniTestsDB.xml");
        }

        /// <summary>
        /// Método de prueba para la creación de variables.
        /// </summary>
        /// <param name="name">Nombre de la variable.</param>
        /// <param name="code">Código de la variable.</param>
        /// <param name="actuatorPos">Posición del actuador en la base de datos.</param>
        /// <param name="sensorPos">Posición del sensor en la base de datos.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetCreateVariableData), DynamicDataSourceType.Method)]
        public void Can_Create_Variable(string name, string code, string actuatorPos, string sensorPos)
        {
            int actuatorPosition = int.Parse(actuatorPos);
            int sensorPosition = int.Parse(sensorPos);

            _repository.BeginTransaction();

            //Obtengo todos los actuadores y verifico que existan.
            var actuators = _repository.GetAllActuators();
            Assert.IsNotNull(actuators);
            Assert.AreNotEqual(actuators.Count(), 0);

            //Obtengo todos los sensores y verifico que existan.
            var sensors = _repository.GetAllSensors();
            Assert.IsNotNull(sensors);
            Assert.AreNotEqual(actuators.Count(), 0);

            //Obtengo el actuador que le corresponde la variable.
            var actuator = _repository.GetActuatorById(actuators.ElementAt(actuatorPosition).Id);
            Assert.IsNotNull(actuator);

            //Obtengo el sensor que le corresponde el sensor.
            var sensor = _repository.GetSensorById(sensors.ElementAt(sensorPosition).Id);
            Assert.IsNotNull(sensor);

            //Creo la variable y verifico que se creo.
            var variable = _repository.CreateVariable(name, code, sensor.Id, actuator.Id);
            Assert.IsNotNull(variable);

            //Otengo la variable creada y verifico que tenga los parametros que le corresponde.
            var readVariable = _repository.GetVariableById(variable.Id);
            Assert.IsNotNull(readVariable);
            Assert.AreEqual(name, readVariable.Name);
            Assert.AreEqual(code, readVariable.Code);
            Assert.AreEqual(actuator.Id, readVariable.ActuatorId);
            Assert.AreEqual(sensor.Id, readVariable.SensorId);

            _repository.CommitTransaction();
        }
        /// <summary>
        /// Obtiene la data para el método de prueba <see cref="Can_Create_Variable(string, string, string, string)"/>.
        /// </summary>
        /// <returns>Data para el método <see cref="Can_Create_Variable(string, string, string, string)"/>.</returns>
        public static IEnumerable<object[]> GetCreateVariableData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach(var param in source.Element("VariablesTest").Element("Create").Elements())
            {
                yield return new object[]
                {
                    param.Attribute("name").Value,
                    param.Attribute("code").Value,
                    param.Attribute("actuatorPos").Value,
                    param.Attribute("sensorPos").Value
                };

            }
        }
        /// <summary>
        /// Método de prueba para le obtención de variables.
        /// </summary>
        /// <param name="pos">Posición de la variable.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetGetVariableData), DynamicDataSourceType.Method)]
        public void Can_Get_Variable(string pos)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            //Obtengo todas las variables en la base de datos y compruebo que existan.
            var variables = _repository.GetAllVariables();
            Assert.IsNotNull(variables);
            Assert.AreNotEqual(variables.Count(), 0);

            //Obtengo la variable mediante el identificador y compruebo que exista.
            var readVariable = _repository.GetVariableById(variables.ElementAt(position).Id);
            Assert.IsNotNull(readVariable);

            _repository.CommitTransaction();
        }

        /// <summary>
        /// Obtiene la data para el método de prueba <see cref="Can_Get_Variable(string)"/>.
        /// </summary>
        /// <returns>Data para el método <see cref="Can_Get_Variable(string)"/>.</returns>
        public static IEnumerable<object[]> GetGetVariableData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach (var param in source.Element("VariablesTest").Element("Get").Elements())
            {
                yield return new object[]
                {
                    param.Attribute("pos").Value
                };
            }
        }

        /// <summary>
        /// Método de prueba para la atualización de variables.
        /// </summary>
        /// <param name="pos">Posición de la variable en la base de datos.</param>
        /// <param name="name">Nombre a actualizar de la variable.</param>
        /// <param name="code">Código a actualizar de la variable.</param>
        /// <param name="description">Descripción a actualizar de la variable.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetUpdateVariableData), DynamicDataSourceType.Method)]
        public void Can_Update_Variable(string pos, string name, string code, string description)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            //Obtengo todas las variables ya compruebo que existan.
            var variables = _repository.GetAllVariables();
            Assert.IsNotNull(variables);
            Assert.AreNotEqual(variables.Count(), 0);

            //Obtengo la variable a modificar y compruebo que exista.
            var readVariable = _repository.GetVariableById(variables.ElementAt(position).Id);
            Assert.IsNotNull(readVariable);

            //Verifico que parámetros se van a actualizar y los agrego a readVariable.
            if (!string.IsNullOrEmpty(name))
                readVariable.Name = name;
            if (!string.IsNullOrEmpty(code))
                readVariable.Code = code;
            if (!string.IsNullOrEmpty(description)) 
                readVariable.Description = description;

            //Actualizo la variable y guardo los cambios.
            _repository.UpdateVariable(readVariable);
            _repository.PartialCommit();

            //Obtengo la variable y verifico que esté en la base de datos.
            readVariable = _repository.GetVariableById(readVariable.Id);
            Assert.IsNotNull(readVariable);

            //Analizo que los parámetros actualizado estén con sus valores correspondientes.
            if (!string.IsNullOrEmpty(name))
                Assert.AreEqual(readVariable.Name, name);
            if (!string.IsNullOrEmpty(code))
                Assert.AreEqual(readVariable.Code, code);
            if (!string.IsNullOrEmpty(description))
                Assert.AreEqual(readVariable.Description, description);

            _repository.CommitTransaction();
        }

        /// <summary>
        /// Obtiene la data para el método de prueba <see cref="Can_Update_Variable(string, string, string, string)"/>.
        /// </summary>
        /// <returns>Data para el método de prueba <see cref="Can_Update_Variable(string, string, string, string)"/>.</returns>
        public static IEnumerable<object[]> GetUpdateVariableData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach (var param in source.Element("VariablesTest").Element("Update").Elements())
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

        /// <summary>
        /// Método de prueba para la eliminación de variables.
        /// </summary>
        /// <param name="pos">Posición de la variable.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetDeleteVariableData), DynamicDataSourceType.Method)]
        public void Can_Delete_Variable(string pos)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            //Obtengo todas las variables y compruebo que existan.
            var variables = _repository.GetAllVariables();
            Assert.IsNotNull(variables);
            Assert.AreNotEqual(variables.Count(), 0);

            //Obtengo la variable a eliminar
            var readVariable = _repository.GetVariableById(variables.ElementAt(position).Id);
            Assert.IsNotNull(readVariable);

            //Elimino la variable y guardo los cambios.
            _repository.DeleteVariableById(readVariable.Id);
            _repository.PartialCommit();

            //Verifico que la variable ya no esté en la base de datos.
            readVariable = _repository.GetVariableById(readVariable.Id);
            Assert.IsNull(readVariable);

            _repository.CommitTransaction();
        }

        /// <summary>
        /// Obtiene la data para el método de prueba <see cref="Can_Delete_Variable(string)"/>.
        /// </summary>
        /// <returns>Data para el método de prueba <see cref="Can_Delete_Variable(string)"/>.</returns>
        public static IEnumerable<object[]> GetDeleteVariableData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach (var param in source.Element("VariablesTest").Element("Delete").Elements())
            {
                yield return new object[]
                {
                    param.Attribute("pos").Value,
                };
            }
        }
    }
}
