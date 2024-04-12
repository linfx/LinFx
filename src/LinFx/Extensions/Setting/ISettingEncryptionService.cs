using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Setting
{
    public interface ISettingEncryptionService
    {
        string Encrypt([NotNull] SettingDefinition settingDefinition, [AllowNull] string plainValue);

        string Decrypt([NotNull] SettingDefinition settingDefinition, [AllowNull] string encryptedValue);
    }
}
