using Authentication.User;
using FluentAssertions;
using static Authentication.User.Interpreter;
using static Authentication.User.UserAccountOperations;

namespace Authentication.Tests;

public class InterpreterTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void GivenInvalidUserId_WhenCreateAccount_ThenIsLeft(string? id) =>
        // Given, When, Then
        Interpret(CreateAccount(id), UserAccount.New)
            .IsLeft
            .Should()
            .BeTrue();

    [Fact]
    public void GivenValidUserId_WhenCreateAccount_ThenIsRight() =>
        // Given, When, Then
        Interpret(CreateAccount("hello"), UserAccount.New)
            .IsRight
            .Should()
            .BeTrue();

    [Theory]
    [InlineData("hello")]
    public void GivenValidUserId_WhenCreateAccount_ThenUserAccountIdCorrect(string userId) =>
        // Given, When, Then
        Interpret(CreateAccount(userId), UserAccount.New)
            .Match(Right: tuple => tuple.Item2.AccountId.Value.Should()
                                        .Be(userId),
                   error => { });

    [Fact]
    public void GivenUserAccount_WhenAddAccessToken_ThenIsLeft() =>
        // Given, When, Then
        Interpret(AddAccessToken(new AuthenticationResultFixture().WithAccessToken().WithRefreshToken()), UserAccount.New)
            .IsLeft
            .Should()
            .BeTrue();

    [Fact]
    public void GivenUserAccount_WhenAddAccessToken_ThenIsRight() =>
        // Given, When, Then
        Interpret(AddAccessToken(new AuthenticationResultFixture().WithRefreshToken()), UserAccount.New)
            .IsRight
            .Should()
            .BeTrue();
}