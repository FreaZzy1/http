using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;

namespace HttpNewsPAT_Cheremnykh
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WebRequest request = WebRequest.Create("http://news.permaviat.ru/main");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(response.StatusDescription);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer=reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
            Console.Read();
            SingIn("student", "Asdfg123");
            Console.Read();


        }
        public static void SingIn(string Login, string Password)
        {
            
            string url = "http://news.permaviat.ru/ajax/login.php";
           
            Debug.WriteLine($"Выполняем запрос: {url}");
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            
            request.Method = "POST";
            
            request.ContentType = "application/x-www-form-urlencoded";
            
            request.CookieContainer = new CookieContainer();
            
            string postData = $"Login={Login}&password={Password}";
            
            byte[] Data = Encoding.ASCII.GetBytes(postData);
            
            request.ContentLength = Data.Length;
            
            using (var stream = request.GetRequestStream())
            {
                stream.Write(Data, 0, Data.Length);
            }
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
           
            Debug.WriteLine($"Статус выполннения: {response.StatusCode}");
           
            string responseFromServer = new StreamReader(response.GetResponseStream()).ReadToEnd();
            
            Console.WriteLine(responseFromServer);
        }
        public static void GetContent(Cookie Token)
        {
            // Задаём URL
            string url = "http://news.permaviat.ru/main";
            Debug.WriteLine($"Выполняем запрос: {url}");
            // Создаём запрос для авторизации на сайте
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            // Создаём контейнер для Cookies
            request.CookieContainer = new CookieContainer();
            // Добавляем Cookie авторизированного пользователя
            request.CookieContainer.Add(Token);
            // Выполняем запрос, записывая результат в переменную response
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Выводим статус обращения
            Debug.WriteLine($"Статус выполнения: {response.StatusCode}");
            // Читаем ответ
            string responseFromServer = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Console.WriteLine(responseFromServer);
        }
        public static void ParsingHtml(string htmlCode)
        {
            var html = new HtmlDocument();
            // Загружаем код страницы в объект HTML
            html.LoadHtml(htmlCode);
            // Получаем структуру страницы
            var Document = html.DocumentNode;
            // Получаем элементы по классу News
            IEnumerable<HtmlNode> DivsNews = Document.Descendants().Where(n => n.HasClass("news"));
            // Перебираем элементы
            foreach (HtmlNode DivNews in DivsNews)
            {
                // Получаем SRC картинки
                var src = DivNews.ChildNodes[1].GetAttributeValue("src", "none");
                // Получаем наименование
                var name = DivNews.ChildNodes[3].InnerText;
                // Получаем описание
                var description = DivNews.ChildNodes[5].InnerText;
                // Выводим надпись
                Console.WriteLine(name + "\n" + "Изображение: " + src + "\n" + "Описание: " + description + "\n");
            }
        }
    }
}
