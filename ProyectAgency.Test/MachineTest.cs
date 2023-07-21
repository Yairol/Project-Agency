using ProjectAgency.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProjectAgency.Repository.Entities.Abstract;
using Domain.Entities.Concrete;

namespace ProjectAgency.Test
{
    /// <summary>
    /// Clase sobre la que se realizarán las pruebas unitarias a la  implementación de <see cref="IMachineRepository"/>
    /// </summary>
    [TestClass]
    public class MachineTest
    {
        /// <summary>
        /// Repositorio sobre el que se realizarán las pruebas.
        /// </summary>
        ProjectAgencyRepository _repository;

        /// <summary>
        /// Crea una instancia de <see cref="MachineTest"/>.
        /// </summary>
        public MachineTest()
        {
            _repository = new ProjectAgencyRepository(@"UniTestsDB.xml");
        }

        #region Create
        /// <summary>
        /// Método de prueba para la creación de proyectos.
        /// </summary>
        /// <param name="name">Nombre de la máquina.</param>
        /// <param name="code">Código de la máquina.</param>
        /// <param name="projcetPos">Id del proyecto al que pertenece la máquina.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetCreateMachineData), DynamicDataSourceType.Method)]
        public void Can_Create_Machine(string name, string code, string projcetPos)
        {
            int projectPosition = int.Parse(projcetPos);

            //Inicio la transacción.
            _repository.BeginTransaction();

            //Obtengo todos los proyectos anteriormente creados
            var projects = _repository.GetAllProjects();
            Assert.IsNotNull(projects);

            //Obtengo los Id de los proyectos para asignarle ese Id a las máquinas que voy a crear
            var projectId = projects.ElementAt(projectPosition).Id;

            //Creo la máquina y verifico que esté creada
            var machine = _repository.CreateMachine(name, code, projectId);
            Assert.IsNotNull(machine);
            Assert.AreEqual(name, machine.Name);
            Assert.AreEqual(code, machine.Code);
            Assert.AreEqual(projectId, machine.ProjectId);

            //Verifico que la máquina está en la base de datos.
            var readedMachine = _repository.GetMachineById(machine.Id);

            Assert.IsNotNull(readedMachine);

            //Cierro la transacción.
            _repository.CommitTransaction();
        }
        /// <summary>
        /// Obtiene los datos para las pruebas de <see cref="Can_Create_Machine"/>
        /// </summary>
        /// <returns>Data para las pruebas de <see cref="Can_Create_Machine"/></returns>
        public static IEnumerable<object[]> GetCreateMachineData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach(var param in source.Element("MachinesTest").Element("Create").Elements())
            {
                yield return new object[]
                {
                    param.Attribute("name").Value,
                    param.Attribute("code").Value,
                    param.Attribute("projectPos").Value
                };
            }
        }
        #endregion

        #region Get
        /// <summary>
        /// Método de prueba para la obtención de máquinas. 
        /// </summary>
        /// <param name="pos">Posición del proyecto</param>
        [DataTestMethod]
        [DynamicData(nameof(GetGetMachineData), DynamicDataSourceType.Method)]
        public void Can_Get_Machine(string pos)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            var machines = _repository.GetAllMachines();

            Assert.IsNotNull(machines);
            Assert.AreNotEqual(machines.Count(), 0);

            var readMachine = _repository.GetMachineById(machines.ElementAt(position).Id);

