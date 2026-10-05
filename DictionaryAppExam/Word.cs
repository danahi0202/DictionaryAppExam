using System;
using System.Collections.Generic;

class Word
{
    public string Text { get; set; }
    public List<string> Translations { get; set; }

    public Word(string text)
    {
        Text = text;
        Translations = new List<string>();
    }
}