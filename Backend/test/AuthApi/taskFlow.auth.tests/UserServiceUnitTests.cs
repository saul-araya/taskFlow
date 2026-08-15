
using FluentAssertions;
using Moq;
using taskFlow.auth.Application.Dtos.User;
using taskFlow.auth.Application.Dtos.UserProvider;
using taskFlow.auth.Application.Exceptions;
using taskFlow.auth.Application.Interfaces;
using taskFlow.auth.Application.Mappers.Interfaces;
using taskFlow.auth.Application.Services;
using taskFlow.auth.Domain.Entities;
using taskFlow.auth.Domain.Enums;
using taskFlow.auth.Domain.Exceptions;
using taskFlow.auth.Domain.Repositories;

namespace taskFlow.auth.tests;

public class UserServiceUnitTests
{
    private readonly Mock<IUserRepository> repositoryMock = new();
    private readonly Mock<IUnitOfWork> unitOfWorkMock = new();
    private readonly Mock<IUserMapper> mapperMock = new();
    private readonly Mock<IUserProviderMapper> providerMapperMock = new();
    private readonly Mock<IEncriptionService> encryptionServiceMock = new();
    private readonly UserService sut;

    public UserServiceUnitTests()
    {
        sut = new UserService(
            encryptionServiceMock.Object,
            mapperMock.Object,
            providerMapperMock.Object,
            repositoryMock.Object,
            unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task AddUserAsync_WithLocalProvider_ShouldEncryptPassword()
    {
        var providerDto = new CreateUserProviderItemDto(AuthProvider.LOCAL, null, "testPassword123");
        var userDto = new CreateUserDto("TestingName", "TestingDisplayName", "testEmail@example.com", null, providerDto);

        var mappedProvider = new UserProvider { Provider = AuthProvider.LOCAL, PasswordHash = "testPassword123" };
        var mappedUser = new User();

        mapperMock.Setup(x => x.MapToEntity(userDto)).Returns(mappedUser);
        providerMapperMock.Setup(x => x.MapToEntity(providerDto)).Returns(mappedProvider);
        encryptionServiceMock.Setup(x => x.EncryptPassword("testPassword123")).Returns("HashPassword");

        await sut.AddUserAsync(userDto);

        mappedProvider.PasswordHash.Should().Be("HashPassword");
        encryptionServiceMock.Verify(e => e.EncryptPassword("testPassword123"), Times.Once);    
    }

    [Fact]
    public async Task FindUserAsync_WithGuid_ShouldReturnTheUser()
    {
        var uuid = Guid.NewGuid();
        var user = new User();
        
        var mappedUser = new ResUserDto(uuid, "TestingName", "TestingDisplayName", "testEmail@example.com", null, true);

        mapperMock.Setup(x => x.MapToDto(user)).Returns(mappedUser);
        repositoryMock.Setup(x => x.FindByIdAsync(uuid)).ReturnsAsync(user);

        var result = await sut.FindUserByIdAsync(uuid);

        result.Should().NotBeNull();
        result.Should().Be(mappedUser);
        repositoryMock.Verify(x => x.FindByIdAsync(uuid), Times.Once);
    }

    [Fact]
    public async Task FindUserAsync_WithGuid_ShouldReturnNotFound()
    {
        var uuid = Guid.NewGuid();

        var user = new User();
        repositoryMock.Setup(x => x.FindByIdAsync(uuid)).ReturnsAsync((User?)null);

        var act = () => sut.FindUserByIdAsync(uuid);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task FindUserAsync_WithEmail_ShouldReturnTheUser()
    {
        var email = "testEmail@example.com";
        var user = new User();

        var mappedUser = new ResUserDto(new Guid(), "TestingName", "TestingDisplayName", email, null, true);

        mapperMock.Setup(x => x.MapToDto(user)).Returns(mappedUser);
        repositoryMock.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync(user);

        var result = await sut.FindUserByEmailAsync(email);

        result.Should().NotBeNull();
        result.Should().Be(mappedUser);
        repositoryMock.Verify(x => x.FindByEmailAsync(email), Times.Once);
    }

    [Fact]
    public async Task FindUserAsync_WithEmail_ShouldReturnNotFound()
    {
        var email = "testEmail@example.com";

        var user = new User();
        repositoryMock.Setup(x => x.FindByEmailAsync(email)).ReturnsAsync((User?)null);

        var act = () => sut.FindUserByEmailAsync(email);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task SoftDeleteUserAsync_WithGuid_ShouldChangeActiveValue()
    {
        var uuid = Guid.NewGuid();
        var user = new User { Active = true };

        repositoryMock.Setup(x => x.FindByIdAsync(uuid)).ReturnsAsync(user);

        await sut.SoftUserDeleteAsync(uuid);

        user.Active.Should().BeFalse();
        unitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task SoftDeleteUserAsync_WithGuid_ShouldNotFound()
    {
        var uuid = Guid.NewGuid();

        repositoryMock.Setup(x => x.FindByIdAsync(uuid)).ReturnsAsync((User?)null);

        var act = () => sut.SoftUserDeleteAsync(uuid);

        await act.Should().ThrowAsync<NotFoundException>();
        repositoryMock.Verify(x => x.FindByIdAsync(uuid), Times.Once);
    }

    [Fact]
    public async Task AddUserAsync_WithLocalProvider_ShouldEncryptPasswordAndSave()
    {
        var providerDto = new CreateUserProviderItemDto(AuthProvider.LOCAL, null, "testPassword123");
        var userDto = new CreateUserDto("TestingName", "TestingDisplayName", "testEmail@example.com", null, providerDto);

        var mappedProvider = new UserProvider { Provider = AuthProvider.LOCAL, PasswordHash = "testPassword123" };
        var mappedUser = new User();
        var expectedDto = new ResUserDto(Guid.NewGuid(), "TestingName", "TestingDisplayName", "testEmail@example.com", null, true);

        mapperMock.Setup(m => m.MapToEntity(userDto)).Returns(mappedUser);
        providerMapperMock.Setup(m => m.MapToEntity(providerDto)).Returns(mappedProvider);
        encryptionServiceMock.Setup(e => e.EncryptPassword("testPassword123")).Returns("hashedPassword123");
        mapperMock.Setup(m => m.MapToDto(mappedUser)).Returns(expectedDto);

        var result = await sut.AddUserAsync(userDto);

        mappedProvider.PasswordHash.Should().Be("hashedPassword123");
        encryptionServiceMock.Verify(e => e.EncryptPassword("testPassword123"), Times.Once);
        repositoryMock.Verify(r => r.AddUserAsync(mappedUser), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        result.Should().Be(expectedDto);
    }

    [Fact]
    public async Task AddUserAsync_WithExternalProvider_ShouldNotEncryptPassword()
    {
        var providerDto = new CreateUserProviderItemDto(AuthProvider.GOOGLE, "googleId123", null);
        var userDto = new CreateUserDto("TestingName", "TestingDisplayName", "testEmail@example.com", null, providerDto);

        var mappedProvider = new UserProvider { Provider = AuthProvider.GOOGLE, ProviderUserId = "googleId123" };
        var mappedUser = new User();
        var expectedDto = new ResUserDto(Guid.NewGuid(), "TestingName", "TestingDisplayName", "testEmail@example.com", null, true);

        mapperMock.Setup(m => m.MapToEntity(userDto)).Returns(mappedUser);
        providerMapperMock.Setup(m => m.MapToEntity(providerDto)).Returns(mappedProvider);
        mapperMock.Setup(m => m.MapToDto(mappedUser)).Returns(expectedDto);

        var result = await sut.AddUserAsync(userDto);

        encryptionServiceMock.Verify(e => e.EncryptPassword(It.IsAny<string>()), Times.Never);
        result.Should().Be(expectedDto);
    }

    [Fact]
    public async Task AddUserAsync_WithLocalProviderAndNullPassword_ShouldThrowInvalidPasswordException()
    {
        var providerDto = new CreateUserProviderItemDto(AuthProvider.LOCAL, null, null);
        var userDto = new CreateUserDto("TestingName", "TestingDisplayName", "testEmail@example.com", null, providerDto);

        var mappedProvider = new UserProvider { Provider = AuthProvider.LOCAL, PasswordHash = null };
        var mappedUser = new User();

        mapperMock.Setup(m => m.MapToEntity(userDto)).Returns(mappedUser);
        providerMapperMock.Setup(m => m.MapToEntity(providerDto)).Returns(mappedProvider);

        
        var act = () => sut.AddUserAsync(userDto);

        await act.Should().ThrowAsync<InvalidPasswordException>();
        repositoryMock.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Never);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Never);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUserExists_ShouldMapUpdateAndSave()
    {
        var uuid = Guid.NewGuid();
        var user = new User { DisplayName = "OldName" };
        var updateDto = new UpdateUserDto("NewDisplayName", "testEmail@example.com", null);
        var expectedDto = new ResUserDto(uuid, "TestingName", "NewDisplayName", "testEmail@example.com", null, true);

        repositoryMock.Setup(r => r.FindByIdAsync(uuid)).ReturnsAsync(user);
        mapperMock.Setup(m => m.MapToDto(user)).Returns(expectedDto);

        var result = await sut.UpdateUserAsync(updateDto, uuid);

        mapperMock.Verify(m => m.MapToUpdate(user, updateDto), Times.Once);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Once);
        result.Should().Be(expectedDto);
    }

    [Fact]
    public async Task UpdateUserAsync_WhenUserDoesNotExist_ShouldThrowNotFoundException()
    {
        var uuid = Guid.NewGuid();
        var updateDto = new UpdateUserDto("NewDisplayName", "testEmail@example.com", null);

        repositoryMock.Setup(r => r.FindByIdAsync(uuid)).ReturnsAsync((User?)null);

        var act = () => sut.UpdateUserAsync(updateDto, uuid);

        await act.Should().ThrowAsync<NotFoundException>();
        mapperMock.Verify(m => m.MapToUpdate(It.IsAny<User>(), It.IsAny<UpdateUserDto>()), Times.Never);
        unitOfWorkMock.Verify(u => u.SaveChangesAsync(default), Times.Never);
    }
}
