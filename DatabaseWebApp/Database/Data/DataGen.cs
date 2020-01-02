using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Pages;
using Database.Services;
using DatabaseAdapter.OracleLib.Models;

namespace Database.Data
{
    public class DataGen
    {

        public static List<WallPost> getDefault(int amount)
        {
            
            List<WallPost> ret = new List<WallPost>();
            for (int i = 0; i < amount; i++)
            {
                ret.Add(new WallPost(Databaze_API.Controllers.UserController.ActiveUser));
            }

            return ret;
        }

        public static Group getDefaultGroup()
        {
            Random rnd = new Random();
            Group group = new Group();
            int id = rnd.Next(500);
            group.Name = $"UCB{id}";
            group.MaxCapacity = 20;
            group.ActualCapacity = group.MaxCapacity - rnd.Next(20);
            group.GroupId = id;
            group.TeacherId = rnd.Next(10);
            return group;
        }

        public static List<User> getDefaultUser(int amount)
        {
        string[] _maleNames = {"Jiří","0Adam0-=","Jan","Petr","Josef","Pavel","Martin","[];',./","Jaroslav","><","Tomáš","","Miroslav","Zdeněk","František","Václav","Michal","Milan","Karel","Jakub","Lukáš","David","Vladimír","\t\t\t\t\t\t\t\t\t\t\\t\t\t\t\t\t\t\t\t\t\n\n\n\n\n\n\neof","Ladislav" };
        string[] _maleSurnames = {
            "Novák", "Svoboda","DROP TABLE FILES;", "Novotný", "Dvořák", "Černý", "Procházka","`~<>?>}:|{}", "Kučera","\r","\\", "Veselý", "Horák", "Němec",
            "Pokorný", "Marek", "Pospíšil", "Hájek", "Jelínek", "Král","echo error >> ./err.txt" ,"Růžička", "Beneš", "Fiala", "Sedláček"
        };
        Random rnd = new Random();
        
            List<User> ret = new List<User>();
            for (int i = 0; i < amount; i++)
            {
                var usr = new User(i,$"st{i}",_maleNames[rnd.Next(20)],_maleNames[rnd.Next(20)],_maleSurnames[rnd.Next(20)],$"st{i}@upce.cz", "generic bio");
                for (int j = 0; j < 3; j++)
                {
                    usr.Groups.Add(getDefaultGroup());
                }
                ret.Add(usr);
            }

            return ret;
        }

        public static List<PrivateMessages> getDefaultMessages(int amount)
        {
            List<PrivateMessages> ret = new List<PrivateMessages>();
            for (int i = 0; i < amount; i++)
            {
                ret.Add(new PrivateMessages(Databaze_API.Controllers.UserController.ActiveUser));
            }

            return ret;
        }
    }
}
