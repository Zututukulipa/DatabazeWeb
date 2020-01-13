using System;
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

        private readonly int ARRAYLENGHT = 10;
        [Fact]
        public void UserIntegration()
        {
            //1	vrana	Daniel	Marek	Vrána	vrana@upce.cz
            User user;
            Random random = new Random();
            var email = new StringBuilder();
            Assert.True(true);
            for (int i = 0; i < 1000; i++)
            {
                user = (i % 2 == 0) ?  GenerateRandomFemaleUser(random, email, i) : GenerateRandomMaleUser(random, email, i);
                user.Password = "password";
                user.UserId = _controls.InsertUser(user);
                User generatedUser = _controls.GetUserById(user.UserId);
                if (!generatedUser.Equals(user))
                {
                    Assert.True(false);
                }

                if (i % 2 == 0)
                {
                    _controls.DeleteUser(user);
                    User userDeleted = _controls.GetUserById(user.UserId);
                    if (userDeleted.StatusId != 0)
                        Assert.True(false);
                }
            }

            int[] yyyy = new[] {1, 2, 3, 4, 5};
            for (int i = 30; i < 100; i++)
            {
                _controls.AssignAsStudent(i,yyyy[random.Next(5)]);
            } 
            
            
            
        }

        private User GenerateRandomFemaleUser(Random random, StringBuilder email, int i)
        {
            User user;
            user = new User();
            user.LastName = _femaleSurnames[random.Next(0, ARRAYLENGHT)];
            user.FirstName = _femaleNames[random.Next(0, ARRAYLENGHT)];
            user.StatusId = 1;

            GenerateUserEmail(email, user, i);

            user.MiddleName = i % 5 == 0 ? _femaleNames[random.Next(0, ARRAYLENGHT)] : "";

            return user;
        }

        private User GenerateRandomMaleUser(Random random, StringBuilder email, int i)
        {
            User user;
            user = new User();
            user.LastName = _maleSurnames[random.Next(0, ARRAYLENGHT)];
            user.FirstName = _maleNames[random.Next(0, ARRAYLENGHT)];
            user.StatusId = 1;

            GenerateUserEmail(email, user, i);

            user.MiddleName = i % 5 == 0 ? _maleNames[random.Next(0, ARRAYLENGHT)] : "";
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
        
        public static string RemoveDiacritism(string Text)
        {
            string stringFormD = Text.Normalize(NormalizationForm.FormD);
            StringBuilder retVal = new StringBuilder();
            for(int index = 0; index < stringFormD.Length ; index ++)
            {
                if(System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stringFormD[index]) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    retVal.Append(stringFormD[index]);
            }
            return retVal.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}