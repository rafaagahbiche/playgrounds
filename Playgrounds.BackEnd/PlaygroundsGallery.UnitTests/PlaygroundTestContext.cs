using System;
using System.Collections.Generic;
using PlaygroundsGallery.Domain.Models;

namespace Tests
{
    public class PlaygroundTestContext
    {
		private static HashSet<Member> _membersContainer;

		public static HashSet<Member> Members => _membersContainer ?? (_membersContainer = initMembers());
		
        public void AddToContainer(Member m)
        {
            Console.WriteLine("member " + m.LoginName + " is added.");
            _membersContainer.Add(m);
        }

        private static HashSet<Member> initMembers() 
        {
            return new HashSet<Member>(){
                new Member()
                {
                    EmailAddress = "yyy@yahoo.fr",
                    LoginName = "yyy"
                },

                new Member()
                {
                    EmailAddress = "zzz@yahoo.fr",
                    LoginName = "zzz"
                },

                new Member()
                {
                    EmailAddress = "xxx@yahoo.fr",
                    LoginName = "xxx"
                }
            };
        }
    }
}