            Assert.IsNotNull(readMachine);

            
            _repository.CommitTransaction();
        }

        /// <summary>
        /// Obtiene la data para el método de prueba <see cref="Can_Get_Machine"/>.
        /// </summary>
        /// <returns>Data para el método de prueba <see cref="Can_Get_Machine"/>.</returns>
        public static IEnumerable<object[]> GetGetMachineData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach(var param in source.Element("MachinesTest").Element("Get").Elements())
            {
                yield return new object[]
                {
                    param.Attribute("pos").Value
                };
            }
        }
        #endregion

        #region GetAll
        /// <summary>
        /// Método de prueba para la obtención de máquinas asociadas a un proyecto.
        /// </summary>
        /// <param name="pos">Posición del proyecto.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetGetAllMachineByIdProjectData), DynamicDataSourceType.Method)]
        public void Can_GetAllMachineByIdProject_Machine(string pos)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            //Cargo todos los proyectos en la base de datos
            var proyects = _repository.GetAllProjects();

            //Verifico que estén que esten cargados
            Assert.IsNotNull(proyects);
            Assert.AreNotEqual(proyects.Count(), 0);

            //Obtengo todos las máquinas asociados al Id del proyecto obtenido
            var readAllMachine = _repository.GetAllMachineByIdProject(proyects.ElementAt(position).Id);

            //Verifico que no sea nulo y que existan máquinas
            Assert.IsNotNull(readAllMachine);
            Assert.AreNotEqual(readAllMachine.Count(), 0);

            _repository.CommitTransaction();
        }

        /// <summary>
        /// Obtiene la Data para el método de prueba <see cref="Can_GetAllMachineByIdProject_Machine(string)"/>.
        /// </summary>
        /// <returns>Data para el método de prueba <see cref="Can_GetAllMachineByIdProject_Machine(string)"/>.</returns>
        public static IEnumerable<object[]> GetGetAllMachineByIdProjectData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            //Obtengo las posiciones de los proyectos
            foreach(var param in source.Element("ProjectsTest").Element("Get").Elements())
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
        /// Método de prueba para la actulización de una máquina.
        /// </summary>
        /// <param name="pos">Posición de la máquina a Actulizar.</param>
        /// <param name="name">Nombre de la máquina a Actulizar.</param>
        /// <param name="code">Código de la máquina a Actulizar.</param>
        /// <param name="Description">Descripción de la máquina a Actulizar.</param>
        /// <param name="Type">Tipo de la máquina a Actulizar.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetUpdateMachineData), DynamicDataSourceType.Method)]
        public void Can_Update_Machine(string pos, string name, string code, string Description, string Type)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            var machines = _repository.GetAllMachines();

            Assert.IsNotNull(machines);
            Assert.AreNotEqual(machines.Count(), 0);
            
            var readMachine = _repository.GetMachineById(machines.ElementAt(position).Id);
            
            Assert.IsNotNull(readMachine);
            //Verifico que parametros se van a actulizar y se lo asigno a la máquina.
            if(!string.IsNullOrEmpty(name))
                readMachine.Name = name;
            if(!string.IsNullOrEmpty(code))
                readMachine.Code = code;
            if (!string.IsNullOrEmpty(Description))
                readMachine.Description = Description;
            if (!string.IsNullOrEmpty(Type))
            {
                readMachine.Type = (TypeMachine)Enum.Parse(typeof(TypeMachine), Type);
            }
            //Actualizo la máquina con los parámetros que se les ha sido asignado y guardo los cambios en la base de datos.
            _repository.UpdateMachine(readMachine);
            _repository.PartialCommit();

            readMachine = _repository.GetMachineById(machines.ElementAt(position).Id);

            Assert.IsNotNull(readMachine);

            //Verifico que los parametros se actualizaron correctamente
            if (!string.IsNullOrEmpty(name))
                Assert.AreEqual(readMachine.Name, name);
            if (!string.IsNullOrEmpty(code))
                Assert.AreEqual(readMachine.Code, code);
            if (!string.IsNullOrEmpty(Description))
                Assert.AreEqual(readMachine.Description, Description);
            if (!string.IsNullOrEmpty(Type))
            {
                Assert.AreEqual(readMachine.Type.ToString(), Type);
            }

            _repository.CommitTransaction();

        }

        /// <summary>
        /// Método para la obtención de data para el metodo de prueba <see cref="Can_Update_Machine(string, string, string, string, string)"/>.
        /// </summary>
        /// <returns>Data para el método de prueba <see cref="Can_Update_Machine(string, string, string, string, string)"/>.</returns>
        public static IEnumerable<object[]> GetUpdateMachineData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach (var param in source.Element("MachinesTest").Element("Update").Elements())
            {
                yield return new object[]
                {
                    param.Attribute("pos").Value,
                    param.Attribute("name").Value,
                    param.Attribute("code").Value,
                    param.Attribute("Description").Value,
                    param.Attribute("Type").Value
                };
            }
        }

        #endregion

        #region Delete
        /// <summary>
        /// Método de prueba para la eliminación de máquinas.
        /// </summary>
        /// <param name="pos">Posición de la máquina a eliminar.</param>
        [DataTestMethod]
        [DynamicData(nameof(GetDeleteMachineData), DynamicDataSourceType.Method)]
        public void Can_Delete_Machine(string pos)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            //Obtengo todas las máquinas y verifico que existan.
            var machines = _repository.GetAllMachines();
            Assert.IsNotNull(machines);
            Assert.AreNotEqual(machines.Count(), 0);

            //Busco la máquina que quiero eliminar y verifico que exista
            var readMachine = _repository.GetMachineById(machines.ElementAt(position).Id);
            Assert.IsNotNull(readMachine);

            //La elimino y guardo los cambios
            _repository.DeleteMachineById(readMachine.Id);

            _repository.PartialCommit();

            //Verifico que la máquina no exista
            readMachine = _repository.GetMachineById(readMachine.Id);
            Assert.IsNull(readMachine);

            _repository.CommitTransaction();
        }

        /// <summary>
        /// Obtiene la data para el método de prueba <see cref="Can_Delete_Machine(string)"./> 
        /// </summary>
        /// <returns>Data para el metodo <see cref="Can_Delete_Machine(string)"/>.</returns>
        public static IEnumerable<object[]> GetDeleteMachineData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach (var param in source.Element("MachinesTest").Element("Delete").Elements())
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
