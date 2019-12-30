using System.Collections.Generic;
using AutoMapper;
using Moq;
using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Managers;
using PlaygroundsGallery.Domain.Models;
using PlaygroundsGallery.Domain.Repositories;
using PlaygroundsGallery.Helper;

namespace Tests
{
    public class FrontManagerSetter
    {
        internal IFrontManager MockFrontManager { get; set; }
        
        private Mock<IPasswordManager> _mockPasswordManager;
        
        private Mock<IPhotoUploader> _mockPhotoUploader;
        
        private Mock<ITokenManager> _mockTokenManager;
        
        private Mock<IMapper> _mockMapper;

        public FrontManagerSetter() 
        {
            this._mockPasswordManager = new Mock<IPasswordManager>();
            this._mockPhotoUploader = new Mock<IPhotoUploader>();
            this._mockTokenManager = new Mock<ITokenManager>();
            this._mockMapper = new Mock<IMapper>();
            SetUpMocks();
        }

        private void SetUpMocks()
        {
            this._mockTokenManager.Setup(t => t.CreateToken(
                It.IsAny<int>(), 
                It.IsAny<string>(), 
                It.IsAny<string>())).Returns("new token");
            this._mockPasswordManager.Setup(p => p.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>())).Returns(true);
            this._mockPasswordManager.Setup(p => p.SetPasswordHashAndSalt(It.IsAny<string>())).Verifiable();
            this._mockMapper.Setup(m => m.Map<MemberToLoginDto>(It.IsAny<Member>())).Returns((Member m) => {
                return new MemberToLoginDto() {
                    EmailAddress = m.EmailAddress,
                    LoginName = m.LoginName
                };
            });
            this._mockMapper.Setup(m => m.Map<Member>(It.IsAny<MemberToLoginDto>())).Returns((MemberToLoginDto m) => {
                return new Member() {
                    EmailAddress = m.EmailAddress,
                    LoginName = m.LoginName
                };
            });
            this._mockMapper.Setup(m => m.Map<MemberDto>(It.IsAny<Member>())).Returns((Member m) => {
                return new MemberDto() {
                    EmailAddress = m.EmailAddress,
                    LoginName = m.LoginName
                };
            });
        }
        
        internal void InitFrontManager(
            Mock<IMemberRepository> memberRepo, 
            Mock<IPhotoRepository> photoRepo)
        {
            this.MockFrontManager = new FrontManager(
                photoRepo?.Object,
                memberRepo?.Object,
                this._mockPhotoUploader.Object,
                this._mockPasswordManager.Object,
                this._mockTokenManager.Object,
                this._mockMapper.Object);
        }
    }
}