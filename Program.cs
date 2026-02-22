using System;
using System.IO;
namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            string filePath = "TransactionId.txt";
            string[] lines = File.ReadAllLines(filePath);
            Wallet wallet = new Wallet();
            wallet.Balance = new Balance();
            string currentContext = "root";
            Transaction currentTransaction = null;
        }
    }
}