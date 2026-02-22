using System;

namespace ConsoleApp1
{
    public class Wallet
    {
        public int userId { get; set; }
    }

    public class Owner
    {
        public bool isVerified { get; set; }
    }

    public class Balance
    {
        public double available { get; set; }
    }
}