using System;
using System.IO;

namespace YAYACC
{
    class Program
    {
        static void Main(string[] args)
        {
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
            }
            else
            {
                Console.WriteLine("Incorrect file extension, " + extension +" is not a valid extension");
            }
            Console.ReadKey();
        }
    }
}
