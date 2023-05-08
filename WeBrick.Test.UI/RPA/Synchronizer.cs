using Npgsql;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WeBrick.Test.UI.Extensions;
using Xunit;


namespace WeBrick.Test.UI.RPA
{
    public class Synchronizer
    {
        [Fact(Skip = "this is just an RPA to sync users from DK to PT it should not be used as a test. remember to disable/delete user id constraints in Pt Auth Db before running and enable/create them after finish of RPA")]
        [Trait("Category", "RPA_NotATest")]
        public void SyncAllUsersFromDkToPt()
        {
            var prodConnectionString = Configuration.GetOptionalValue("prod", "connectionString");
            var prodAuthConnectionString = Configuration.GetOptionalValue("prod", "authConnectionString");


            UpdateSubjectIds(prodAuthConnectionString);

            using (var conn = new NpgsqlConnection(prodAuthConnectionString))
            {
                conn.Open();
                List<string> prodPhoneNumbers = GetOneColumnValues(prodAuthConnectionString, "PhoneNumber");
                List<string> qaPhoneNumbers = GetOneColumnValues(Configuration.GetOptionalValue("qa", "authConnectionString"), "PhoneNumber");
                prodPhoneNumbers.RemoveAll(x => qaPhoneNumbers.Contains(x));
                foreach (var prodPhoneNumber in prodPhoneNumbers)
                {
                    qaPhoneNumbers = GetOneColumnValues(Configuration.GetOptionalValue("qa", "authConnectionString"), "PhoneNumber");
                    if (qaPhoneNumbers.Contains(prodPhoneNumber))
                    {
                        continue;
                    }
                    try
                    {

                        string query2 = $@"SELECT uc.""ClaimType"", uc.""ClaimValue"" FROM  public.""UserClaims"" uc, public.""Users"" u  where u.""PhoneNumber"" = '{prodPhoneNumber}';";
                        string name = "name";
                        string surname = "surname";
                        string phoneNumber = "b6vt56";
                        string email = "test@test.test";
                        using (var command2 = new NpgsqlCommand(query2, conn))
                        {
                            using (NpgsqlDataReader dr2 = command2.ExecuteReader())
                            {


                                while (dr2.Read())
                                {
                                    string claimType = dr2.GetString(0);
                                    string claimValue = dr2.GetString(1);


                                    if (string.IsNullOrWhiteSpace(claimValue.ToLower().Trim()))
                                    {
                                        continue;
                                    }

                                    switch (claimType.ToLower().Trim())
                                    {
                                        case "email":
                                            email = claimValue;
                                            break;
                                        case "given_name":
                                            name = claimValue;
                                            break;
                                        case "family_name":
                                            surname = claimValue;
                                            break;
                                        case "phone_number":
                                            phoneNumber = prodPhoneNumber;
                                            break;
                                    }
                                }
                            }

                            using (IWebDriver driver = Utils.GetChromeDriver())
                            {
                                var registrationPage = new Websites.RegistrationPage(driver);
                                driver.Navigate().GoToUrl(Configuration.GetOptionalValue("local", "registrationPage"));
                                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                                wait.WaitForDocumentReadyStateComplete();

                                // Act
                                registrationPage.Name.SendKeys(name);
                                registrationPage.Surname.SendKeys(surname);
                                registrationPage.Phone.SendKeys(phoneNumber.Replace("+45", ""));
                                registrationPage.Email.SendKeys(email);
                                registrationPage.ContinueBtn.Click();
                                Utils.Wait(0.01);
                                registrationPage.SmsVerificationTextbox.SendKeys("123456");
                                registrationPage.ContinueBtn.Click();
                                Utils.Wait(0.01);
                                registrationPage.PasswordTextbox.SendKeys(@"!""#¤%&123456");
                                registrationPage.VerifyPasswordTextbox.SendKeys(@"!""#¤%&123456");
                                registrationPage.AcceptConditionsCheckbox.Click();
                                Utils.Wait(0.01);
                                registrationPage.ContinueBtn.Click();
                                Utils.Wait(0.1);
                                Utils.MakeSureChromeDriveDoesNotExist();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        File.WriteAllText(@".\problematicIds.txt", prodPhoneNumber);
                    }
                }
            }

            UpdateSubjectIds(prodAuthConnectionString);

        }

        private void UpdateSubjectIds(string prodAuthConnectionString)
        {
            var prodPhoneUserIds = new Dictionary<string, string>();
            GetOneColumnValues(prodAuthConnectionString, @"PhoneNumber"",""SubjectId").ToDictionary(x => x.Split(',')[0]).Select(keyVal =>
            {
                string val = keyVal.Value.Split(',').Last();
                prodPhoneUserIds.Add(keyVal.Key, val);
                return val;
            }).ToList();
            var qaPhoneUserIds = new Dictionary<string, string>();
            GetOneColumnValues(Configuration.GetOptionalValue("qa", "authConnectionString"), @"PhoneNumber"",""SubjectId").ToDictionary(x => x.Split(',')[0]).Select(keyVal =>
            {
                string val = keyVal.Value.Split(',').Last();
                qaPhoneUserIds.Add(keyVal.Key, val);
                return val;
            }).ToList();
            foreach (string prodPhoneNumber in prodPhoneUserIds.Keys)
            {
                if (qaPhoneUserIds.ContainsKey(prodPhoneNumber) && qaPhoneUserIds[prodPhoneNumber] != prodPhoneUserIds[prodPhoneNumber])
                {
                    using (var conn = new NpgsqlConnection(Configuration.GetOptionalValue("qa", "authConnectionString")))
                    {
                        conn.Open();
                        string query2 = $@"update public.""Users"" set ""SubjectId""='{prodPhoneUserIds[prodPhoneNumber]}' where ""PhoneNumber""= '{prodPhoneNumber}';";
                        using (var command2 = new NpgsqlCommand(query2, conn))
                        {
                            command2.ExecuteNonQuery();
                        }
                        query2 = $@"update public.""UserClaims"" set ""UserSubjectId""='{prodPhoneUserIds[prodPhoneNumber]}' where ""UserSubjectId""= '{qaPhoneUserIds[prodPhoneNumber]}';";
                        using (var command2 = new NpgsqlCommand(query2, conn))
                        {
                            command2.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private List<string> GetOneColumnValues(string connectionString, string columnName)
        {
            var subjectIds = new List<string>();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                string query1 = $@"SELECT ""{columnName}"" 	FROM public.""Users"";";
                var command = new NpgsqlCommand(query1, conn);
                using (NpgsqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        subjectIds.Add(dr.GetString(0));
                        if (columnName.Contains(","))
                        {
                            subjectIds[subjectIds.Count - 1] += "," + dr.GetString(1);
                        }
                    }
                }
            }
            return subjectIds;
        }
    }
}
