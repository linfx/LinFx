using JetBrains.Annotations;

namespace LinFx.Extensions.Setting
{
    public interface ISettingEncryptionService
    {
        [CanBeNull]
        string Encrypt([NotNull] SettingDefinition settingDefinition, [CanBeNull] string plainValue);

        [CanBeNull]
        string Decrypt([NotNull] SettingDefinition settingDefinition, [CanBeNull] string encryptedValue);
    }
}
