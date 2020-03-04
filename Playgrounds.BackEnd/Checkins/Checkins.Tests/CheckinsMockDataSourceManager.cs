using System;
using System.Collections.Generic;
using PlaygroundsGallery.DataEF.Models;

namespace Checkins.Tests
{
    public class CheckinsMockDataSourceManager
    {
        internal static ICollection<CheckIn> CheckinsCollection;
        private DateTime _today = DateTime.Today;
        public void FillCheckinsMockDataSourceWithDumpData()
        {
            CheckinsCollection = new HashSet<CheckIn>();
            CheckinsCollection.Add(
                new CheckIn(){
                    Id = 1,
                    MemberId = 3,
                    PlaygroundId = 1,
                    CheckInDate = _today.AddHours(-5)
                }
            );
            CheckinsCollection.Add(
                new CheckIn(){
                    Id = 2,
                    MemberId = 5,
                    PlaygroundId = 1,
                    CheckInDate = _today.AddHours(-4)
                }
            );
            CheckinsCollection.Add(
                new CheckIn(){
                    Id = 3,
                    MemberId = 7,
                    PlaygroundId = 1,
                    CheckInDate = _today.AddHours(2).AddMinutes(30)
                }
            );
            CheckinsCollection.Add(
                new CheckIn(){
                    Id = 4,
                    MemberId = 9,
                    PlaygroundId = 1,
                    CheckInDate = _today.AddHours(1).AddMinutes(15)
                }
            );
            CheckinsCollection.Add(
                new CheckIn(){
                    Id = 5,
                    MemberId = 10,
                    PlaygroundId = 1,
                    CheckInDate = _today.AddHours(3).AddMinutes(15)
                }
            );
        }
    }
}