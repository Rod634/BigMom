using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace BigMom.Driver
{
    public class Blip
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;
        public Blip()
        {
            // init driver in the blip chat
            _driver = new ChromeDriver($"C:/Users/Rodrigo.silva/Documents/bin");
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(120));

            Login();
            LoadBotsAndUsers();
        }

        public void Login()
        {
            _driver.Navigate().GoToUrl(@"https://portal.blip.ai/");
            _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.Id("main-navbar")));
        }

        public void LoadBotsAndUsers()
        {
            string[] bots = File.ReadAllLines(@"..\..\..\Files\bots.csv");
            string[] users = File.ReadAllLines(@"..\..\..\Files\users.csv");

            bots.ToList().ForEach(d => AccessTeams(d, users));
        }

        private void AccessTeams(string bot, string[] users)
        {
            _driver.Navigate().GoToUrl($"https://portal.blip.ai/application/detail/{bot}/team");
            users.ToList().ForEach(u => AddTeam(u));
        }

        private void AddTeam(string user)
        {
            _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.XPath("/html/body/div[3]/div[1]/section/ui-view/section/div/page-header/div[1]/div[1]/div[1]/div[2]/custom-content/button")));
            //Click in add users
            IWebElement addButton = _driver.FindElement(By.XPath(@"/html/body/div[3]/div[1]/section/ui-view/section/div/page-header/div[1]/div[1]/div[1]/div[2]/custom-content/button"));
            _wait.Until(ExpectedConditions.ElementToBeClickable(addButton));

            addButton.Click();

            //set user email
            _driver.FindElement(By.XPath(@"/html/body/div[7]/div[2]/div[2]/form/div[1]/material-input/div/input")).SendKeys(user);
            //add admin permission
            _driver.FindElement(By.XPath(@"/html/body/div[7]/div[2]/div[2]/form/div[1]/div[3]/ul/li[4]/span[2]")).Click();
            //save
            IWebElement saveButton = _driver.FindElement(By.XPath(@"/html/body/div[7]/div[2]/div[2]/form/div[2]/button[2]"));
            _wait.Until(ExpectedConditions.ElementToBeClickable(saveButton));

            saveButton.Click();

            Thread.Sleep(8000);
        }

    }
}
