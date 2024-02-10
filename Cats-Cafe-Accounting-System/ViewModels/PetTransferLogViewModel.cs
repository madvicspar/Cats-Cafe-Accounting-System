﻿using Cats_Cafe_Accounting_System.Models;
using Cats_Cafe_Accounting_System.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace Cats_Cafe_Accounting_System.ViewModels
{
    public class PetTransferLogViewModel : ObservableObject
    {
        private ObservableCollection<PetTransferLogEntryModel> petTransferLogEntries;
        public ObservableCollection<PetTransferLogEntryModel> PetTransferLogEntries
        {
            get { return petTransferLogEntries; }
            set
            {
                petTransferLogEntries = value;
                OnPropertyChanged(nameof(PetTransferLogEntries));
            }
        }
        public PetTransferLogViewModel()
        {
            // Инициализация коллекции питомцев
            PetTransferLogEntries = GetPetTransferLogEntriesFromTable("pet_transfer_log_entries");
        }
        public static ObservableCollection<PetTransferLogEntryModel> GetPetTransferLogEntriesFromTable(string table)
        {
            ObservableCollection<PetTransferLogEntryModel> petTransferLogEntries = new ObservableCollection<PetTransferLogEntryModel>();

            DataTable dataTable = DBContext.GetTable(table);

            foreach (DataRow row in dataTable.Rows)
            {
                PetTransferLogEntryModel petTransferLogEntry = new PetTransferLogEntryModel(Convert.ToInt32(row["id"]), 
                    DateTime.Parse(row["date"].ToString()), Convert.ToInt32(row["visitorid"]), Convert.ToInt32(row["petid"]));
                petTransferLogEntries.Add(petTransferLogEntry);
            }

            return petTransferLogEntries;
        }
    }
}
