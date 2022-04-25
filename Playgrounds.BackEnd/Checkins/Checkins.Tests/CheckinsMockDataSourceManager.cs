using System;
using System.Collections.Generic;
using PlaygroundsGallery.DataEF.Models;

namespace Checkins.Tests
{
    public static class CheckinsDumbData
    {
        private static DateTime _today = DateTime.Today;
        public static List<CheckIn> GetCheckinsList() =>
            new List<CheckIn>(){
            //     new CheckIn(){
            //         Id = 1,
            //         MemberId = 3,
            //         PlaygroundId = 1,
            //         CheckInDate = _today.AddHours(-5)
            //     },
            //     new CheckIn(){
            //         Id = 2,
            //         MemberId = 5,
            //         PlaygroundId = 1,
            //         CheckInDate = _today.AddHours(-4)
            //     },
            //     new CheckIn(){
            //         Id = 3,
            //         MemberId = 7,
            //         PlaygroundId = 1,
            //         CheckInDate = _today.AddHours(2).AddMinutes(30)
            //     },
            //     new CheckIn(){
            //         Id = 4,
            //         MemberId = 9,
            //         PlaygroundId = 1,
            //         CheckInDate = _today.AddHours(1).AddMinutes(15)
            //     },
            
                new CheckIn(){
                    Id = 5,
                    MemberId = 10,
                    PlaygroundId = 1,
                    CheckInDate = _today.AddHours(3).AddMinutes(15)
                },
                new CheckIn(){
                    Id = 6,
                    MemberId = 122,
                    PlaygroundId = 2,
                    CheckInDate = _today,
                    Member = new Member()
                    {
                        ProfilePictures = new List<ProfilePicture>()
                    }
                },
                new CheckIn(){
                    Id = 7,
                    MemberId = 37,
                    PlaygroundId = 2,
                    CheckInDate = _today
                }
            };
    }
}