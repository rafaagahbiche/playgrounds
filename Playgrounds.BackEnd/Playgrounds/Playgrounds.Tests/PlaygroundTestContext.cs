using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using PlaygroundsGallery.DataEF.Models;
using PlaygroundsGallery.DataEF.Repositories;

namespace Tests
{
    public class PlaygroundTestContext
    {
		private static HashSet<Member> _membersContainer;
		private static HashSet<Photo> _photosContainer;

		public static HashSet<Member> Members => _membersContainer ?? (_membersContainer = initMembers());
		public static HashSet<Photo> Photos => _photosContainer ?? (_photosContainer = initPhotos());
		
        public void AddToContainer(Member m)
        {
            Console.WriteLine("member " + m.LoginName + " is added.");
            _membersContainer.Add(m);
        }

        private static HashSet<Photo> initPhotos()
        {
            return new HashSet<Photo>(){
                new Photo()
                {
                    PublicId = "mmmm",
                    MemberId = 1,
                    Created = DateTime.UtcNow
                },
                new Photo()
                {
                    PublicId = "aaaaa",
                    MemberId = 1,
                    Created = DateTime.UtcNow
                },
                new Photo()
                {
                    PublicId = "yyyyy",
                    MemberId = 1,
                    Created = DateTime.UtcNow
                }
            };
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

        public static void SetupRepositoryWithData<T>(
            Mock<IRepository<T>> mockRepository, 
            ICollection<T> dataContainer) where T: Entity  
		{
			var queryableDataContainer = dataContainer.AsQueryable();
            mockRepository?.Setup(e => e.Add(It.IsAny<T>())).ReturnsAsync((T e) =>
            {
                dataContainer.Add(e);
                return true;
            });

            mockRepository?.Setup(e => e.Remove(
                It.IsAny<T>()))
                    .ReturnsAsync((T e) =>
                        {
                            dataContainer.Remove(e);
                            return true;
                        });

            mockRepository?.Setup(e => e.SingleOrDefault(
                It.IsAny<Expression<Func<T, bool>>>(), 
                It.IsAny<Expression<Func<T, object>>[]>()))
                    .ReturnsAsync((Expression<Func<T, bool>> query, Expression<Func<T, object>>[] includes) =>
                        {
                            return queryableDataContainer.SingleOrDefault(query);
                        });
            
            mockRepository?.Setup(e => e.AnyAsync(
                It.IsAny<Expression<Func<T, bool>>>()))
                    .ReturnsAsync((Expression<Func<T, bool>> query) =>
                        {
                            return queryableDataContainer.Any(query);
                        });
                        
            mockRepository?.Setup(e => e.Update(
                It.IsAny<T>()))
                    .ReturnsAsync((T e) =>
                        {
                            return true;
                        });
		}

    }
}