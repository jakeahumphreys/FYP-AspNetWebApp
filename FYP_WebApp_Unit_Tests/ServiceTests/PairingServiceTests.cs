using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Castle.Components.DictionaryAdapter.Xml;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Moq;

namespace FYP_WebApp_Unit_Tests.ServiceTests
{
    [TestClass]
    public class PairingServiceTests
    {
        private readonly Mock<IPairingRepository> _mockPairingRepository;

        public PairingServiceTests()
        {
            _mockPairingRepository = new Mock<IPairingRepository>();

        }

        [TestMethod]
        public void TestGetAll()
        {
            _mockPairingRepository.Setup(x => x.GetAll()).Returns(new List<Pairing>());
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.GetAll();
            Assert.AreEqual(typeof(List<Pairing>), result.GetType());
        }

        [TestMethod]
        public void TestDetails()
        {
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(new Pairing());
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.GetDetails(1);
            Assert.AreEqual(typeof(Pairing), result.GetType());
        }


        [TestMethod]
        public void TestDetailsNoId()
        {
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(new Pairing());
            var pairingService = new PairingService(_mockPairingRepository.Object);
            Assert.ThrowsException<ArgumentException>(() => pairingService.GetDetails(0));
        }

        [TestMethod]
        public void TestDetailsNotFound()
        {
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(new Pairing());
            var pairingService = new PairingService(_mockPairingRepository.Object);
            Assert.ThrowsException<PairingNotFoundException>(() => pairingService.GetDetails(400));
        }

        [TestMethod]
        public void TestCreate()
        {
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.Create(new Pairing());
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestCreateNoContent()
        {
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.Create(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestCreateNoContentErrorType()
        {
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.Create(null);
            Assert.AreEqual(ResponseErrors.NullParameter, result.ResponseError);
        }

        [TestMethod]
        public void TestEdit()
        {
            var pairing = new Pairing{Id = 1};
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.Edit(pairing);
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestEditNoContent()
        {
            var pairing = new Pairing { Id = 1 };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.Edit(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestEditNoContentCheckErrorType()
        {
            var pairing = new Pairing { Id = 1 };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.Edit(null);
            Assert.AreEqual(ResponseErrors.NullParameter, result.ResponseError);
        }

        [TestMethod]
        public void TestDelete()
        {
            var pairing = new Pairing { Id = 1 };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.Delete(pairing);
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestDeleteNoContent()
        {
            var pairing = new Pairing { Id = 1 };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.Delete(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestDeleteNoContentErrorType()
        {
            var pairing = new Pairing { Id = 1 };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.Delete(null);
            Assert.AreEqual(ResponseErrors.NullParameter, result.ResponseError);
        }

        [TestMethod]
        public void TestCheckConflictingPairingsNoConflicts()
        {
            var pairing = new Pairing { Id = 1, Start = DateTime.Now, End = DateTime.Now.AddDays(1) };
            var pairings = new List<Pairing> { pairing};
            _mockPairingRepository.Setup(x => x.GetAll()).Returns(pairings);
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.CheckConflictingPairings(pairing);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestCheckConflictingPairingsWithConflicts()
        {
            var pairing = new Pairing { Id = 1 , Start = DateTime.Now, End = DateTime.Now.AddDays(1)};
            var conflictingPairing = new Pairing { Id = 2, Start = DateTime.Now, End = DateTime.Now.AddDays(1) };
            var pairings = new List<Pairing>{pairing, conflictingPairing};
            _mockPairingRepository.Setup(x => x.GetAll()).Returns(pairings);
            var pairingService = new PairingService(_mockPairingRepository.Object);
            var result = pairingService.CheckConflictingPairings(pairing);
            Assert.AreEqual(1, result.Count);
        }
    }
}
