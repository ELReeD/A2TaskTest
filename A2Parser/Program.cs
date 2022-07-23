using A2Parser.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace A2Parser
{
    class Program
    {

        static void Main(string[] args)
        {
            //количество данных с одного запроса
            int SIZE = 5000;

            //Начало работы с Бд , создание бд,таблиц,процедур
            DatabaseServices databaseServices = new DatabaseServices();

            if (databaseServices.CreateDataBase()
                && databaseServices.CreateTable()
                && databaseServices.CreateProcedure())
            {
                while (true)
                {
                    ProcessData(SIZE, databaseServices);
                    //После окончания обработки данных на 10 минут ставлю перерыв
                    Thread.Sleep(600000);
                }

            }
        }

        public static void ProcessData(int SIZE, DatabaseServices databaseServices)
        {

            Console.WriteLine($"Началась  обработка , Время : {DateTime.Now}");
            ScrapingService scrapingService = new ScrapingService();

            //Тут беру количество записей 
            var res = scrapingService.GetTotal();

            //Вычесляю количество страниц , округляю в большую сторону 
            var pageCount = Math.Ceiling((double)res / SIZE);
            Console.WriteLine($"Количество Запивсей :{res}");
            Console.WriteLine($"Количество Страниц : {pageCount} ");

            int currentPage = 0;

            while (pageCount >= currentPage)
            {
                //Тут берётся основная информация
                var data = scrapingService.GetData(SIZE, currentPage);
                Console.WriteLine($"Текующая Страница : {currentPage}");
                Console.WriteLine("Количество данных  : " +data.Data.SearchReportWoodDeal.Content.Count());
                currentPage++;


                databaseServices.AddData(data.Data.SearchReportWoodDeal.Content);
            }

            Console.WriteLine($"Закончилась обработка , Время :{DateTime.Now}");
        }
    }
}