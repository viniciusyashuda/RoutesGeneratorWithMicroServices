﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RoutesGeneratorWithMicroServices.Services
{
    public class WriteFile
    {
        public void WriteDocxFile(List<string> headers, List<string> teams, string service, string city, string pathWebRoot)
        {
            List<IDictionary<string, string>> content = ReadFile.ReadExcelFile(headers, pathWebRoot);
            List<IDictionary<string, string>> services = new();
            List<string> others = new();
            string os, @base = "", cep = "", address = "", number = "", district = "", complement = "";
            string path = pathWebRoot + "\\file\\RoutesRelatory.docx";
            int count = 0;
            int teamCount = 0;
            int servicesPerTeam;

            foreach (var item in content)
            {
                if ((item["CIDADE"] == city) && (item["SERVIÇO"] == service))
                    services.Add(item);
            }


            if (services.Count > teams.Count)
                servicesPerTeam = services.Count / teams.Count;
            else if (services.Count > teams.Count)
                servicesPerTeam = services.Count / teams.Count;
            else
                servicesPerTeam = 1;


            using (StreamWriter streamwriter = new StreamWriter(path, false, Encoding.GetEncoding("iso-8859-1")))
            {
                foreach (var item in services)
                {
                    string othersString = "";

                    foreach (var header in headers)
                    {
                        if (header == "OS")
                            os = header + ": " + item[header];
                        else if (header == "BASE")
                            @base = header + ": " + item[header];
                        else if (header == "CEP")
                            cep = header + ": " + item[header];
                        else if (header == "ENDEREÇO")
                            address = header + ": " + item[header] + ",";
                        else if (header == "NUMERO")
                            number = header + ": " + item[header];
                        else if (header == "BAIRRO")
                            district = header + ": " + item[header] + ",";
                        else if (header == "COMPLEMENTO")
                            complement = header + ": " + item[header];
                        else if (headers.Count > 9 && header != "SERVIÇO" && header != "CIDADE")
                            others.Add("\n" + header + ": " + item[header]);
                    }
                    if (count < servicesPerTeam)
                    {
                        string line = "";

                        foreach (var other in others)
                        {
                            othersString = othersString + other;
                        }

                        line = "\n\nROTA TRABALHO - " + DateTime.Now.ToString("dd/MM/yyyy") + "\n"
                            + $"\nSERVIÇO: {service}"
                            + $"\nTIME: {teams[teamCount]}, " + $"CIDADE: {city}"
                            + $"\n{@base}"
                            + $"\n{address} {number}"
                            + $"\n{cep}"
                            + $"\n{district} {complement}"
                            + $"{othersString}";

                        count++;

                        if (teamCount < teams.Count - 1 && count == servicesPerTeam)
                        {
                            count = 0;
                            teamCount++;
                        }
                        streamwriter.WriteLine(line);
                    }
                    others.Clear();
                }

            }
        }
    }
}