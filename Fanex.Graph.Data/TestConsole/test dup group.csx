#r "System.Xml.Linq"
using System;

var path = @"C:\Users\simon.huynh\Desktop\test.xml";
var xml = XElement.Load(path);
var data = xml.Descendants("Association").Select(x => x.Attribute("CustId").Value).Select(int.Parse);

Console.WriteLine($"[{string.Join(",", data)}]");