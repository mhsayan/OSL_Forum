using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using OSL.Forum.Entities;
using OSL.Forum.Entities.Contexts;
using OSL.Forum.DAO;
using OSL.Forum.Services;

namespace OSL.Forum.DAOTests
{
    [TestClass]
    public class CategoryRepositoryTests
    {
        [TestMethod]
        public void GetForumById()
        {
            var data = new List<Entities.Forum>
            {
                new Entities.Forum { Id = 1, Name = "BBB" },
                new Entities.Forum { Id = 2, Name = "ZZZ" },
                new Entities.Forum { Id = 3,  Name = "AAA" }
            };

            var repository = new Mock<IForumRepository>();
            repository.Setup(x => x.GetById(It.IsAny<long>()))
                .Returns((long i) => data.Single(bo => bo.Id == i));

            var forumService = new ForumService(repository.Object);
            var forum = forumService.GetForumById(2);

            Assert.IsNotNull(forum);
            Assert.AreEqual(forum.Id, 2);
            Assert.AreEqual(forum.Name, "ZZZ");
        }
        
        [TestMethod]
        public void RepositoryGetForumById()
        {
            var forums = new List<Entities.Forum>(){
                new Entities.Forum(){Id = 1, Name = "Forum_O1"},
                new Entities.Forum(){Id = 2, Name = "Forum_O2"},
                new Entities.Forum(){Id = 3, Name = "Forum_O3"},
                new Entities.Forum(){Id = 4, Name = "Forum_O4"}
            };
            
            var mockForumRepository = new Mock<IForumRepository>();

            mockForumRepository.Setup(mr => mr.GetById(
                It.IsAny<long>())).Returns((long i) => forums.FirstOrDefault(x => x.Id == i));

            var forumRepository = mockForumRepository.Object;

            var forum = forumRepository.GetById(2);

            Assert.IsNotNull(forum);
            Assert.AreEqual(forum.Id, 2);
            Assert.AreEqual(forum.Name, "Forum_O2");
        }
    }
}
