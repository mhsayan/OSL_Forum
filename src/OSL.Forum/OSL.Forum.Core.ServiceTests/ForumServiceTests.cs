using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NUnit.Framework;
using OSL.Forum.Core.Services;
using System;
using OSL.Forum.Core.Repositories;

namespace OSL.Forum.Core.ServiceTests
{
    [TestClass]
    public class ForumServiceTests
    {
        private IForumRepository _mockForumRepository = new ForumRepository();
        private IForumService _mockForumService = Substitute.For<IForumService>();

        [Test]
        public void GetForumCount_()
        {
        }
    }
}
