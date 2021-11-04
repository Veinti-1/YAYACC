using System;

namespace YAYACC
{
    class Program
    {
        static void Main(string[] args)
        {
            string regexp = Console.ReadLine();
            Parser prser = new Parser();
            Scanner scanner = new Scanner(regexp);
            //prser.Parse(regexp);
            Token nextToken = new Token();
            do
            {
                nextToken = scanner.GetToken();
                Console.WriteLine("Token: {0}, Valor: {1}", nextToken.Tag, nextToken.Value);
            } while (nextToken.Tag != TokenType.EOF);


            Console.ReadKey();
        }
    }
}
