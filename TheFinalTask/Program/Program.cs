﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Добро пожаловать в казино!");
        Console.Write("Введите ваше имя: ");
        string name = Console.ReadLine();

        var saveLoadService = new FileSystemSaveLoadService("Profiles");

        string savedProfile = saveLoadService.Load(name);
        PlayerProfile player;

        if (savedProfile != null)
        {
            Console.WriteLine($"Найден сохраненный профиль:\n{savedProfile}");
            string balanceInput = Console.ReadLine();

            if (string.IsNullOrEmpty(balanceInput))
            {
                var lines = savedProfile.Split('\n');
                int balance = int.Parse(lines[1].Split(':')[1].Trim());
                player = new PlayerProfile(name, balance);
            }
            else
            {
                player = new PlayerProfile(name, int.Parse(balanceInput));
            }
        }
        else
        {
            int initialBalance = 1000;
            player = new PlayerProfile(name, initialBalance);
        }

        var casino = new Casino(player, saveLoadService);
        casino.ShowMenu();
    }
}
