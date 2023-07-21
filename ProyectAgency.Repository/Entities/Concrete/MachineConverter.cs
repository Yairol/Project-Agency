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
    /// Modela un convertidor de <see cref="Machine"/> a <see cref="XElement"/> y viceversa.
    /// </summary>
    public class MachineConverter : IXmlConverter<Machine>
    {
        #region Implemntación de IXmlConverter
        public Machine FromXml(XElement element)
        {
            if(element.Name != nameof(Machine))
                throw new ArgumentException("sladkjflsadjflsajflsadfj");
            var attributes = element.Attributes();
            Machine machine = new Machine(attributes.Single(o => o.Name == nameof(Machine.Name)).Value, attributes.Single(o => o.Name == nameof(Machine.Code)).Value);

            machine.Id = int.Parse(attributes.Single(o => o.Name == nameof(Machine.Id)).Value);
            machine.ProjectId = int.Parse(attributes.Single(o => o.Name == nameof(Machine.ProjectId)).Value);
            //Verifico que el elemento tenga descripción
            if(!String.IsNullOrEmpty(attributes.Single(o => o.Name == nameof(Machine.Description)).Value))
                machine.Description = attributes.Single(o => o.Name == nameof(Machine.Description)).Value;
            machine.Type = (TypeMachine)Enum.Parse(typeof(TypeMachine), attributes.Single(o => o.Name == nameof(Machine.Type)).Value);
            
            return machine;
        }

        public XElement ToXml(Machine entity)
        {
            XElement element = new XElement(nameof(Machine));
            element.SetAttributeValue(nameof(Machine.Id), entity.Id);
            element.SetAttributeValue(nameof(Machine.Name), entity.Name);
            element.SetAttributeValue(nameof(Machine.Code), entity.Code);
            element.SetAttributeValue(nameof(Machine.ProjectId), entity.ProjectId);
            //Verifico si el elemento tiene descripción.
            if (entity.Description != null)
                element.SetAttributeValue(nameof(Machine.Description), entity.Description);
            else
                element.SetAttributeValue(nameof(Machine.Description), "");

            element.SetAttributeValue(nameof(Machine.Type), entity.Type.ToString());

            return element;
        }
        #endregion
    }
}
