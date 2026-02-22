using System;

namespace ConsoleApp
{
    public class Wallet
    {
        public string? WalletId { get; set; }
        public Balance Balance { get; set; }
        
    }
    public class Balance
    {
        public string Currency { get; set; }
        public decimal Available { get; set; }
        public decimal Blocked { get; set; }
    }

    public class Transaction
    {
        public string TransactionId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string Timestamp { get; set; }
        public Metadata Metadata { get; set; } 
    }
    
    public class Metadata
    {
        public string Source { get; set; }
        public string Reference { get; set; }
    }
}