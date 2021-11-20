using System;
using System.IO;

namespace YAYACC
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             Iván Alexander Canel García - 1301019
             Erick Estuardo Sabán Avila - 1195619
             Juan Sanchez - 1023819
             */
            string nombres = "Iván Alexander Canel García - 1301019 \nErick Estuardo Sabán Avila -1195619 \nJuan Sanchez - 1023819 \n";
            Console.WriteLine(nombres);
            Console.WriteLine("Enter file path");
            string path = Console.ReadLine();
            string extension = Path.GetExtension(path);
            if (extension == ".y")
            {
                string text = File.ReadAllText(path);
                Parser prser = new Parser();

                Grammar newGram = prser.Parse(text);
                Validator vldtr = new Validator(newGram);
                int i = 0;
                foreach (var item in vldtr.validate())
                {
                    i++;
                    Console.WriteLine(item);
                }
                if (i == 0)
                {
                    Console.WriteLine("No errors found");
                }
                Console.WriteLine("-------------------");
                Console.WriteLine(newGram.ToString());
                newGram.GenerateFirsts();
                newGram.GenerateCLR();
                Console.WriteLine("fin");
            }
            else
            {
                Console.WriteLine("Incorrect file extension, " + extension +" is not a valid extension");
            }
            //C:\Users\Usuario\Downloads\prueba.y
            Console.ReadKey();
        }
    }
}
