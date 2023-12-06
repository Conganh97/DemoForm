using Common.Shared.Constants;

namespace Infrastructure.Extensions
{
    public static class PermissionHelper
    {
        public static string GetPermission(FunctionCode functionCode, CommandCode commandCode)
            => string.Join(".", functionCode, commandCode);
    }
}