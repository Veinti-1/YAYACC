using System;

namespace YAYACC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese ruta del archivo");
            string path = Console.ReadLine();
            string text = System.IO.File.ReadAllText(path);

            Parser prser = new Parser();
           
            Grammar newGram =  prser.Parse(text);
            Validator vldtr = new Validator(newGram);
            int i = 0;
            foreach (var item in vldtr.validate())
            {
                i++;
                Console.WriteLine(item);
            }
            if (i ==0)
            {
                Console.WriteLine("No errors found");
            }

            Console.ReadKey();
        }
    }
}
