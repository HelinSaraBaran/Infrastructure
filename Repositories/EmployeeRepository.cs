using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Domain.Models;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    // Repository der håndterer medarbejderdata med JSON-lagring
    public class EmployeeRepository : IEmployeeRepository
    {
        private const string JsonPath = "Data/employees.json"; // Sti til JSON-filen
        private List<Employee> _employees; // Intern liste med alle medarbejdere

        // Constructor – læser medarbejdere fra fil ved opstart
        public EmployeeRepository()
        {
            if (File.Exists(JsonPath))
            {
                string json = File.ReadAllText(JsonPath);
                _employees = JsonSerializer.Deserialize<List<Employee>>(json);
            }
            else
            {
                _employees = new List<Employee>();
            }
        }

        // Returnerer alle medarbejdere
        public List<Employee> GetAll()
        {
            return _employees;
        }

        // Tilføjer en ny medarbejder og gemmer listen
        public void Add(Employee employee)
        {
            _employees.Add(employee);
            SaveToFile(); // Gem efter ændring
        }

        // Fjerner medarbejder baseret på ID og gemmer listen
        public void RemoveById(int id)
        {
            for (int i = 0; i < _employees.Count; i++)
            {
                if (_employees[i].Id == id)
                {
                    _employees.RemoveAt(i);
                    break;
                }
            }

            SaveToFile(); // Gem efter ændring
        }

        // Finder en medarbejder ud fra ID
        public Employee GetById(int id)
        {
            foreach (Employee emp in _employees)
            {
                if (emp.Id == id)
                {
                    return emp;
                }
            }
            return null;
        }

        // Overskriver hele medarbejderlisten og gemmer
        public void SaveAll(List<Employee> employees)
        {
            _employees = employees;
            SaveToFile();
        }

        // Hjælpefunktion: skriver listen til JSON-fil
        private void SaveToFile()
        {
            string json = JsonSerializer.Serialize(_employees, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            Directory.CreateDirectory("Data");
            File.WriteAllText(JsonPath, json);
        }
    }
}
