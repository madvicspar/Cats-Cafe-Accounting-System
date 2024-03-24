﻿using Cats_Cafe_Accounting_System.ViewModels;
using Cats_Cafe_Accounting_System.RegularClasses;
using Cats_Cafe_Accounting_System.Utilities;
using Cats_Cafe_Accounting_System.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;

namespace Cats_Cafe_Accounting_System.ViewModels.Tests
{
    [TestClass()]
    public class AuthorizationViewModelTests
    {
        private static ServiceProvider _serviceProvider;
        private static ApplicationDbContext _dbContext;

        [ClassInitialize] 
        public static void ClassInitialize(TestContext context)
        {
            var services = new ServiceCollection();

            // Лучше использовать фейковый DbContext для тестирования
            _dbContext = ApplicationDbContext.CreateInMemoryDatabase();

            // Добавление фейковых данных в таблицы Genders, Breeds, Statuses
            _dbContext.Genders.Add(new Gender { Title = "женский" });
            _dbContext.Genders.Add(new Gender { Title = "мужской" });

            _dbContext.Statuses.Add(new Status { Title = "чилится" });
            _dbContext.Statuses.Add(new Status { Title = "не числится" });

            _dbContext.Breeds.Add(new Breed { Title = "сиамская", Id = "SMS" });
            _dbContext.Breeds.Add(new Breed { Title = "мейн-кун", Id = "MNC" });

            _dbContext.SaveChanges();

            services.AddSingleton(_dbContext);
            services.AddSingleton(new PetsViewModel(_dbContext));

            _serviceProvider = services.BuildServiceProvider();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _dbContext.Dispose();
        }

        [TestMethod()]
        public void ExecuteAddPetCommand_ShouldAddNewPet()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>(); // Используем Dependency Injection

                // Arrange
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test";

                // Act
                petsViewModel.ExecuteAddPetCommand();

                // Assert
                Assert.AreEqual(1, petsViewModel.Items.Count);
            }
        }

        [TestMethod()]
        public void ExecuteSearchNameCommandTest()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test2";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Name";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Pass";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test3";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test3";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.SearchName = "Tes";
                petsViewModel.ExecuteSearchNameCommand();
                // Assert
                Assert.AreEqual(3, petsViewModel.FilterNames.Count);
            }
        }
        [TestMethod()]
        public void ExecuteSearchNameCommandTest_EmptySearchText()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var petsViewModel = scope.ServiceProvider.GetRequiredService<PetsViewModel>();

                // Arranges and Acts
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.Name = "Test2";
                petsViewModel.FilterItems[petsViewModel.FilterItems.Count - 1].Item.PassNumber = "Test2";
                petsViewModel.ExecuteAddPetCommand();
                petsViewModel.SearchName = "Поиск по имени";
                petsViewModel.ExecuteSearchNameCommand();
                // Assert
                Assert.AreEqual("Поиск по имени", petsViewModel.SearchName);
            }
        }
    }
}