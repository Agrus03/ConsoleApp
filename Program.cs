using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            string filePath = "Transaction1.json"; 

            try
            {
                Wallet myWallet = CustomJsonParser.Deserialize<Wallet>(filePath);
                
                Console.WriteLine("Парсинг успішний! Ось розпаковані дані:\n");
                
                Console.WriteLine($"ID Гаманця: {myWallet.WalletId}");
                
                if (myWallet.Balance != null)
                {
                    Console.WriteLine($"Валюта: {myWallet.Balance.Currency}");
                    Console.WriteLine($"Доступний баланс: {myWallet.Balance.Available}");
                    Console.WriteLine($"Заблокований баланс: {myWallet.Balance.Blocked}");
                }
                
                if (myWallet.Transactions != null)
                {
                    Console.WriteLine($"\nКількість транзакцій: {myWallet.Transactions.Count}");
                    foreach (var tx in myWallet.Transactions)
                    {
                        Console.WriteLine($"Транзакція {tx.TransactionId}: {tx.Amount} {tx.Currency} (Статус: {tx.Status})");
                        
                        if (tx.Metadata != null)
                        {
                            Console.WriteLine($"Джерело: {tx.Metadata.Source}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nВиникла критична помилка:");
                Console.WriteLine($"ЩО СТАЛОСЯ: {ex.Message}");
                Console.WriteLine($"ДЕ СТАЛОСЯ (Слід):\n{ex.StackTrace}");
            }

            Console.WriteLine("\nНатисни Enter, щоб вийти...");
            Console.ReadLine();
        }
    }
}