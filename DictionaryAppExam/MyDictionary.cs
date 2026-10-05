using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class MyDictionary
{
    public string Name { get; set; }
    public List<Word> Words { get; set; }

    public MyDictionary(string name)
    {
        Name = name;
        Words = new List<Word>();
    }

    public void AddWord()
    {
        Console.Write("Введіть слово: ");
        string wordText = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(wordText))
        {
            Console.WriteLine("Слово не може бути порожнім.");
            return;
        }

        Word existingWord = Words.FirstOrDefault(
            w => w.Text.Equals(wordText, StringComparison.OrdinalIgnoreCase));

        if (existingWord != null)
        {
            Console.WriteLine("Таке слово вже існує.");
            return;
        }

        Word word = new Word(wordText);

        while (true)
        {
            Console.Write("Введіть переклад: ");
            string translation = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(translation))
            {
                word.Translations.Add(translation);
            }

            Console.Write("Додати ще один переклад? (y/n): ");
            string answer = Console.ReadLine();

            if (answer.ToLower() != "y")
                break;
        }

        if (word.Translations.Count == 0)
        {
            Console.WriteLine("Слово не додано, оскільки немає перекладу.");
            return;
        }

        Words.Add(word);
        Console.WriteLine("Слово успішно додано!");
    }

    public void EditWord()
    {
        Console.Write("Введіть слово, яке потрібно змінити: ");
        string oldText = Console.ReadLine();

        Word word = FindWord(oldText);

        if (word == null)
        {
            Console.WriteLine("Слово не знайдено.");
            return;
        }

        Console.Write("Введіть нове слово: ");
        string newText = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newText))
        {
            Console.WriteLine("Слово не може бути порожнім.");
            return;
        }

        word.Text = newText;

        Console.WriteLine("Слово успішно змінено!");
    }

    public void EditTranslation()
    {
        Console.Write("Введіть слово: ");
        string wordText = Console.ReadLine();

        Word word = FindWord(wordText);

        if (word == null)
        {
            Console.WriteLine("Слово не знайдено.");
            return;
        }

        ShowTranslations(word);

        Console.Write("Введіть номер перекладу: ");
        if (!int.TryParse(Console.ReadLine(), out int number))
        {
            Console.WriteLine("Некоректний номер.");
            return;
        }

        if (number < 1 || number > word.Translations.Count)
        {
            Console.WriteLine("Такого перекладу немає.");
            return;
        }

        Console.Write("Введіть новий переклад: ");
        string newTranslation = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(newTranslation))
        {
            Console.WriteLine("Переклад не може бути порожнім.");
            return;
        }

        word.Translations[number - 1] = newTranslation;

        Console.WriteLine("Переклад успішно змінено!");
    }

    public void DeleteWord()
    {
        Console.Write("Введіть слово для видалення: ");
        string wordText = Console.ReadLine();

        Word word = FindWord(wordText);

        if (word == null)
        {
            Console.WriteLine("Слово не знайдено.");
            return;
        }

        Words.Remove(word);

        Console.WriteLine("Слово та всі його переклади видалено.");
    }

    public void DeleteTranslation()
    {
        Console.Write("Введіть слово: ");
        string wordText = Console.ReadLine();

        Word word = FindWord(wordText);

        if (word == null)
        {
            Console.WriteLine("Слово не знайдено.");
            return;
        }

        if (word.Translations.Count <= 1)
        {
            Console.WriteLine(
                "Неможливо видалити переклад. У слова повинен залишитися хоча б один переклад.");
            return;
        }

        ShowTranslations(word);

        Console.Write("Введіть номер перекладу для видалення: ");

        if (!int.TryParse(Console.ReadLine(), out int number))
        {
            Console.WriteLine("Некоректний номер.");
            return;
        }

        if (number < 1 || number > word.Translations.Count)
        {
            Console.WriteLine("Такого перекладу немає.");
            return;
        }

        word.Translations.RemoveAt(number - 1);

        Console.WriteLine("Переклад видалено.");
    }

    public void SearchWord()
    {
        Console.Write("Введіть слово для пошуку: ");
        string wordText = Console.ReadLine();

        Word word = FindWord(wordText);

        if (word == null)
        {
            Console.WriteLine("Слово не знайдено.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"Слово: {word.Text}");
        Console.WriteLine("Переклади:");

        for (int i = 0; i < word.Translations.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {word.Translations[i]}");
        }
    }

    public void ExportWord()
    {
        Console.Write("Введіть слово для експорту: ");
        string wordText = Console.ReadLine();

        Word word = FindWord(wordText);

        if (word == null)
        {
            Console.WriteLine("Слово не знайдено.");
            return;
        }

        string fileName = "export_" + word.Text + ".txt";

        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine($"Слово: {word.Text}");
            writer.WriteLine("Переклади:");

            foreach (string translation in word.Translations)
            {
                writer.WriteLine("- " + translation);
            }
        }

        Console.WriteLine($"Слово експортовано у файл: {fileName}");
    }

    public void SaveToFile()
    {
        string fileName = Name + ".txt";

        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (Word word in Words)
            {
                writer.Write(word.Text);

                foreach (string translation in word.Translations)
                {
                    writer.Write("|" + translation);
                }

                writer.WriteLine();
            }
        }

        Console.WriteLine($"Словник збережено у файл: {fileName}");
    }

    public void LoadFromFile()
    {
        string fileName = Name + ".txt";

        if (!File.Exists(fileName))
            return;

        Words.Clear();

        string[] lines = File.ReadAllLines(fileName);

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            string[] parts = line.Split('|');

            if (parts.Length < 2)
                continue;

            Word word = new Word(parts[0]);

            for (int i = 1; i < parts.Length; i++)
            {
                word.Translations.Add(parts[i]);
            }

            Words.Add(word);
        }
    }

    private Word FindWord(string text)
    {
        return Words.FirstOrDefault(
            w => w.Text.Equals(text, StringComparison.OrdinalIgnoreCase));
    }

    private void ShowTranslations(Word word)
    {
        Console.WriteLine($"Слово: {word.Text}");

        for (int i = 0; i < word.Translations.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {word.Translations[i]}");
        }
    }

    public void ShowAllWords()
    {
        if (Words.Count == 0)
        {
            Console.WriteLine("Словник порожній.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine($"===== {Name} =====");

        foreach (Word word in Words)
        {
            Console.Write(word.Text + " - ");

            Console.WriteLine(string.Join(", ", word.Translations));
        }
    }
}