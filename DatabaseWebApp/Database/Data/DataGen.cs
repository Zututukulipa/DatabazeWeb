using System;
using System.Collections.Generic;
using Database.Services;
using DatabaseAdapter.OracleLib.Models;

namespace Database.Data
{
    public class DataGen
    {

        public static List<GroupMessages> GetDefault(int amount)
        {
            return null;
        }

        public static Group GetDefaultGroup()
        {
            var rnd = new Random();
            var group = new Group();
            var id = rnd.Next(500);
            group.Name = $"UCB{id}";
            group.MaxCapacity = 20;
            group.ActualCapacity = group.MaxCapacity - rnd.Next(20);
            group.GroupId = id;
            group.TeacherId = rnd.Next(10);
            return group;
        }

        public static List<User> GetDefaultUser(int amount)
        {
        string[] maleNames = {"Jiří","0Adam0-=","Jan","Petr","Josef","Pavel","Martin","[];',./","Jaroslav","><","Tomáš","","Miroslav","Zdeněk","František","Václav","Michal","Milan","Karel","Jakub","Lukáš","David","Vladimír","\t\t\t\t\t\t\t\t\t\t\\t\t\t\t\t\t\t\t\t\t\n\n\n\n\n\n\neof","Ladislav" };
        string[] maleSurnames = {
            "Novák", "Svoboda","DROP TABLE FILES;", "Novotný", "Dvořák", "Černý", "Procházka","`~<>?>}:|{}", "Kučera","\r","\\", "Veselý", "Horák", "Němec",
            "Pokorný", "Marek", "Pospíšil", "Hájek", "Jelínek", "Král","echo error >> ./err.txt" ,"Růžička", "Beneš", "Fiala", "Sedláček"
        };
        var rnd = new Random();
        
            var ret = new List<User>();
            for (var i = 0; i < amount; i++)
            {
                var usr = new User(i,$"st{i}",maleNames[rnd.Next(20)],maleNames[rnd.Next(20)],maleSurnames[rnd.Next(20)],$"st{i}@upce.cz", "generic bio");
                for (var j = 0; j < 3; j++)
                {
                    usr.Groups.Add(GetDefaultGroup());
                }
                ret.Add(usr);
            }

            return ret;
        }

        public static List<PrivateMessages> GetDefaultMessages(int amount)
        {
            var ret = new List<PrivateMessages>();
            for (var i = 0; i < amount; i++)
            {
                ret.Add(new PrivateMessages(UserService.ActiveUser));
            }

            return ret;
        }
    }
}
