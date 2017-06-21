using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayorElection;
using System.Web.Http;
using System.Net.Http;
using MayorElection.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace MayorElection.Tests.Controllers
{
    [TestClass]
    public class CandidatesController
    {
        [TestMethod]
        // does my function all canddiates from election
        public void GetCandidates_Count()
        {
            var controller = new MayorElection.Controllers.CandidatesController();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            var res = controller.GetCandidates();            
            Dictionary<string, Candidate> ranksDictCached;
            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.IsTrue(res.TryGetContentValue<Dictionary<string, Candidate>>(out ranksDictCached));
            Assert.AreEqual(13, ranksDictCached.Count());
        }

        [TestMethod]
        // do object values from different candidates match
        public void GetCandidates_VoteRank3()
        {
            var controller = new MayorElection.Controllers.CandidatesController();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            var res = controller.GetCandidates();
            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Dictionary<string, Candidate> ranksDictCached;
            Assert.IsTrue(res.TryGetContentValue<Dictionary<string, Candidate>>(out ranksDictCached));
            Assert.AreEqual(10, ranksDictCached["0000064"].Rank3Count);
        }

        [TestMethod]
        // do object values from different candidates match
        public void GetCandidates_VoteRank2()
        {
            var controller = new MayorElection.Controllers.CandidatesController();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            var res = controller.GetCandidates();
            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Dictionary<string, Candidate> ranksDictCached;
            Assert.IsTrue(res.TryGetContentValue<Dictionary<string, Candidate>>(out ranksDictCached));
            Assert.AreEqual(21700, ranksDictCached["0000037"].Rank2Count);
        }

        [TestMethod]
        // do object values from different candidates match
        public void GetCandidates_VoteRank1()
        {
            var controller = new MayorElection.Controllers.CandidatesController();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            var res = controller.GetCandidates();
            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Dictionary<string, Candidate> ranksDictCached;
            Assert.IsTrue(res.TryGetContentValue<Dictionary<string, Candidate>>(out ranksDictCached));
            Assert.AreEqual(4612, ranksDictCached["0000038"].Rank1Count);
        }

        [TestMethod]
        // do object values from different candidates match for findbyID function
        public void GetCandidateswithId_CheckObjectValues()
        {
            var controller = new MayorElection.Controllers.CandidatesController();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            var res = controller.GetCandidateswithId("0000039");
            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Candidate cand;
            Assert.IsTrue(res.TryGetContentValue<Candidate>(out cand));
            Assert.AreEqual("AMY FARAH WEISS", cand.candName);
            Assert.AreEqual(23099, cand.Rank1Count);
            Assert.AreEqual(32191, cand.Rank2Count);
            Assert.AreEqual(24478, cand.Rank3Count);
        }

        [TestMethod]
        // does function give right status code when canddiate id does not exist
        public void GetCandidateswithId_WrongId()
        {
            var controller = new MayorElection.Controllers.CandidatesController();
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            var res = controller.GetCandidateswithId("0000099"); // this candidateid does not exist
            Assert.IsNotNull(res);
            Assert.AreEqual(HttpStatusCode.BadRequest, res.StatusCode);
        }            

    }
}
