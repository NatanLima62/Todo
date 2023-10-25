using Microsoft.AspNetCore.Http;
using Todo.Core.Enums;

namespace Todo.Application.Contracts;

public interface IFileService
{
    Task<string> Upload(IFormFile arquivo, EUploadPath uploadPath, EPathAccess pathAccess = EPathAccess.Private,
        int urlLimitLength = 255);
    string ObterPath(Uri uri);
    bool Apagar(Uri uri);
}