using Domain.Entities.Concrete;
using ProjectAgency.Repository.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectAgency.Repository.Entities.Concrete
{
    /// <summary>
    /// Modela un conversor de <see cref="Project"/> a <see cref="XElement"/> y viceversa.
    /// </summary>
    public class ProjectConverter : IXmlConverter<Project>
    {
        #region Implementación de IXmlConverter
        public Project FromXml(XElement element)
        {           
            if (element.Name != nameof(Project))
                throw new ArgumentException("The supplied entity is not a Project´s XElement.");
            var attributes = element.Attributes();
            
            string[] cadenas = attributes.Single(o => o.Name == nameof(Project.FristDate)).Value.Split('/');

            Project proyect = new Project(attributes.Single(o => o.Name == nameof(Project.Name)).Value,
                new DateTime(int.Parse(cadenas[2]), int.Parse(cadenas[1]), int.Parse(cadenas[0])));

            proyect.Id = int.Parse(attributes.Single(o => o.Name == nameof(Project.Id)).Value);
            proyect.ProjectYear = int.Parse(attributes.Single(o => o.Name == nameof(Project.ProjectYear)).Value);
            //Verifco que el elemento tenga descripción.
            if (!String.IsNullOrEmpty(attributes.Single(o => o.Name == nameof(Project.Description)).Value))
                proyect.Description = attributes.Single(o => o.Name == nameof(Project.Description)).Value;
            //Verifico que el proyecto esté terminado.        
            if(!String.IsNullOrEmpty(attributes.Single(o => o.Name == nameof(Project.LastDate)).Value))
            {
                string[] cadenas2 = attributes.Single(o => o.Name == nameof(proyect.LastDate)).Value.Split("/");
                proyect.LastDate = new DateTime(int.Parse(cadenas2[2]), int.Parse(cadenas2[1]), int.Parse(cadenas2[0]));
            }

            return proyect;                    
        }

        public XElement ToXml(Project entity)
        {
            XElement element = new XElement(nameof(Project));

            element.SetAttributeValue(nameof(Project.Id), entity.Id);
            element.SetAttributeValue(nameof(Project.Name), entity.Name);
            element.SetAttributeValue(nameof(Project.FristDate), entity.FristDate.ToString("d"));
            element.SetAttributeValue(nameof(Project.ProjectYear), entity.ProjectYear);
            //Verifico que el elemento tenga descripción.
            if (entity.Description != null)
                element.SetAttributeValue(nameof(Project.Description), entity.Description);
            else
                element.SetAttributeValue(nameof(Project.Description), "");
            //Verifico que el proyecto esté terminado.
            if (entity.LastDate != null)
                element.SetAttributeValue(nameof(Project.LastDate), entity.LastDate.Value.ToString("d"));
            else
                element.SetAttributeValue(nameof(Project.LastDate), "");

            return element;
        }
        #endregion
    }
}
