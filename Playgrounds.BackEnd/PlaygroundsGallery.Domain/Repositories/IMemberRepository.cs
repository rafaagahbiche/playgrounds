using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PlaygroundsGallery.Domain.Models;

namespace PlaygroundsGallery.Domain.Repositories
{
    public interface IMemberRepository: IRepository<Member>
    {
        // Task<Member> GetMemberByEmailAddress(string emailAddress);
    }
}