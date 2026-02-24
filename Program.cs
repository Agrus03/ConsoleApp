using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            string filePath = "Transaction.json";
            string[] lines = File.ReadAllLines(filePath);
            Wallet wallet = new Wallet();
            wallet.Balance = new Balance();
            wallet.Transactions = new List<Transaction>();
            string currentContext = "root";
            Transaction currentTransaction = null;
            foreach (var line in lines)
            {
                string cleanLine = line.Trim();
                if (string.IsNullOrWhiteSpace(cleanLine))
                {
                    continue;
                }

                if (cleanLine.StartsWith("\"balance\":"))
                {
                    currentContext = "balance";
                    continue;
                }

                if (cleanLine.StartsWith("\"transactions\":"))
                {
                    currentContext = "transactions";
                    continue;
                }

                if (cleanLine.StartsWith("\"metadata\":"))
                {
                    currentContext = "metadata";
                    if (currentTransaction != null)
                    {
                        currentTransaction.Metadata = new Metadata();
                    }
                    continue;
                }

                if (cleanLine == "{")
                {
                    if (currentContext == "transactions")
                    {
                        currentTransaction = new Transaction();
                    }
                    else if (currentContext == "metadata")
                    {
                        currentTransaction.Metadata = new Metadata();
                    }
                    continue;
                }
                if (cleanLine == "}" || cleanLine == "},")
                {
                    if (currentContext == "metadata")
                    {
                        currentContext = "transactions";
                    }
                    else if (currentContext == "balance")
                    {
                        currentContext = "root";
                    }
                    else if (currentContext == "transactions")
                    {
                        if (currentTransaction != null)
                        {
                            wallet.Transactions.Add(currentTransaction);
                            currentTransaction = null;
                        }
                    }
                    continue;
                }

                if (cleanLine == "]")
                {
                    if (currentContext == "transactions")
                    {
                        currentContext = "root";
                    }
                }
                if (!cleanLine.Contains(":")) {continue;}
                string[] parts = cleanLine.Split(':', 2);
                string key = parts[0].Replace("\"", "").Trim();
                string value = parts[1].Replace("\"", "").Replace(",", "").Trim();
                if (currentContext == "root")
                {
                    if (key == "walletId")
                    {
                        wallet.WalletId = value;
                    }
                }
                else if (currentContext == "balance")
                {
                    if (key == "currency")
                    {
                        wallet.Balance.Currency = value;
                    }
                    else if (key == "available")
                    {
                        wallet.Balance.Available = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else if (key == "blocked")
                    {
                        wallet.Balance.Blocked = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                else if (currentContext == "transactions" && currentTransaction != null)
                {
                    if (key == "transactionId") currentTransaction.TransactionId = value;
                    else if (key == "type") currentTransaction.Type = value;
                    else if (key == "amount") currentTransaction.Amount = decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                    else if (key == "currency") currentTransaction.Currency = value;
                    else if (key == "status") currentTransaction.Status = value;
                    else if (key == "timestamp") currentTransaction.Timestamp = value;
                }
                else if (currentContext == "metadata" && currentTransaction != null &&
                         currentTransaction.Metadata != null)
                {
                    if (key == "source") currentTransaction.Metadata.Source = value;
                    else if (key == "reference") currentTransaction.Metadata.Reference = value;
                }
            }
            Console.WriteLine("================================");
            Console.WriteLine($"Wallet Transaction Count: {wallet.Transactions.Count}");
            if (wallet.Transactions.Count > 0)
            {
                Console.WriteLine($"First Transaction: {wallet.Transactions[0].Amount} {wallet.Transactions[0].Currency}");
                Console.WriteLine($"Source (Metadata): {wallet.Transactions[0].Metadata.Source}");
            }
        }
    }
}