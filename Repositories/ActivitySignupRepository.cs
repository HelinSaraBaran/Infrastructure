using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Domain.Models;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    // Repository der håndterer aktivitetstilmeldinger med JSON-lagring
    public class ActivitySignupRepository : IActivitySignupRepository
    {
        private const string JsonPath = "Data/activitysignups.json"; // Sti til JSON-filen
        private List<ActivitySignup> _signups; // Intern liste med tilmeldinger

        // Constructor – læser tilmeldinger fra fil ved opstart
        public ActivitySignupRepository()
        {
            if (File.Exists(JsonPath))
            {
                string json = File.ReadAllText(JsonPath);
                _signups = JsonSerializer.Deserialize<List<ActivitySignup>>(json);
            }
            else
            {
                _signups = new List<ActivitySignup>();
            }
        }

        // Returnerer alle tilmeldinger
        public List<ActivitySignup> GetAll()
        {
            return _signups;
        }

        // Tilføjer en ny tilmelding og gemmer listen
        public void Add(ActivitySignup signup)
        {
            _signups.Add(signup);
            SaveToFile();
        }

        // Overskriver hele listen og gemmer
        public void SaveAll(List<ActivitySignup> signups)
        {
            _signups = signups;
            SaveToFile();
        }

        // Hjælpefunktion: skriver listen til JSON-fil
        private void SaveToFile()
        {
            string json = JsonSerializer.Serialize(_signups, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            Directory.CreateDirectory("Data");
            File.WriteAllText(JsonPath, json);
        }
    }
}
