using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Domain.Models;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    // Repository der håndterer læsning og skrivning af dyr til/fra JSON-fil
    public class AnimalRepository : IAnimalRepository
    {
        // Sti til JSON-filen med dyr
        private readonly string _jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "animals.json");
        private List<Animal> _animals; // Intern liste med dyr

        // Constructor - henter dyr fra filen, eller opretter en startliste
        public AnimalRepository()
        {
            try
            {
                if (File.Exists(_jsonPath))
                {
                    string json = File.ReadAllText(_jsonPath);
                    _animals = JsonSerializer.Deserialize<List<Animal>>(json);

                    if (_animals == null || _animals.Count == 0)
                    {
                        InitializeAnimals(); // Tilføj startdyr
                        SaveAll(_animals);
                    }
                }
                else
                {
                    InitializeAnimals(); // Hvis filen ikke findes
                    SaveAll(_animals);
                }
            }
            catch (Exception)
            {
                _animals = new List<Animal>();
            }
        }

        // Returnerer hele listen af dyr
        public List<Animal> GetAll()
        {
            return _animals;
        }

        // Finder et dyr på ID, returnerer null hvis ikke fundet
        public Animal GetById(int id)
        {
            foreach (Animal animal in _animals)
            {
                if (animal.Id == id)
                {
                    return animal;
                }
            }
            return null;
        }

        // Gemmer listen af dyr til JSON-fil
        public void SaveAll(List<Animal> animals)
        {
            _animals = animals;

            try
            {
                string json = JsonSerializer.Serialize(_animals, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                Directory.CreateDirectory(Path.GetDirectoryName(_jsonPath));
                File.WriteAllText(_jsonPath, json);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Tilføjer et nyt dyr med unikt ID og gemmer listen
        public void Add(Animal animal)
        {
            int maxId = 0;

            foreach (Animal a in _animals)
            {
                if (a.Id > maxId)
                {
                    maxId = a.Id;
                }
            }

            animal.Id = maxId + 1;
            _animals.Add(animal);
            SaveAll(_animals);
        }

        // Opretter en standardliste med dyr
        private void InitializeAnimals()
        {
            _animals = new List<Animal>
    {
        new Animal
        {
            Id = 1,
            Name = "Bella",
            Gender = "Hun",
            Species = "Hund",
            Race = "Labrador",
            Size = "Stor",
            BirthYear = 2018,
            ChipNumber = "1234567890",
            SpecialMarks = "Hvid pote",
            Description = "En legesyg og kærlig hund",
            IsSterilized = true,
            IsVaccinated = true,
            ImagePath = "images/bella.jpg",
            VisitLog = new List<VisitLogEntry>()
        },
        new Animal
        {
            Id = 2,
            Name = "Milo",
            Gender = "Han",
            Species = "Kat",
            Race = "Blanding",
            Size = "Lille",
            BirthYear = 2020,
            ChipNumber = "9876543210",
            SpecialMarks = "Sort plet på næsen",
            Description = "En rolig og nysgerrig kat",
            IsSterilized = false,
            IsVaccinated = true,
            ImagePath = "images/milo.jpg",
            VisitLog = new List<VisitLogEntry>()
        },
        new Animal
        {
            Id = 3,
            Name = "Luna",
            Gender = "Hun",
            Species = "Kanin",
            Race = "Løvehoved",
            Size = "Mellem",
            BirthYear = 2021,
            ChipNumber = "000111222",
            SpecialMarks = "Lang pels på hovedet",
            Description = "En nysgerrig og sød kanin",
            IsSterilized = false,
            IsVaccinated = false,
            ImagePath = "images/luna.jpg",
            VisitLog = new List<VisitLogEntry>()
        }
    };
        }

    }
}
