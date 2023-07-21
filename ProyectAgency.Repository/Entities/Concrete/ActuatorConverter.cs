using Domain.Entities.Concrete;
using ProjectAgency.Repository.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjectAgency.Repository.Entities.Concrete
{
    /// <summary>
    /// Modela un convertidor de <see cref="XElement"/> a <see cref="Actuator"/> y viceversa
    /// </summary>
    public class ActuatorConverter : IXmlConverter<Actuator>
    {
        #region Implementación de IXmlConverter
        public Actuator FromXml(XElement element)
        {
            if (element.Name == nameof(Actuator.Name))
                throw new ArgumentException("The supplied entity is not a Actuator´s XElement.");
            var attributes = element.Attributes();
            Actuator actuator = new Actuator(attributes.Single(o => o.Name == nameof(Actuator.Name)).Value,
                                attributes.Single(o => o.Name == nameof(Actuator.Code)).Value);

            actuator.Id = int.Parse(attributes.Single(o => o.Name == nameof(Actuator.Id)).Value);
            actuator.MachineId = int.Parse(attributes.Single(o => o.Name == nameof(Actuator.MachineId)).Value);
            //Verifico si el elemnto tiene descripción o no.
            if(!String.IsNullOrEmpty(attributes.Single(o => o.Name == nameof(Actuator.Description)).Value))
                actuator.Description = attributes.Single(o => o.Name == nameof(Actuator.Description)).Value;

            return actuator;
        }

        public XElement ToXml(Actuator entity)
        {
            XElement element = new XElement(nameof(Actuator));
            element.SetAttributeValue(nameof(Actuator.Id), entity.Id);
            element.SetAttributeValue(nameof(Actuator.Name), entity.Name);
            element.SetAttributeValue(nameof(Actuator.Code), entity.Code);
            element.SetAttributeValue(nameof(Actuator.MachineId), entity.MachineId);
            //Verifico que el actuador tenga descripción.
            if(entity.Description != null)
                element.SetAttributeValue(nameof(Actuator.Description), entity.Description);
            else
                element.SetAttributeValue(nameof(Actuator.Description), "");

            return element;
        }
        #endregion
    }
}
