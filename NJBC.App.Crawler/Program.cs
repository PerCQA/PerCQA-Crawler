﻿using System;
using NJBC.Models.Crawler;
using NJBC.Services.Crawler;

namespace NJBC.App.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            CrawlerHelper crawler = new CrawlerHelper();
            string message = "Method List: \n";
            message += "1_ Get Messages From Ninisite\n";
            message += "2_ Convert Json to CSV\n";
            message += "Enter Method Number:";
            Console.Write(message);
            int method = int.Parse(Console.ReadLine());
            switch (method)
            {
                case 1:
                    crawler.StartCrawling();  // متد اصلی
                    break;
                case 2:
                    crawler.convertToCSV();
                    break;
                default:
                    break;
            }
            Console.WriteLine("Hello World!");
        }
    }
}