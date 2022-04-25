using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PlaygroundsGallery.DataEF.Models;

namespace PlaygroundsGallery.DataEF.Seed
{
    public class CheckinSeed
    {
        private readonly GalleryContext context;

        public CheckinSeed(GalleryContext context)
        {
            this.context = context;
        }

        public bool AddNextweekCheckinsFromJsonFile(string jsonFilePath)
        {
            var checkinsData = System.IO.File.ReadAllText(jsonFilePath);
            var checkinsEntities = JsonConvert.DeserializeObject<List<CheckIn>>(checkinsData);
            context.AddRange(checkinsEntities);
            return context.SaveChanges() > 0;
        }

        public bool SetCheckinsByLocationDatesAsToday(int locationId, int checkinsCount)
        {
            var playgroundIds = getPlaygroundIdsByLocationId(locationId);
            if (this.context.CheckIns.Any() && playgroundIds.Count() > 0)
            {
                var checkins = getCheckinsInPlaygrounds(playgroundIds, checkinsCount);
                if (checkins.Any())
                {
                    checkins = setCheckinsToDateAndSameTime(checkins, DateTime.Today);
                    this.context.CheckIns.UpdateRange(checkins);
                    return this.context.SaveChanges() > 0;
                }
            }

            return false;
        }

        // checkinsCount = 280
        // daysCount = 3 + today
        // checkinsCountPerDay = 70
        public bool SetUpcomingCheckinsByLocation(int locationId, int checkinsCount, int daysCount)
        {
            var playgroundIds = getPlaygroundIdsByLocationId(locationId);
            if (this.context.CheckIns.Any())
            {
                var checkins = getCheckinsInPlaygrounds(playgroundIds, checkinsCount);
                checkins = checkins.Reverse();
                DateTime oldestCheckinDate = checkins.Select(c => c.CheckInDate).FirstOrDefault();
                if (oldestCheckinDate.Date < DateTime.Today.Date)
                {
                    var newChckins = new List<CheckIn>();
                    var checkinsCountPerDay = checkinsCount/(daysCount + 1);
                    for (int i = 0; i<=daysCount; i++)
                    {
                        var checkinsDay = setCheckinsToDateAndSameTime(
                            checkins.Skip(checkinsCountPerDay*i)
                                    .Take(checkinsCountPerDay), 
                            DateTime.Today.AddDays(i));
                        newChckins.AddRange(checkinsDay);
                    }

                    this.context.CheckIns.UpdateRange(newChckins);
                    return this.context.SaveChanges() > 0;
                }
            }
            
            return false;
        }

        private int [] getPlaygroundIdsByLocationId(int locationId) =>
            this.context.Playgrounds
                                .Where(p =>p.LocationId == locationId)
                                .Select(p => p.Id)
                                .ToArray();
        

        private IEnumerable<CheckIn> getCheckinsInPlaygrounds(int[] playgroundIds, int checkinsCountToUpdate) =>
            this.context.CheckIns
                    .OrderByDescending(ch => ch.CheckInDate)
                    .Take(checkinsCountToUpdate)
                    .Where(ch => playgroundIds.Contains(ch.PlaygroundId)
                            && ch.CheckInDate.Date !=  DateTime.Today.Date);

        private IEnumerable<CheckIn> setCheckinsToDateAndSameTime(IEnumerable<CheckIn> checkins, DateTime date)
        {
            var todayDay = date.Day;
            var todayYear = date.Year;
            var todayMonth = date.Month;     
            foreach (var checkin in checkins)
            {
                checkin.CheckInDate = new DateTime(
                    todayYear, 
                    todayMonth,
                    todayDay, 
                    checkin.CheckInDate.Hour,
                    checkin.CheckInDate.Minute,
                    0);
            }

            return checkins;
        }
    }
}