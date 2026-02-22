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
}