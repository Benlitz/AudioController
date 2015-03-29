using System;
using System.Linq;
using AudioController.Core;

namespace AudioController.Cmd
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var devices = Controller.GetDevices().ToList();
            Console.WriteLine("Enter the corresponding number to change the default audio device:");
            for (int i = 0; i < devices.Count; ++i)
            {
                Console.WriteLine("{0}: {1}", i, devices[i].Name);
            }

            var answer = Console.ReadLine();
            int deviceId;
            if (int.TryParse(answer, out deviceId) && deviceId >= 0 && deviceId < devices.Count)
            {
                var result = Controller.SetAsDefault(devices[deviceId]);
                if (result)
                    Console.WriteLine("Default audio device successfully changed to {0}.", devices[deviceId]);
                else
                    Console.WriteLine("An error occurred while changing the default audio device.");
            }
            else
            {
                Console.WriteLine("Unrecognized number.");
            }
        }
    }
}
