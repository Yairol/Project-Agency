using ProjectAgency.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProjectAgency.Repository.Entities.Abstract;

namespace ProjectAgency.Test
{
    /// <summary>
    /// Clase sobre la cual se realizaran las pruebas a la interfaz <see cref="IActuatorRepository"/>
    /// </summary>
    [TestClass]
    public class ActuatorTest
    {
        /// <summary>
        /// Repositorio que contiene la base de datos
        /// </summary>
        ProjectAgencyRepository _repository;

        /// <summary>
        /// Crea una instancia de <see cref="ActuatorTest"/>
        /// </summary>
        public ActuatorTest()
        {
            _repository = new ProjectAgencyRepository(@"UniTestsDB.xml");
        }

        #region Create
        /// <summary>
        /// Método de prueba para la creación de Actuadores.
        /// </summary>
        /// <param name="name">Nombre del actuador.</param>
        /// <param name="code">Código del actuador.</param>
        /// <param name="machinePos">Posición de la máquina asociada al actuador.</param>
        [TestMethod]
        [DynamicData(nameof(GetCreateActuatorData), DynamicDataSourceType.Method)]
        public void Can_Create_Actuator(string name, string code, string machinePos)
        {
            int positionMahine = int.Parse(machinePos);
            _repository.BeginTransaction();

            //Obtengo todas las máquinas y verifico que existan
            var machines = _repository.GetAllMachines();
            Assert.IsNotNull(machines);
            Assert.AreNotEqual(machines.Count(), 0);

            //Obtengo la máquina que contendrá al actuador.
            var readMachine = _repository.GetMachineById(machines.ElementAt(positionMahine).Id);
            Assert.IsNotNull(readMachine);

            //Creo un actuador y verifico que se creó correctamente
            var actuator = _repository.CreateActuator(name, code, readMachine.Id);
            Assert.IsNotNull(actuator);

            //Obtengo el actuador creado y verifico que los parámetros sean los correctos.
            var readActuator = _repository.GetActuatorById(actuator.Id);
            Assert.IsNotNull(readActuator);
            Assert.AreEqual(readActuator.Name, name);
            Assert.AreEqual(readActuator.Code, code);

            _repository.CommitTransaction();
        }

        public static IEnumerable<object[]> GetCreateActuatorData()
        {
            var sourcePath = @"D:\Estudios\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach(var param in source.Element("ActuatorsTest").Element("Create").Elements())
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
        /// Método de prueba para la obtención de actuadores.
        /// </summary>
        /// <param name="pos">Posición del proyecto en la base de datos.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetGetActuatorData), DynamicDataSourceType.Method)]
        public void Can_Get_Actuator(string pos)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            //Obtengo todos los actuadores y compruebo que existan.
            var actuators = _repository.GetAllActuators();
            Assert.IsNotNull(actuators);
            Assert.AreNotEqual(actuators.Count(), 0);

            //Obtengo un actuador por medio del identificador y compruebo que exista.
            var readActuator = _repository.GetActuatorById(actuators.ElementAt(position).Id);
            Assert.IsNotNull(readActuator);

            _repository.CommitTransaction();
        }

        /// <summary>
        /// Obtiene la data para el método de prueba <see cref="Can_Get_Actuator(string)"/>.
        /// </summary>
        /// <returns>Data para el método de prueba <see cref="Can_Get_Actuator(string)"/>.</returns>
        public static IEnumerable<object[]> GetGetActuatorData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach (var param in source.Element("ActuatorsTest").Element("Get").Elements())
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
        /// Método de prueba para la actualización de Actuadores.
        /// </summary>
        /// <param name="pos">Posición del actuador en la base de datos.</param>
        /// <param name="name">Nombre del actuador.</param>
        /// <param name="code">Codigo del actuador.</param>
        /// <param name="description">Descripción del actuador.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetUpdateActuatorData), DynamicDataSourceType.Method)]
        public void Can_Update_Actuator(string pos, string name, string code, string description)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            //Obtengo todos los actuadores y verifico de que existan.
            var actuators = _repository.GetAllActuators();
            Assert.IsNotNull(actuators);
            Assert.AreNotEqual(actuators.Count(), 0);

            //Busco el actuador que quiero modificar y verifico que exista.
            var readActuator = _repository.GetActuatorById(actuators.ElementAt(position).Id);
            Assert.IsNotNull(readActuator);

            //Compruebo que elementos se quieren modificar y los añado
            if (!string.IsNullOrEmpty(name))
                readActuator.Name = name;
            if (!string.IsNullOrEmpty(code))
                readActuator.Code = code;
            if (!string.IsNullOrEmpty(description))
                readActuator.Description = description;

            //Actualizo el actuador y guardo los cambios en la base de datos.
            _repository.UpdateActuator(readActuator);
            _repository.PartialCommit();

            //Obtengo el actuador modificado y verifico que exista.
            readActuator = _repository.GetActuatorById(readActuator.Id);
            Assert.IsNotNull(readActuator);

            //Verifico que los elementos se hayan modificado correctamente.
            if (!string.IsNullOrEmpty(name))
                Assert.AreEqual(readActuator.Name, name);
            if (!string.IsNullOrEmpty(code))
                Assert.AreEqual(readActuator.Code, code);
            if (!string.IsNullOrEmpty(description))
                Assert.AreEqual(readActuator.Description, description);

            _repository.CommitTransaction();
        }

        public static IEnumerable<object[]> GetUpdateActuatorData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach (var param in source.Element("ActuatorsTest").Element("Update").Elements())
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
        /// Método de prueba para la eliminación de actuadores.
        /// </summary>
        /// <param name="pos">Posición del actuador en la base de datos.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetDeleteActuatorData), DynamicDataSourceType.Method)]
        public void Can_Delete_Actuator(string pos)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            //Obtengo todos los actuadores y verifico de que existan.
            var actuators = _repository.GetAllActuators();
            Assert.IsNotNull(actuators);
            Assert.AreNotEqual(actuators.Count(), 0);

            //Obtengo el actuador a eliminar
            var readActuator = _repository.GetActuatorById(actuators.ElementAt(position).Id);
            Assert.IsNotNull(readActuator);

            //Elimino el actuador  guardo los cambios
            _repository.DeleteActuatorById(readActuator.Id);
            _repository.PartialCommit();

            //Verifico que el actuador no exista
            readActuator = _repository.GetActuatorById(readActuator.Id);
            Assert.IsNull(readActuator);

            _repository.CommitTransaction();
        }

        /// <summary>
        /// Obtiene la data para el método de prueba <see cref="Can_Delete_Actuator(string)"/>.
        /// </summary>
        /// <returns>Data para el método de prueba <see cref="Can_Delete_Actuator(string)"/>.</returns>
        public static IEnumerable<object[]> GetDeleteActuatorData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach (var param in source.Element("ActuatorsTest").Element("Delete").Elements())
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
