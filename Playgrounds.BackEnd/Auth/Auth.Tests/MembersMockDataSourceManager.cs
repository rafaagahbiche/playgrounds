using System.Collections.Generic;
using PlaygroundsGallery.DataEF.Models;

namespace Auth.Tests
{
    public class MembersMockDataSourceManager
    {
        internal static ICollection<Member> CheckinsCollection;
        public static void FillDataSourceWithMembers()
        {
            CheckinsCollection = new HashSet<Member>();
            CheckinsCollection.Add(new Member(){
                Id = 1,
                
            });
        }
    }
}