using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Configuration;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace MayorElection.Models
{
    [Serializable]
    [DataContract(Name="Candidate")]
    [XmlRoot(ElementName = "Promotions")]
    public class Candidate
    {        
        [DataMember(Name="CandidateId")]
        [XmlAttribute("CandidateId")]
        public string candId;
        [DataMember(Name = "CandidateName")]
        [XmlAttribute("CandidateName")]
        public string candName;
        [DataMember(Name = "Rank1Count")]
        [XmlAttribute("Rank1Count")]
        public int Rank1Count = 0;
        [DataMember(Name = "Rank2Count")]
        [XmlAttribute("Rank2Count")]
        public int Rank2Count = 0;
        [DataMember(Name = "Rank3Count")]
        [XmlAttribute("Rank3Count")]
        public int Rank3Count = 0;

        public Dictionary<string,string> GetAllCandidatesIdName()
        {
            Dictionary<string, string> candidatesDict = new Dictionary<string, string>();
            try
            {
                //string MasterLookup = @"C:\Users\tambe\Desktop\Jayesh\Programming\Prosper\MayorElection\20151119_masterlookup.txt";
                string MasterLookup = ConfigurationManager.AppSettings["masterlookup"];

                foreach (string line in File.ReadLines(MasterLookup, Encoding.UTF8))
                {
                    if (line.Substring(0, 10).ToLower().Contains("Candidate".ToLower()))
                    {
                        candidatesDict.Add(line.Substring(10, 7), line.Substring(17, 50).Trim()); //id:name
                    }
                }
                return candidatesDict;
            }
            catch(Exception ex)
            {
                return candidatesDict;
            }
        }
    }


    // I created this class for sake of separation of concerns in project.
    // I like to see Contests as separate entity from candidates
    public class Contest
    {

        public  string ContestId;
        public  string GetContestId(string contestDesc)
        {
            try
            {
                string contestid = "";

                //string MasterLookup = @"C:\Users\tambe\Desktop\Jayesh\Programming\Prosper\MayorElection\20151119_masterlookup.txt";
                string MasterLookup = ConfigurationManager.AppSettings["masterlookup"];

                foreach (string line in File.ReadLines(MasterLookup, Encoding.UTF8))
                {
                    if (line.Substring(17, 50).ToLower().Contains(contestDesc.ToLower()) && line.Substring(0, 10).ToLower().Contains("Contest".ToLower()))
                    {
                        contestid = line.Substring(10, 7);
                    }
                }
                return contestid;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

    }
}