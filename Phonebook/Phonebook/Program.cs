using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;


namespace Phonebook
{
    class Program
    {
        /// <summary>
        /// Trace класс
        /// </summary>
        internal static TraceSource trace =
            new TraceSource("Phonebook");
        static void TraceSourceDemo1()
        {
            trace.TraceInformation("Info Message");
            trace.TraceEvent(TraceEventType.Error, 3, "Error Message");
            trace.TraceData(TraceEventType.Information, 2, "data1", 4, 5);
            trace.Flush();
            trace.Close();
        }
         /// <summary>
         /// Этот метод читает файл input так же осуществляет поиск по ов, так же создает файл xml
         /// </summary>
         /// <returns>Файл xml</returns>
        private static void Main(string[] args)
        {
            Spravka[] spravkaDataBase;
            XmlSerializer formatter = new XmlSerializer(typeof(Spravka[]));
            using (FileStream fs = new FileStream("input.txt", FileMode.OpenOrCreate))
            {
                spravkaDataBase = (Spravka[])formatter.Deserialize(fs);
            }

            foreach (var s in FindSurname(spravkaDataBase, "ов"))
                s.PrintInfo();


            using (FileStream fs = new FileStream("people.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, spravkaDataBase);
            }

        }
        /// <summary>
        /// Осуществляет поиск по массиву по характеристикам фамилии или по имени организации
        /// </summary>
        /// <param name="spravkaDataBase">в нем хранится все из нашего документа</param>
        /// <param name="surnameSubstring">Шаблон для поиска</param>
        private static IEnumerable<Spravka> FindSurname(Spravka[] spravkaDataBase, string surnameSubstring)
        {
            foreach (var s in spravkaDataBase)
                if (s is Person)
                {
                    if ((s as Person).Surname.Contains(surnameSubstring))
                        yield return s as Person;
                } else if (s is Organization)
                {
                    if ((s as Organization).Name.Contains(surnameSubstring))
                        yield return s as Organization;
                }

        }
    }
    
    [XmlInclude(typeof(Person)), XmlInclude(typeof(Organization)), XmlInclude(typeof(Friend))]
    [Serializable]
    ///<summary>
    /// абстрактный класс от него наследуется все остальное
    /// </summary>
    public abstract class Spravka
    {
        protected Spravka()
        {
        }

        public abstract void PrintInfo();
    }

    [Serializable]
    ///<summary>
    ///класс персона в котором содержится человек 
    /// </summary>
    public class Person : Spravka
    {
        public Person()
        {
        }

        public string Surname;
        public string Adres;
        public decimal Telephone;

        public Person(string surname, string adres, decimal telephone)
        {
            Surname = surname;
            Adres = adres;
            Telephone = telephone;
        }

        public override void PrintInfo()
        {
            Console.WriteLine("Фамилия: {0}", Surname);
            Console.WriteLine("Адрес: {0}", Adres);
            Console.WriteLine("Телефон: {0}", Telephone);
        }
    }

    [Serializable]
    ///<summary>
    ///класс организация в котором содержится все наши организации 
    /// </summary>
    public class Organization : Spravka
    {
        public Organization()
        {
        }

        public string Name;
        public string AdresOrg;
        public int Number;
        public int Faks;
        public string Kontakt;



        public Organization(string name, string adresOrg, int number, int faks, string kontakt)
        {
            Name = name;
            AdresOrg = adresOrg;
            Number = number;
            Faks = faks;
            Kontakt = kontakt;
        }

        public override void PrintInfo()
        {
            Console.WriteLine("Название организации: {0}", Name);
            Console.WriteLine("Адрес: {0}", AdresOrg);
            Console.WriteLine("Номер телефона: {0}", Number);
            Console.WriteLine("Факс: {0}", Faks);
            Console.WriteLine("Контактное лицо: {0}", Kontakt);
        }
    }

   
    [Serializable]
    ///<summary>
    ///класс друзей в котором содержится друзья 
    /// </summary>
    public class Friend : Person
    {
        public Friend()
        {
        }

        public DateTime BirthdayDate;

        public Friend(string surname, string adres, int telephone, DateTime birthdayDate) : base(surname, adres, telephone)
        {
            BirthdayDate = birthdayDate;
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine("Дата рождения: {0}", BirthdayDate);
        }
    }
}
