using System;
using System.Collections.Generic;

namespace ConsoleApp9
{
    interface ICommand
    {
        void Execute();
        void Undo();
    }

    class Light
    {
        public void On() => Console.WriteLine("Свет включен");
        public void Off() => Console.WriteLine("Свет выключен");
    }

    class Thermostat
    {
        public int Temperature = 20;

        public void SetTemperature(int temp)
        {
            Temperature = temp;
            Console.WriteLine("Температура: " + temp);
        }
    }

    class LightOnCommand : ICommand
    {
        private Light light;

        public LightOnCommand(Light l)
        {
            light = l;
        }

        public void Execute()
        {
            light.On();
        }

        public void Undo()
        {
            light.Off();
        }
    }

    class SetTemperatureCommand : ICommand
    {
        private Thermostat thermostat;
        private int newTemp;
        private int oldTemp;

        public SetTemperatureCommand(Thermostat t, int temp)
        {
            thermostat = t;
            newTemp = temp;
        }

        public void Execute()
        {
            oldTemp = thermostat.Temperature;
            thermostat.SetTemperature(newTemp);
        }

        public void Undo()
        {
            thermostat.SetTemperature(oldTemp);
        }
    }

    class MacroCommand : ICommand
    {
        private List<ICommand> commands = new List<ICommand>();

        public void Add(ICommand cmd)
        {
            commands.Add(cmd);
        }

        public void Execute()
        {
            foreach (var cmd in commands)
                cmd.Execute();
        }

        public void Undo()
        {
            foreach (var cmd in commands)
                cmd.Undo();
        }
    }

    class RemoteControl
    {
        public ICommand Command;

        public void Press()
        {
            Command.Execute();
        }

        public void Undo()
        {
            Command.Undo();
        }
    }

    class Program
    {
        static void Main()
        {
            var light = new Light();
            var thermostat = new Thermostat();

            var lightCmd = new LightOnCommand(light);
            var tempCmd = new SetTemperatureCommand(thermostat, 18);

            var remote = new RemoteControl();

            remote.Command = lightCmd;
            remote.Press();

            remote.Command = tempCmd;
            remote.Press();

            Console.WriteLine("Отмена:");
            remote.Undo();

            remote.Command = lightCmd;
            remote.Undo();

            Console.WriteLine("\nУхожу из дома:");

            var macro = new MacroCommand();
            macro.Add(new LightOnCommand(light));
            macro.Add(new SetTemperatureCommand(thermostat, 16));

            macro.Execute();
        }
    }
}
