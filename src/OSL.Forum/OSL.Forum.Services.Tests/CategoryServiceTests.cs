using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NUnit.Framework;
using Moq;
using OSL.Forum.DAO;
using OSL.Forum.Entities;
using BoCategory = OSL.Forum.Entities.BusinessObjects.Category;
using Assert = NUnit.Framework.Assert;
using EoCategory = OSL.Forum.Entities.Category;

namespace OSL.Forum.Services.Tests
{
    [TestClass]
    public class CategoryServiceTests
    {
        private IList<EoCategory> EoCategories { get; set; }
        private IList<BoCategory> BoCategories { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            #region Category Initialization

            DateTime time = DateTime.Now;

            var eoForums = new List<Entities.Forum>()
            {
                new Entities.Forum()
                {
                    Id = 1,
                    Name = "Test_01",
                    ModificationDate = time,
                    CreationDate = time,
                    CategoryId = 1,
                    Category = new EoCategory()
                    {
                        Id = 1,
                        Name = "Category_01",
                        CreationDate = time,
                        ModificationDate = time
                    }
                }
            };

            EoCategories = new List<EoCategory>(){
                new EoCategory(){ Id = 1, Name = "ProthomAlo", Forums = eoForums },
                new EoCategory(){Id = 2, Name = "Jugantor", Forums = eoForums},
                new EoCategory(){Id = 3, Name = "DainikShiksha", Forums = eoForums},
                new EoCategory(){Id = 4, Name = "Somokal", Forums = eoForums}
            };

            var boForums = new List<Entities.BusinessObjects.Forum>()
            {
                new Entities.BusinessObjects.Forum()
                {
                    Id = 1,
                    Name = "Test_01",
                    ModificationDate = time,
                    CreationDate = time,
                    CategoryId = 1,
                    Category = new BoCategory()
                    {
                    Id = 1,
                    Name = "Category_01",
                    CreationDate = time,
                    ModificationDate = time
                }
                }
            };

            BoCategories = new List<BoCategory>(){
                new BoCategory()
                {
                    Id = 1,
                    Name = "ProthomAlo",
                },
                new BoCategory(){Id = 2, Name = "Jugantor", Forums = boForums},
                new BoCategory(){Id = 3, Name = "DainikShiksha", Forums = boForums},
                new BoCategory(){Id = 4, Name = "Somokal", Forums = boForums}
            };

            #endregion
        }

        #region GetCategoryByName

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        public void GetCategoryByName_CategoryNameNotReceived_ThrowException(string categoryName)
        {
            var categoryRepository = new Mock<ICategoryRepository>();
            var categoryService = new CategoryService(categoryRepository.Object);

            Assert.Throws<ArgumentNullException>(() => categoryService.GetCategoryByName(categoryName));
        }

        [TestMethod]
        public void GetCategoryByName_ReturnedNullCategory_ReturnNull()
        {
            //Arrange
            const string categoryName = "ProthomAlo";
            Category category = null;

            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(mr => mr.GetByName(
                It.IsAny<string>())).Returns(category);

            var categoryRepository = mockCategoryRepository.Object;
            var categoryService = new CategoryService(categoryRepository);

            //Act
            var categoryEntity = categoryService.GetCategoryByName(categoryName);

            //Assert
            Assert.IsNull(categoryEntity);
        }

        [TestMethod]
        public void GetCategoryByName_ReceivedCategory_ReturnCategory()
        {
            //Arrange
            const string categoryName = "ProthomAlo";
            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(mr => mr.GetByName(
                It.IsAny<string>())).Returns((string i) => EoCategories.FirstOrDefault(x => x.Name == i));

            var categoryRepository = mockCategoryRepository.Object;
            var categoryService = new CategoryService(categoryRepository);

            //Act
            var categoryEntity = categoryService.GetCategoryByName(categoryName);

            //Assert
            Assert.IsNotNull(categoryEntity);
            Assert.AreEqual(categoryEntity.Name, categoryName);
        }

