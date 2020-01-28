// using NUnit.Framework;
// using PlaygroundsGallery.Domain.DTOs;
// using PlaygroundsGallery.Domain.Models;
// using PlaygroundsGallery.Helper.Exceptions;
// using PlaygroundsGallery.Domain.Repositories;
using Moq;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Tests
{
    public class MemberManagerTests
    {
        // internal Mock<IMemberRepository> MockMemberRepository { get; set; }

        // private FrontManagerSetter _frontManagerSetter;

        // [SetUp]
        // public void Setup()
        // {
        //     _frontManagerSetter = new FrontManagerSetter();
        //     MockMemberRepository = new Mock<IMemberRepository>();
        //     PlaygroundTestContext.SetupRepositoryWithData<Member>(MockMemberRepository.As<IRepository<Member>>(), PlaygroundTestContext.Members);
        //     _frontManagerSetter.InitFrontManager(MockMemberRepository, null);
        // }

        // [Test]
        // public void Login_WrongPassword_MemberLoginExceptionThrown()
        // {
        //     // var memberToLogin = new MemberToInsertDto() {
        //     //     EmailAddress = "sss",
        //     //     Password = "password"
        //     // };

        //     // Assert.ThrowsAsync<MemberLoginException>(() => _frontManagerSetter.MockFrontManager.Login(memberToLogin));
        // }

        // [Test]
        // public async Task Login_MemberExists_Success()
        // {
        //     var memberToLogin = new MemberEntityDto() {
        //         EmailAddress = "yyy",
        //         Password = "password"
        //     };

        //     var token = _frontManagerSetter.MockMemberManager.Login(memberToLogin);
        //     Assert.AreEqual(token.ToString(),"new token");
        // }

        // [Test]
        // public void Register_LoginNameExists_MemberCreationExceptionThrown()
        // {
        //     var memberToLogin = new MemberEntityDto() {
        //         EmailAddress = "yyy@yahoo.fr",
        //         LoginName = "yyy",
        //         Password = "password"
        //     };

        //      Assert.ThrowsAsync<MemberCreationException>(() => _frontManagerSetter.MockMemberManager.Register(memberToLogin));
        // }

        // [Test]
        // public async Task Register_NewMember_MemberCreationSucceed()
        // {
        //     var memberToLogin = new MemberEntityDto() {
        //         EmailAddress = "new@yahoo.fr",
        //         LoginName = "Newest",
        //         Password = "password"
        //     };

        //     var membersCount = PlaygroundTestContext.Members.Count;
        //     var memberEntity = await _frontManagerSetter.MockMemberManager.Register(memberToLogin);
        //     Assert.IsNotNull(memberEntity);
        //     Assert.AreEqual(membersCount + 1, PlaygroundTestContext.Members.Count);
        // }
    }
}