using LinFx.Extensions.DependencyInjection;
using LinFx.Security.Encryption;

namespace LinFx.Extensions.Setting
{
    [Service]
    public class SettingEncryptionService : ISettingEncryptionService
    {
        protected IStringEncryptionService StringEncryptionService { get; }

        public SettingEncryptionService(IStringEncryptionService stringEncryptionService)
        {
            StringEncryptionService = stringEncryptionService;
        }

        public virtual string Encrypt(SettingDefinition settingDefinition, string plainValue)
        {
            if (string.IsNullOrEmpty(plainValue))
            {
                return plainValue;
            }
            return StringEncryptionService.Encrypt(plainValue);
        }

        public virtual string Decrypt(SettingDefinition settingDefinition, string encryptedValue)
        {
            if (string.IsNullOrEmpty(encryptedValue))
            {
                return encryptedValue;
            }
            return StringEncryptionService.Decrypt(encryptedValue);
        }
    }
}