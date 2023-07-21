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
    /// Clase sobre la que se realizarán las pruebas unitarias a la  implementación de <see cref="IProjectRepository"/>
    /// </summary>
    [TestClass]
    public class ProjectTest
    {
        /// <summary>
        /// Repositorio sobre el que se realizarán las pruebas.
        /// </summary>
        ProjectAgencyRepository _repository;

        /// <summary>
        /// Crea una instancia del tipo <see cref="ProjectTest"/>
        /// </summary>
        public ProjectTest()
        {
            _repository = new ProjectAgencyRepository("UniTestsDB.xml");
        }

        #region Create
        /// <summary>
        /// Metodo de prueba para la creación de Proyectos
        /// </summary>
        /// <param name="name">Nombre del proyecto</param>
        /// <param name="date">Fecha de inicio del proyecto</param>
        [DataTestMethod]
        [DynamicData(nameof(GetCreateProjectData), DynamicDataSourceType.Method)]
        public void Can_Create_Project(string name, DateTime date)
        {
            _repository.BeginTransaction();

            //Creamos el proyecto
            var project = _repository.CreateProject(name, date);

            //Verificamos que se a creado correctamente
            Assert.IsNotNull(project);
            Assert.AreEqual(name, project.Name);
            
            //Verificamos que el proyecto se ha introducido correctamente
            var readProyect = _repository.GetProjectById(project.Id);
            Assert.IsNotNull(readProyect);

            _repository.CommitTransaction();
        }

        /// <summary>
        /// Obtiene los datos para las prueba <see cref="Can_Create_Project"/>
        /// </summary>
        /// <returns>Data para las pruebas de <see cref="Can_Create_Project"/></returns>
        public static IEnumerable<object[]> GetCreateProjectData()
        {
            //Cargamos los datos de pruebas
            var sourcePath = @"D:\Estudios\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            //Enviamos los atributos name y FristDate para la creacion del proyecto
            foreach (var param in source.Element("ProjectsTest").Element("Create").Elements())
            {
                //Divido la cadena de caracteres para obtener los digitos 
                string[] number = param.Attribute("DateTime").Value.Split('/');
                //Creo un objeto de tipo DateTime para que sea mas facil analizar la fecha 
                DateTime date = new(int.Parse(number[2]), int.Parse(number[1]), int.Parse(number[0]));

                yield return new object[]
                {
                    param.Attribute("name").Value,
                    date
                };
            }
        }
        #endregion

        #region Get
        /// <summary>
        /// Metodo de prueba para la obtención por Id de Proyectos
        /// </summary>
        /// <param name="pos">Posición del Proyecto</param>
        [DataTestMethod]
        [DynamicData(nameof(GetGetProjectByIdData), DynamicDataSourceType.Method)]
        public void Can_Get_Project(string pos)
        {
            int posistion = int.Parse(pos);
            _repository.BeginTransaction();

            //Obtenemos todos los proyectos que hay en el contendor
            var projects = _repository.GetAllProjects();

            //Verifico que estén que esten cargados
            Assert.IsNotNull(projects);
            Assert.AreNotEqual(projects.Count(), 0);

            //Mediante el Id del proyecto lo busco en el contendor
            var readedProject = _repository.GetProjectById(projects.ElementAt(posistion).Id);

            //Verifico que exista proyecto
            Assert.IsNotNull(readedProject);

            _repository.CommitTransaction();
        }
        /// <summary>
        /// Obtiene los datos para las prueba <see cref="Can_Get_Project"/>
        /// </summary>
        /// <returns>Data para las pruebas de <see cref="Can_Get_Project"/></returns>
        public static IEnumerable<object[]> GetGetProjectByIdData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

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
        /// Metodo de prueba para la actualización de Proyectos en la Base de Datos
        /// </summary>
        /// <param name="pos">Posición del proyecto</param>
        /// <param name="name">Nombre del proyecto</param>
        /// <param name="description">Descripción del proyecto</param>
        /// <param name="lastDate">Fecha final del proyecto</param>
        /// <param name="projectYear">Proyecto en el año</param>
        [DataTestMethod]
        [DynamicData(nameof(GetUpdateProjectData), DynamicDataSourceType.Method)]
        public void Can_Update_Project(string pos, string name, string description, string lastDate,string projectYear)
        {
            _repository.BeginTransaction();

            var proyects = _repository.GetAllProjects();
            //Obtengo el proyecto que voy a modificar
            var readedProject = _repository.GetProjectById(proyects.ElementAt(int.Parse(pos)).Id);
            //Verifico que el proyecto exista
            Assert.IsNotNull(readedProject);

            //Verifco que los datos que quieran ser actualizados y los que existan los añado al elemnto leído
            if (!string.IsNullOrEmpty(name))
                readedProject.Name = name;
            if(!string.IsNullOrEmpty(description))
                readedProject.Description = description;
            if (!string.IsNullOrEmpty(lastDate))
            {
                string[] cadenas = lastDate.Split('/');
                DateTime date = new DateTime(int.Parse(cadenas[2]), int.Parse(cadenas[1]), int.Parse(cadenas[0]));
                readedProject.LastDate = date;
            }
            if (!string.IsNullOrEmpty(projectYear))
                readedProject.ProjectYear = int.Parse(projectYear);

            //Mando a actulizar el proyecto
            _repository.UpdateProyect(readedProject);

            //Guardo los cambios realizados
            _repository.PartialCommit();

            //Obtengo el proyecto recientemente actualizado
            readedProject = _repository.GetProjectById(readedProject.Id);

            //Verifico que el proyecto esté en la base de datos
            Assert.IsNotNull(readedProject);

            //Compara los elemntos que quería acualizar con los que están en la base de datos ya actualizados
            if (!string.IsNullOrEmpty(name))
                Assert.AreEqual(name, readedProject.Name);
            if (!string.IsNullOrEmpty(description))
                Assert.AreEqual(description, readedProject.Description);
            if (!string.IsNullOrEmpty(lastDate))
            {
                string[] cadenas = lastDate.Split('/');
                DateTime date = new DateTime(int.Parse(cadenas[2]), int.Parse(cadenas[1]), int.Parse(cadenas[0]));
                Assert.AreEqual(date,readedProject.LastDate );
            }
            if (!string.IsNullOrEmpty(projectYear))
                Assert.AreEqual(projectYear, readedProject.ProjectYear.ToString());

            _repository.CommitTransaction();
        }
        /// <summary>
        /// Obtiene los datos para las prueba <see cref="Can_Update_Project"/>
        /// </summary>
        /// <returns>Data para las pruebas de <see cref="Can_Get_Project"/></returns>
        public static IEnumerable<object[]> GetUpdateProjectData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);
            
            foreach(var param in source.Element("ProjectsTest").Element("Update").Elements())
            {

                yield return new object[]
                {
                    param.Attribute("pos").Value,
                    param.Attribute("name").Value,
                    param.Attribute("Description").Value,
                    param.Attribute("LastDate").Value,
                    param.Attribute("ProjectYear").Value
                };
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Método de prueba para la eliminación de proyectos
        /// </summary>
        /// <param name="pos">Posición del proyecto en la base de pruebas</param>
        [DataTestMethod]
        [DynamicData(nameof(GetDeleteProjectData), DynamicDataSourceType.Method)]
        public void Can_Delete_Project(string pos)
        {
            int position = int.Parse(pos);
            _repository.BeginTransaction();

            //Obtengo los proyectos en forma de lista y compruebo que existan.
            var projects = _repository.GetAllProjects().ToList();
            Assert.IsNotNull(projects);
            Assert.AreNotEqual(projects.Count(), 0);

            //Obtengo el proyecto que voy a eliminar.
            var readedProject = _repository.GetProjectById(projects[position].Id);
            Assert.IsNotNull(readedProject);

            //Elimino el proyecto y guardo los cambios
            _repository.DeleteProyectById(readedProject.Id);
            _repository.PartialCommit();

            //Busco el proyecto en la base de datos y compruebo que no exista.
            readedProject = _repository.GetProjectById(projects[position].Id);
            Assert.IsNull(readedProject);

            _repository.CommitTransaction();
        }
        /// <summary>
        /// Obtiene los datos para las prueba <see cref="Can_Delete_Project"/>
        /// </summary>
        /// <returns>Data para las pruebas de <see cref="Can_Delete_Project"/></returns>
        public static IEnumerable<object[]> GetDeleteProjectData()
        {
            var sourcePath = @"D:\Detección de Fallos y Parámetros\ProjectAgency 1.1\ProjectAgency\ProyectAgency.Test\Data\TestsSource.xml";
            var source = XElement.Load(sourcePath);

            foreach(var param in source.Element("ProjectsTest").Element("Delete").Elements())
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
