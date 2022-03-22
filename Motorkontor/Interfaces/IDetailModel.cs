﻿using System.Collections.Generic;

namespace Motorkontor.Data
{
    public interface IDetailModel
    {
        public Dictionary<Field, string> GetFields();
        public void UpdateFields(Dictionary<Field, string> fields);
    }

    public class Field
    {
        public string Name { get; set; }
        public string Text { get; set; }

        public static string NullToEmpty(string str)
        {
            return (str != null) ? str : "";
        }

        public Field(string name, string text)
        {
            Name = name;
            Text = text;
        }
    }
}