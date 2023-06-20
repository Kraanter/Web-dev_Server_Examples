using Moq;

namespace xUnitTestVoorToets;

public class MockingUserTest 
{
    private Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();

    public MockingUserTest()
    {
        mockUserRepository.Setup(repo => repo.GetUser(It.IsAny<string>()))
            .Returns((string user) =>
            {
                if (user != "John Doe")
                    return null;
                
                return new User() { Age = 18, Name = "John Doe" };
            }
        );
    }
    
    [Fact]
    public void GetUser_ShouldReturnUser()
    {
        // Arrange
        UserService userService = new UserService(mockUserRepository.Object);
        string name = "John Doe";

        // Act
        var result = userService.GetUser(name);

        // Assert
        Assert.Equal(name, result.Name);
    }

    [Fact]
    public void GetNonExistingUser_ShouldReturnNull()
    {
        UserService userService = new UserService(mockUserRepository.Object);
        string name = "Krijn Grimme";

        // Act
        var result = userService.GetUser(name);

        // Assert
        Assert.Null(result);
    }
}