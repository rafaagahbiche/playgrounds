using System;
using System.Linq;

namespace PlaygroundsGallery.DataEF.Seed
{
    public class CheckinUpdate
    {
        private static bool changesHaveBeenMade = false; 
        public static bool UpdateTodayCheckins(GalleryContext context)
        {
            changesHaveBeenMade = changesHaveBeenMade || CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 15, 6);
            changesHaveBeenMade = changesHaveBeenMade || CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 5, 10);
            changesHaveBeenMade = changesHaveBeenMade || CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 17, 7);
            changesHaveBeenMade = changesHaveBeenMade || CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 16, 7);
            changesHaveBeenMade = changesHaveBeenMade || CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 2, 10);
            changesHaveBeenMade = changesHaveBeenMade || CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 3, 10);
            changesHaveBeenMade = changesHaveBeenMade || CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 4, 10);

            return changesHaveBeenMade;
        }
        private static bool UpdateCheckinsByPlaygroundId(GalleryContext context, int playgroundId, int checkinsCountToUpdate)
        {
            var checkinUpdated = false;
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

                    checkinUpdated = true;
                }
            }

            return checkinUpdated;
        }
    }
}