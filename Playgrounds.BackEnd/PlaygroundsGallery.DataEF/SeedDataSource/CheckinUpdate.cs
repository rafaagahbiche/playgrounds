using System;
using System.Linq;
using PlaygroundsGallery.DataEF.Models;
using PlaygroundsGallery.DataEF.Repositories;

namespace PlaygroundsGallery.DataEF.Seed
{
    public class CheckinUpdate
    {
        public static bool UpdateCheckinsByPlaygroundId(GalleryContext context, int playgroundId, int checkinsCountToUpdate)
        {
            var changesHaveBeenMade = false;
            if (context.CheckIns.Any())
            {
                // var checkinsRepo = new Repository<CheckIn>(context);
                var todayDay = DateTime.Today.Day;
                var todayYear = DateTime.Today.Year;
                var todayMonth = DateTime.Today.Month;

                var checkins = context.CheckIns
                    .Where(ch => ch.PlaygroundId == playgroundId)
                    .OrderByDescending(ch => ch.CheckInDate)
                    .Take(checkinsCountToUpdate)
                    .Where(che => che.CheckInDate.Date !=  DateTime.Today);

                if (checkins.Any())
                {
                    foreach (var checkin in checkins)
                    {
                        checkin.CheckInDate = new DateTime(
                            todayYear, 
                            todayMonth,
                            todayDay, 
                            checkin.CheckInDate.Hour,
                            checkin.CheckInDate.Minute,
                            0);
                        context.CheckIns.Update(checkin);
                    }

                    changesHaveBeenMade = true;
                }
            }

            return changesHaveBeenMade;
        }
    }
}