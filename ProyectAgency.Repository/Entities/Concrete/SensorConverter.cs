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
    /// Modelo un conversor de <see cref="Sensor"/> a <see cref="XElement"/> y viceversa.
    /// </summary>
    public class SensorConverter : IXmlConverter<Sensor>
    {
        #region Implementación de IXmlConverter
        public Sensor FromXml(XElement element)
        {
            if (element.Name == nameof(Sensor.Name))
                throw new ArgumentException("The supplied entity is not a Sensor´s XElement.");
            var attributes = element.Attributes();

            Sensor sensor = new Sensor(attributes.Single(o => o.Name == nameof(Sensor.Name)).Value,
                              attributes.Single(o => o.Name == nameof(Sensor.Code)).Value);
            sensor.Id = int.Parse(attributes.Single(o => o.Name == nameof(Sensor.Id)).Value);
            sensor.MachineId = int.Parse(attributes.Single(o => o.Name == nameof(Sensor.MachineId)).Value);
            //Verifico si el elemento tiene descripción.
            if(!String.IsNullOrEmpty(attributes.Single(o => o.Name == nameof(Sensor.Description)).Value))
                sensor.Description = attributes.Single(o => o.Name == nameof(Sensor.Description)).Value;

            return sensor;
        }

        public XElement ToXml(Sensor entity)
        {
            XElement element = new XElement(nameof(Sensor));
            element.SetAttributeValue(nameof(Sensor.Id), entity.Id);
            element.SetAttributeValue(nameof(Sensor.Name), entity.Name);
            element.SetAttributeValue(nameof(Sensor.Code), entity.Code);
            element.SetAttributeValue(nameof(Sensor.MachineId), entity.MachineId);
            //Verifico si el elemento tiene descripción.
            if (entity.Description != null)
                element.SetAttributeValue(nameof(Sensor.Description), entity.Description);
            else
                element.SetAttributeValue(nameof(Sensor.Description), "");

            return element;
        }
        #endregion
    }
}
