namespace TD.OpenData.WebApi.Shared.DTOs.Identity;

public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);