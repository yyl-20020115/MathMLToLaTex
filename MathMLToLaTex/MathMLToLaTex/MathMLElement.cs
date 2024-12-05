﻿namespace MaTex.MathMLToLaTex;

public class MathMLElement
{
    public static readonly MathMLElement Default = new();
    public string Name;
    public string Value;
    public MathMLElement[] Children = [];
    public Dictionary<string, string> Attributes = [];
    public MathMLElement(string Name = "Void", string Value = "", params (string name,string value)[] attributes)
    {
        this.Name = Name;
        this.Value = Value;
        foreach (var (name, value) in attributes)
        {
            this.Attributes.Add(name, value);
        }
    }
    public MathMLElement(string Name, string Value, MathMLElement[] Children)
    {
        this.Name = Name;
        this.Value = Value;
        this.Children = Children;
    }
}
