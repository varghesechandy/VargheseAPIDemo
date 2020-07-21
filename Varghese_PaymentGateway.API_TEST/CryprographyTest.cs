using Microsoft.AspNetCore.DataProtection; 
using Moq; 
using System.Text; 
using Xunit;

namespace Varghese_PaymentGateway.API_TEST
{
    public class CryprographyTest
    {
        private readonly Mock<IDataProtectionProvider> mockDataProvider = new Mock<IDataProtectionProvider>();
        private readonly Mock<IDataProtector> mockDataProtector = new Mock<IDataProtector>();
        private const string textToBeProtected = "This is a test";
        public CryprographyTest()
        {
            mockDataProtector.Setup(sut => sut.Protect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes(textToBeProtected));
            mockDataProtector.Setup(sut => sut.Unprotect(It.IsAny<byte[]>())).Returns(Encoding.UTF8.GetBytes(textToBeProtected));
        }

        [Fact]
        public void EncryptonAndDecryption()
        {
            mockDataProvider.Setup(s => s.CreateProtector(It.IsAny<string>())).Returns(mockDataProtector.Object); 
            //Encrypt
            var encrypted = mockDataProtector.Object.Protect(textToBeProtected);  
            //Decrypt
            var decrypt = mockDataProtector.Object.Unprotect(encrypted);  
            //Compare results
            Assert.Equal(textToBeProtected, decrypt); 
        }
    }
}
