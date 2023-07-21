using Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAgency.Repository.Entities.Abstract
{
    /// <summary>
    /// Modela las funcionalidades de un Repositorio de Proyectos.
    /// </summary>
    public interface IProjectRepository: IRepository
    {
        /// <summary>
        /// Crea un Proyecto.
        /// </summary>
        /// <param name="name">Nombre del Proyecto.</param>
        /// <param name="firstDate">Fecha inicial del Proyecto.</param>
        /// <returns>Instancia del tipo <see cref="Project"/> con información del Proyecto creado.</returns>
        Project CreateProject(string name, DateTime firstDate);
        /// <summary>
        /// Obtiene un Proyecto de la Base de Datos mediante su identificador.
        /// </summary>
        /// <param name="projectId">Identidicador del Proyecto</param>
        /// <returns>Instancia del tipo<see cref="Project"/>.
        /// Si no, devuelve <see langword="null"/>.</returns>
        Project? GetProjectById(int projectId);
        /// <summary>
        /// Obtiene todos los Proyectos de la Base de Datos.
        /// </summary>
        /// <returns>Lista de Proyectos.</returns>
        IEnumerable<Project> GetAllProjects();
        /// <summary>
        /// Actualiza la informacion de un Proyecto. 
        /// </summary>
        /// <param name="proyect">Instancia del Proyecto modificado</param>
        void UpdateProyect(Project proyect);
        /// <summary>
        /// Elimina un Proyecto mediante su identificador.
        /// </summary>
        /// <param name="proyectId">Identificador del Proyecto a Eliminar</param>
        void DeleteProyectById(int proyectId);
    }
}
