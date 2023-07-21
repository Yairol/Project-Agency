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
    /// Modela un conversor de <see cref="Variable"/> a <see cref="XElement"/> y viceversa
    /// </summary>
    public class VariableConverter : IXmlConverter<Variable>
    {
        #region Implementación de IXmlConverter
        public Variable FromXml(XElement element)
        {
            if (element.Name != nameof(Variable))
                throw new ArgumentException("The supplied entity is not a Variable's XElement.");
            var attributes = element.Attributes();
            Variable variable = new Variable(attributes.Single(o => o.Name == nameof(Variable.Name)).Value,
                                attributes.Single(o => o.Name == nameof(Variable.Code)).Value);

            variable.Id = int.Parse(attributes.Single(o => o.Name == nameof(Variable.Id)).Value);
            variable.ActuatorId = int.Parse(attributes.Single(o => o.Name == nameof(Variable.ActuatorId)).Value);
            variable.SensorId = int.Parse(attributes.Single(o => o.Name == nameof(Variable.SensorId)).Value);
            //Verifico si el elemento tiene descripción.
            if(!String.IsNullOrEmpty(attributes.Single(o => o.Name == nameof(Variable.Description)).Value))
                variable.Description = attributes.Single(o => o.Name == nameof(Variable.Description)).Value;
            return variable;
            
        }

        public XElement ToXml(Variable entity)
        {
            XElement element = new XElement(nameof(Variable));
            element.SetAttributeValue(nameof(Variable.Id), entity.Id);
            element.SetAttributeValue(nameof(Variable.Name), entity.Name);
            element.SetAttributeValue(nameof(Variable.Code), entity.Code);
            element.SetAttributeValue(nameof(Variable.SensorId), entity.SensorId);
            element.SetAttributeValue(nameof(Variable.ActuatorId),entity.ActuatorId);
            //Verifico si el elemento tiene descripción.
            if (entity.Description != null)
                element.SetAttributeValue(nameof(Variable.Description), entity.Description);            
            else
                element.SetAttributeValue(nameof(Variable.Description), "");

            return element;
        }
        #endregion
    }
}