        #endregion

        #region GetCategoryById

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void GetCategoryById_CategoryIdNotReceived_ThrowException(long categoryId)
        {
            var categoryRepository = new Mock<ICategoryRepository>();
            var categoryService = new CategoryService(categoryRepository.Object);

            Assert.Throws<ArgumentException>(() => categoryService.GetCategoryById(categoryId));
        }

        [TestMethod]
        public void GetCategoryById_ReturnedNullCategory_ReturnNull()
        {
            //Arrange
            const long categoryId = 1;
            Category category = null;

            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(mr => mr.GetByName(
                It.IsAny<string>())).Returns(category);

            var categoryRepository = mockCategoryRepository.Object;
            var categoryService = new CategoryService(categoryRepository);

            //Act
            var categoryEntity = categoryService.GetCategoryById(categoryId);
            
            //Assert
            Assert.IsNull(categoryEntity);
        }

        [TestMethod]
        public void GetCategoryById_ReceivedCategory_ReturnCategory()
        {
            //Arrange
            const long categoryId = 2;
            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(mr => mr.GetById(
                It.IsAny<long>())).Returns((long i) => EoCategories.FirstOrDefault(x => x.Id == i));

            var categoryRepository = mockCategoryRepository.Object;
            var categoryService = new CategoryService(categoryRepository);

            //Act
            var categoryEntity = categoryService.GetCategoryById(categoryId);

            //Assert
            Assert.IsNotNull(categoryEntity);
            Assert.AreEqual(categoryEntity.Id, categoryId);
        }

        #endregion

        #region GetCategoryCount

        [TestMethod]
        public void GetCategoryCount_ReceivedCount()
        {
            //Arrange
            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(mr => mr.GetCount()).Returns(EoCategories.Count);

            var categoryRepository = mockCategoryRepository.Object;
            var categoryService = new CategoryService(categoryRepository);

            //Act
            var categoryCount = categoryService.GetCategoryCount();

            //Assert
            Assert.AreEqual(categoryCount, EoCategories.Count);
        }

        #endregion

        #region GetCategories

        [TestMethod]
        public void GetCategories_ReceivedNullCategories_ThrowException()
        {
            //Arrange
            const int pageIndex = 1;
            const int pageSize = 10;
            const bool tracking = false;
            const string includedproperty = "Forums";
            IList<Category> categories = null;


            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(mr => mr.Load(pageIndex, pageSize, tracking, includedproperty)).Returns(categories);

            var categoryRepository = mockCategoryRepository.Object;
            var categoryService = new CategoryService(categoryRepository);

            //Act & Assert
            Assert.Throws<InvalidOperationException>(() => categoryService.GetCategories(pageIndex, pageSize));
        }

        #endregion

        #region DeleteCategory

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void DeleteCategory_CategoryIdNotReceived_ThrowException(long categoryId)
        {
            var categoryRepository = new Mock<ICategoryRepository>();
            var categoryService = new CategoryService(categoryRepository.Object);

            Assert.Throws<ArgumentException>(() => categoryService.DeleteCategory(categoryId));
        }

        [TestMethod]
        public void DeleteCategory_CategoryIdReceived_CategoryDeleted()
        {
            //Arrange
            const long categoryId = 10;
            var mockCategoryRepository = new Mock<ICategoryRepository>();
            mockCategoryRepository.Setup(mr => mr.RemoveById(categoryId)).Verifiable();
            mockCategoryRepository.Setup(mr => mr.Save()).Verifiable();
            var categoryService = new CategoryService(mockCategoryRepository.Object);

            //Act
            categoryService.DeleteCategory(categoryId);

            //Assert
            mockCategoryRepository.VerifyAll();
        }

        #endregion

        #region CreateCategory

        [TestMethod]
        public void CreateCategory_ReceivedNullCategory_ThrowException()
        {
            //Arrange
            BoCategory category = null;

            var categoryRepository = new Mock<ICategoryRepository>();
            var categoryService = new CategoryService(categoryRepository.Object);

            //Act & Assert
            Assert.Throws<ArgumentNullException>(() => categoryService.CreateCategory(category));
        }

