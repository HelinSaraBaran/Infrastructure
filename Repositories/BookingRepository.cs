﻿using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Domain.Models;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    // Repository der håndterer kundebesøg/bookinger med JSON-lagring
    public class BookingRepository : IBookingRepository
    {
        private const string JsonPath = "Data/bookings.json"; // Sti til JSON-filen
        private List<Visit> _bookings; // Intern liste med bookinger

        // Constructor – læser bookinger fra fil ved opstart
        public BookingRepository()
        {
            if (File.Exists(JsonPath))
            {
                string json = File.ReadAllText(JsonPath);
                _bookings = JsonSerializer.Deserialize<List<Visit>>(json);
            }
            else
            {
                _bookings = new List<Visit>();
            }
        }

        // Returnerer alle bookinger
        public List<Visit> GetAll()
        {
            return _bookings;
        }

        // Tilføjer en ny booking med automatisk ID og gemmer listen
        public void Add(Visit visit)
        {
            int nextId = 1;

            foreach (Visit b in _bookings)
            {
                if (b.Id >= nextId)
                {
                    nextId = b.Id + 1;
                }
            }

            visit.Id = nextId;
            _bookings.Add(visit);
            SaveToFile();
        }

        // Overskriver hele bookinglisten og gemmer
        public void SaveAll(List<Visit> visits)
        {
            _bookings = visits;
            SaveToFile();
        }

        // Sletter en booking baseret på ID og gemmer listen
        public void Delete(int id)
        {
            Visit toRemove = null;

            foreach (Visit booking in _bookings)
            {
                if (booking.Id == id)
                {
                    toRemove = booking;
                    break;
                }
            }

            if (toRemove != null)
            {
                _bookings.Remove(toRemove);
                SaveToFile();
            }
        }

        // Hjælpefunktion: skriver listen til JSON-fil
        private void SaveToFile()
        {
            string json = JsonSerializer.Serialize(_bookings, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            Directory.CreateDirectory("Data");
            File.WriteAllText(JsonPath, json);
        }
    }
}
