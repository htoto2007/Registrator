using System;
using System.Collections.Generic;
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
            VitFTP vitFTP = new VitFTP();
            Console.WriteLine("Создаем новый фвйл конфигураций...");
            vitFTP.CreateNewConfig();
            //Thread.Sleep(2000);
            Console.WriteLine("Создаем профили дисков...");
            vitFTP.CrateProfilesAllDisks();
            Console.WriteLine("Создаем профиль админа...");
            vitFTP.CreateProfileAdmin();
            //Thread.Sleep(1000);
            Console.WriteLine("Обновляем конфигурацию сервера...");
            vitFTP.Reloade();
            //Console.WriteLine("Нажмите любую клавишу для продолжения...!!!!");
            //Console.ReadKey(true);
        }
    }
}
