using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Registrator
{
    class Program
    {
        static void Main(string[] args)
        {
            File.WriteAllText("Registrator-redme!.txt", "Используйте ключ /yes для запуска регистрации сервера.", Encoding.Unicode);
            if (args.GetLength(0) < 1) return;
            if (args[0] != "/yes") return;

            VitFTP vitFTP = new VitFTP();
            Console.Write("Удаление стандартной конфигурации ");
            Console.WriteLine(vitFTP.DeleteConfig());
            Console.WriteLine("Создаем новый фвйл конфигураций.");
            vitFTP.CreateNewConfig();
            //Thread.Sleep(2000);
            //Console.WriteLine("Создаем профили дисков.");
            //vitFTP.CrateProfilesAllDisks();
            Console.WriteLine("Создаем профиль админа.");
            vitFTP.CreateProfileAdmin();
            Console.WriteLine("Создаем системный профиль.");
            vitFTP.CreateProfileSystem();
            //Thread.Sleep(1000);
            Console.WriteLine("Обновляем конфигурацию сервера.");
            vitFTP.Reloade();
            //Console.WriteLine("Нажмите любую клавишу для продолжения...!!!!");
            //Console.ReadKey(true);
        }
    }
}
