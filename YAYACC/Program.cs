using System;

namespace YAYACC
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Console.ReadLine();
            string text = System.IO.File.ReadAllText(path);

            Parser prser = new Parser();
            prser.Parse(text);

            //Scanner scanner = new Scanner(regexp);
            //Token nextToken = new Token();
            //do
            //{
            //    nextToken = scanner.GetToken();
            //    Console.WriteLine("Token: {0}, Valor: {1}", nextToken.Tag, nextToken.Value);
            //} while (nextToken.Tag != TokenType.EOF);


            Console.WriteLine("EXITO");

            Console.ReadKey();
        }
    }
}
