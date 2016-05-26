﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NashGaming.Models;
using System.Data.Entity;

namespace NashGaming.Tests.Models
{
    [TestClass]
    public class RepoPostTests
    {
        private Mock<DbSet<Posts>> _postSet;
        private Mock<NashGamingContext> _context;
        private NashGamingRepository _repo;
        [TestMethod]
        private void ConnectMocksToDataStore(IEnumerable<Posts> data_store)
        {
            var data_source = data_store.AsQueryable();
            _postSet.As<IQueryable<Posts>>().Setup(data => data.Provider).Returns(data_source.Provider);
            _postSet.As<IQueryable<Posts>>().Setup(data => data.Expression).Returns(data_source.Expression);
            _postSet.As<IQueryable<Posts>>().Setup(data => data.ElementType).Returns(data_source.ElementType);
            _postSet.As<IQueryable<Posts>>().Setup(data => data.GetEnumerator()).Returns(data_source.GetEnumerator());
            _context.Setup(a => a.Posts).Returns(_postSet.Object);
        }

        [TestInitialize]
        public void Initialize()
        {
            _context = new Mock<NashGamingContext>();
            _repo = new NashGamingRepository(_context.Object);
            _postSet = new Mock<DbSet<Posts>>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context = null;
            _repo = null;
            _postSet = null;
        }
        [TestMethod]
        public void RepositoryTestsEnsureICanGetAllPosts()
        {
            List<Posts> expected = new List<Posts>
            {
                new Posts {PostID= 1, Date = new DateTime(2015, 12, 2), Content = "What??"},
                new Posts {PostID= 2, Date = new DateTime(2015, 12, 5), Content = "Who??"  }
            };


            _postSet.Object.AddRange(expected);
            ConnectMocksToDataStore(expected);

            var actual = _repo.GetAllPosts();
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(2, actual[0].PostID);
        }

        [TestMethod]
        public void RepositoryTestsEnsureICanSearchPostsByContent()
        {
            List<Posts> expected = new List<Posts>
            {
                new Posts {PostID= 1, Date = new DateTime(2015, 12, 2), Content = "What is the name of the game??"},
                new Posts {PostID= 2, Date = new DateTime(2015, 12, 5), Content = "Who??"  },
                new Posts {PostID= 3, Date = new DateTime(2015, 12, 7, 10, 15, 00), Content = "Game Game" }
            };


            _postSet.Object.AddRange(expected);
            ConnectMocksToDataStore(expected);

            var actual = _repo.SearchPostsByContent("game");
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(3, actual[0].PostID);
            Assert.AreEqual(1, actual[1].PostID);
        }

        [TestMethod]
        public void RepositoryTestsEnsureICanDeletePostById()
        {
            List<Posts> expected = new List<Posts>
            {
                new Posts {PostID= 1, Date = new DateTime(2015, 12, 2), Content = "What is the name of the game??"},
                new Posts {PostID= 2, Date = new DateTime(2015, 12, 5), Content = "Who??"  },
                new Posts {PostID= 3, Date = new DateTime(2015, 12, 7, 10, 15, 00), Content = "Game Game" }
            };


            _postSet.Object.AddRange(expected);
            ConnectMocksToDataStore(expected);
            _postSet.Setup(o => o.Remove(It.IsAny<Posts>())).Callback((Posts p) => expected.Remove(p));

            bool actual = _repo.DeletePostById(1);
            var numPosts = _repo.GetAllPosts();
            Assert.IsTrue(actual);
            Assert.AreEqual(2, numPosts.Count);
        }

        [TestMethod]
        public void RepositoryTestsEnsureICanCreateANewPost()
        {
            Gamer me = new Gamer { GamerID = 1, Username = "Stiffy" };
            string input = "This is my post";
            List<Posts> posts = new List<Posts> {
                new Posts { PostID = 1, Author = me, Content = "blah"  }
            };

            _postSet.Object.AddRange(posts);
            ConnectMocksToDataStore(posts);
            _postSet.Setup(o => o.Add(It.IsAny<Posts>())).Callback((Posts p) => posts.Add(p));

            bool actual = _repo.CreateAPost(me, input);
            Assert.IsTrue(actual);
            Assert.AreEqual(2, posts.Count);
        }
    }
}