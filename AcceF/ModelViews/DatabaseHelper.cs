using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceF.ModelViews
{
    class DatabaseHelper
    {

        public DatabaseHelper()
        {

        }

        public static void DeletePartyEntry(Party element)
        {
            RemoveStands(element);
            using (var db = new PartyContext())
            {
                db.Entry(element).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                db.SaveChanges();
            }

        }
        public static void DeleteEmployeeEntry(Person element)
        {
            using (var db = new PartyContext())
            {
                db.Entry(element).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public static void DeleteToolEntry(Tool element)
        {
            using (var db = new PartyContext())
            {
                db.Entry(element).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public static List<Party> SearchPartiesByName(Party party)
        {
            using (var db = new PartyContext())
            {
                return db.parties.Where(x => x.Name == party.Name).OrderBy(x => x.Date).ToList();
            }
        }
        public static List<Party> FindPartyByDate(DateTimeOffset date)
        {
            List<Party> returnList = new List<Party>();
            using (var db = new PartyContext())
            {
                foreach(Party tmp in db.parties.ToList())
                {
                    if (date.Date.CompareTo(tmp.Date.Date) >= 0 && date.Date.CompareTo(tmp.ToDate.Date) <= 0)
                    {
                        returnList.Add(tmp);
                    }
                }
            }
            return returnList;
        }
        public static List<Person> FilterPerson(string text)
        {
            using (var db = new PartyContext())
            {
                return db.employees.Where(t => t.Name.ToLower().Contains(text.ToLower()) || t.Surname.ToLower().Contains(text.ToLower()) || t.City.ToLower().Contains(text.ToLower())).ToList();
            }
        }

        public static List<Organisateur> FilterOrga(string text)
        {
            text = text.ToLower();
            using (var db = new PartyContext())
            {
                return db.organisateurs.Where(t => t.Name.ToLower().Contains(text) || t.Surname.ToLower().Contains(text)).ToList();
            }
        }
        public static List<Party> FilterParty(int index, string text)
        {
            text = text.ToLower();
            using (var db = new PartyContext())
            {
                switch (index)
                {
                    case 0:
                        return db.parties.Where(t => t.Name.ToLower().Contains(text) || t.Adress.ToLower().Contains(text) || t.City.ToLower().Contains(text) || t.Email.ToLower().Contains(text) || t.Phone.ToLower().Contains(text) ||  IsContained(GetOrgasByID(t.OrgaIDs), FilterOrga(text)) || IsContained(GetEmployeesByIDFromStand(t.StandsIDs), FilterPerson(text))).ToList();
                        
                    case 1:
                        return db.parties.Where(t => t.Name.ToLower().Contains(text)).ToList();

                    case 2:
                        return db.parties.Where(t =>  t.Adress.ToLower().Contains(text) || t.City.ToLower().Contains(text) ).ToList();

                    case 3:
                        return db.parties.Where(i => IsContained(GetOrgasByID(i.OrgaIDs), FilterOrga(text))).ToList();
                     
                    case 4:
                        return db.parties.Where(i => IsContained(GetEmployeesByIDFromStand(i.StandsIDs), FilterPerson(text))).ToList();
                    default:
                        return null;
                }
                
            }
        }
        public static bool IsContained(List<Person> tmp, List<Person> tmp2)
        {
            
            if (tmp == null || tmp2 == null)
            {
                return false;
            
            }
            else
            {
                return tmp.Intersect(tmp2, new ProductComparer()).Any();
            }
        }
        public static bool IsContained(List<Organisateur> tmp, List<Organisateur> tmp2)
        {

            if (tmp == null || tmp2 == null)
            {
                return false;

            }
            else
            {
                return tmp.Intersect(tmp2, new ProductComparer2()).Any();
            }
        }
        public static List<Person> GetEmployeesByID(List<String> Ids)
        {
            using (var db = new PartyContext())
            {
                return db.employees.Where(x => Ids.Contains(x.PersonId.ToString())).ToList();
            }

        }
        public static List<Person> GetEmployeesByIDFromStand(List<String> Ids)
        {
            List<Person> result = new List<Person>();
            List<Stand> stands = GetStandsByID(Ids);
            using (var db = new PartyContext())
            {
                foreach(Stand stand in stands)
                {
                    result.Concat(db.employees.Where(x => stand.EmployeesIDs.Contains(x.PersonId.ToString())).ToList());
                }
                return result;
            }

        }
        public static List<Stand> GetStandsByID(List<String> Ids)
        {
            using (var db = new PartyContext())
            {
                return db.stands.Where(x => Ids.Contains(x.StandId.ToString())).ToList();
            }

        }
        public static List<Organisateur> GetOrgasByID(List<String> Ids)
        {
            using (var db = new PartyContext())
            {
                return db.organisateurs.Where(x => Ids.Contains(x.OrganisateurId.ToString())).ToList();
            }
        }
        public static List<File> GetFilesByID(List<String> Ids)
        {
            using (var db = new PartyContext())
            {
                return db.files.Where(x => Ids.Contains(x.FileId.ToString())).ToList();
            }
        }

        public static List<Skill> GetSkillsByID(List<String> Ids)
        {
            using (var db = new PartyContext())
            {
                return db.skills.Where(x => Ids.Contains(x.ToolId.ToString())).ToList();
            }
        }
        public static List<Party> GetToBeNotifiedParties()
        {
            List<Party> parties = new List<Party>();
            List<int> tmpMonths = new List<int> { 1, 3, 6 };
            using(var db = new PartyContext())
            {
                
                foreach (Party party in db.parties.ToList())
                {
                    if (party.MonthToGo >= 0)
                    {
                        int result = DateTime.Today.AddMonths(tmpMonths[party.WriteMonthly]).CompareTo(party.Date);
                        int result2 = party.Lastwrote.AddMonths(tmpMonths[party.WriteMonthly]).CompareTo(party.Date);
                        Debug.WriteLine(result);
                        Debug.WriteLine(result2);
                        if (result >= 0 && result2 < 0)
                        {
                            parties.Add(party);
                        }
                    }
                }
            }
            return parties;
        }
        public static int CountTool(Tool tool, Party party, Stand currentStand)
        {
            int counter = 0;
            using(var db= new PartyContext())
            {
                var parties =db.parties.Where(x => ( party.Date.CompareTo(x.ToDate) <= 0  && x.Date.CompareTo(party.Date) < 0) || (party.ToDate.CompareTo(x.Date) >= 0 && x.ToDate.CompareTo(party.ToDate) >= 0) || (party.Date.CompareTo(x.Date) <= 0 && party.ToDate.CompareTo(x.ToDate) > 0)).ToList<Party>();
                foreach(Party tmp in parties)
                {
                    foreach(Stand stand in GetStandsByID(tmp.StandsIDs))
                    {
                        if(stand.StandId != currentStand.StandId)
                        {
                            List<string> toolsIds = stand.ToolsAsStrings.Split(',').ToList();
                            counter += toolsIds.Where(x => x.Equals(tool.ToolId.ToString())).Count();
                        }
                      
                    }
                }
                if(currentStand.ToolsAsStrings != null)
                {
                    Debug.WriteLine(currentStand.ToolsAsStrings);
                    List<string> tmp2 = currentStand.ToolsAsStrings.Split(',').ToList();
                    counter += tmp2.Where(x => x.Equals(tool.ToolId.ToString())).Count();
                }
                
            }
            return counter;
        }
        public static void DeleteStands(Party partyTmp, List<string> Stands)
        {
            
                using (var db = new PartyContext())
                {
                Party party = db.parties.Find(partyTmp.PartyId);
                if (party != null)
                {
                    List<string> realStands = party.StandsIDs;
                    List<string> removedstand = Stands.Except(realStands).ToList();
                    foreach (string standId in removedstand)
                    {
                        var stand = db.stands.Find(Convert.ToInt32(standId));
                        db.Entry(stand).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                    }
                    
                }
                else
                {
                    if(partyTmp.StandsIDs != null)
                    {
                        foreach (string standId in partyTmp.StandsIDs)
                        {
                            var stand = db.stands.Find(Convert.ToInt32(standId));
                            db.Entry(stand).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;

                        }
                    }
                }
                db.SaveChanges();
            }
        }

        public static List<Tool> GetToolsByID(List<string> Ids)
        {
            using (var db = new PartyContext())
            {
                List<Tool> result = new List<Tool>();
                var ids = db.tools.Select(i => i.ToolId.ToString());
                if (Ids != null)
                foreach (string id in Ids)
                {
                    if (!id.Equals(""))
                        {
                            result.Add(db.tools.Find(Convert.ToInt32(id)));
                        }
                   

                }
                return result;
          
            }
           
        }

        public static void RemoveStands(Party party)
        {
            using (var db = new PartyContext())
            {
                foreach(Stand stand in GetStandsByID(party.StandsIDs))
                {
                    db.Entry(stand).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
                db.SaveChanges();
            }

        }

        public static void CheckParties()
        {
            using (var db = new PartyContext())
            {
                Debug.WriteLine("fff");
                foreach (Party party in db.parties)
                {
                    if (party.ToDate.CompareTo(DateTime.Today) < 0 && !party.AlreadyDone)
                    {
                        party.AlreadyDone = true;
                        var partyDb = db.parties.Find(party.PartyId);
                        db.Entry(partyDb).CurrentValues.SetValues(party);

                        Party tmp = new Party();
                        Party newParty = Utilities.CloneJson<Party>(party);
                        newParty.PartyId = tmp.PartyId;
                        newParty.StandsAsStrings = null;
                        newParty.FilesAsStrings = null;
                        newParty.Accepted = true;
                        newParty.AlreadyDone = false;
                        newParty.ToDate = newParty.ToDate.AddMonths(party.Frequency);
                        newParty.Date = newParty.Date.AddMonths(party.Frequency);
                        db.parties.Add(newParty);
                    }
                }
                db.SaveChanges();
            }
        }

 
    }
    public class ProductComparer : IEqualityComparer<Person>
    {

        public bool Equals(Person x, Person y)
        {
            //Check whether the objects are the same object. 
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether the products' properties are equal. 
            return  x.PersonId == y.PersonId;
        }

        public int GetHashCode(Person obj)
        {
            //Get hash code for the Name field if it is not null. 
            int hashProductName = obj.PersonId.GetHashCode();

            //Calculate the hash code for the product. 
            return hashProductName;
        }

    }
    public class ProductComparer2 : IEqualityComparer<Organisateur>
    {

        public bool Equals(Organisateur x, Organisateur y)
        {
            //Check whether the objects are the same object. 
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether the products' properties are equal. 
            return x.OrganisateurId == y.OrganisateurId;
        }

        public int GetHashCode(Organisateur obj)
        {
            //Get hash code for the Name field if it is not null. 
            int hashProductName = obj.OrganisateurId.GetHashCode();

            //Calculate the hash code for the product. 
            return hashProductName;
        }
    }
    
}
