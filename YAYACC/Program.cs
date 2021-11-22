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
                    Console.WriteLine("-------------------");
                    Console.WriteLine(newGram.ToString());
                    newGram.GenerateLALR();
                    Console.WriteLine("-------------------");
                    Console.WriteLine("Enter input for the Grammar:");
                    bool continueInput = true;
                    do
                    {
                        string input = Console.ReadLine();
                        Console.WriteLine("Enter path for the output file:");
                        string pathOut = Console.ReadLine();
                        if (newGram.Parse(input, pathOut))
                        {
                            Console.WriteLine("Input is valid");
                        }
                        else
                        {
                            Console.WriteLine("Input is NOT valid");
                        }
                        Console.WriteLine("Enter new input for the Grammar? [Y/N]");
                        if (Console.ReadLine().ToLower() == "N")
                        {
                            continueInput = false;
                        }
                        Console.Clear();
                        Console.WriteLine("Enter input for the Grammar:");
                    } while (continueInput);
                }
            }
            else
            {
                Console.WriteLine("Incorrect file extension, " + extension +" is not a valid extension");
            }
            Console.ReadKey();
        }
    }
}
