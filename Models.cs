using System;

namespace ConsoleApp
{
    public class Wallet
    {
        public string WalletId { get; set; }
        public Owner Owner { get; set; }
        public Balance Balance { get; set; }
        
    }

    public class Owner
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
    }

    public class Balance
    {
        public string Currency { get; set; }
        public decimal Available { get; set; }
        public decimal Blocked { get; set; }
    }
}