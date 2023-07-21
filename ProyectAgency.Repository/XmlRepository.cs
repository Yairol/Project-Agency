using ProjectAgency.Repository.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectAgency.Repository
{
    /// <summary>
    /// Modela un repositorio de un archivo XML.
    /// </summary>
    public abstract class XmlRepository : IRepository
    {
        #region Fields
        /// <summary>
        /// Documento xml base de datos.
        /// </summary>
        protected XElement _document;
        /// <summary>
        /// Ruta del fichero a manejar
        /// </summary>
        protected string _filePath;
        #endregion

        #region Constructors
        /// <summary>
        /// Crea una instancia del tipo <see cref="XmlRepository"/>.
        /// </summary>
        /// <param name="filePath">Ruta del fichero a manejar.</param>
        public XmlRepository(string filePath, bool createIfNotExist = true)
        {
            if (!File.Exists(filePath))
                if (createIfNotExist)
                {  // Decide si crear el archivo o lanzar una excepcion.
                    File.Create(filePath).Close();
                    //Generando nodo raíz.
                    XElement content = new XElement("Content", "");
                    content.Save(filePath);
                }
                else
                    throw new FileNotFoundException("File: " + filePath + " not found.");

            _filePath = filePath;
            IsInTransaction = false;
        }
        #endregion

        #region IRepository Implementation

        public bool IsInTransaction { get; private set; }

        public void BeginTransaction()
        {
            if (!IsInTransaction)
                _document = XElement.Load(_filePath);
            IsInTransaction = true;
        }

        public void CommitTransaction()
        {
            if (IsInTransaction)
                _document.Save(_filePath);
            IsInTransaction = false;
        }

        public void Dispose()
        {
            //En este caso no es necesario liberar ningun recurso.
        }

        public void PartialCommit()
        {
            if (IsInTransaction)
                _document.Save(_filePath);
        }

        public void RollbackTransaction()
        {
            IsInTransaction = false;
        }

        #endregion
    }
}
