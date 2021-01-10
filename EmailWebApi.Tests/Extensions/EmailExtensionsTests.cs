using System;
using EmailWebApi.Db.Entities;
using EmailWebApi.Extensions;
using Xunit;

namespace EmailWebApi.Tests.Extensions
{
    public class EmailExtensionsTests
    {
        [Theory]
        [InlineData(EmailStatus.Error)]
        [InlineData(EmailStatus.Query)]
        [InlineData(EmailStatus.Sent)]
        public void SetState(EmailStatus status)
        {
            //Arrange
            var email = new Email();

            //Act
            email.SetState(status);

            //Assert
            Assert.Equal(status, email.State.Status);
        }

        [Fact]
        public void SetEmailInfo()
        {
            //Arrange
            var email = new Email();

            //Act
            email.SetEmailInfo();

            //Assert
            Assert.NotEqual(DateTime.MinValue, email.Info.Date);
            Assert.NotEqual(Guid.Empty, email.Info.UniversalId);
        }
    }
}