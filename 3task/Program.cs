using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace _3task
{
    class Program
    {
        static string userMove = "";
        static int userMoveNum = 0;
        static string computerMove = "";
        static byte[] getKey()
        {
            byte[] secretKey = new byte[16];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(secretKey);
            }
            return secretKey;
        }

        static byte[] getHash(byte[] secretKey , string move)
        {
            using (var hmac = new HMACSHA256(secretKey))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(move));
            }
        }

        static string getComputerMove(string[] args)
        {
            return args[RandomNumberGenerator.GetInt32(args.Length - 1)];
        }

        static bool checkArguments(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Not enough parametrs");
                return false;
            }
            else 
            {
                if (args.Length % 2 == 0)
                {
                    Console.WriteLine("Number of parametrs can't be even");
                    return false;
                }
            }
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = 0; j < args.Length; j++)
                {
                    if (args[i] == args[j] && j != i)
                    {
                        Console.WriteLine("Parametrs can't be equal.Please, be sure all the move strings are unique");
                        return false;
                    }
                }
            }
            return true;
        }
        static void printMenu(string[] args)
        {
            Console.WriteLine("Available moves:");
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine("{0} - {1}", i , args[i]);
            }
            Console.WriteLine("Please, enter your move number: ");
        }

        static List<string> getLosersList(int index, int move , string[] args)
        {
            List<string> losers = new List<string>();
            if(index - (move/2) >= 0)
            {
                for(int i = index - move/2; i < index; i++)
                {
                    losers.Add(args[i]);
                }
            }
            else
            {
                for (int i = 0; i < index; i++)
                {
                    losers.Add(args[i]);
                }
                for (int j = move - 1; j > index + (move/2); j--)
                {
                    losers.Add(args[j]);
                }
            }
            return losers;
        }

        static List<string> getWinnersList(string[] args , int move , int index)
        {
            
            List<string> winners = new List<string>();
            if (index + (move / 2) <= move-1)
            {
                for (int i = index + move/2; i > index; i--)
                {
                    winners.Add(args[i]);
                }
            }
            else
            {
                for (int i = move-1; i > index; i--)
                {
                    winners.Add(args[i]);
                }
                for (int j = 0; j < index - (move / 2); j++)
                {
                    winners.Add(args[j]);
                }
            }
            return winners;
        }
        static void Main(string[] args)
        {
            if (!checkArguments(args))
                return;
            byte[] key = getKey();
            computerMove = getComputerMove(args);
            byte[] hash = getHash(key, computerMove);
            Console.WriteLine("hash:\r\n{0}", BitConverter.ToString(hash).Replace("-", ""));
            while (true)
            {
                printMenu(args);
                userMove = Console.ReadLine();
                try
                {
                    userMoveNum = Int32.Parse(userMove);
                    userMove = args[userMoveNum];
                    break;
                }
                catch(Exception)
                {
                    Console.WriteLine("No such move");
                }
            }
            Console.WriteLine("Your move:\r\n{0}", userMove);
            Console.WriteLine("Computer move:\r\n{0}", computerMove);
            var losers = getLosersList(userMoveNum, args.Length, args);
            var winners = getWinnersList(args , args.Length, userMoveNum);
            if (losers.Exists(e=>e.Equals(computerMove)))
            {
                Console.WriteLine("Allright, okay, you WIN");
            }
            else
            {
                if(winners.Exists(e => e.Equals(computerMove)))
                {
                    Console.WriteLine("You lose");
                }
                else
                {
                    Console.WriteLine("It's a draw");
                }
            }
            Console.WriteLine("key:\r\n{0}", BitConverter.ToString(key).Replace("-", string.Empty));
        }
    }
}
