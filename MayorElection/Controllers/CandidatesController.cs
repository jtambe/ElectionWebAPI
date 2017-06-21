using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Text;
using MayorElection.Models;
using System.Configuration;

namespace MayorElection.Controllers
{
    //[RoutePrefix("api/candidates")]
    public class CandidatesController : ApiController
    {
        // following is mapping Dict<CandId, Dict<voterank, count>>. This is average data structure for my liking. I have changed it to following
        //static Dictionary<string, Dictionary<string, int>> ranksDict = new Dictionary<string, Dictionary<string, int>>();

        // following is mapping Dict<CandId, Candidate>. I really like this input from Scott of mapping an id against whole candidate class type        
        static Dictionary<string, Candidate> ranksDictCached = null;
        

        // following maintains mapping of candidate Ids against candidate names
        static Dictionary<string, string> CandidNameMap = new Dictionary<string, string>();
        static string MayorContestId;
              
        [HttpGet]
        //[CacheFilter(TimeDuration = 10)]
        public HttpResponseMessage GetCandidates()
        {
            try
            {
                Contest mayorContest = new Contest();
                Candidate cand = new Candidate();

                if (MemoryCacher.GetValue("CandidateRanksCounts") != null)
                {
                    ranksDictCached = (Dictionary<string, Candidate>)(MemoryCacher.GetValue("CandidateRanksCounts"));
                    return Request.CreateResponse(HttpStatusCode.OK, ranksDictCached);
                }                

                // I had to reinitialize this object because, when I was sending multiple request with same canddiate id, I observed that votes were getting added
                ranksDictCached = new Dictionary<string, Candidate>();

                CandidNameMap = cand.GetAllCandidatesIdName();

                // I created this function for sake of separation of concerns in project.
                // I like to see Contests as separate entity from candidates
                MayorContestId = mayorContest.GetContestId("Mayor");

                
                string ballotimage = ConfigurationManager.AppSettings["ballotimage"];
                //string ballotimage = @"C:\Users\tambe\Desktop\Jayesh\Programming\Prosper\MayorElection\20151119_ballotimage.txt";
                foreach (string line in File.ReadLines(ballotimage, Encoding.UTF8))
                {
                    if (line.Substring(0, 7).Equals(MayorContestId))
                    {
                        string candId = line.Substring(36, 7);
                        string voteRank = line.Substring(33, 3);                                            
                        if(ranksDictCached.ContainsKey(candId))
                        {
                            Candidate candObj = ranksDictCached[candId];
                            if (voteRank.Equals("001"))
                            {
                                candObj.Rank1Count++;
                            }
                            else if(voteRank.Equals("002"))
                            {
                                candObj.Rank2Count++;
                            }
                            else // As discussed on 19 June 2017, I am assuming there would only be 3 types of ranking
                            {
                                candObj.Rank3Count++;
                            }
                            ranksDictCached[candId] = candObj;
                        }
                        else
                        {
                            Candidate candObj = new Candidate();
                            candObj.candId = candId;
                            // Like Marcus said, Data is never as we expect. I was getting key not found on canddiateId 0000000
                            if (CandidNameMap.ContainsKey(candId)) 
                            {
                                candObj.candName = CandidNameMap[candId];
                            }                            
                            if (voteRank.Equals("001"))
                            {
                                candObj.Rank1Count++;
                            }
                            else if (voteRank.Equals("002"))
                            {
                                candObj.Rank2Count++;
                            }
                            else // As discussed on 19 June 2017, I am assuming there would only be 3 types of ranking
                            {
                                candObj.Rank3Count++;
                            }
                            ranksDictCached.Add(candId, candObj);
                        }
                    }
                }
                MemoryCacher.Add("CandidateRanksCounts", ranksDictCached, DateTimeOffset.UtcNow.AddSeconds(10));
                return Request.CreateResponse(HttpStatusCode.OK, ranksDictCached);
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong: "+ ex.Message);
            }
        }

        [HttpGet]
        //[CacheFilter(TimeDuration = 10)]
        //[Route("{id:alpha}")] 
        public HttpResponseMessage GetCandidateswithId(string id) 
        {
            try
            {
                Contest mayorContest = new Contest();
                Candidate cand = new Candidate();

                if (MemoryCacher.GetValue("CandidateRanksCounts") != null)
                {
                    ranksDictCached = (Dictionary<string, Candidate>)(MemoryCacher.GetValue("CandidateRanksCounts"));
                    return Request.CreateResponse(HttpStatusCode.OK, ranksDictCached[id]); // 
                }

                // I had to reinitialize this object because, when I was sending multiple request with same canddiate id, I observed that votes were getting added
                ranksDictCached = new Dictionary<string, Candidate>();
               

                CandidNameMap = cand.GetAllCandidatesIdName();
                MayorContestId = mayorContest.GetContestId("Mayor");

                string ballotimage = ConfigurationManager.AppSettings["ballotimage"];
                //string ballotimage = @"C:\Users\tambe\Desktop\Jayesh\Programming\Prosper\MayorElection\20151119_ballotimage.txt";

                foreach (string line in File.ReadLines(ballotimage, Encoding.UTF8))
                {
                    if (line.Substring(0, 7).Equals(MayorContestId))
                    {
                        string candId = line.Substring(36, 7);
                        string voteRank = line.Substring(33, 3);
                        if (ranksDictCached.ContainsKey(candId))
                        {
                            Candidate candObj = ranksDictCached[candId];
                            if (voteRank.Equals("001"))
                            {
                                candObj.Rank1Count++;
                            }
                            else if (voteRank.Equals("002"))
                            {
                                candObj.Rank2Count++;
                            }
                            else
                            {
                                candObj.Rank3Count++;
                            }
                            ranksDictCached[candId] = candObj;
                        }
                        else
                        {
                            Candidate candObj = new Candidate();
                            candObj.candId = candId;                           
                            if (CandidNameMap.ContainsKey(candId))
                            {
                                candObj.candName = CandidNameMap[candId];
                            }
                            if (voteRank.Equals("001"))
                            {
                                candObj.Rank1Count++;
                            }
                            else if (voteRank.Equals("002"))
                            {
                                candObj.Rank2Count++;
                            }
                            else
                            {
                                candObj.Rank3Count++;
                            }
                            ranksDictCached.Add(candId, candObj);
                        }
                    }
                }
                MemoryCacher.Add("CandidateRanksCounts", ranksDictCached, DateTimeOffset.UtcNow.AddSeconds(10));
                return Request.CreateResponse(HttpStatusCode.OK, ranksDictCached[id]);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong: " + ex.Message);
            }
        }


    }
}