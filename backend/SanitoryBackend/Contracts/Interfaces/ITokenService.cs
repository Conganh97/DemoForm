using System;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Shared.DTOs.Identity;

namespace Contracts.Interfaces
{
    public interface ITokenService
    {
        TokenResponse GetToken(TokenRequest request);
    }
}
