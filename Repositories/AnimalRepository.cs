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
                    Description = "En legesyg og kærlig hund",
                    ImagePath = "images/bella.jpg"
                },
                new Animal
                {
                    Id = 2,
                    Name = "Milo",
                    Gender = "Han",
                    Species = "Kat",
                    Description = "En rolig og nysgerrig kat",
                    ImagePath = "images/milo.jpg"
                },
                new Animal
                {
                    Id = 3,
                    Name = "Luna",
                    Gender = "Hun",
                    Species = "Kanin",
                    Description = "En nysgerrig og sød kanin",
                    ImagePath = "images/luna.jpg"
                }
            };
        }
    }
}
