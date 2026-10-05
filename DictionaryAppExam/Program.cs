using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static List<MyDictionary> dictionaries = new List<MyDictionary>();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        LoadDictionaries();

        while (true)
        {
            Console.Clear();

            Console.WriteLine("========== СЛОВНИКИ ==========");
            Console.WriteLine();
            Console.WriteLine("1. Створити словник");
            Console.WriteLine("2. Відкрити словник");
            Console.WriteLine("3. Показати список словників");
            Console.WriteLine("4. Видалити словник");
            Console.WriteLine("0. Вихід");
            Console.WriteLine();
            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateDictionary();
                    break;

                case "2":
                    OpenDictionary();
                    break;

                case "3":
                    ShowDictionaries();
                    break;

                case "4":
                    DeleteDictionary();
                    break;

                case "0":
                    SaveAllDictionaries();
                    return;

                default:
                    Console.WriteLine("Невірний вибір.");
                    Pause();
                    break;
            }
        }
    }

    static void CreateDictionary()
    {
        Console.Clear();

        Console.WriteLine("===== СТВОРЕННЯ СЛОВНИКА =====");
        Console.Write("Введіть назву словника: ");

        string name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Назва не може бути порожньою.");
            Pause();
            return;
        }

        foreach (MyDictionary dictionary in dictionaries)
        {
            if (dictionary.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Такий словник вже існує.");
                Pause();
                return;
            }
        }

        MyDictionary newDictionary = new MyDictionary(name);

        dictionaries.Add(newDictionary);

        newDictionary.SaveToFile();

        Console.WriteLine("Словник успішно створено!");

        Pause();
    }

    static void OpenDictionary()
    {
        if (dictionaries.Count == 0)
        {
            Console.WriteLine("Словників немає.");
            Pause();
            return;
        }

        ShowDictionaries();

        Console.Write("Введіть номер словника: ");

        if (!int.TryParse(Console.ReadLine(), out int number))
        {
            Console.WriteLine("Некоректний номер.");
            Pause();
            return;
        }

        if (number < 1 || number > dictionaries.Count)
        {
            Console.WriteLine("Такого словника немає.");
            Pause();
            return;
        }

        MyDictionary dictionary = dictionaries[number - 1];

        DictionaryMenu(dictionary);
    }

    static void DictionaryMenu(MyDictionary dictionary)
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine($"========== {dictionary.Name} ==========");
            Console.WriteLine();
            Console.WriteLine("1. Додати слово");
            Console.WriteLine("2. Змінити слово");
            Console.WriteLine("3. Змінити переклад");
            Console.WriteLine("4. Видалити слово");
            Console.WriteLine("5. Видалити переклад");
            Console.WriteLine("6. Знайти переклад");
            Console.WriteLine("7. Експортувати слово");
            Console.WriteLine("8. Показати всі слова");
            Console.WriteLine("9. Зберегти словник");
            Console.WriteLine("0. Назад");
            Console.WriteLine();

            Console.Write("Ваш вибір: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    dictionary.AddWord();
                    Pause();
                    break;

                case "2":
                    dictionary.EditWord();
                    Pause();
                    break;

                case "3":
                    dictionary.EditTranslation();
                    Pause();
                    break;

                case "4":
                    dictionary.DeleteWord();
                    Pause();
                    break;

                case "5":
                    dictionary.DeleteTranslation();
                    Pause();
                    break;

                case "6":
                    dictionary.SearchWord();
                    Pause();
                    break;

                case "7":
                    dictionary.ExportWord();
                    Pause();
                    break;

                case "8":
                    dictionary.ShowAllWords();
                    Pause();
                    break;

                case "9":
                    dictionary.SaveToFile();
                    Pause();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Невірний вибір.");
                    Pause();
                    break;
            }
        }
    }

    static void ShowDictionaries()
    {
        Console.Clear();

        Console.WriteLine("========== СПИСОК СЛОВНИКІВ ==========");
        Console.WriteLine();

        if (dictionaries.Count == 0)
        {
            Console.WriteLine("Словників немає.");
        }
        else
        {
            for (int i = 0; i < dictionaries.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {dictionaries[i].Name}");
            }
        }

        Pause();
    }

    static void DeleteDictionary()
    {
        if (dictionaries.Count == 0)
        {
            Console.WriteLine("Словників немає.");
            Pause();
            return;
        }

        Console.Clear();

        Console.WriteLine("========== ВИДАЛЕННЯ ==========");
        Console.WriteLine();

        for (int i = 0; i < dictionaries.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {dictionaries[i].Name}");
        }

        Console.WriteLine();
        Console.Write("Введіть номер словника: ");

        if (!int.TryParse(Console.ReadLine(), out int number))
        {
            Console.WriteLine("Некоректний номер.");
            Pause();
            return;
        }

        if (number < 1 || number > dictionaries.Count)
        {
            Console.WriteLine("Такого словника немає.");
            Pause();
            return;
        }

        MyDictionary dictionary = dictionaries[number - 1];

        string fileName = dictionary.Name + ".txt";

        dictionaries.RemoveAt(number - 1);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        Console.WriteLine("Словник видалено.");

        Pause();
    }

    static void LoadDictionaries()
    {
        string[] files = Directory.GetFiles(".", "*.txt");

        foreach (string file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);

            if (fileName.StartsWith("export_"))
                continue;

            MyDictionary dictionary = new MyDictionary(fileName);

            dictionary.LoadFromFile();

            dictionaries.Add(dictionary);
        }
    }

    static void SaveAllDictionaries()
    {
        foreach (MyDictionary dictionary in dictionaries)
        {
            dictionary.SaveToFile();
        }
    }

    static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Натисніть Enter для продовження...");
        Console.ReadLine();
    }
}