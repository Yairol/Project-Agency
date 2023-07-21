using Domain.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectAgency.Repository.Entities.Abstract
{
    /// <summary>
    /// Modela las funcionalidades de un elemento que funciona como conversor, hacia y desde XML, de un objeto de tipo <see cref="{T}"/>.
    /// </summary>
    /// <typeparam name="T">Tipo de dato al que se le aplica la conversión.</typeparam>
    public interface IXmlConverter<T> where T : Entity
    {
        /// <summary>
        /// Convierte una instancia de tipo <see cref="{T}"/> a un objeto de tipo <see cref="XElement"/>.
        /// </summary>
        /// <param name="elemet">Elemento a convertir.</param>
        /// <returns>Elemento convertido</returns>
        T FromXml(XElement elemet);
        /// <summary>
        /// Crea una instancia de tipo <see cref="XElement"/> a partir de la información contenida en el elemento <see cref="{T}"/>.
        /// </summary>
        /// <param name="entity">Elemento que contiene la información a convertir.</param>
        /// <returns>Elemento convertido a <see cref="XElement"/>.</returns>
        XElement ToXml(T entity);

    }
}
