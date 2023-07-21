using Domain.Entities.Concrete;
using ProjectAgency.Repository.Entities.Abstract;
using ProjectAgency.Repository.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ProjectAgency.Repository
{
    /// <summary>
    /// Modela una Agencia de Proyectos industriales.
    /// </summary>
    public class ProjectAgencyRepository : XmlRepository, IProjectRepository, IMachineRepository, ISensorRepository, IActuatorRepository, IVariableRepository
    {
        #region Constructor
        /// <summary>
        /// Crea una instancia del tipo <see cref="ProjectAgencyRepository"/>.
        /// </summary>
        /// <param name="filePath">Ruta del archivo Xml.</param>
        /// <param name="createIfNotExist">Identifica si el archivo existe.</param>
        public ProjectAgencyRepository(string filePath, bool createIfNotExist = true) : base(filePath, createIfNotExist)
        {
            
            BeginTransaction();

            if (!_document.Elements().Any(o => o.Name == "ProjectAgency"))
            {
                XElement projectAgency = new XElement("ProjectAgency", new XAttribute("LastGeneratedId", -1));

                projectAgency.Add(new XElement("Projects"));
                projectAgency.Add(new XElement("Machines"));
                projectAgency.Add(new XElement("Sensors"));
                projectAgency.Add(new XElement("Actuators"));
                projectAgency.Add(new XElement("Variables"));

                _document.Add(projectAgency);
            }
            CommitTransaction();
        }
        #endregion

        #region Implementación de IVariableRepository
        public Variable CreateVariable(string Name, string Code, int IdSensor, int IdActuator)
        {
            int lastId = GetLastGeneratedId();

            //Creo la Variable
            Variable variable = new Variable(Name, Code);

            //Enlazo la Variable con su sensor y su actuador
            variable.SensorId = IdSensor;
            variable.ActuatorId = IdActuator;

            //Le asigno un Id a la variable
            variable.Id = ++lastId;

            XElement newVariableElement = new VariableConverter().ToXml(variable);

            XElement variablesNode = GetVariablesContainer();

            variablesNode.Add(newVariableElement);

            UpdateLastGeneratedId(lastId);

            return variable;
        }
        public Variable? GetVariableById(int VariableId)
        {
            XElement variablesNodes = GetVariablesContainer();

            XElement? variableElement = variablesNodes.Elements().SingleOrDefault(o => o.Attributes().Single(u => u.Name == "Id").Value == VariableId.ToString());

            if (variableElement == null)
                return null;
            return new VariableConverter().FromXml(variableElement);
        }
        public IEnumerable<Variable> GetAllVariables()
        {
            XElement variablesNodes = GetVariablesContainer();

            IEnumerable<XElement> variablesElements = variablesNodes.Elements();

            var converter = new VariableConverter();

            return variablesElements.Select(o => converter.FromXml(o));
        }
        public IEnumerable<Variable> GetAllVariablesBySensorId(int sensorId)
        {
            return GetAllVariables().Where(o => o.SensorId == sensorId);
        }

        public IEnumerable<Variable> GetAllVariablesByActuatorId(int actuatorId)
        {
            return GetAllVariables().Where(o => o.ActuatorId == actuatorId);
        }

        public void UpdateVariable(Variable variable)
        {
            XElement varibalesNodes = GetVariablesContainer();

            XElement variableElement = varibalesNodes.Elements().Single(o => o.Attributes().Single(u => u.Name == "Id").Value == variable.Id.ToString());

            variableElement.ReplaceWith(new VariableConverter().ToXml(variable));
        }
        public void DeleteVariableById(int variableId)
        {
            XElement variableNodes = GetVariablesContainer();

            variableNodes.Elements().Single(o => o.Attributes().Single(u => u.Name == "Id").Value == variableId.ToString()).Remove();
           
        }
        #endregion

        #region Implemntación de ISensorRepository
        public Sensor CreateSensor(string Name, string Code, int IdMachine)
        {
            int lastId = GetLastGeneratedId();

            Sensor sensor = new Sensor(Name, Code);

            sensor.MachineId = IdMachine;
            sensor.Id = ++lastId;

            XElement newSensorElement = new SensorConverter().ToXml(sensor);

            XElement sensorNodes = GetSensorsContainer();

            sensorNodes.Add(newSensorElement);

            UpdateLastGeneratedId(lastId);

            return sensor;
        }
        public Sensor? GetSensorById(int SensorId)
        {
            XElement sensorNodes = GetSensorsContainer();

            XElement? sensorElement = sensorNodes.Elements().SingleOrDefault(o => o.Attributes().Single(u => u.Name == "Id").Value == SensorId.ToString());

            if (sensorElement == null)
                return null;
            return new SensorConverter().FromXml(sensorElement);
        }
        public IEnumerable<Sensor> GetAllSensors()
        {
            XElement sensorNodes = GetSensorsContainer();

            IEnumerable<XElement> sensorElements = sensorNodes.Elements();

            var converter = new SensorConverter();

            return sensorElements.Select(o => converter.FromXml(o));
        }

        public IEnumerable<Sensor> GetAllSensorsByMachineId(int machineId)
        {
            return GetAllSensors().Where(o => o.MachineId == machineId);
        }
        public void UpdateSensor(Sensor sensor)
        {
            XElement sensorNodes = GetSensorsContainer();

            XElement sensorElement = sensorNodes.Elements().Single(o => o.Attributes().Single(u => u.Name == "Id").Value == sensor.Id.ToString());

            sensorElement.ReplaceWith(new SensorConverter().ToXml(sensor));

        }
        public void DeleteSensorById(int sensorId)
        {
            XElement sensorNodes = GetSensorsContainer();

            IEnumerable<Variable> sensorVariables = GetAllVariablesBySensorId(sensorId);

            if (sensorVariables.Count() != 0)
            {
                List<Variable> variablesToDelete = new List<Variable>();

                foreach (var variable in sensorVariables)
                {
                    variablesToDelete.Add(variable);
                }

                foreach (var variableToDelete in variablesToDelete)
                {
                    DeleteVariableById(variableToDelete.Id);
                }
            }

            sensorNodes.Elements().Single(o => o.Attributes().Single(u => u.Name == "Id").Value == sensorId.ToString()).Remove();

        }
        #endregion

        #region Implementación de IActuatorRepository
        public Actuator CreateActuator(string Name, string Code, int IdMachine)
        {
            int lastId = GetLastGeneratedId();

            Actuator actuator = new Actuator(Name, Code);
            actuator.MachineId = IdMachine;
            actuator.Id = ++lastId;

            XElement newActuatorElement = new ActuatorConverter().ToXml(actuator);

            XElement actuatorNodes = GetActuatorsContainer();

            actuatorNodes.Add(newActuatorElement);

            UpdateLastGeneratedId(lastId);

            return actuator;
        }
        public Actuator? GetActuatorById(int ActuatorId)
        {
            XElement actuatorNodes = GetActuatorsContainer();

            XElement? actuatorElement = actuatorNodes.Elements().SingleOrDefault(o => o.Attributes().Single(u => u.Name == "Id").Value == ActuatorId.ToString());

            if (actuatorElement == null)
                return null;
            return new ActuatorConverter().FromXml(actuatorElement);
        }
        public IEnumerable<Actuator> GetAllActuators()
        {
            XElement actuatorNodes = GetActuatorsContainer();

            IEnumerable<XElement> actuatorElements = actuatorNodes.Elements();

            var converter = new ActuatorConverter();

            return actuatorElements.Select(o => converter.FromXml(o));
        }
        public IEnumerable<Actuator> GetAllActuatorsByMachineId(int MachineId)
        {
            return GetAllActuators().Where(o => o.MachineId== MachineId);
        }
        public void UpdateActuator(Actuator actuator)
        {
            XElement actuatorNodes = GetActuatorsContainer();

            XElement actuatorElement = actuatorNodes.Elements().Single(o => o.Attributes().Single(u => u.Name == "Id").Value == actuator.Id.ToString());

            actuatorElement.ReplaceWith(new ActuatorConverter().ToXml(actuator));
        }
        public void DeleteActuatorById(int ActuatorId)
        {
            XElement actuatorNodes = GetActuatorsContainer();

            IEnumerable<Variable> actuatorVariables = GetAllVariablesByActuatorId(ActuatorId);

            if(actuatorVariables.Count() != 0)
            {
                List<Variable> variablesToDelete = new List<Variable>();

                foreach(var variables in actuatorVariables)
                {
                    variablesToDelete.Add(variables);
                }

                foreach(var variableToDelete in variablesToDelete)
                {
                    DeleteVariableById(variableToDelete.Id);
                }
            }

            actuatorNodes.Elements().Single(o => o.Attributes().Single(u => u.Name == "Id").Value == ActuatorId.ToString()).Remove();
        }
        #endregion

        #region Implementación de IMachineRepository
        public Machine CreateMachine(string Name, string Code, int IdProyect)
        {
            int lastId = GetLastGeneratedId();

            Machine machine = new Machine(Name, Code);
            machine.ProjectId = IdProyect;
            machine.Id = ++lastId;

            XElement newMachineElement = new MachineConverter().ToXml(machine);

            XElement machineNodes = GetMachinesContainer();

            machineNodes.Add(newMachineElement);

            UpdateLastGeneratedId(lastId);

            return machine;

        }
        public Machine? GetMachineById(int MachineId)
        {
            XElement machineNodes = GetMachinesContainer();

            XElement? machineElement = machineNodes.Elements().SingleOrDefault(o => o.Attributes().Single(u => u.Name == "Id").Value == MachineId.ToString());

            if (machineElement == null)
                return null;
            return new MachineConverter().FromXml(machineElement);
        }
        public IEnumerable<Machine> GetAllMachines()
        {
            XElement machineNodes = GetMachinesContainer();

            IEnumerable<XElement> machineElements = machineNodes.Elements();

            var converter = new MachineConverter();

            return machineElements.Select(o => converter.FromXml(o));
        }

        public IEnumerable<Machine> GetAllMachineByIdProject(int projectId)
        {
            return GetAllMachines().Where(o => o.ProjectId == projectId);
        }
        public void UpdateMachine(Machine machine)
        {
            XElement machineNodes = GetMachinesContainer();

            XElement machineElement = machineNodes.Elements().Single(o => o.Attributes().Single(u => u.Name == "Id").Value == machine.Id.ToString());

            machineElement.ReplaceWith(new MachineConverter().ToXml(machine));
        }
        public void DeleteMachineById(int machineId)
        {
            XElement machineNodes = GetMachinesContainer();
            
            IEnumerable<Sensor> sensors = GetAllSensorsByMachineId(machineId);
            IEnumerable<Actuator> actuators = GetAllActuatorsByMachineId(machineId);

            if (sensors.Count() != 0)
            {
                List<Sensor> sensorsToDelete = new List<Sensor>();

                foreach (var sensor in sensors)
                    sensorsToDelete.Add(sensor);

                foreach (var sensorToDelete in sensorsToDelete)
                    DeleteSensorById(sensorToDelete.Id);
            }
            if (actuators.Count() != 0)
            {
                List<Actuator> actuatorsToDelete = new List<Actuator>();

                foreach (var actuator in actuators)
                    actuatorsToDelete.Add(actuator);

                foreach (var actuatorToDelete in actuatorsToDelete)
                    DeleteActuatorById(actuatorToDelete.Id);
            }
            machineNodes.Elements().Single(o => o.Attributes().Single(u => u.Name == "Id").Value == machineId.ToString()).Remove();
        }
        #endregion

        #region Implementación de IProyectRepository
        public Project CreateProject(string Name, DateTime FristDate)
        {
            int lastId = GetLastGeneratedId();

            Project proyect = new Project(Name, FristDate);
            proyect.Id = ++lastId;

            XElement proyectNodes = GetProyectsContainer();
            XElement newProyectElement = new ProjectConverter().ToXml(proyect);

            proyectNodes.Add(newProyectElement);

            UpdateLastGeneratedId(lastId);

            return proyect;
        }
        public Project? GetProjectById(int ProyectId)
        {
            XElement proyectNodes = GetProyectsContainer();

            XElement? proyectElement = proyectNodes.Elements().SingleOrDefault(o => o.Attributes().Single(u => u.Name == "Id").Value == ProyectId.ToString());

            if(proyectElement == null) 
                return null;
            return new ProjectConverter().FromXml(proyectElement);

        }
        public IEnumerable<Project> GetAllProjects()
        {
            XElement proyectNodes = GetProyectsContainer();

            IEnumerable<XElement> proyectElements = proyectNodes.Elements();

            var converter = new ProjectConverter();

            return proyectElements.Select(o => converter.FromXml(o));
        }
        public void UpdateProyect(Project proyect)
        {
            XElement proyectNodes = GetProyectsContainer();

            XElement proyectElement = proyectNodes.Elements().Single(o => o.Attributes().Single(u => u.Name == "Id").Value == proyect.Id.ToString());

            proyectElement.ReplaceWith(new ProjectConverter().ToXml(proyect));
        }
        public void DeleteProyectById(int ProyectId)
        {
            XElement proyectNodes = GetProyectsContainer();

            IEnumerable<Machine> machines = GetAllMachineByIdProject(ProyectId);

            if (machines.Count() != 0)
            {
                List<Machine> machinesToDelete = new List<Machine>(); // Lista para almacenar las máquinas a eliminar

                // Recopilar las máquinas a eliminar en la lista
                foreach (var machine in machines)
                {
                    machinesToDelete.Add(machine);
                }

                // Eliminar las máquinas de la lista
                foreach (var machineToDelete in machinesToDelete)
                {
                    DeleteMachineById(machineToDelete.Id);
                }
            }
            proyectNodes.Elements().Single(o => o.Attributes().Single(u => u.Name == "Id").Value == ProyectId.ToString()).Remove();
        }
        #endregion

        #region Funciones de Ayuda
        /// <summary>
        /// Obtiene el ultimo identificador generado en la Base de Datos.
        /// </summary>
        /// <returns>Identificador del ultimo identificador generado</returns>
        private int GetLastGeneratedId()
        {
            if(!IsInTransaction)
                throw new InvalidOperationException("Cannot accesss Database without an open transaction.");
            return int.Parse(_document.Element("ProjectAgency").Attribute("LastGeneratedId").Value);
        }
        /// <summary>
        /// Actualiza el atributo del generador de identificadores.
        /// </summary>
        /// <param name="id">Valor a actualizar.</param>
        private void UpdateLastGeneratedId(int id)
        {
            if(!IsInTransaction)
                throw new InvalidOperationException("Cannot accesss Database without an open transaction.");
            _document.Elements().First(o => o.Name == "ProjectAgency").Attributes().First(o => o.Name == "LastGeneratedId").Value = id.ToString();
        }
        /// <summary>
        /// Obtiene el contenedor de Variables.
        /// </summary>
        /// <returns>Instancia del tipo <see cref="XElement"/> que contiene todas las sensorVariables.</returns>
        private XElement GetVariablesContainer()
        {
            if(!IsInTransaction)
                throw new InvalidOperationException("Cannot accesss Database without an open transaction.");
            return _document.Elements().First(o => o.Name == "ProjectAgency").Elements().First(o => o.Name == "Variables");
        }
        /// <summary>
        /// Obtiene el contenedor de Sensores.
        /// </summary>
        /// <returns>Instancia del tipo <see cref="XElement"/> que contiene todos los Sensores.</returns>
        private XElement GetSensorsContainer()
        {
            if (!IsInTransaction)
                throw new InvalidOperationException("Cannot accesss Database without an open transaction.");
            return _document.Elements().First(o => o.Name == "ProjectAgency").Elements().First(o => o.Name == "Sensors");
        }
        /// <summary>
        /// Obtiene el contenedor de Actuadores.
        /// </summary>
        /// <returns>Instancia del tipo <see cref="XElement"/> que contiene todos los Actuadores.</returns>
        private XElement GetActuatorsContainer()
        {
            if (!IsInTransaction)
                throw new InvalidOperationException("Cannot accesss Database without an open transaction.");
            return _document.Elements().First(o => o.Name == "ProjectAgency").Elements().First(o => o.Name == "Actuators");
        }
        /// <summary>
        /// Obtiene el contenedor de Máquinas.
        /// </summary>
        /// <returns>Instancia del tipo <see cref="XElement"/> que contiene todas las Máquinas.</returns>
        private XElement GetMachinesContainer()
        {
            if (!IsInTransaction)
                throw new InvalidOperationException("Cannot accesss Database without an open transaction.");
            return _document.Elements().First(o => o.Name == "ProjectAgency").Elements().First(u => u.Name == "Machines");
        }
        /// <summary>
        /// Obtiene el contendor de Proyectos
        /// </summary>
        /// <returns>Instancia del tipo <see cref="XElement"/> que contiene todas las Máquinas.</returns>
        private XElement GetProyectsContainer()
        {
            if (!IsInTransaction)
                throw new InvalidOperationException("Cannot accesss Database without an open transaction.");
            return _document.Elements().First(o => o.Name == "ProjectAgency").Elements().First(o => o.Name == "Projects");
        }
        #endregion
    }
}
