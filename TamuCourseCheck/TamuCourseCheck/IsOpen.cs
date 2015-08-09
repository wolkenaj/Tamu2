using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

namespace TamuCourseCheck
{
    [TestFixture]
    public class IsOpen
    {
        private ChromeDriver driver;
        
        
        public void Setup()
        {
            driver = new ChromeDriver();
        }

       /* [TestFixtureTearDown]
        public void TearDown()
        {
            driver.Dispose();
        }*/

        public void GetToLogIn()
        {
            //Arrange
            driver.Url = "https://howdy.tamu.edu/cp/home/displaylogin";
            //var expectedUrl ="https://cas.tamu.edu/cas/login?service=https%3A%2F%2Fhowdy.tamu.edu%2Fsso?m=f&renew=true";

            //Act
            var loginButton = driver.FindElementByClassName("loginButton");
            loginButton.Click();

            Thread.Sleep(500);

            /*var actualUrl = driver.Url;

            Assert.That(actualUrl == expectedUrl);*/
        }

        public void LogIn()
        {
            //Arrange
            /*GetToLogIn();
            var expectedUrl = "https://howdy.tamu.edu/render.userLayoutRootNode.uP?uP_root=root";
*/
            //Act
            var IdBlank = driver.FindElementById("username");
            IdBlank.SendKeys("wolkenaj"); /*place your username here*/

            Thread.Sleep(1000);

            var PasswordBlank = driver.FindElementById("password");
            PasswordBlank.SendKeys("School*13*"); /*place your password here*/

            Thread.Sleep(1000);

            var submitButton = driver.FindElementByCssSelector("button.maroonBtn");
            submitButton.Click();

            Thread.Sleep(4000);

            /*var actualUrl = driver.Url;

            Assert.That(actualUrl == expectedUrl);*/
        }


        public void GetToLookUp(string departmentChoice, string courseChoice, string crn)
        {
            //Arrange
                
                var expectedUrl = "https://compass-ssb.tamu.edu/pls/PROD/bwykfcls.P_GetCrse?deviceType=C";
                driver.Url =
        "https://howdy.tamu.edu/render.UserLayoutRootNode.uP?uP_tparam=utf&utf=%2fcp%2fip%2flogin%3fsys%3dsctssb%26url%3dhttps://compass-ssb.tamu.edu/pls/PROD/bwykfcls.p_sel_crse_search";
                driver.Url =
                    "https://compass-ssb.tamu.edu/pls/PROD/bwykfcls.p_sel_crse_search";
                var expectedValue = 0;

                //Act
                var buttons = driver.FindElementsByTagName("input");
                var submitButton = buttons[4];
                submitButton.Click();
                Thread.Sleep(2000);

                var department = driver.FindElementsByTagName("option");
                department.First(option => option.GetAttribute("value") == departmentChoice).Click();  /*Can change.  This choose Department.  Default is Accounting[0]*/
                Thread.Sleep(500);

                var buttonsSecondPage = driver.FindElementsByTagName("input");
                var courseSearchButton = buttonsSecondPage[23];
                courseSearchButton.Click();
                Thread.Sleep(3000);

                var sectionAreas = driver.FindElementsByCssSelector("form");

                for (int i = 3; i < sectionAreas.Count; i++)
                {
                    if (sectionAreas[i].FindElements(By.TagName("input"))[3].GetAttribute("value") == courseChoice)
                    {
                        sectionAreas[i].FindElements(By.TagName("input"))[29].Click();
                        break;
                    }
                }

                Thread.Sleep(3000);

                var classes = driver.FindElementsByCssSelector("tr");

                var capacityValueString = "this is not set"; 
                for (int i = 5; i < sectionAreas.Count-2; i++)
                {
                    if (classes[i].FindElements(By.CssSelector("td > a"))[0].Text == crn)
                    {
                        capacityValueString = classes[i].FindElements(By.TagName("td"))[12].Text;
                        break;
                    }
                }
                 
                /*Can change.  This chooses the section to monitor.  
                                                     * Add 17 for every line down from the first.
                                                     * Default is the first course [18]
                                                     * Note that each course is multiple lines down from the last depending on how often the classes are given*/
                
                Thread.Sleep(3000);

                var actualUrl = driver.Url;

                var numCapacity = Int32.Parse(capacityValueString);

                Console.Write(capacityValueString);

                if (numCapacity > 0)       /*Change to > when actual*/
                {
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                    message.To.Add("sethsmitherman2@gmail.com");       /*Change to recipient's email*/
                    message.Subject = "TAMU course opening";
                    message.From = new System.Net.Mail.MailAddress("wolkenaj@gmail.com");
                    message.Body = "The available capacity of " + departmentChoice + " " + courseChoice + " (CRN " + crn + ") is: " + capacityValueString;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("aspmx.l.google.com");
                    smtp.Send(message);
               }
                //Thread.Sleep(2000000);       /*Thirty three minute (approximate) sleep*/
            }

                   
        }



}
