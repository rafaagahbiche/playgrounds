using NUnit.Framework;
using Moq;
using PlaygroundsGallery.Domain.DTOs;
using System.Collections.Generic;
using PlaygroundsGallery.Domain.Models;
using System.Linq;
using System;
using PlaygroundsGallery.Helper.Exceptions;
using System.Threading.Tasks;
using PlaygroundsGallery.Domain.Repositories;
using System.Linq.Expressions;

namespace Tests
{
    public class MemberManagerTests
    {
        internal Mock<IMemberRepository> MockMemberRepository { get; set; }

        private FrontManagerSetter _frontManagerSetter;

        [SetUp]
        public void Setup()
        {
            _frontManagerSetter = new FrontManagerSetter();
            MockMemberRepository = new Mock<IMemberRepository>();
            PlaygroundTestContext.SetupRepositoryWithData<Member>(MockMemberRepository.As<IRepository<Member>>(), PlaygroundTestContext.Members);
            _frontManagerSetter.InitFrontManager(MockMemberRepository, null);
        }

        [Test]
        public void Login_WrongPassword_MemberLoginExceptionThrown()
        {
            var memberToLogin = new MemberToLoginDto() {
                EmailAddress = "sss",
                Password = "password"
            };

            Assert.ThrowsAsync<MemberLoginException>(() => _frontManagerSetter.MockFrontManager.Login(memberToLogin));
        }

        [Test]
        public async Task Login_MemberExists_Success()
        {
            var memberToLogin = new MemberToLoginDto() {
                EmailAddress = "yyy",
                Password = "password"
            };

            var token = await _frontManagerSetter.MockFrontManager.Login(memberToLogin);
            Assert.AreEqual(token.ToString(),"new token");
        }

        [Test]
        public void Register_LoginNameExists_MemberCreationExceptionThrown()
        {
            var memberToLogin = new MemberToLoginDto() {
                EmailAddress = "yyy@yahoo.fr",
                LoginName = "yyy",
                Password = "password"
            };

             Assert.ThrowsAsync<MemberCreationException>(() => _frontManagerSetter.MockFrontManager.Register(memberToLogin));
        }

        [Test]
        public async Task Register_NewMember_MemberCreationSucceed()
        {
            var memberToLogin = new MemberToLoginDto() {
                EmailAddress = "new@yahoo.fr",
                LoginName = "Newest",
                Password = "password"
            };

            var membersCount = PlaygroundTestContext.Members.Count;
            var memberEntity = await _frontManagerSetter.MockFrontManager.Register(memberToLogin);
            Assert.IsNotNull(memberEntity);
            Assert.AreEqual(membersCount + 1, PlaygroundTestContext.Members.Count);
        }
    }
}