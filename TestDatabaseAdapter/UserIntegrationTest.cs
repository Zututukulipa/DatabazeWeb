using System;
using System.Globalization;
using System.Text;
using DatabaseAdapter.OracleLib;
using DatabaseAdapter.OracleLib.Models;
using Xunit;

namespace TestDatabaseAdapter
{
    public class UnitTest1
    {
        private readonly OracleDatabaseControls _controls = new OracleDatabaseControls("DATA SOURCE=localhost/XE;USER ID=schoold; password=heslo;");

        private string[] _maleSurnames = {
            "Novák", "Svoboda", "Novotný", "Dvořák", "Černý", "Procházka", "Kučera", "Veselý", "Horák", "Němec",
            "Pokorný", "Marek", "Pospíšil", "Hájek", "Jelínek", "Král", "Růžička", "Beneš", "Fiala", "Sedláček"
        };

        private string[] _femaleSurnames = {
            "Nováková", "Svobodová", "Novotná", "Dvořáková", "Černá", "Procházková", "Kučerová", "Veselá", "Horáková", "Němcová",
            "Pokorná", "Marková", "Pospíšilová", "Hájková", "Jelínková", "Králová", "Růžičková", "Benešová", "Fialová", "Sedláčková"
        };

        private string[] _maleNames = {"Jiří","Jan","Petr","Josef","Pavel","Martin", "Jaroslav", "Tomáš","Miroslav","Zdeněk","František","Václav","Michal","Milan","Karel","Jakub","Lukáš","David","Vladimír","Ladislav" };

        private string[] _femaleNames =
        {
            "Marie", "Jana", "Eva", "Hana", "Anna", "Lenka", "Kateřina", "Věra", "Lucie", "Alena", "Petra", "Jaroslava",
            "Veronika", "Martina", "Jitka","https://upce.cz/:1521", "Tereza", "Ludmila", "Helena", "Michaela", "Zdeňka"
        };

        private const int ArrayLength = 10;

        [Fact]
        public void UserIntegration()
        {
            //1	vrana	Daniel	Marek	Vrána	vrana@upce.cz
            User user;
            var random = new Random();
            var email = new StringBuilder();
            for (var i = 0; i < 1000; i++)
            {
                user = (i % 2 == 0) ?  GenerateRandomFemaleUser(random, email, i) : GenerateRandomMaleUser(random, email, i);
                user.Password = "password";
                user.UserId = _controls.InsertUser(user);
                var generatedUser = _controls.GetUserById(user.UserId);
                if (i % 3 == 0)
                {
                    _controls.DeleteUser(user);
                    var userDeleted = _controls.GetUserById(user.UserId);
                }
            }

            int[] yyyy = {1, 2, 3, 4, 5};
            for (var i = 0; i < 50; i++)
            {
                _controls.InsertTeacher(_controls.GetUserById(i));
            }
            
            for (var i = 50; i < 400; i++)
            {
                _controls.InsertStudent(_controls.GetUserById(i),yyyy[random.Next(5)]);
            } 
            
            
            
        }

        private User GenerateRandomFemaleUser(Random random, StringBuilder email, int i)
        {
            User user;
            user = new User();
            user.LastName = _femaleSurnames[random.Next(0, ArrayLength)];
            user.FirstName = _femaleNames[random.Next(0, ArrayLength)];

            GenerateUserEmail(email, user, i);

            user.MiddleName = i % 5 == 0 ? _femaleNames[random.Next(0, ArrayLength)] : "";

            return user;
        }

        private User GenerateRandomMaleUser(Random random, StringBuilder email, int i)
        {
            User user;
            user = new User();
            user.LastName = _maleSurnames[random.Next(0, ArrayLength)];
            user.FirstName = _maleNames[random.Next(0, ArrayLength)];

            GenerateUserEmail(email, user, i);

            user.MiddleName = i % 5 == 0 ? _maleNames[random.Next(0, ArrayLength)] : "";
            return user;
        }

        private static void GenerateUserEmail(StringBuilder email, User user, int i)
        {
            email.Append(RemoveDiacritism(user.LastName));
            email.Append(i);
            email.Append('.');
            email.Append(RemoveDiacritism(user.FirstName));
            user.Username = email.ToString();
            email.Append("@schoold.gg");
            user.Email = email.ToString();
            email.Clear();
        }
        
        public static string RemoveDiacritism(string text)
        {
            var stringFormD = text.Normalize(NormalizationForm.FormD);
            var retVal = new StringBuilder();
            for(var index = 0; index < stringFormD.Length ; index ++)
            {
                if(CharUnicodeInfo.GetUnicodeCategory(stringFormD[index]) != UnicodeCategory.NonSpacingMark)
                    retVal.Append(stringFormD[index]);
            }
            return retVal.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}