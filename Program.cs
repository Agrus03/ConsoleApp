using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            // ===================================
            // ЕТАП 1: Створюємо та читаєм файл
            // ===================================
            string filePath = "Transaction.json"; // Створюємо адресу
            if (!File.Exists(filePath)) // Перевірка чи взагалі даний файл існує
            {
                Console.WriteLine("File not found");
                return;
            }
            string[] lines = File.ReadAllLines(filePath); // Кладем у RAM витягнуті, порізані дані
            // ===================================
            // ЕТАП 2: Ініціалізація
            // ===================================
            // Створюємо головні обєкти, щоб уникнути NullReferenceException
            Wallet wallet = new Wallet();
            wallet.Balance = new Balance();
            wallet.Transactions = new List<Transaction>(); // Ініціалізуємо і Захизаємо від NullReferenceException
            // State Machine 
            string currentContext = "root"; // Корінь документа (початковий стан)
            Transaction currentTransaction = null; // Тільки створюємо
            // ===================================
            // ЕТАП 3: Парсинг 
            // ===================================
            foreach (var line in lines)
            {
                string cleanLine = line.Trim(); // Очищаємо від пустих місць
                if (string.IsNullOrWhiteSpace(cleanLine) || cleanLine == "[" || cleanLine == "]")
                {
                    continue; // Пропускаємо сміття та квадратні дужки масивів
                }
                // Читаємо "вивіску", Заходимо в кінату , Запамятовуємо 
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
                    // Якщо зараз є активна транзакція, створюємо для неї об'єкт метаданих
                    if (currentTransaction != null)
                    {
                        currentTransaction.Metadata = new Metadata();
                    }
                    continue;
                }
                // Життєвий цикл об'єктів: Створення
                if (cleanLine == "{")
                {
                    if (currentContext == "transactions")
                    {
                        currentTransaction = new Transaction(); // Читаємо нову транзакцію
                    }
                    else if (currentContext == "metadata")
                    {
                        currentTransaction.Metadata = new Metadata(); // Читаємо вкладені дані Транзакції
                    }
                    continue;
                }
                // Життєвий цикл: Збереження
                if (cleanLine == "}" || cleanLine == "},")
                {
                    if (currentContext == "metadata")
                    {
                        currentContext = "transactions"; // Вийшли з метаданих до транзакції
                    }
                    else if (currentContext == "balance")
                    {
                        currentContext = "root"; // Вийшли в корінь
                    }
                    else if (currentContext == "transactions")
                    {
                        // Транзакції закінчились, Додаєм, Ощищаємо
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
                // Якщо нема двокрапки, пропускаємо це не дані ключ значення
                if (!cleanLine.Contains(":")) {continue;}
                // Розрізаємо на 2 частини і чистимо від лапок та ком
                string[] parts = cleanLine.Split(':', 2);
                string key = parts[0].Replace("\"", "").Trim();
                string value = parts[1].Replace("\"", "").Replace(",", "").Trim();
                // ===================================
                // ЕТАП 4: Мапінг
                // ===================================
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
                    // Записуємо дані(string) у фінансове число(decimal) згідно міжнародних норм
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
                // Потрійний захист: ми в кімнаті метаданих + є транзакція + для неї вже створена "комірка" Metadata
                else if (currentContext == "metadata" && currentTransaction != null &&
                         currentTransaction.Metadata != null)
                {
                    if (key == "source") currentTransaction.Metadata.Source = value;
                    else if (key == "reference") currentTransaction.Metadata.Reference = value;
                }
            }
            // ==========================================
            // ЕТАП 4: ВИВІД РЕЗУЛЬТАТІВ (Перевірка)
            // ==========================================
            Console.WriteLine("=== Final Parcings Results ===");
            Console.WriteLine($"Wallet ID: '{wallet.WalletId}'");
            Console.WriteLine($"Ballance: {wallet.Balance.Available} {wallet.Balance.Currency}");
            Console.WriteLine($"Transactions Count: {wallet.Transactions.Count}");
            
            if (wallet.Transactions.Count > 0)
            {
                var firstTx = wallet.Transactions[0];
                Console.WriteLine($"[First Transactions Info]");
                Console.WriteLine($" - ID: {firstTx.TransactionId}");
                Console.WriteLine($" - Amount: {firstTx.Amount} {firstTx.Currency}");
                Console.WriteLine($" - Status: {firstTx.Status}");
                Console.WriteLine($" - Source (Metadata): {firstTx.Metadata.Source}");
            }
        }
    }
}