        [TestMethod]
        public void CreateCategory_ReceivedOldCategory_ThrowException()
        {
            //Arrange
            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(mr => mr.GetByName(
                It.IsAny<string>())).Returns((string i) => EoCategories.FirstOrDefault(x => x.Name == i));

            var categoryRepository = mockCategoryRepository.Object;
            var categoryService = new CategoryService(categoryRepository);

            //Act & Assert
            Assert.Throws<DuplicateNameException>(() => categoryService.CreateCategory(BoCategories[0]));
        }

        [TestMethod]
        public void CreateCategory_ReceivedNullCategory_CategoryCreated()
        {
            //Arrange
            EoCategory category = null;
            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(mr => mr.GetByName(
                It.IsAny<string>())).Returns(category).Verifiable();
            mockCategoryRepository.Setup(mr => mr.Add(It.IsAny<EoCategory>())).Verifiable();
            mockCategoryRepository.Setup(mr => mr.Save()).Verifiable();

            var categoryRepository = mockCategoryRepository.Object;
            var categoryService = new CategoryService(categoryRepository);

            //Act
            categoryService.CreateCategory(BoCategories[0]);

            //Assert
            mockCategoryRepository.VerifyAll();
        }

        #endregion

        #region EditCategory

        [TestMethod]
        public void EditCategory_ReceivedNullCategory_ThrowException()
        {
            //Arrange
            BoCategory category = null;

            var categoryRepository = new Mock<ICategoryRepository>();
            var categoryService = new CategoryService(categoryRepository.Object);

            //Act & Assert
            Assert.Throws<ArgumentNullException>(() => categoryService.EditCategory(category));
        }

        [TestMethod]
        public void EditCategory_ReceivedOldCategory_ThrowException()
        {
            //Arrange
            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(mr => mr.GetByName(
                It.IsAny<string>())).Returns((string i) => EoCategories.FirstOrDefault(x => x.Name == i));

            var categoryRepository = mockCategoryRepository.Object;
            var categoryService = new CategoryService(categoryRepository);

            //Act & Assert
            Assert.Throws<DuplicateNameException>(() => categoryService.EditCategory(BoCategories[0]));
        }

        [TestMethod]
        public void EditCategory_ReceivedNullOldCategoryById_CategoryCreated()
        {
            //Arrange
            EoCategory category = null;
            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(mr => mr.GetByName(
                It.IsAny<string>())).Returns(category).Verifiable();
            mockCategoryRepository.Setup(mr => mr.GetById(
                It.IsAny<long>())).Returns(category).Verifiable();
            //mockCategoryRepository.Setup(mr => mr.Add(It.IsAny<EoCategory>())).Verifiable();
            //mockCategoryRepository.Setup(mr => mr.Save()).Verifiable();

            var categoryRepository = mockCategoryRepository.Object;
            var categoryService = new CategoryService(categoryRepository);

            //Act & Assert
            Assert.Throws<InvalidOperationException>(() => categoryService.EditCategory(BoCategories[0]));
        }

        [TestMethod]
        public void EditCategory_ReceivedNullCategory_CategoryCreated()
        {
            //Arrange
            EoCategory category = null;
            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(mr => mr.GetByName(
                It.IsAny<string>())).Returns(category).Verifiable();
            mockCategoryRepository.Setup(mr => mr.GetById(
                It.IsAny<long>())).Returns((long i) => EoCategories.FirstOrDefault(x => x.Id == i)).Verifiable();
            mockCategoryRepository.Setup(mr => mr.Save()).Verifiable();

            var categoryRepository = mockCategoryRepository.Object;
            var categoryService = new CategoryService(categoryRepository);

            //Act
            categoryService.EditCategory(BoCategories[0]);

            //Assert
            mockCategoryRepository.VerifyAll();
        }

        #endregion
    }
}
